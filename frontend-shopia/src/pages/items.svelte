<script>
  import List from '$components/List.svelte';
  import * as service from '$services/itemService.js';
  import { navigate } from '$libs/router.js';
  import { money, yesNo } from '$libs/formatter.js';
</script>

<List
  baseName="item"
  header="Artículos"
  properties={[
    { label: 'Nombre', field: 'name' },
    { label: 'Descripción', field: 'description' },
    { label: 'Categoría', field: 'category.name' },
    { label: 'Precio', field: 'price', formatter: money, className: 'money' },
    { label: 'Cantidad', field: 'stock', className: 'number' },
    { label: 'Regalo', field: 'isPresent', formatter: yesNo, className: 'center' },
  ]}
  {service}
  actions={[
    'edit',
    'delete',
  ]}
/>