<script>
  import { writable } from 'svelte/store';
  import { get } from '$services/itemService.js';
  import Item from '$components/ExploreItem.svelte';
  import Modal from '$components/Modal.svelte';
  import ButtonSettings from '$components/buttons/Settings.svelte';
  import ButtonClose from '$components/buttons/Close.svelte';
  import Button from '$components/controls/Button.svelte';
  import SelectField from '$components/fields/SelectField.svelte';
  import * as commerceService from '$services/commerceService.js';
  import * as storeService from '$services/storeService.js';

  const params = new URLSearchParams(window.location.search);
  let paramCommerceUuid = params.get('commerce');

  let items = $state([]);
  let showSettings = $state(false);
  let selectedCommerce = writable(null);
  let selectedCommerceUuid = $state(null);
  let selectedStoreUuid = $state(null);
  let commerces = writable([]);
  let stores = writable([]);
  
  $effect(() => {
    const options = {limit: 10};
    if (selectedCommerceUuid) options.commerceUuid = selectedCommerceUuid;
    if (selectedStoreUuid) options.storeUuid = selectedStoreUuid;
    
    get(options)
      .then(data => items = data.rows);
  });

  $effect(() => {
    commerceService.getAllForSelect()
      .then(commercesData => {
        commerces.set([{}, ...commercesData]);

        if (paramCommerceUuid && commercesData?.find(c => c.value === paramCommerceUuid)) {
          selectedCommerceUuid = paramCommerceUuid;
        }
      });
  });

  $effect(() => {
    if (selectedCommerceUuid) {
      const newSelectedCommerce = $commerces.find(c => c.value === selectedCommerceUuid);
      selectedCommerce.set(newSelectedCommerce);
    } else {
      selectedCommerce.set(null);
    }

    storeService.getAllForSelect({commerceUuid: selectedCommerceUuid})
      .then(data => stores.set([{}, ...data]));
  });
</script>

<h2
  style="width: 100%; text-align: center; position: relative;"
>
  <ButtonSettings
    style="float: right; margin: 0 0.3em;"
    onclick={() => showSettings = true}
  />
  <div>Artículos</div>
</h2>

{#if $selectedCommerce}
  <h3>
    Comercio: {$selectedCommerce.label}
  </h3>
{/if}

<Modal
  open={showSettings}
>
  {#snippet header()}
    Opciones de exploración
    <ButtonClose onClick={() => showSettings = false} />
  {/snippet}

  <SelectField
    label="Comercio"
    options={$commerces}
    bind:value={selectedCommerceUuid}
  />

  {#snippet footer()}
    <Button onClick={() => showSettings = false}>Cerrar</Button>
  {/snippet}
</Modal>

{#if items.length === 0}
  <p>Cargando...</p>
{:else}
  <div
    class="item-list"
  >
    {#each items as item}
      <Item
        {...item}
      />
    {/each}
  </div>
{/if}

<style>
  .item-list {
    width: 100%;
    padding: 0 .3em;
    box-sizing: border-box;
    display: flex;
    flex-wrap: wrap;
  }
</style>