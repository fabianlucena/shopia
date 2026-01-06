<script>
  let {
    id = '',
    value = $bindable([]),
  } = $props();
</script>

<div
  {id}
  class="tags"
>
  {#each value as item (item.value)}
    <span class="tag">{item.label}</span>
  {/each}
</div>

<style>
  .tags {
    display: flex;
    flex-wrap: wrap;
    gap: 0.3em;
  }

  .tag {
    border: .1em solid var(--border-color);
    background-color: var(--tag-background-color);
    padding: 0.2em 0.5em;
    border-radius: 0.5em;
    font-size: 0.9em;
  }
</style>
