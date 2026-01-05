<script>
  import * as commerceService from '$services/commerceService.js';
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

<ServiceForm
  {...restProps}
  header="Comercio"
  service={commerceService}
  {validate}
  fields={[
    'isEnabled',
    'name',
    'description',
  ]}
/>