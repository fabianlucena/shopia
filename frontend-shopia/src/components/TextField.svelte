<script>
  import Field from '$components/Field.svelte';
  import { getContext } from 'svelte';

  let {
    label = '',
    type = 'text',
    value = $bindable(''),
    disabled = null,
    id = crypto.randomUUID(),
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
  <input
    type={type}
    {id}
    bind:value
    disabled={disabled ?? $isDisabled}
    {required}
    {...rest}
  />
</Field>