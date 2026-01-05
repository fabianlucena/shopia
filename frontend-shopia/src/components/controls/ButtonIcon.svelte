<script>
  import Button from '$components/controls/Button.svelte';
  
  let {
    children,
    className = '',
    ...props
  } = $props();
</script>

<Button
  className={`icon ${className}`}
  {...props}
>
  {@render children()}
</Button>