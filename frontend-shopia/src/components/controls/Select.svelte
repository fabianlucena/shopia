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
    ...rest
  } = $props();

  let disabledForm = getContext('disabled-form');
  let isDisabled = $derived(disabledForm);

  // @ts-check
  let options = writable(originalOptions);
  $effect(() => {
    if (service) {
      console.log(service);
      service()
        .then(opt => {
          options.set([...originalOptions, ...opt]);
        });
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
    <option value={option.value}>{option.label}</option>
  {/each}
</select>