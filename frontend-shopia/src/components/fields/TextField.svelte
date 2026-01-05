<script>
  import Field from '$components/fields/Field.svelte';
  import Text from '$components/controls/Text.svelte';
  import { getContext } from 'svelte';

  let {
    id = crypto.randomUUID(),
    label = '',
    type = 'text',
    value = $bindable(''),
    disabled = null,
    required = false,
    preControl = null,
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
  {preControl}
  <Text
    type={type}
    {id}
    bind:value
    disabled={disabled ?? $isDisabled}
    {required}
    {...rest}
  />
</Field>