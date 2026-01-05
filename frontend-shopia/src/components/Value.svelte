<script>
  import Switch from './controls/Switch.svelte';

  let {
    row,
    column,
    getValue,
    onChange = null,
  } = $props();
</script>

{#snippet control({ row, column, value })}
  {#if column.control === 'switch'}
    <Switch
      {value}
      onChange={value => {
        onChange?.({
          row,
          column,
          value,
        });
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