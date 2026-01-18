<script>
  import ServiceForm from '$components/ServiceForm.svelte';
  import * as storeService from '$services/storeService.js';
  import MyCommerce from '$components/MyCommerce.svelte';
  import { myCommerces } from '$stores/session';

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

<MyCommerce />

<ServiceForm
  {...restProps}
  header="Local"
  service={storeService}
  {validate}
  fields={[
    'isEnabled',
    { name: 'commerceUuid', type: 'select',  label: 'Comercio', required: true, options: $myCommerces?.map(c => ({ label: c.name, value: c.uuid })) },
    'name',
    '*description',
  ]}
/>