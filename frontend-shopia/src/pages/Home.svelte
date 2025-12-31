<script>
  import { get } from '../services/itemService.js';

  let items = $state([]);
  
  $effect(() => {
    get({ limit: 10 })
      .then(data => items = data.rows);
  });
</script>

<h1>Inicio</h1>

{#if items.length === 0}
  <p>Cargando...</p>
{:else}
  <ul>
    {#each items as item}
      <li>{item.name}</li>
    {/each}
  </ul>
{/if}

