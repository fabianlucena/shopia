<script>
  import { writable } from 'svelte/store';
  import Table from '$components/Table.svelte';
  import { get } from '$services/itemService.js';
  
  let items = writable([]);
  
  $effect(() => {
    get({ limit: 10 })
      .then(data => items.set(data.rows));
  });
</script>

<Table
  columns={[
    { label: 'Nombre', field: 'name' },
    { label: 'Descripción', field: 'description' },
    { label: 'Categoría', field: 'category.name' },
    { label: 'Precio', field: 'price' },
    { label: 'Cantidad', field: 'stock' },
    { label: 'Regalo', field: 'isPresent', type: 'boolean' },
  ]}
  data={$items}
/>