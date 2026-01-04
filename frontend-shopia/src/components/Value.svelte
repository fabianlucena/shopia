<script>
  import YesNo from './YesNo.svelte';

  let {
    row,
    column,
    getValue,
  } = $props();
</script>

{#snippet control({ row, column, value })}
  {#if column.control === 'yesNo'}
    <YesNo
      checked={value}
      onChange={value => {
        console.log(`${column.label} changed: ${value? 'Yes' : 'No' }`);
      }}
    />
  {:else}
    <strong>No {column.control} Control</strong>
  {/if}
{/snippet}

{#if column.renderCell}
  {@render column.renderCell({ row, column, value: getValue(row, column) })}
{:else if column.control}
  {@render control({ row, column, value: getValue(row, column) })}
{:else}
  {getValue(row, column)}
{/if}