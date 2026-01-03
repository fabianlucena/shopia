<script>
  import { getContext } from 'svelte';

  let {
    type = 'button',
    disabled = null,
    children,
  } = $props();
  

  let disabledForm = getContext('disabled-form');
  let isDisabled = $derived(disabledForm);
</script>

<button 
  {type}
  disabled={disabled ?? $isDisabled}
>
  {@render children()}
</button>