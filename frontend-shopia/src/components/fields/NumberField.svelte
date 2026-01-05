<script>
  import TextField from '$components/fields/TextField.svelte';
  
  let {
    type = 'number',
    value = $bindable(0),
    onChange = null,
    ...restProps
  } = $props();

  let textValue = $derived('' + value);

  function handleInput(evt) {
    const raw = evt.value;
    const num = Number(raw);

    console.log(evt, num);

    value = isNaN(num) ? null : num;
    textValue = '' + value;

    onChange?.(value);
  }
</script>

<TextField
  {type}
  bind:value={textValue}
  onChange={handleInput}
  {...restProps}
>
</TextField>
