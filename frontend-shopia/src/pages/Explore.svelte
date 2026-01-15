<script>
  import { get } from '$services/itemService.js';
  import Item from '$components/ExploreItem.svelte';

  let items = $state([]);
  
  $effect(() => {
    get({ limit: 10 })
      .then(data => items = data.rows);
  });
</script>

<h1>Artículos</h1>

{#if items.length === 0}
  <p>Cargando...</p>
{:else}
  <div
    class="item-list"
  >
    {#each items as item}
      <Item
        {...item}
      />
    {/each}
  </div>
{/if}

<style>
  .item-list {
    width: 100%;
    padding: 0 .3em;
    box-sizing: border-box;
    display: flex;
    flex-wrap: wrap;
  }
</style>