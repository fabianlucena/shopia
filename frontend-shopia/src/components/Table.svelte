<script>
  import { getFormattedValue } from '$libs/formatter.js';

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
            {#if column.renderCell}
              {@render column.renderCell({ row, column, value: getValue(row, column) }) }
            {:else}
              {getValue(row, column)}
            {/if}
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

  td, th {
    border-bottom: .1em solid var(--border-color);
    padding: .15em .5em;
    text-align: left;
    background-color: var(--input-background-color);
  }

  th {
    background-color: var(--header-background-color);
    color: var(--header-text-color);
    text-align: left;
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