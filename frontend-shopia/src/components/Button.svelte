<script>
  import { getContext } from 'svelte';

  let {
    type = 'button',
    disabled = null,
    variant = '',
    className = '',
    children,
    ...props
  } = $props();
  

  let disabledForm = getContext('disabled-form');
  let isDisabled = $derived(disabledForm);
</script>

<button 
  {type}
  class={`${variant} ${className}`}
  disabled={disabled ?? $isDisabled}
  {...props}
>
  {@render children()}
</button>

<style>
  button {
    padding: .4em .7em;
    border: none;
    border-radius: .4em;
    cursor: pointer;
  }

  button:disabled {
    background-color: #cccccc;
    cursor: not-allowed;
  }

  button:hover {
    background-color: var(--button-background-color-highlight);
    outline: var(--border-color) solid .1em;
    box-shadow: 0 0 5px var(--border-color);
  }

  .primary {
    transition: background-color 0.3s ease;
    background-color: var(--button-background-color);
    color: white;
  }

  .primary:hover {
    transition: background-color 0.3s ease;
    background-color: var(--button-background-color-highlight);
  }

  .danger {
    transition: background-color 0.3s ease;
    background-color: #dc3545;
    color: white;
  }

  .danger:hover {
    transition: background-color 0.3s ease;
    background-color: #a71d2a;
  }

  .icon {
    padding: 0;
    height: 1em;
    width: auto;
    vertical-align: middle;
  }

  .icon:hover {
    background-color: var(--menu-background-color);
  }

  .icon :global(svg) {
    height: 100%;
    width: auto;
  }
</style>