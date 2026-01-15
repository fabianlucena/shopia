<script>
  import List from '$components/List.svelte';
  import * as service from '$services/storeService.js';
</script>

<List
  baseName="store"
  header="Locales"
  fields={[
    { label: 'Nombre', field: 'name' },
    { label: 'Habilitado', field: 'isEnabled', control: 'switch' },
    { label: 'Descripción', field: 'description' },
  ]}
  {service}
  filters={[ 'includeDisabled' ]}
/>