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

<div class="cards">
  {#each data as row (row[fieldId])}
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
          <Value
            data={row}
            options={column}
            getValue={getValue}
            onChange={onChange}
          />
        </div>
      {/each}
    </div>
  {/each}
</div>

<style>
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
    margin-bottom: 0.5em;
  }

  .item-actions {
    text-align: right;
    display: flex;
    gap: .5em;
    justify-content: flex-end;
    font-size: 150%;
  }
</style>