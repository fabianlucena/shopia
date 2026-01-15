<script>
  import { writable } from 'svelte/store';
  import { Api } from '$services/api.js';
  import AddImage from '$components/controls/AddImage.svelte';
  import EditImage from '$components/controls/EditImage.svelte';
  import DeleteButton from '$components/buttons/Delete.svelte';
  import RestoreButton from '$components/buttons/Restore.svelte';
  import CloseButton from '$components/buttons/Close.svelte';
  import CancelIcon from '$icons/cancel.svelte';
  import { arrangeProps } from '$libs/props.js';
  
  let {
    id = '',
    value = $bindable([]),
    aspectRatio = 0,
    maxWidth = 0,
    maxHeight = 0,
    defaultSelSize = 0,
    readonly = false,
    imageProps = {},
    slideInterval = -1,
    ...restProps
  } = $props();

  let gallery;
  let showDialog;
  let editorDialog;
  let newImage = writable(null);
  let showImageIndex = writable(null);
  let showImageMaximize = writable(false);
  let _imageProps = writable({});
  let _slideInterval = writable(-1);
  let _slideIntervalHandler = null;
  let currentImage;
  let currentImageIndex = writable(0);

  $effect(() => {
    // @ts-ignore
    _imageProps.set(arrangeProps(imageProps, {addValues: { class: 'image'}}));

    var newSlideInterval = slideInterval;
    if (readonly) {
      if (newSlideInterval < 0) {
        newSlideInterval = 3000;
      }

      if (newSlideInterval) {
        _slideInterval.set(newSlideInterval);
      }
    }

    if (newSlideInterval > 0) {
      if (!_slideIntervalHandler) {
        _slideIntervalHandler = setInterval(() => {
          if (!gallery)
            return;

          if (!currentImage) {
            currentImage = gallery.firstElementChild;
            currentImageIndex.set(0);
            if (!currentImage)
              return;
            
            let left = currentImage.getBoundingClientRect().left;
            while (left < 0) {
              currentImage = currentImage.nextElementSibling;
              currentImageIndex.update(n => n + 1);
              if (!currentImage) {
                currentImage = gallery.firstElementChild;
                currentImageIndex.set(0);
                break;
              }
              
              left = currentImage.getBoundingClientRect().left;
            }
          }

          let newLeft;
          currentImage = currentImage.nextElementSibling;
          currentImageIndex.update(n => n + 1);
          if (!currentImage) {
            currentImage = gallery.firstElementChild;
            currentImageIndex.set(0);
            if (!currentImage)
              return;

            newLeft = 0;
          } else {
            newLeft = currentImage.getBoundingClientRect().left;
          }
          
          gallery.scrollTo({ left: newLeft, behavior: 'smooth' });
        }, newSlideInterval);
      }
    }

    if ($newImage)
      editorDialog.showModal();
    else
      editorDialog.close();

    return () => {
      if (_slideIntervalHandler) {
        clearInterval(_slideIntervalHandler);
        _slideIntervalHandler = null;
      }
    };
  });

  function getImageProps(image) {
    if (!image)
      return {};

    return {
      src: `${image.baseUrl ?? Api.baseUrl}${image.url}`,
      alt: image.label
    };
  }
</script>

<div
  class="container"
>
  {#if !readonly}
    <AddImage
      addButton={{
        class: "add-button",
        icon: { class: "add-button" },
      }}
      onChange={addImage => {
        let img = new Image();
        img.dataset['name'] = addImage.name;
        img.src = URL.createObjectURL(addImage);
        newImage.set(img);
        URL.revokeObjectURL(img.src);
      }}
    />
  {/if}
  <div
    class="scroll"
  >
    {#each value as image, index (image.url)}
      <div
        class={
          "dot"
          + (index === $currentImageIndex ? " active" : "")
        }
      ></div>
    {/each}
  </div>
  <div
    bind:this={gallery}
    {id}
    class="gallery"
    {...restProps}
  >
    {#each value as image, index (image.url)}
      <div {...$_imageProps} >
        {#if !readonly}
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
              onclick={evt => {
                evt.stopPropagation();
                evt.preventDefault();

                value = value.map(i => {
                  if (i.url !== image.url)
                    return i;
                  
                  const {added, ...newItem} = { ...i, deleted: true };
                  return newItem;
                });
              }}
            />
          {/if}
        {/if}
        <button
          class="image-button"
          onclick={evt => {
            evt.stopPropagation();
            evt.preventDefault();
            showImageIndex.set(index);
            showDialog.showModal();
          }}
        >
          <img
            src={`${image.baseUrl ?? Api.baseUrl}${image.url}`}
            alt={image.label}
            class={image.deleted ? 'deleted' : ''}
          />
        </button>
      </div>
    {/each}
  </div>
  <dialog
    bind:this={showDialog}
  >
    <CloseButton
      class="close-button"
      onClick={() => showDialog.close()}
    />
    <div
      class={'image-container show' + ($showImageMaximize ? ' maximize' : '')}
    >
      <button
        onclick={evt => {
          evt.stopPropagation();
          evt.preventDefault();
          showImageMaximize.update(v => !v);
        }}
        title="Maximizar / Restaurar"
      >
        <img
          class={'show' + ($showImageMaximize ? ' maximize' : '')}
          {...getImageProps(value[$showImageIndex])}
        />
      </button>
    </div>
    <button
      class="change-button left"
      onclick={evt => {
        evt.stopPropagation();
        evt.preventDefault();
        showImageIndex.update(v => (v - 1 + value.length) % value.length);
      }}
    >
      &lt;
    </button>
    <button
      class="change-button right"
      onclick={evt => {
        evt.stopPropagation();
        evt.preventDefault();
        showImageIndex.update(v => (v + 1) % value.length);
      }}
    >
      &gt;
    </button>
  </dialog>
  <dialog
    bind:this={editorDialog}
  >
    <EditImage
      class="edit-image-control"
      image={$newImage}
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
        newImage.set(null);
      }}
      onCancel={() => newImage.set(null)}
    />
  </dialog>
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
    overflow-x: hidden;
    overflow-y: auto;
    gap: .5em;
    scroll-behavior: smooth;
    align-items: stretch;
  }

  .gallery.no-scroll {
    overflow: hidden;
  }

  .image-button {
    background: transparent;
    border: none;
    padding: 0;
    cursor: pointer;
  }

  .image {
    width: 100%;
    margin: 0;
    padding: 0;
    position: relative;
    height: 100%;
    background-color: var(--tag-background-color);
    font-size: 0.9em;
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

  .image-container.show {
    margin: auto;
    display: block;
    overflow: auto;
    width: 100%;
    height: 100%;
    text-align: center;
  }

  img.show {
    max-width: 100%;
    max-height: auto;
    margin: auto;
  }

  img.show.maximize {
    max-width: none;
  }

  :global(
    button.icon.add-button,
    button.icon.delete-button,
    button.icon.restore-button,
    button.icon.close-button
  ) {
    z-index: 1;
    font-size: 1.3em;
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
    right: 0.2em;
    bottom: 0.2em;
  }

  :global(button.icon.restore-button) {
    color: var(--add-color);
    right: 0.5em;
    bottom: 0.2em;
  }

  :global(button.icon.close-button) {
    right: 0.5em;
    top: 0.2em;
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
    position: absolute;
    overflow: auto;
    height: 90%;
  }

  dialog::backdrop {
    backdrop-filter: blur(2px);
    background: rgba(0, 0, 0, 0.5);
  }

  .scroll {
    z-index: 2;
    position: absolute;
    bottom: .5em;
    height: 0.8em;
    width: 100%;
    margin: 0;
    padding: 0;
    text-align: center;
  }

  .dot {
    width: 0.5em;
    height: 0.3em;
    border: .1em solid var(--button-background-color-highlight);
    background-color: color-mix(
      in srgb,
      var(--button-disabled-background-color) 30%,
      transparent
    );
    border-radius: .3em;
    display: inline-block;
    margin: 0 .08em;
  }

  .dot.active {
    background-color: color-mix(
      in srgb,
      var(--button-background-color-highlight) 70%,
      transparent
    );
  }

  .change-button {
    z-index: 1;
    font-size: 2em;
    height: 1.5em;
    position: absolute;
    top: 50%;
    border: .02em solid transparent;
    border-radius: 0.2em;
    background-color: transparent;
    opacity: .5;
    padding: 0;
    border: none;
    cursor: pointer;
    
  }

  .change-button:hover {
    border: .02em solid var(--border-color);
    opacity: .75;
    background-color: color-mix(
      in srgb,
      var(--background-color) 60%,
      transparent
    );
  }

  .change-button.left {
    left: 0.2em;
  }

  .change-button.right {
    right: 0.2em;
  }
</style>
