<script>
  import { writable } from 'svelte/store';
  import * as commerceService from '$services/commerceService.js';
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

    commerceService.getSingleForUuid(uuid)
      .then(commerceData => {
        console.log(commerceData);
        data.set(commerceData);
      })
      .catch(err => {
        pushNotification('Error al cargar el comercio', 'error');
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
  {/if}
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
</style>
