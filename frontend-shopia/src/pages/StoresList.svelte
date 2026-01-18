<script>
  import List from '$components/List.svelte';
  import * as service from '$services/storeService.js';
  import { selectedMyCommerce } from '$stores/session.js';
</script>

<div class="header">
  {$selectedMyCommerce ? `Commercio: ${$selectedMyCommerce.name}` : 'Selecciona un comercio para ver sus locales'}
</div>
<List
  baseName="store"
  header="Locales"
  getFilters={{
    mine: true,
    commerce: $selectedMyCommerce?.uuid
  }}
  fields={[
    { label: 'Nombre', field: 'name' },
    { label: 'Habilitado', field: 'isEnabled', control: 'switch' },
    { label: 'Descripción', field: 'description' },
  ]}
  {service}
  filters={[ 'includeDisabled' ]}
/>
