<script>
  import { writable } from 'svelte/store';
  import * as itemService from '$services/itemService.js';
  import { pushNotification } from '$libs/notification';
  import ImagesGallery from '$components/controls/ImagesGallery.svelte';
  import WaitOverlay from '$components/WaitOverlay.svelte';
  import { money } from '$libs/formatter.js';
  import { yesNo } from '$libs/formatter.js';

  let {
    uuid = null,
  } = $props();

  let data = writable(null);
  let message = writable('Cargando artículo');
  $effect(() => {
    if (!uuid) {
      message.set('Artículo desconocido');
      return;
    }

    return;

    itemService.getSingleForUuid(uuid)
      .then(itemData => {
        data.set(itemData);
      })
      .catch(err => {
        message.set('Error al cargar el artículo');
        pushNotification('Error al cargar el artículo', 'error');
      });
  });
</script>

<div
  class="item"
>
  {#if !$data}
    <WaitOverlay show={!!$message}>
      {$message}
    </WaitOverlay>
  {:else}
    <div class="name">{$data.name}</div>
    {#if $data.images && $data.images.length > 0}
      <ImagesGallery
        bind:value={$data.images}
        readonly={true}
      />
    {/if}
    <div class="category">{$data.category.name}</div>
    <div class="description">{$data.description}</div>
    <div class="price">Precio: {money($data.price)}</div>
    <div class="stock">Disponibilidad: {$data.stock}</div>
    <div class="commerce">
      <span class="label">Comercio:</span>
      <a href="/commerce/{$data.commerce.uuid}">{$data.commerce.name}</a>
    </div>
    <div class="stores">
      <span class="label">Locales:</span> 
      {#each $data.stores as store (store.uuid)}
        <a href="/store/{store.uuid}" class="store">{store.name}</a>
      {/each}
    </div>
    <div class="is-present">¿Para regalar? {yesNo($data.isPresent)}</div>
    {#if $data.isPresent}
      <div class="ages">
        {$data.minAge} - {$data.maxAge} años
      </div>
    {/if}
  {/if}
</div>

<style>
  .item {
    position: relative;
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
    font-size: 1.5em;
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

  .label {
    font-weight: bold;
    font-size: .7em;
  }

  a {
    text-decoration: none;
    color: var(--color);
    display: inline-block;
  }

  a:hover {
    box-shadow: .1em .1em .3em rgba(0, 0, 0, 0.5);
    border-radius: .3em;
  }

  .commerce,
  .stores {
    display: flex;
    gap: .3em;
    align-items: baseline;
  }

  .category {
    font-size: 0.7em;
    font-style: italic;
    border: .15em solid var(--border-color);
    border-radius: .5em;
    background-color: var(--background-color);
    display: block;
    padding: .1em .3em;
    margin-top: .3em;
    margin-left: auto;
    margin-right: auto;
  }

  .description {
    max-height: 3em;
    overflow: hidden;
    text-overflow: ellipsis;
  }
</style>
