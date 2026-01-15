<script>
  import { writable } from 'svelte/store';
  import ImagesView from '$components/controls/ImagesView.svelte';
  import { money } from '$libs/formatter.js';
  import { getFormattedValue, yesNo } from '$libs/formatter.js';
  import Value from '$components/Value.svelte';

  let {
    fields,
    data,
    getValue = getFormattedValue,
    onChange = null,
  } = $props();

  let actions = writable(null);
  $effect(() => {
    actions.set(fields.find(field => field.type === 'actions'));
  });
</script>

<div
  class="item"
>
  <div class="name">{data.name}</div>
  <div class="category">Rubro: {data.category.name}</div>
  <div class="stores">
    {#each data.stores as store (store.uuid)}
      <span class="store">{store.name}</span>
    {/each}
  </div>
  {#if data.images && data.images.length > 0}
    <ImagesView
      bind:value={data.images}
    />
  {/if}
  <div class="is-enabled">
    Habilitado:
    <Value
      data={data}
      options={fields.find(field => field.field === 'isEnabled')}
      getValue={getValue}
      onChange={onChange}
    />
  </div>
  <div class="description">{data.description}</div>
  <div class="price">Precio: {money(data.price)}</div>
  <div class="stock">Disponibilidad: {data.stock}</div>
  <div class="is-present">¿Para regalar? {yesNo(data.isPresent)}</div>
  
  {#if $actions}
    <div class="actions">
      <Value
        data={data}
        options={$actions}
        getValue={getValue}
        onChange={onChange}
      />
    </div>
  {/if}
</div>

<style>
  .item {
    background-color: var(--item-background-color);
    padding: .1em;
    margin: .1em;
    border-radius: .8em;
    min-width: 10em;
    width: 100%;
    box-sizing: border-box;
    display: inline-block;
    flex: 1;
    display: flex;
    flex-direction: column;
  }

  :global(.images) {
    margin-top: .5em;
    justify-content: center;
  }

  .name {
    font-weight: bold;
    text-align: center;
  }

  .price {
    font-size: 1.2em;
    text-align: left;
    color: var(--item-price-color);
  }

  .description {
    margin-top: .5em;
    font-size: 0.9em;
    text-align: left;
  }

  .stores {
    display: flex;
    gap: .3em;
    justify-content: center;
  }

  .category,
  .store {
    font-size: 0.7em;
    font-style: italic;
    border: .15em solid var(--border-color);
    border-radius: .5em;
    background-color: var(--background-color);
    display: block;
    padding: .1em .3em;
    margin-top: .3em;
  }

  .category {
    margin: auto;
  }

  .description {
    max-height: 3em;
    overflow: hidden;
    text-overflow: ellipsis;
  }

  .actions {
    margin-top: .5em;
    text-align: right;
    font-size: 1.5em;
  }
</style>
