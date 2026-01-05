<script>
  import YesNo from '$components/controls/YesNo.svelte';
  import Field from '$components/fields/Field.svelte';
  import { getContext } from 'svelte';

  let {
    id = crypto.randomUUID(),
    label = '',
    value = $bindable(false),
    disabled = null,
    required = false,
    ...props
  } = $props();

  let disabledForm = getContext('disabled-form');
  let isDisabled = $derived(disabledForm);
</script>

<Field
  for={id}
  {label}
  {required}
>
  <YesNo
    {id}
    bind:value
    disabled={disabled ?? $isDisabled}
    {required}
    {...props}
  />
</Field>
