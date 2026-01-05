<script>
  import List from '$components/List.svelte';
  import * as service from '$services/itemService.js';
  import { money } from '$libs/formatter.js';
</script>

<List
  baseName="item"
  header="Artículos"
  properties={[
    { label: 'Nombre', field: 'name' },
    { label: 'Habilitado', field: 'isEnabled', control: 'switch' },
    { label: 'Descripción', field: 'description' },
    { label: 'Categoría', field: 'category.name' },
    { label: 'Precio', field: 'price', formatter: money, className: 'money' },
    { label: 'Cantidad', field: 'stock', className: 'number' },
    { label: 'Regalo', field: 'isPresent', control: 'switch' },
  ]}
  {service}
  filters={[ 'includeDisabled' ]}
/>