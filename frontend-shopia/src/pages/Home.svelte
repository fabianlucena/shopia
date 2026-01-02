<script>
  import { get } from '$services/itemService.js';
  import Item from '$components/Item.svelte';

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
    display: flex;
    flex-wrap: wrap;
    gap: .25em;
    padding: 0 .3em;
    justify-content: center;
    align-content: flex-start;
    box-sizing: border-box;
  }
</style>