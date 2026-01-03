<script>
  import { writable } from 'svelte/store';
  import Table from '$components/Table.svelte';
  import Cards from '$components/Cards.svelte';
  import Button from '$components/Button.svelte';
  import { confirm } from '$libs/confirm';
  import { permissions } from '$stores/session';
  import { navigate } from '$libs/router.js';

  let {
    baseName = '',
    header = '',
    service = null,
    actions : originalActions = [],
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

  const actions = writable([]);
  $effect(() => {
    const newActions = originalActions.map(action => {
        if (typeof action === 'string') {
          if (action === 'edit') {
            return {
              label: 'Editar',
              permission: `${baseName}.edit`,
              action: row => navigate(`/${baseName}/${row.uuid}`)
            };
          }
          if (action === 'delete') {
            return {
              label: 'Eliminar',
              permission: `${baseName}.delete`,
              action: row => {
                confirm({
                  title: 'Confirmar eliminación',
                  body: confirmDeleteBody,
                  confirmText: 'Eliminar',
                  onConfirm: () => {
                    service.deleteForUuid(row.uuid)
                      .then(() => {
                        service.get({ limit: 10 })
                          .then(res => data.set(res.rows));
                      });
                  },
                });
              }
            };
          }
        }
        return action;
      })
      .filter(action => !action.permission || $permissions.includes(action.permission));

    actions.set(newActions);
  });
</script>

{#snippet confirmDeleteBody()}
  <p>¿Estás seguro de que deseas eliminar este elemento?</p>
  <p>Esta acción no se puede deshacer.</p>
{/snippet}

{#snippet actionsCell({ row })}
  {#each $actions as action}
    <Button onclick={() => action.action(row)}>{action.label}</Button>
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