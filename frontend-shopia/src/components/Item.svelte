<script>
  import ImagesGallery from "./controls/ImagesGallery.svelte";

  let item = $props();
</script>

<div 
  class="item" 
>
  <div>{item.name}</div>
  <ImagesGallery
    readonly={true}
    bind:value={item.images}
  />
  <div>{item.description}</div>
  <div>{item.category.name}</div>
  <div>{item.price}</div>
</div>

<style>
  .item {
    flex: 1;
    border: .1em solid var(--border-color);
    padding: .1em;
    margin: 0;
    border-radius: .2em;
    min-width: 10em;
    max-width: 20em;
  }
</style>