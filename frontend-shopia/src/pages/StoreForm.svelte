<script>
  import ServiceForm from '$components/ServiceForm.svelte';
  import * as storeService from '$services/storeService.js';
  import * as commerceService from '$services/commerceService.js';

  let {
    ...restProps
  } = $props();

  function validate(data, fields) {
    if (!data.name) {
      return 'Debe proporcionar un nombre para el comercio.';
    }
    
    if (!data.description) {
      return 'Debe proporcionar una descripción para el comercio.';
    }
    
    return true;
  }
</script>

<ServiceForm
  {...restProps}
  header="Local"
  service={storeService}
  {validate}
  fields={[
    'isEnabled',
    { name: 'commerceUuid', type: 'select',  label: 'Comercio', required: true, service: commerceService },
    'name',
    '*description',
  ]}
/>