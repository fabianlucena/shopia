<script>
  import List from '$components/List.svelte';
  import * as service from '$services/itemService.js';
  import { money } from '$libs/formatter.js';
  import ItemCard from '$components/ItemCard.svelte';
  import { selectedMyCommerce } from '$stores/session.js';
</script>

{#snippet cardRender(props)}
  <ItemCard {...props} />
{/snippet}

<div class="header">
  {$selectedMyCommerce ? `Commercio: ${$selectedMyCommerce.name}` : 'Selecciona un comercio para ver sus artículo'}
</div>
<List
  baseName="item"
  header="Artículos"
  getFilters={{
    mine: true,
    commerce: $selectedMyCommerce?.uuid
  }}
  fields={[
    { label: 'Nombre', field: 'name' },
    { label: 'Habilitado', field: 'isEnabled', control: 'switch' },
    { label: 'Imágenes', field: 'images', control: 'imagesView' },
    { label: 'Precio', field: 'price', formatter: money, className: 'money' },
    { label: 'Descripción', field: 'description' },
    { label: 'Categoría', field: 'category.name' },
    { label: 'Locales', field: 'stores', control: 'tagsView', getValue: ({value}) => value.map(store => ({label: store.name, value: store.uuid})) },
    { label: 'Cantidad', field: 'stock', className: 'number' },
    { label: 'Regalo', field: 'isPresent', control: 'switch' },
  ]}
  {service}
  filters={[ 'includeDisabled' ]}
  cardRender={cardRender}
/>