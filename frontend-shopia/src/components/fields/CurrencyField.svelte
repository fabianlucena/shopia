<script>
  import NumberField from '$components/fields/NumberField.svelte';
  
  let {
    value = $bindable(0),
    step = .01,
    ...restProps
  } = $props();

  </script>

<NumberField
  bind:value={value}
  {step}
  {...restProps}
>
</NumberField>