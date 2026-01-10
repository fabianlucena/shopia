<script>
  import AddImage from '$components/controls/AddImage.svelte';
  import EditImage from '$components/controls/EditImage.svelte';
  import DeleteButton from '$components/buttons/Delete.svelte';
  import { writable } from 'svelte/store';

  let {
    id = '',
    value = $bindable([]),
    aspectRatio = 0,
    defaultSelSize = 0,
    ...restProps
  } = $props();

  let image = writable(null);
</script>

<div
  {id}
  class="gallery"
  {...restProps}
>
  {#each [...value, { addImage: true }] as item (item.value)}
    {#if item.addImage}
      {#if !$image}
        <div class="image">
          <AddImage
            addButton={{
              class: "add-image-button",
              icon: { class: "add-image-button" },
            }}
            onChange={newImage => {
              let img = new Image();
              img.src = URL.createObjectURL(newImage);
              image.set(img);
              URL.revokeObjectURL(img.src);
            }}
          />
        </div>
      {/if}
    {:else}
      <div class="image">
        <DeleteButton
          class="delete-button"
          onclick={() => {
            value = value.filter(i => i.value !== item.value);
          }}
        />
        <img src={item.value} alt={item.label} />
      </div>
    {/if}
  {/each}
  {#if $image}
    <EditImage
      image={$image}
      {aspectRatio}
      {defaultSelSize}
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
    border-radius: 0.3em;
    font-size: 0.9em;
    width: 10em;
    position: relative;
  }

  img {
    display: block;
    max-width: 100%;
    max-height: 100%;
    margin-top: 0.2em;
    border-radius: 0.1em;
  }

  :global(button.icon.delete-button) {
    color: var(--danger-color);
    height: 2.5em;
    position: absolute;
    right: 0.2em;
    bottom: 0.2em;
    background-color: color-mix(
      in srgb,
      var(--background-color) 60%,
      transparent
    );
    padding: 0.2em;
    border-radius: 0.4em;
    border: .1em solid var(--border-color);
    cursor: pointer;
  }

  :global(button.icon.add-image-button) {
    color: var(--border-color);
    width: 100%;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 0.2em;
    border-radius: 0.4em;
    border: .1em solid var(--border-color);
    cursor: pointer;
  }

  :global(svg.add-image-button) {
    max-width: 4em;
  }
</style>
