<script>
  import { getFormattedValue } from '$libs/formatter.js';

  let {
    header = '',
    columns = [],
    data = [],
    getValue = getFormattedValue,
  } = $props();
</script>

{#if header}
  <div class="header">{header}</div>
{/if}
<div class="cards">
  {#each data as row}
    <div class="card">
      {#each columns as column}
        <div class="field">
          {column.label}:
          {#if column.renderCell}
            {@render column.renderCell({ row, column, value: getValue(row, column) }) }
          {:else}
            {getValue(row, column)}
          {/if}
        </div>
      {/each}
    </div>
  {/each}
</div>

<style>
  .header {
    color: var(--header-text-color);
    background-color: var(--header-background-color);
    margin-top: 0.5em;
    padding: .5em;
    font-size: 1.5em;
    font-weight: bold;
    text-align: center;
  }
  
  .cards {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  }

  .card {
    border: .1em solid var(--border-color);
    border-radius: .5em;
    padding: .2em .5em;
    margin: .15em .35em;
    box-shadow: 2px 2px 6px rgba(0, 0, 0, 0.1);
    background-color: var(--input-background-color);
  }

  .field {
    margin-bottom: 0.5rem;
  }
</style>