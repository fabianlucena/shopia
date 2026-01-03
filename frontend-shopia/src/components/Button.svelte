<script>
  import { getContext } from 'svelte';

  let {
    type = 'button',
    disabled = null,
    variant = 'primary',
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
    padding: .4rem .7rem;
    border: none;
    border-radius: .4em;
    cursor: pointer;
    font-size: 1rem;
    transition: background-color 0.3s ease;
    background-color: var(--button-background-color);
    color: white;
  }

  button.icon {
    padding: 0;
    background-color: transparent;
    color: var(--menu-item-color);
  }

  button:hover {
    background-color: var(--button-background-color-highlight);
  }

  button.icon:hover {
    background-color: var(--menu-background-color);
  }

  .danger {
    background-color: #dc3545;
    color: white;
  }

  .danger:hover {
    background-color: #a71d2a;
  }

  button:disabled {
    background-color: #cccccc;
    cursor: not-allowed;
  }
</style>