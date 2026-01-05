<script>
  import * as storeService from '$services/storeService.js';
  import ServiceForm from '$components/ServiceForm.svelte';;

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
    'name',
    'description',
  ]}
/>