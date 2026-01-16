<script>
  import { writable } from 'svelte/store';
  import * as commerceService from '$services/commerceService.js';
  import { pushNotification } from '$libs/notification';
  import ImagesGallery from '$components/controls/ImagesGallery.svelte';
  import Button from '$components/controls/Button.svelte';
  import QRCode from 'qrcode';

  let {
    uuid = null,
  } = $props();

  let data = writable(null);
  $effect(() => {
    if (!uuid) {
      return;
    }

    commerceService.getSingleForUuid(uuid, { query: { includeStores: true }})
      .then(commerceData => data.set(commerceData))
      .catch(err => {
        pushNotification('Error al cargar el comercio', 'error');
      });
  });

  const qrDataUrl = writable(null);
  $effect(() => {
    if (!$data)
      return;

    const url = new URL(window.location.href);
    url.pathname = '/explore';
    url.search = `commerce=${uuid}`

    QRCode.toDataURL(url.href, { width: 200 })
      .then(url => {
        qrDataUrl.set(url);
      })
      .catch(err => {
        pushNotification('Error al generar el código QR', 'error');
      });
  });
</script>

<div
  class="commerce"
>
  {#if !$data}
    <div class="disabled-banner">Comercio no encontrado</div>
  {:else}
    <div class="name">{$data.name}</div>
    {#if $data.images && $data.images.length > 0}
      <ImagesGallery
        bind:value={$data.images}
        readonly={true}
      />
    {/if}
    <div class="description">{$data.description}</div>
    <div class="stores">
      {#each $data.stores as store (store.uuid)}
        <a href="/store/{store.uuid}" class="store">
          <div>{store.name}</div>
          <div>{store.description}</div>
        </a>
      {/each}
    </div>
  {/if}
  <Button
    class="explore-button"
    navigateTo="/explore?commerce={uuid}"
  >
    Explorar artículos
  </Button>
  <img
    class="qr-code"
    src="{$qrDataUrl}"
    alt="QR code"
  />
</div>

<style>
  .commerce {
    background-color: var(--background-color);
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

  .description {
    margin-top: .5em;
    font-size: 0.9em;
    text-align: left;
  }

  .description {
    max-height: 3em;
    overflow: hidden;
    text-overflow: ellipsis;
  }

  .stores {
    margin-top: .5em;
    display: flex;
    gap: .5em;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: center;
  }

  a {
    text-decoration: none;
    color: var(--color);
    display: block;
    border-radius: .5em;
  }

  a:hover {
    box-shadow: .1em .1em .3em rgba(0, 0, 0, 0.5);
  }

  a.store {
    border: .1em solid var(--border-color);
    padding: .5em 1em;    
  }

  :global(.explore-button) {
    margin-top: 1em;
    align-self: center;
  }

  .qr-code {
    width: 16em;
    height: 16em;
    align-self: center;
    margin-top: 1em;
  }
</style>
