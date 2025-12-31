<script>
  let {
    type = 'button',
    children,
  } = $props();
</script>

<button 
  type={type}
>
  {@render children()}
</button>

<style>
</style>