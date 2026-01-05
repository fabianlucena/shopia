<script>
  import { writable } from 'svelte/store';
  import Form from './Form.svelte';
  import { pushNotification } from '$libs/notification';
  import TextField from '$components/fields/TextField.svelte';
  import NumberField from '$components/fields/NumberField.svelte';
  import SwitchField from '$components/fields/SwitchField.svelte';
  import CurrencyField from '$components/fields/CurrencyField.svelte';
  import SelectField from '$components/fields/SelectField.svelte';
  import TextAreaField from '$components/fields/TextAreaField.svelte';

  let {
    uuid = null,
    service,
    fields : originalFields = {},
    submitLabel = 'Guardar',
    onSubmit = null,
    cancelable = true,
    validate = null,
    ...restProps
  } = $props();

  let data = writable({});
  let fields = writable([]);

  function prepareFields() {
    if (!Array.isArray(originalFields)) {
      pushNotification('ServiceForm: fields debe ser un array', 'error');
      return;
    }

    const autoNames = {
      isEnabled: 'Habilitado',
      name: 'Nombre',
      description: 'Descripción',
    };

    let preparedFields = [];
    for (let field of originalFields) {
      if (typeof field === 'string') {
        if (field === 'isEnabled') {
          field = {
            name: 'isEnabled',
            type: 'switch',
            label: autoNames[field] || field,
          }
        } else if (field === 'description') {
          field = {
            name: field,
            type: 'textarea',
            label: autoNames[field] || field,
          }
        } else {
          field = {
            name: field,
            label: autoNames[field] || field,
            required: ['name'].includes(field),
          }
        }
      }

      field.name ??= field.field || field.label || '';
      field.type ??= 'text';
      field.label ??= field.name;

      preparedFields.push(field);
      data.update(d => {
        d[field.name] ??= '';
        return d;
      });
    }
    
    fields.set(preparedFields);
  }

  function loadData() {
    if (uuid && uuid !== 'new') {
      service.getSingleForUuid(uuid)
        .then(resData => data.set(resData));
    }
  }
  
  $effect(() => {
    prepareFields();
    loadData();
  });

  function handleSubmit(evt) {
    evt.preventDefault();

    onSubmit?.($data);
    if (!service) {
      if (!onSubmit) {
        pushNotification('ServiceForm: service no está definido', 'error');
      }

      return;
    }

    const dataToSend = {};
    for (let field of $fields) {
      dataToSend[field.name] = $data[field.name];
    }

    service.updateForUuid(uuid, dataToSend)
      .then(() => pushNotification('Datos guardados correctamente', 'success'))
      .catch(err => pushNotification('Error al guardar los datos', 'error'));
  }

</script>

<Form
  {submitLabel}
  {cancelable}
  validate={() => validate?.($data, $fields)}
  onSubmit={handleSubmit}
  {...restProps}
>
  {#each $fields.filter(field => {
      if (field.condition && typeof field.condition === 'function') {
        return field.condition($data);
      }
      return true;
    }) as field}
    {#if field.type === 'text'}
      <TextField
        label={field.label}
        bind:value={$data[field.name]}
        required={field.required}
      />
    {:else if field.type === 'switch'}
      <SwitchField
        label={field.label}
        bind:value={$data[field.name]}
        required={field.required}
      />
    {:else if field.type === 'number'}
      <NumberField
        label={field.label}
        bind:value={$data[field.name]}
        required={field.required}
      />
    {:else if field.type === 'currency'}
      <CurrencyField
        label={field.label}
        bind:value={$data[field.name]}
        required={field.required}
      />
    {:else if field.type === 'select'}
      <SelectField
        label={field.label}
        bind:value={$data[field.name]}
        service={field.service?.getAllForSelect}
        required={field.required}
      />
    {:else if field.type === 'textarea'}
      <TextAreaField
        label={field.label}
        bind:value={$data[field.name]}
        required={field.required}
      />
    {:else}
      <p>Tipo de campo no soportado: {field.type}</p>
    {/if}
  {/each}
</Form>