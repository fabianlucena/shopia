<script>
  import ServiceForm from '$components/ServiceForm.svelte';
  import * as storeService from '$services/storeService.js';
  import MyCommerce from '$components/MyCommerce.svelte';
  import { mySelectedCommerceUuid } from '$stores/session';

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
  header="Local"
  service={storeService}
  prepareData={({data, uuid}) => {
    if (!uuid || uuid === 'new')
      data.commerceUuid = $mySelectedCommerceUuid;
  }}
  {validate}
  fields={[
    'isEnabled',
    'name',
    '*description',
  ]}
/>