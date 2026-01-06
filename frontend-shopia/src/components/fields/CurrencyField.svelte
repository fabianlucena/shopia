<script>
  import TextField from '$components/fields/TextField.svelte';
  import { writable } from 'svelte/store';
  
  let {
    value = $bindable('0'),
    onChange = null,
    pattern = "^[\\d.]+(,\\d{1,2})?$",
    ...restProps
  } = $props();

  let textValue = writable();

  $effect(() => {
    const newTextValue = new Intl.NumberFormat("es-AR", {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    }).format(Number(value));

    if (newTextValue !== $textValue) {
      textValue.set(newTextValue);
    }
  });

  function handleChange(evt) {
    const raw = evt.target.value;

    value = raw
      .replace('.', '')
      .replace(',', '.');

    onChange?.(evt);
  }

</script>

<TextField
  {pattern}
  value={$textValue}
  onChange={handleChange}
  preControl="$"
  {...restProps}
>
</TextField>