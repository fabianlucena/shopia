<script>
  import { setContext } from 'svelte';
  import { writable } from 'svelte/store';
  import Button from './Button.svelte';

  let {
    header = '',
    children,
    disabled = false,
    footer,
    submitLabel = '',
    onsubmit = null,
    submitPosition = 'first',
    layout = 'single-column',
    class: formClass = '',
    ...otherProps
  } = $props();
  const disabledStore = writable();
  setContext('disabled-form', disabledStore);

  $effect(() => {
    disabledStore.set(disabled);
  });

  async function handleSubmit(evt) {
    evt.preventDefault();

    const setDisabled = !disabled && disabled !== false;
    setDisabled && disabledStore.set(true);
    await onsubmit?.(evt);
    setDisabled && disabledStore.set(false);
  }
</script>

<form
  onsubmit={handleSubmit}
  class={(layout + ' ' + formClass).trim()}
  {...otherProps}
>
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
  }

  form.single-column {
    max-width: 40em;
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