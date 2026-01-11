<script>
  import AddButton from '$components/buttons/Add.svelte';

  let {
    onChange = null,
    onchange = null,
    multiple = false,
    addButton = {},
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

<div
  {...restProps}
>
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
  <AddButton
    {...addButton}
    onClick={addImageHandler}
  />
</div>

<style>
</style>