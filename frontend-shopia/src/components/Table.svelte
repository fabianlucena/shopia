<script>
  import { getValue } from '$libs/object.js';

  let {
    columns = [],
    data = [],
    getCellValue = thisGetCellValue,
  } = $props();

  function thisGetCellValue(row, column) {
    if (column.getCellValue) {
      return column.getCellValue(row);
    }

    if (column.type === 'boolean') {
      return row[column.field] ? 'Sí' : 'No';
    }
    
    if (column.field) {
      return getValue(row, column.field);
    }
  }
</script>

<table>
  <thead>
    <tr>
      {#each columns as column}
        <th>{column.label}</th>
      {/each}
    </tr>
  </thead>
  <tbody>
    {#each data as row}
      <tr>
        {#each columns as column}
          <td>
            {#if column.renderCell}
              {@render column.renderCell({ row, column, value: thisGetCellValue(row, column) }) }
            {:else}
              {thisGetCellValue(row, column)}
            {/if}
          </td>
        {/each}
      </tr>
    {/each}
  </tbody>
</table>

<style >
</style>