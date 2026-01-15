<script>
  import ImagesGallery from './controls/ImagesGallery.svelte';
  import { money } from '$libs/formatter.js';

  let item = $props();
</script>

<div 
  class="item" 
>
  <div
    class="images"
  >
    <ImagesGallery
      readonly={true}
      bind:value={item.images}
      slideInterval={3000}
      slideIntervalJitter={1000}
    />
  </div>
  <div
    class="data"
  >
    <div class="name">{item.name}</div>
    <div class="category">{item.category.name}</div>
    <div class="description">{item.description}</div>
    <div class="price">{money(item.price)}</div>
  </div>
</div>

<style>
  .item {
    background-color: var(--item-background-color);
    padding: .1em;
    margin: .1em;
    border-radius: .8em;
    min-width: 10em;
    max-width: 20em;
    width: calc(50% - 0.2em);
    box-sizing: border-box;
    display: inline-block;
    flex: 1;
    display: flex;
    flex-direction: column;
  }

  .images {
    width: 100%;
    height: auto;
    overflow: hidden;
    border-radius: .5em;
    flex: 1;
  }

  .data {
    background-color: var(--item-data-background-color);
    text-align: center;
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

  .category {
    font-size: 0.7em;
    font-style: italic;
    border: .15em solid var(--border-color);
    border-radius: .5em;
    background-color: var(--background-color);
    display: inline-block;
    padding: .1em .3em;
    margin: auto;
  }

  .description {
    height: 3em;
    overflow: hidden;
    text-overflow: ellipsis;
  }
</style>