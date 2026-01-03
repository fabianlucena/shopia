<script>
  import { getValue } from '$libs/object.js';

  let {
    columns = [],
    data = [],
    renderCell = thisRenderCell,
  } = $props();

  function thisRenderCell(row, column) {
    if (column.type === 'boolean') {
      return row[column.field] ? 'Sí' : 'No';
    }
    
    return getValue(row, column.field);
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
          <td>{renderCell(row, column)}</td>
        {/each}
      </tr>
    {/each}
  </tbody>
</table>

<style >
</style>