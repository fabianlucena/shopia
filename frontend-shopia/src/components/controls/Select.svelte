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
    ...rest
  } = $props();

  let disabledForm = getContext('disabled-form');
  let isDisabled = $derived(disabledForm);
  let _originalOptions = originalOptions || [];
  let _service = service;

  // @ts-check
  let options = writable([]);
  $effect(() => {
    if (!_service) {
      options.set(_originalOptions);
      return;
    }

    _service()
      .then(opt => {
        options.set([..._originalOptions, ...opt]);
      });
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
    <option value={option.value}>{option.label}</option>
  {/each}
</select>
