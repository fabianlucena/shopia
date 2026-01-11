<script>
  import { Api } from '$services/api.js';

  let {
    id = '',
    value = $bindable([]),
  } = $props();
</script>

<div
  {id}
  class="images"
>
  {#each value as item (item.uuid)}
    <img src={`${Api.baseUrl}${item.url}`} alt={item.label} class="image" />
  {/each}
</div>

<style>
  .images {
    display: flex;
    flex-wrap: wrap;
    gap: 0.3em;
  }

  .image {
    border: .1em solid var(--border-color);
    border-radius: 0.2em;
    height: 3em;
  }
</style>
