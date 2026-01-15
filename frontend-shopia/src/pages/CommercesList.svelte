<script>
  import List from '$components/List.svelte';
  import * as service from '$services/commerceService.js';
</script>

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