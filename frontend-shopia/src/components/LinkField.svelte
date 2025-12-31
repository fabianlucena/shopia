<script>
  let {
    label = '',
    href = '#',
    children,
    ...rest
  } = $props();
</script>

<a
  {href}
  {...rest}
>
  {label}
  {@render children?.()}
</a>

<style>
  a {
    align-self: center;
  }
</style>