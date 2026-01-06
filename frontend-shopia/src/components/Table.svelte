<script>
  import { getFormattedValue } from '$libs/formatter.js';
  import Value from './Value.svelte';

  let {
    columns = [],
    data = [],
    getValue = getFormattedValue,
    onChange = null,
    fieldId,
  } = $props();
</script>

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
    {#each data as row (row[fieldId]) }
      <tr>
        {#each columns as column }
          <td
            class={column.className}
          >
            <Value
              data={row}
              options={column}
              getValue={getValue}
              onChange={onChange}
            />
          </td>
        {/each}
      </tr>
    {/each}
  </tbody>
</table>

<style>
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
    font-size: 100%;
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