<script>
  import Field from '$components/fields/Field.svelte';
  import Select from '$components/controls/Select.svelte';
  import { getContext } from 'svelte';

  let {
    id = crypto.randomUUID(),
    label = '',
    value = $bindable(''),
    disabled = null,
    required = false,
    ...rest
  } = $props();

  let disabledForm = getContext('disabled-form');
  let isDisabled = $derived(disabledForm);
</script>

<Field
  for={id}
  {label}
  {required}
>
  <Select
    {id}
    bind:value
    disabled={disabled ?? $isDisabled}
    {required}
    {...rest}
  />
</Field>