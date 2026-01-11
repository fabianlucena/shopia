<script>
  import { writable } from 'svelte/store';
  import { Api } from '$services/api.js';
  import AddImage from '$components/controls/AddImage.svelte';
  import EditImage from '$components/controls/EditImage.svelte';
  import DeleteButton from '$components/buttons/Delete.svelte';
  import RestoreButton from '$components/buttons/Restore.svelte';
  import CancelIcon from '$icons/cancel.svelte';
  
  let {
    id = '',
    value = $bindable([]),
    aspectRatio = 0,
    maxWidth = 0,
    maxHeight = 0,
    defaultSelSize = 0,
    ...restProps
  } = $props();

  let editorDialog;
  let image = writable(null);

  $effect(() => {
    if ($image)
      editorDialog.showModal();
    else
      editorDialog.close();
  });
</script>

<div
  class="container"
>
  <AddImage
    addButton={{
      class: "add-button",
      icon: { class: "add-button" },
    }}
    onChange={newImage => {
      let img = new Image();
      img.dataset['name'] = newImage.name;
      img.src = URL.createObjectURL(newImage);
      image.set(img);
      URL.revokeObjectURL(img.src);
    }}
  />
  <div
    class="scroll"
  >
    {#each value as image (image.url)}
      <div class="dot">
      </div>
    {/each}
  </div>
  <div
    {id}
    class="gallery"
    {...restProps}
  >
    {#each value as image (image.url)}
      <div class="image">
        {#if image.deleted}
          <RestoreButton
            class="restore-button"
            onClick={() => {
              value = value.map(i => {
                if (i.url !== image.url)
                  return i;
                
                const { deleted, ...newItem } = i;
                return newItem;
              });
            }}
          />
          <CancelIcon
            class="deleted-icon"
          />
        {:else}
          <DeleteButton
            class="delete-button"
            onclick={() => {
              value = value.map(i => {
                if (i.url !== image.url)
                  return i;
                
                const {added, ...newItem} = { ...i, deleted: true };
                return newItem;
              });
            }}
          />
        {/if}
        <img
          src={`${image.baseUrl ?? Api.baseUrl}${image.url}`}
          alt={image.label}
          class={image.deleted ? 'deleted' : ''}
        />
      </div>
    {/each}
    <dialog
      bind:this={editorDialog}
    >
      <EditImage
        class="edit-image-control"
        image={$image}
        {aspectRatio}
        {maxWidth}
        {maxHeight}
        {defaultSelSize}
        onOk={({ blob, name, type }) => {
          value = [
            ...value,
            {
              added: true,
              baseUrl: '',
              blob,
              name,
              type,
              label: name || ('Imagen ' + (value.length + 1)),
              url: URL.createObjectURL(blob),
            }
          ],
          image.set(null);
        }}
        onCancel={() => image.set(null)}
      />
    </dialog>
  </div>
</div>

<style>
  .container {
    width: 100%;
    overflow: hidden;
    position: relative;
    min-height: 2.5em;
  }

  .gallery {
    display: flex;
    flex-wrap: nowrap;
    overflow-x: auto;
    overflow-y: hidden;
    gap: .5em;
    scroll-behavior: smooth;
    align-items: stretch;
    padding-bottom: .8em;
  }

  .image {
    border: .1em solid var(--border-color);
    background-color: var(--tag-background-color);
    padding: 0.2em 0.5em ;
    border-radius: 0.3em;
    margin: 0;
    font-size: 0.9em;
    position: relative;
    height: 100%;
    width: 25%;
    flex: 0 0 auto;
  }

  img {
    display: block;
    max-width: 100%;
    max-height: 100%;
    margin-top: 0.2em;
    border-radius: 0.1em;
  }

  img.deleted {
    opacity: 0.4;
    filter: grayscale(100%);
  }

  :global(
    button.icon.add-button,
    button.icon.delete-button,
    button.icon.restore-button
  ) {
    z-index: 1;
    font-size: 1.5em;
    height: 1.5em;
    position: absolute;
    background-color: color-mix(
      in srgb,
      var(--background-color) 60%,
      transparent
    );
    padding: 0.2em;
    border-radius: 0.4em;
    border: .015em solid var(--border-color);
    cursor: pointer;
  }

  :global(button.icon.add-button) {
    color: var(--add-color);
    top: .1em;
    left: .1em;
  }

  :global(button.icon.delete-button) {
    color: var(--danger-color);
    right: 0.5em;
    bottom: 0.2em;
  }

  :global(button.icon.restore-button) {
    color: var(--add-color);
    right: 0.5em;
    bottom: 0.2em;
  }

  :global(.deleted-icon) {
    height: 6em;
    width: auto;
    position: absolute;
    left: 50%;
    top: 50%;
    margin-top: -3em;
    margin-left: -3em;
  }

  dialog {
    border: none;
    background-color: var(--background-color);
    padding: 0;
    max-width: 100%;
    width: 600px;
  }

  dialog::backdrop {
    backdrop-filter: blur(2px);
    background: rgba(0, 0, 0, 0.5);
  }

  .scroll {
    z-index: 2;
    position: absolute;
    bottom: .2em;
    height: 0.8em;
    width: 100%;
    margin: 0;
    padding: 0;
    text-align: center;
  }

  .dot {
    width: 0.6em;
    height: 0.6em;
    background-color: var(--border-color);
    opacity: 0.5;
    border-radius: .3em;
    display: inline-block;
    margin: 0 .08em;
  }
</style>
