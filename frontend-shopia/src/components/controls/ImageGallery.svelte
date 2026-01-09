<script>
  import AddImage from '$components/controls/AddImage.svelte';
  import EditImage from '$components/controls/EditImage.svelte';
  import DeleteButton from '$components/buttons/Delete.svelte';
  import { writable } from 'svelte/store';

  let {
    id = '',
    value = $bindable([]),
  } = $props();

  let image = writable(null);
</script>

<div
  {id}
  class="gallery"
>
  {#each value as item (item.value)}
    <span class="image">
      <DeleteButton
        on:click={() => {
          value = value.filter(i => i.value !== item.value);
        }}
      />
      <img src={item.value} alt={item.label} />
    </span>
  {/each}
  <AddImage
    limit
    onChange={newImage => {
      let img = new Image();
      img.src = URL.createObjectURL(newImage);
      image.set(img);
      URL.revokeObjectURL(img.src);
    }}
  />
  {#if $image}
    <EditImage
      image={$image}
      aspectRatio={.66}
      onOk={blob => {
        const url = URL.createObjectURL(blob);
        value = [
          ...value,
          {
            added: true,
            label: 'Imagen ' + (value.length + 1),
            value: url,
          }
        ],
        image.set(null);
      }}
      onCancel={() => image.set(null)}
    />
  {/if}
</div>

<style>
  .gallery {
    display: flex;
    flex-wrap: wrap;
    gap: 0.3em;
  }

  .image {
    border: .1em solid var(--border-color);
    background-color: var(--tag-background-color);
    padding: 0.2em 0.5em;
    border-radius: 0.5em;
    font-size: 0.9em;
  }

  img {
    display: block;
    max-width: 100px;
    max-height: 100px;
    margin-top: 0.2em;
    border-radius: 0.3em;
  }
</style>
