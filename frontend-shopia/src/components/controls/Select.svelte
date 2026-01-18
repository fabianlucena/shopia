<script>
  import { getContext } from 'svelte';
  import { writable } from 'svelte/store';

  let {
    id = crypto.randomUUID(),
    value = $bindable(''),
    options : originalOptions = [],
    service = null,
    disabled = null,
    required = false,
    multiple = false,
    placeholder = null,
    ...rest
  } = $props();

  let disabledForm = getContext('disabled-form');
  let isDisabled = $derived(disabledForm);

  // @ts-check
  let options = writable([]);
  $effect(() => {
    if (!service) {
      setOptions(originalOptions);
      return;
    }

    service()
      .then(opt => setOptions([...originalOptions, ...opt]));
  });

  function setOptions(newOptions) {
    if (placeholder) {
      const placeholderOption = {
        label: placeholder,
        value: '',
        disabled: true,
        hidden: true,
      };

      if (!value) {
        placeholderOption.selected = true;
      }

      newOptions = [placeholderOption, ...newOptions];
    }

    options.set(newOptions);
  }

  $effect(() => {
    if (placeholder && !value) {
      value = '';
    }
  });
</script>

<select
  {id}
  bind:value
  disabled={disabled ?? $isDisabled}
  {required}
  {...rest}
>
  {#each $options as option (option.value)}
    <option value={option.value} disabled={option.disabled} hidden={option.hidden} selected={option.selected} >{option.label}</option>
  {/each}
</select>
