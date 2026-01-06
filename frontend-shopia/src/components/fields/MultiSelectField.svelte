<script>
  import Field from '$components/fields/Field.svelte';
  import MultiSelect from '$components/controls/MultiSelect.svelte';
  import { getContext } from 'svelte';

  let {
    id = crypto.randomUUID(),
    label = '',
    value = $bindable([]),
    required = false,
    ...restProps
  } = $props();
</script>

<Field
  for={id}
  {label}
  {required}
>
  <MultiSelect
    {id}
    bind:value
    {required}
    {...restProps}
  />
</Field>