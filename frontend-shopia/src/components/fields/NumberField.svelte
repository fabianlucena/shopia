<script>
  import TextField from '$components/fields/TextField.svelte';
  
  let {
    type = 'number',
    value = $bindable(0),
    onChange = null,
    ...restProps
  } = $props();

  let textValue = $derived('' + value);

  function handleChange(evt) {
    const raw = evt.target.value;
    const num = Number(raw);

    value = isNaN(num) ? null : num;
    textValue = '' + value;

    onChange?.(evt);
  }
</script>

<TextField
  {type}
  bind:value={textValue}
  onChange={handleChange}
  {...restProps}
>
</TextField>
