<script>
  import List from '$components/List.svelte';
  import * as service from '$services/commerceService.js';
  import { selectedMyCommerce } from '$stores/session.js';
</script>

<div class="header">
  {$selectedMyCommerce ? `Commercio: ${$selectedMyCommerce.name}` : 'Selecciona un comercio para ver sus locales'}
</div>
<List
  baseName="commerce"
  header="Comercios"
  fields={[
    { label: 'Nombre', field: 'name' },
    { label: 'Habilitado', field: 'isEnabled', control: 'switch' },
    { label: 'Descripción', field: 'description' },
  ]}
  {service}
  filters={[ 'includeDisabled' ]}
/>