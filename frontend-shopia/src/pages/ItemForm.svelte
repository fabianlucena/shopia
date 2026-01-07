<script>
  import * as itemService from '$services/itemService.js';
  import ServiceForm from '$components/ServiceForm.svelte';
  import * as categoryService from '$services/categoryService.js';
  import * as storeService from '$services/storeService.js';

  let {
    ...restProps
  } = $props();

  function validate(data, fields) {
    if (!data.name) {
      return 'Debe proporcionar un nombre para el artículo.';
    }
    
    if (!data.description) {
      return 'Debe proporcionar una descripción para el artículo.';
    }
    
    if (!data.categoryUuid) {
      return 'Debe seleccionar un rubro.';
    }

    if (!data.storesUuid) {
      return 'Debe seleccionar al menos un local.';
    }

    if (!data.price) {
      return 'Debe colocar un precio para el artículo.';
    }

    if (!data.stock) {
      return 'Debe colocar la cantidad de artículos disponibles.';
    }
    
    if (data.minAge && data.maxAge && parseFloat(data.minAge) > parseFloat(data.maxAge))
      return 'La edad mínima no puede ser mayor a la edad máxima.';
    
    return true;
  }
</script>

<ServiceForm
  {...restProps}
  header="Artículo"
  service={itemService}
  {validate}
  fields={[
    'isEnabled',
    'name',
    '*description',
    //{ name: 'images',       type: 'imageGalery', label: 'Imágenes', deleteFieldName: 'deletedImages' },
    { name: 'categoryUuid', type: 'select',      label: 'Rubro',            required: true, service: categoryService },
    { name: 'storesUuid',   type: 'multiSelect', label: 'Locales',          required: true, service: storeService },
    { name: 'price',        type: 'currency',    label: 'Precio',           required: true },
    { name: 'stock',        type: 'number',      label: 'Disponibilidad',   required: true },
    { name: 'isPresent',    type: 'switch',      label: 'Apto para regalar' },
    { name: 'minAge',       type: 'number',      label: 'Edad mínima',      required: true, condition: data => data.isPresent },
    { name: 'maxAge',       type: 'number',      label: 'Edad máxima',      required: true, condition: data => data.isPresent },
  ]}
/>