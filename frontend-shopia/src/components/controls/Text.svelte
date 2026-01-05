<script>
  let {
    value = $bindable(''),
    oninput = null,
    onInput = null,
    onchange = null,
    onChange = null,
    ...props
  } = $props();
</script>

<input
  bind:value
  oninput={(evt) => {
    oninput?.(evt);
    onInput?.(evt);
  }}
  onchange={(evt) => {
    onchange?.(evt);
    onChange?.(evt);
  }}
  {...props}
/>
