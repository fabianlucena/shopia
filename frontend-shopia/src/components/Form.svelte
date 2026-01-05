<script>
  import { setContext } from 'svelte';
  import { writable } from 'svelte/store';
  import Button from './controls/Button.svelte';
  import LoadingIcon from '$icons/loading.svelte';

  let {
    header = '',
    children,
    disabled = null,
    loading = null,
    footer = null,
    submitLabel = '',
    onsubmit = null,
    submitPosition = 'first',
    layout = 'single-column',
    class: formClass = '',
    ...otherProps
  } = $props();

  const disabledForm = writable();
  setContext('disabled-form', disabledForm);
  $effect(() => {
    disabledForm.set(!!disabled);
  });

  const loadingForm = writable();
  setContext('loading-form', loadingForm);
  $effect(() => {
    loadingForm.set(!!loading);
  });

  async function handleSubmit(evt) {
    evt.preventDefault();

    const setDisabled = !disabled && disabled !== false;
    const setLoading = !loading && loading !== false;

    setDisabled && disabledForm.set(true);
    setLoading && loadingForm.set(true);
    await onsubmit?.(evt);
    setDisabled && disabledForm.set(false);
    setLoading && loadingForm.set(false);
  }
</script>

<form
  onsubmit={handleSubmit}
  class={(layout + ' ' + formClass).trim()}
  {...otherProps}
>
  {#if $loadingForm}
    <div 
      class="overlay"
    >
      <LoadingIcon />
    </div>
  {/if}
  {#if header}
    <div class="form-header">
      <h2>{header}</h2>
    </div>
  {/if}
  <div
    class="form-body"
  >
    {@render children?.()}
  </div>
  {#if submitLabel || onsubmit || footer}
    <div class="form-footer">
      {#if (submitLabel || onsubmit) && submitPosition === 'first'}
        <Button type="submit">{submitLabel || 'Enviar'}</Button>
      {/if}
      {@render footer?.()}
      {#if (submitLabel || onsubmit) && submitPosition !== 'first'}
        <Button type="submit">{submitLabel || 'Enviar'}</Button>
      {/if}
    </div>
  {/if}
</form>

<style>
  form {
    display: flex;
    flex-direction: column;
    padding: .5em 0;
    position: relative
  }

  .overlay {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(128, 128, 128, 0.3);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.5em;
    color: var(--text-color);
    z-index: 10;
  } 

  form.single-column {
    max-width: 40em;
    width: 100%;
  }

  .form-header,
  .form-body,
  .form-footer {
    flex: 1;
    display: flex;
  }

  .form-body {
    display: flex;
    flex-direction: column;
    gap: .3em;
  }

  .form-footer {
    margin-top: .5em;
    justify-content: space-evenly;
    background-color: var(--input-background-color);
  }

  h2 {
    margin: 0;
    flex: 1;
    font-size: 1.2em;
    text-align: center;
    background-color: var(--header-background-color);
    color: var(--header-text-color);
    padding: .5em;
    margin-bottom: .5em;
  }

  :global(button[type="submit"]) {
    align-self: center;
  }
</style>