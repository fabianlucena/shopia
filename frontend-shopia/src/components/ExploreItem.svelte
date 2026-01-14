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
    slideInterval={2000}
  />
  <div>{item.description}</div>
  <div>{item.category.name}</div>
  <div>{item.price}</div>
</div>

<style>
  .item {
    border: .1em solid var(--border-color);
    padding: .1em;
    margin: .1em;
    border-radius: .2em;
    min-width: 10em;
    max-width: 20em;
    width: calc(50% - 0.2em);
    box-sizing: border-box;
    display: inline-block;
  }
</style>