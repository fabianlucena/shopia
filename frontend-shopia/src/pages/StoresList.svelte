<script>
  import List from '$components/List.svelte';
  import * as service from '$services/storeService.js';
  import MyCommerce from '$components/MyCommerce.svelte';
  import { mySelectedCommerce } from '$stores/session.js';
</script>

<MyCommerce />

<List
  baseName="store"
  header="Locales"
  getFilters={{
    mine: true,
    commerce: $mySelectedCommerce?.uuid,
  }}
  fields={[
    { label: 'Nombre', field: 'name' },
    { label: 'Habilitado', field: 'isEnabled', control: 'switch' },
    { label: 'Descripción', field: 'description' },
  ]}
  {service}
  filters={[ 'includeDisabled' ]}
/>
