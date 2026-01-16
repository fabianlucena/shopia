<script>
  import { setContext } from 'svelte';
  import { writable } from 'svelte/store';
  import Button from './controls/Button.svelte';
  import LoadingIcon from '$icons/loading.svelte';
  import { pushNotification } from '$libs/notification';
  import { navigate } from '$libs/router';

  let {
    header = '',
    children,
    disabled = null,
    loading = null,
    footer = null,
    submitLabel = 'Enviar',
    onSubmit = null,
    canSubmit = true,
    cancelLabel = null,
    onCancel = null,
    cancelable = false,
    submitPosition = 'first',
    layout = 'single-column',
    class: formClass = '',
    validate = null,
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

    if (validate) {
      const validationMessage = validate();
      if (validationMessage !== true) {
        pushNotification(validationMessage ?? 'Error de validación', 'error');
        return;
      }
    }

    const setDisabled = !disabled && disabled !== false;
    const setLoading = !loading && loading !== false;

    setDisabled && disabledForm.set(true);
    setLoading && loadingForm.set(true);
    await onSubmit?.(evt);
    setDisabled && disabledForm.set(false);
    setLoading && loadingForm.set(false);
  }

  function cancelHandler(evt) {
    evt.preventDefault();
    
    console.log(onCancel);

    if (onCancel) {
      onCancel(evt);
    } else {
      navigate(-1);
    }
  }
</script>

{#snippet footButtons()}
  {#if submitLabel || onSubmit}
    <Button type="submit" disabled={!canSubmit}>{submitLabel || 'Enviar'}</Button>
  {/if}
  {#if cancelLabel || onCancel || cancelable}
    <Button onClick={cancelHandler} variant="danger">{cancelLabel || 'Cancelar'}</Button>
  {/if}
{/snippet}

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
  {#if submitLabel || onSubmit || footer || cancelLabel || onCancel || cancelable}
    <div class="form-footer">
      {#if submitPosition === 'first'}
        {@render footButtons()}
      {/if}
      {@render footer?.()}
      {#if submitPosition !== 'first'}
        {@render footButtons()}
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
    padding: .5em;
    display: flex;
    gap: .5em;
    justify-content: flex-end;
    align-items: flex-end;
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