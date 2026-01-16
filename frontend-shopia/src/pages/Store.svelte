<script>
  import { writable } from 'svelte/store';
  import * as storeService from '$services/storeService.js';
  import { pushNotification } from '$libs/notification';
  import ImagesGallery from '$components/controls/ImagesGallery.svelte';

  let {
    uuid = null,
  } = $props();

  let data = writable(null);
  $effect(() => {
    if (!uuid) {
      return;
    }

    storeService.getSingleForUuid(uuid)
      .then(storeData => {
        console.log(storeData);
        data.set(storeData);
      })
      .catch(err => {
        pushNotification('Error al cargar el local', 'error');
      });
  });
</script>

<div
  class="store"
>
  {#if !$data}
    <div class="disabled-banner">Local no encontrado</div>
  {:else}
    <div class="name">{$data.name}</div>
    {#if $data.images && $data.images.length > 0}
      <ImagesGallery
        bind:value={$data.images}
        readonly={true}
      />
    {/if}
    <div class="description">{$data.description}</div>
    <div class="commerce">
      <span class="label">Comercio:</span>
      <a href="/commerce/{$data.commerce.uuid}">{$data.commerce.name}</a>
    </div>
  {/if}
</div>

<style>
  .store {
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

  .commerce {
    display: flex;
    gap: .3em;
    align-items: baseline;
  }

  .description {
    max-height: 3em;
    overflow: hidden;
    text-overflow: ellipsis;
  }
</style>
