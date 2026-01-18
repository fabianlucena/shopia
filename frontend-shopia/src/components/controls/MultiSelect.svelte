<script>
  import { getContext } from 'svelte';
  import { writable } from 'svelte/store';

  let {
    id = crypto.randomUUID(),
    name = null,
    value = $bindable([]),
    options : originalOptions = [],
    service = null,
    disabled = null,
    ...restProps
  } = $props();

  let disabledForm = getContext('disabled-form');
  let isDisabled = $derived(() => disabled ?? $disabledForm);
  let _value = value;

  // @ts-ignore
  let options = writable([]);
  $effect.pre(() => {
    //name ??= id;

    if (!Array.isArray(_value) && typeof _value === 'string') {
      // @ts-ignore
      _value = _value.split(',');
      value = _value.filter(v => v.trim() !== '');
    }

    if (!service) {
      options.set(originalOptions.map(o => {
        o.id ??= crypto.randomUUID();
        return o;
      }));
      return;
    }

    service()
      .then(opt => {
        const newOptions = [...originalOptions, ...opt]
          .map(o => {
            o.id ??= crypto.randomUUID();
            return o;
          });
        options.set(newOptions);
      });
  });

  function handleChange(evt) {
    const optionValue = evt.target.value;
    if (value.includes(optionValue)) {
      value = value.filter(v => v !== optionValue);
    } else {
      value = [...value, optionValue].sort((a, b) => {
        if (a < b) return -1;
        if (a > b) return 1;
        return 0;
      });
    }
  }
</script>

<div
  {id}
  {...restProps}
>
  {#each $options as option (option.value)}
    <div
      data-value={option.value}
      data-selected={value.includes(option.value)}
    >
      <input
        id={option.id}
        group={name}
        value={option.value}
        type="checkbox"
        checked={value.includes(option.value)}
        disabled={isDisabled()}
        onchange={handleChange}
      />
      <label for={option.id}>{option.label}</label>
    </div>
  {/each}
</div>