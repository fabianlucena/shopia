<script>
  import List from '$components/List.svelte';
  import * as service from '$services/itemService.js';
  import { navigate } from '$libs/router.js';
</script>

<List
  header="Artículos"
  properties={[
    { label: 'Nombre', field: 'name' },
    { label: 'Descripción', field: 'description' },
    { label: 'Categoría', field: 'category.name' },
    { label: 'Precio', field: 'price' },
    { label: 'Cantidad', field: 'stock' },
    { label: 'Regalo', field: 'isPresent', type: 'boolean' },
  ]}
  {service}
  actions={[
    { label: 'Editar', action: row => navigate(`/item/${row.uuid}`) }
  ]}
/>