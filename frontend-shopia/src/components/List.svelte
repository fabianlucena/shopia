<script>
  import { writable } from 'svelte/store';
  import Table from '$components/Table.svelte';
  import Cards from '$components/Cards.svelte';
  import Button from '$components/controls/Button.svelte';
  import AddButton from '$components/buttons/Add.svelte';
  import EditButton from '$components/buttons/Edit.svelte';
  import DeleteButton from '$components/buttons/Delete.svelte';
  import Switch from '$components/controls/Switch.svelte';
  import { confirm } from '$libs/confirm';
  import { permissions } from '$stores/session';
  import { navigate } from '$libs/router.js';
  import { pushNotification } from '$libs/notification.js';

  let {
    baseName = '',
    header = '',
    service = null,
    actions : originalActions = [
      'add',
      'edit',
      'delete',
    ],
    fields = [],
    filters : originalFilters = [],
    cardRender = null,
    getFilters = null,
    ...props
  } = $props();

  let container;
  let width = writable(0);

  const actions = writable([]);
  function updateActions() {
    const newActions = originalActions.map(action => {
        if (typeof action === 'string') {
          if (action === 'add') {
            return {
              global: true,
              label: 'Agregar',
              title: 'Agregar nuevo elemento',
              component: addButton,
              permission: `${baseName}.add`,
              action: () => navigate(`/${baseName}-form/new`),
            };
          }

          if (action === 'edit') {
            return {
              label: 'Editar',
              title: 'Editar elemento',
              component: editButton,
              permission: `${baseName}.edit`,
              action: ({ data }) => navigate(`/${baseName}-form/${data.uuid}`),
            };
          }
          
          if (action === 'delete') {
            return {
              label: 'Eliminar',
              title: 'Eliminar elemento',
              component: deleteButton,
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
  }

  const filters = writable([]);
  function updateFilters() {
    const newFilters = originalFilters.map(filter => {
        if (typeof filter === 'string') {
          if (filter === 'includeDisabled') {
            return {
              id: crypto.randomUUID(),
              name: 'includeDisabled',
              label: 'Ver deshabilitados',
              title: 'Incluir elementos deshabilitados',
              value: true,
              component: switchControl,
              onChange: value => {
                let update = false;
                filters.update(fs => {
                  const f = fs.find(f => f.name === 'includeDisabled');
                  if (f?.value !== value) {
                    update = true;
                    f.value = value;
                  }
                  return fs;
                });

                update && updateData();
              }
            };
          }
        }
        return filter;
      });

    filters.set(newFilters);
  }

  const dFilters = $derived($filters);
  const data = writable([]);
  function updateData() {
    if (!service) {
      data.set(props.data);
      return;
    }

    var query = { ...getFilters, limit: 10 };
    dFilters.forEach(filter => {
      if (filter.name && filter.value !== undefined) {
        query[filter.name] = filter.value;
      }
    });

    service?.get(query)
      .then(res => {
        data.set([...props?.data ?? [], ...res.rows]);
      });
  }

  $effect(() => {
    updateActions();
    updateFilters();
    updateData();

    const observer = new ResizeObserver(entries =>
      width.set(entries[0].contentRect.width));
    observer.observe(container);
    return () => observer.disconnect();
  });

  async function onChange({ row, column, value }) {
    const index = $data.findIndex(r => r.uuid === row.uuid);
    if (index < 0)
      return;

    data.update(d => {
      d[index] = { ...$data[index], [column.field]: value };
      return d;
    });

    if (!service.updateForUuid) {
      pushNotification('Error no hay método para actualizar el elemento.', 'error');
    } else if (!$permissions.includes(`${baseName}.edit`)) {
      pushNotification('Error no tiene permiso para actualizar el elemento.', 'error');
    } else {
      try {
        await service.updateForUuid(row.uuid, { [column.field]: value });
        pushNotification('Elemento actualizado correctamente.', 'success');
      } catch {
        pushNotification('Error al actualizar el elemento.', 'error');
      }
    }

    updateData();
  }
</script>

{#snippet confirmDeleteBody()}
  <p>¿Estás seguro de que deseas eliminar este elemento?</p>
  <p>Esta acción no se puede deshacer.</p>
{/snippet}

{#snippet addButton(props)}
  <AddButton {...props} />
{/snippet}

{#snippet editButton(props)}
  <EditButton {...props} />
{/snippet}

{#snippet deleteButton(props)}
  <DeleteButton {...props} />
{/snippet}

{#snippet renderActions(props)}
  {#each $actions.filter(a => !a.global) as action}
    {#if action.component}
      {@render action.component({
        title: action.title || action.label,
        onclick: () => action.action(props)
      })}
    {:else}
      <Button onclick={() => action.action(props)}>{action.label}</Button>
    {/if}
  {/each}
{/snippet}

{#snippet globalActions()}
  {#each $actions.filter(a => a.global) as action}
    {#if action.component}
      {@render action.component({
        title: action.title || action.label,
        onclick: () => action.action(),
      })}
    {:else}
      <Button onclick={() => action.action()}>{action.label}</Button>
    {/if}
  {/each}
{/snippet}

{#snippet switchControl(props)}
  <div class="filter">
    <label for={props.id}>{props.label}: </label>
    <Switch value={props.value} {...props} />
  </div>
{/snippet}

<div
  bind:this={container}
  class="container"
>
  {#if header || globalActions }
    <div class="header">
      {#if header}
        <div class="title">
          {header}
        </div>
      {/if}
      {#if globalActions}
        <div class="actions global-actions">
          {@render globalActions() }
        </div>
      {/if}
    </div>
  {/if}
  {#if $filters.length > 0}
    <div class="filters">
      {#each $filters as filter}
        {#if filter.component}
          {@render filter.component(filter)}
        {:else}
          Control desconocido
        {/if}
      {/each}
    </div>
  {/if}
  {#if $width < 800}
    <Cards
      fields={[
        ...fields,
        {
          type: 'actions',
          render: renderActions,
          className: 'actions item-actions',
        },
      ]}
      fieldId="uuid"
      data={$data}
      onChange={onChange}
      cardRender={cardRender}
    />
  {:else}  
    <Table
      columns={[
        ...fields,
        {
          type: 'actions',
          label: 'Acciones',
          render: renderActions,
          className: 'actions item-actions',
        },
      ]}
      fieldId="uuid"
      data={$data}
      onChange={onChange}
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

  .header .title {
    flex: 1;
  }

  .filters {
    display: flex;
    flex-direction: row;
    gap: 1em;
    padding: 0.2em;
    justify-content: center;
    flex-wrap: wrap;
  }

  .filter {
    text-align: center;
    border: .1em solid var(--border-color);
    padding: .5em;
    border-radius: .5em;
  }
</style>