<script>
  import * as commerceService from '$services/commerceService.js';
  import ServiceForm from '$components/ServiceForm.svelte';
  import MyCommerce from '$components/MyCommerce.svelte';

  let {
    ...restProps
  } = $props();

  function validate({ data }) {
    if (!data.name) {
      return 'Debe proporcionar un nombre para el comercio.';
    }
    
    if (!data.description) {
      return 'Debe proporcionar una descripción para el comercio.';
    }
    
    return true;
  }
</script>

<MyCommerce />

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