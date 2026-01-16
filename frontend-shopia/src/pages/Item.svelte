<script>
  import { writable } from 'svelte/store';
  import * as itemService from '$services/itemService.js';
  import { pushNotification } from '$libs/notification';
  import ImagesView from '$components/controls/ImagesView.svelte';
  import { money } from '$libs/formatter.js';
  import { yesNo } from '$libs/formatter.js';

  let {
    uuid = null,
  } = $props();

  let data = writable(null);
  $effect(() => {
    if (!uuid) {
      return;
    }

    itemService.getSingleForUuid(uuid)
      .then(itemData => {
        data.set(itemData);
      })
      .catch(err => {
        pushNotification('Error al cargar el artículo', 'error');
      });
  });
</script>

<div
  class="item"
>
  {#if !$data}
    <div class="disabled-banner">Artículo no encontrado</div>
  {:else}
    <div class="name">{$data.name}</div>
    <div class="commerce">{$data.commerce.name}</div>
    <div class="stores">
      {#each $data.stores as store (store.uuid)}
        <span class="store">{store.name}</span>
      {/each}
    </div>
    <div class="category">{$data.category.name}</div>
    {#if $data.images && $data.images.length > 0}
      <ImagesView
        bind:value={$data.images}
      />
    {/if}
    <div class="description">{$data.description}</div>
    <div class="price">Precio: {money($data.price)}</div>
    <div class="stock">Disponibilidad: {$data.stock}</div>
    <div class="is-present">¿Para regalar? {yesNo($data.isPresent)}</div>
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

  .commerce {
    font-size: 0.9em;
    text-align: center;
    margin: 0 1em;
    padding: .2em 0;
    background-color: var(--background-color);
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
