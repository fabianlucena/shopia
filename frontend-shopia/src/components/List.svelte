<script>
  import { writable } from 'svelte/store';
  import Table from '$components/Table.svelte';
  import Cards from '$components/Cards.svelte';

  let {
    header = '',
    service = null,
    actions = [],
    properties = [],
    ...props
  } = $props();

  let container;
  let width = writable(0);
  const data = writable([]);

  $effect(() => {
    data.set(props.data);
    service?.get({ limit: 10 })
      .then(res => data.set(res.rows));

    const observer = new ResizeObserver(entries =>
      width.set(entries[0].contentRect.width));
    observer.observe(container);
    return () => observer.disconnect();
  });
</script>

{#snippet actionsCell({ row })}
  {#each actions as action}
    <button onclick={() => action.action(row)}>{action.label}</button>
  {/each}
{/snippet}

<div
  bind:this={container}
  class="container"
>
  {#if $width < 800}
    <Cards
      {header}
      columns={[
        ...properties,
        { label: 'Acciones', renderCell: actionsCell }
      ]}
      data={$data}
    />
  {:else}  
    <Table
      {header}
      columns={[
        ...properties,
        { label: 'Acciones', renderCell: actionsCell }
      ]}
      data={$data}
    />
  {/if}
</div>

<style>
  .container {
    width: 100%;
    flex: 1;
    display: flex;
    flex-direction: column;
  }
</style>