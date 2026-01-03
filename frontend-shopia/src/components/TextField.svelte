<script>
  import Field from '$components/Field.svelte';
  import { getContext } from 'svelte';

  let {
    label = '',
    type = 'text',
    value = $bindable(''),
    disabled = null,
    id = crypto.randomUUID(),
    ...rest
  } = $props();

  let disabledForm = getContext('disabled-form');
  let isDisabled = $derived(disabledForm);
</script>

<Field
  for={id}
  {label}
>
  <input
    type={type}
    {id}
    bind:value
    disabled={disabled ?? $isDisabled}
    {...rest}
  />
</Field>