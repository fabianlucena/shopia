<script>
  import List from '$components/List.svelte';
    import MyCommerce from '$components/MyCommerce.svelte';
  import * as service from '$services/commerceService.js';
</script>

<MyCommerce />

<List
  baseName="commerce"
  header="Comercios"
  getFilters={{
    mine: true,
  }}
  fields={[
    { label: 'Nombre', field: 'name' },
    { label: 'Habilitado', field: 'isEnabled', control: 'switch' },
    { label: 'Descripción', field: 'description' },
  ]}
  {service}
  filters={[ 'includeDisabled' ]}
/>