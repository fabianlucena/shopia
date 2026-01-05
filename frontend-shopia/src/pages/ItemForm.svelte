<script>
  import * as itemService from '$services/itemService.js';
  import FormService from '$components/ServiceForm.svelte';
  import * as categoryService from '$services/categoryService.js';
  import * as storeService from '$services/storeService.js';

  let {
    ...restProps
  } = $props();

  function validate(data, fields) {
    const minAgeField = fields.find(f => f.name === 'minAge');
    if (minAgeField)
      minAgeField.visible = !!data.isPresent;

    const maxAgeField = fields.find(f => f.name === 'maxAge');
    if (maxAgeField)
      maxAgeField.visible = !!data.isPresent;
    
    if (!data.name) {
      return 'Debe proporcionar un nombre para el artículo.';
    }
    
    if (!data.description) {
      return 'Debe proporcionar una descripción para el artículo.';
    }
    
    if (!data.categoryUuid) {
      return 'Debe seleccionar un rubro.';
    }

    if (!data.storeUuid) {
      return 'Debe seleccionar un local.';
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

<FormService
  {...restProps}
  header="Artículo"
  service={itemService}
  {validate}
  fields={[
    'isEnabled',
    'name',
    'description',
    //{ name: 'images',       type: 'imageGalery', label: 'Imágenes', deleteFieldName: 'deletedImages' },
    { name: 'categoryUuid', type: 'select',      label: 'Rubro',    service: categoryService },
    { name: 'storeUuid',    type: 'select',      label: 'Local',    service: storeService },
    { name: 'price',        type: 'currency',    label: 'Precio' },
    { name: 'stock',        type: 'number',      label: 'Disponibilidad' },
    { name: 'isPresent',    type: 'switch',      label: 'Apto para regalar' },
    { name: 'minAge',       type: 'number',      label: 'Edad mínima', condition: data => data.isPresent },
    { name: 'maxAge',       type: 'number',      label: 'Edad máxima', condition: data => data.isPresent },
  ]}
/>