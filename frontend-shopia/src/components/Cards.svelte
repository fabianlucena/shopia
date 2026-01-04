<script>
  import { getFormattedValue } from '$libs/formatter.js';
  import Value from './Value.svelte';

  let {
    header = '',
    globalActions = null,
    columns = [],
    data = [],
    getValue = getFormattedValue,
  } = $props();
</script>

{#if header || globalActions}
  <div class="header">
    {#if header}
      <div class="title">
        {header}
      </div>
    {/if}
    {#if globalActions}
      <div class="actions">
        {@render globalActions() }
      </div>
    {/if}
  </div>
{/if}
<div class="cards">
  {#each data as row}
    <div
      class="card"
    >
      {#each columns as column}
        <div
          class={`field ${column.className || ''}`}
        >
          {#if column.label}
            {column.label}:
          {/if}
          <Value {row} {column} {getValue} />
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
    display: flex;
    flex-direction: row;
  }

  .header .title {
    flex: 1;
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

  .actions {
    text-align: right;
  }
</style>