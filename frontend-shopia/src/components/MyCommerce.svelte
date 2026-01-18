<script>
  import Button from '$components/controls/Button.svelte';
  import Select from '$components/controls/Select.svelte';
  import { mySelectedCommerce, myCommerces, mySelectedCommerceUuid } from '$stores/session.js';
</script>

<div class="header my-commerce">
  {#if $myCommerces.length === 1}
    Mi Comercio: {$myCommerces[0].name}
  {:else if $myCommerces.length > 0}
    Mi Comercio: <Select
      bind:value={$mySelectedCommerceUuid}
      options={$myCommerces?.map(c => ({ label: c.name, value: c.uuid }))}
      placeholder="Seleccionar comercio"
    />
  {:else}
    <Button
      navigateTo={'/commerce-form/new'}
    >
      Crear mi primer comercio
    </Button>
  {/if}
</div>

<style>
  .header {
    font-size: 1.2em;
    font-weight: normal;
  }

  :global(.my-commerce select) {
    font-size: inherit;
    border: none;
    background-color: transparent;
    color: inherit;
    background-color: var(--header-background-color);
    margin: 0;
    padding: 0 0 0 .5em;
    font-weight: normal;
  }

  :global(.my-commerce select option) {
    font-size: .8em;
    padding: 0;
    margin: 0;
    border: none;
  }
</style>