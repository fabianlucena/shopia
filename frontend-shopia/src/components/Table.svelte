<script>
  import { getFormattedValue } from '$libs/formatter.js';
  import Value from './Value.svelte';

  let {
    header = '',
    globalActions = null,
    columns = [],
    data = [],
    getValue = getFormattedValue,
    onChange = null,
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
<table>
  <thead>
    <tr>
      {#each columns as column}
        <th
          class={column.className}
        >
          {column.label}
        </th>
      {/each}
    </tr>
  </thead>
  <tbody>
    {#each data as row}
      <tr>
        {#each columns as column}
          <td
            class={column.className}
          >
            <Value {row} {column} {getValue} {onChange}/>
          </td>
        {/each}
      </tr>
    {/each}
  </tbody>
</table>

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

  table {
    width: 100%;
    border-collapse: collapse;
  }

  tr {
    border-bottom: .1em solid var(--border-color);
    background-color: var(--data-background-color);
  }

  tr:nth-child(even) {
    background-color: var(--alternative-background-color);
  }

  tr:hover {
    background-color: var(--hover-background-color);
  }

  td, th {
    padding: .35em .5em;
    text-align: left;
  }

  th {
    background-color: var(--header-background-color);
    color: var(--header-text-color);
    text-align: left;
  }

  td.actions {
    display: flex;
    gap: .5em;
  }

  th.number {
    text-align: center;
  }

  td.number {
    text-align: right;
  }

  th.money {
    text-align: center;
  }

  td.money {
    text-align: right;
  }

  th.center,
  td.center {
    text-align: center;
  }
</style>