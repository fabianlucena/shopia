<script>
  import Button from '$components/Button.svelte';
  
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

<style>
</style>