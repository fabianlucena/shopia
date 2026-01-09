<script>
  import AddImageButton from '$components/buttons/AddImage.svelte';

  let {
    onChange = null,
    onchange = null,
    multiple = false,
    ...restProps
  } = $props();

  let fileInput;

  function addImageHandler(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    fileInput.click();
  }

  function handleFile(event) {
    const files = event.target.files;
    if (files.length > 0) {
      if (onChange) {
        onChange(...files);
      } else if (onchange) {
        onchange(...files);
      }
    }
  }
</script>

<div>
  <input
    bind:this={fileInput}
    type="file"
    id="fileInput"
    accept="image/*"
    {multiple}
    style="display: none"
    onchange={handleFile}
    {...restProps}
  >
  <AddImageButton
    onClick={addImageHandler}
  />
</div>