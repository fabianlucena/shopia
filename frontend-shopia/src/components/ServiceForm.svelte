<script>
  // @ts-nocheck

  import { writable, get } from 'svelte/store';
  import Form from './Form.svelte';
  import { pushNotification } from '$libs/notification';
  import TextField from '$components/fields/TextField.svelte';
  import NumberField from '$components/fields/NumberField.svelte';
  import SwitchField from '$components/fields/SwitchField.svelte';
  import CurrencyField from '$components/fields/CurrencyField.svelte';
  import SelectField from '$components/fields/SelectField.svelte';
  import MultiSelectField from '$components/fields/MultiSelectField.svelte';
  import TextAreaField from '$components/fields/TextAreaField.svelte';
  import ImageGalleryField from '$components/fields/ImageGalleryField.svelte';
  import { navigate } from '$libs/router';
  import { getDataForSend } from '$libs/fields';

  let {
    noUuid = false,
    uuid = null,
    service,
    serviceOptions = {},
    fields : originalFields = {},
    data : originalData = {},
    submitLabel = 'Guardar',
    prepareData = null,
    onSubmit = null,
    afterSubmit = null,
    cancelable = true,
    validate = null,
    ...restProps
  } = $props();

  // @ts-nocheck
  let _init = false;
  let _uuid;
  let defaultData = {};
  let data = writable({});
  let fields = writable([]);
  
  function prepareFields() {
    if (!Array.isArray(originalFields)) {
      pushNotification('ServiceForm: fields debe ser un array', 'error');
      return;
    }

    const autoLabel = {
      isEnabled: 'Habilitado',
      name: 'Nombre',
      description: 'Descripción',
    };

    let preparedFields = [];
    for (let field of originalFields) {
      if (typeof field === 'string') {
        var name = field;
        var required = false;
        if (field.startsWith('*')) {
          name = field.substring(1);
          required = true;
        }

        if (name === 'isEnabled') {
          field = {
            type: 'switch',
            value: true,
          }
        } else if (name === 'description') {
          field = {
            type: 'textarea',
            required,
          }
        } else {
          field = {};
        }

        field.name ??= name;
        field.label ??= autoLabel[name] || name;
        field.required ??= required || ['name'].includes(name);
      }

      field.name ??= field.field || field.label || '';
      field.type ??= 'text';
      field.label ??= field.name;

      preparedFields.push(field);
      if (field.name) {
        data.update(d => {
          d[field.name] = getDefaultValueForField(field);
          return d;
        });
      }
    }
    
    fields.set(preparedFields);
  }

  function getDefaultValueForField(field) {
    if (typeof defaultData[field.name] !== 'undefined')
      return defaultData[field.name]

    if (typeof field.value !== 'undefined')
      return field.value;

    switch (field.type) {
      case 'currency':
      case 'number':
        return 0;

      case 'switch':
        return false;

      case 'multiSelect':
        return [];
    }

    return '';
  }

  function loadData() {
    if (!_init) {  
      _init = true;
      data.update(d => ({ ...d, ...originalData }));
      defaultData = JSON.parse(JSON.stringify($data));
    }

    let options;
    if (typeof serviceOptions === 'function') {
      options = serviceOptions({ uuid, data: $data });
    } else {
      options = serviceOptions || {};
    }

    if (noUuid) {
      service.getSingle(options)
        .then(resData => {
          data.update(d => ({ ...d, ...resData }));
          defaultData = JSON.parse(JSON.stringify($data));
        });
      
      return;
    }

    if (uuid && uuid !== 'new' && _uuid !== uuid) {
      _uuid = uuid;
      service.getSingleForUuid(uuid, options)
        .then(resData => {
          data.update(d => ({ ...d, ...resData }));
          defaultData = JSON.parse(JSON.stringify($data));
        });
    }
  }
  
  $effect(() => {
    prepareFields();
    loadData();
  });

  async function handleSubmit(evt) {
    evt.preventDefault();

    let sendData = getDataForSend({
      fields: $fields,
      data: $data,
      defaultData: !uuid || uuid === 'new' ? {} : defaultData,
    });

    let isEmpty;
    if (sendData instanceof FormData)
      isEmpty = sendData.entries().next().done;
    else
      isEmpty = Object.keys(sendData).length === 0;

    if (isEmpty) {
      pushNotification('Nada para modificar', 'info');
      return;
    }

    if (!service) {
      if (!onSubmit) {
        pushNotification('ServiceForm: service no está definido', 'error');
      }

      return;
    }

    prepareData?.({uuid, data: sendData});

    onSubmit?.({uuid, data: sendData});

    try {
      if (!uuid || uuid === 'new')
        await service.add(sendData);
      else
        await service.updateForUuid(uuid, sendData)

      pushNotification('Datos guardados correctamente', 'success');
      navigate(-1);
    } catch (err) {
      const message = err.response?.data?.message
        || err.message
        || 'Error al guardar los datos';

      pushNotification(message, 'error');
    }

    afterSubmit?.();
  }

  let modified = $state(false);
  $effect(() => {
    let newValue = false;
    for (let key in $data) {
      if (JSON.stringify($data[key]) !== JSON.stringify(defaultData[key])) {
        newValue = true;
      }
    }

    modified = newValue;
  });

</script>

<Form
  {submitLabel}
  {cancelable}
  validate={() => validate?.({ uuid, data: $data, fields: $fields })}
  onSubmit={handleSubmit}
  canSubmit={modified}
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
        bind:value={$data[field.name]}
        {...field}
      />
    {:else if field.type === 'switch'}
      <SwitchField
        bind:value={$data[field.name]}
        {...field}
      />
    {:else if field.type === 'number'}
      <NumberField
        bind:value={$data[field.name]}
        {...field}
      />
    {:else if field.type === 'currency'}
      <CurrencyField
        bind:value={$data[field.name]}
        {...field}
      />
    {:else if field.type === 'select'}
      <SelectField
        bind:value={$data[field.name]}
        {...{
          ...field,
          service: field.service?.getAllForSelect,
        }}
      />
    {:else if field.type === 'multiSelect'}
      <MultiSelectField
        bind:value={$data[field.name]}
        {...{
          ...field,
          service: field.service?.getAllForSelect,
        }}
      />
    {:else if field.type === 'textarea'}
      <TextAreaField
        bind:value={$data[field.name]}
        {...field}
      />
    {:else if field.type === 'imageGallery'}
      <ImageGalleryField
        bind:value={$data[field.name]}
        {...field}
      />
    {:else}
      <p>Tipo de campo no soportado: {field.type}</p>
    {/if}
  {/each}
</Form>