<script>
  import { writable } from 'svelte/store';
  import Table from '$components/Table.svelte';
  import { get } from '$services/itemService.js';
  import { navigate } from '$libs/router.js';
  
  let items = writable([]);
  
  $effect(() => {
    get({ limit: 10 })
      .then(data => items.set(data.rows));
  });
</script>

{#snippet actionsCell({ row })}
  <button onclick={() => navigate(`/item/${row.uuid}`)}>Editar</button>
{/snippet}

<Table
  columns={[
    { label: 'Nombre', field: 'name' },
    { label: 'Descripción', field: 'description' },
    { label: 'Categoría', field: 'category.name' },
    { label: 'Precio', field: 'price' },
    { label: 'Cantidad', field: 'stock' },
    { label: 'Regalo', field: 'isPresent', type: 'boolean' },
    { label: 'Acciones', renderCell: actionsCell },
  ]}
  data={$items}
/>