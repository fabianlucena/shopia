<script>
  import Switch from '$components/controls/Switch.svelte';
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
  class="switch-field"
>
  <Switch
    {id}
    bind:value
    disabled={disabled ?? $isDisabled}
    {required}
    {...props}
  />
</Field>

<style>
  :global(.field.switch-field) {
    justify-content: flex-start;
    display: flex;
    flex-direction: row;
    gap: .5em;
    align-items: end;
  }

  :global(.switch-field .field-control) {
    width: auto;
  }
</style>