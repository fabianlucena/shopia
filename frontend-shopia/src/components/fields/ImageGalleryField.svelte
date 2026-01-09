<script>
  import Field from '$components/fields/Field.svelte';
  import ImageGallery from '$components/controls/ImageGallery.svelte';

  let {
    id = crypto.randomUUID(),
    label = '',
    value = $bindable([]),
    required = false,
    ...restProps
  } = $props();
</script>

<Field
  for={id}
  {label}
  {required}
>
  <ImageGallery
    {id}
    bind:value
    {required}
    {...restProps}
  />
</Field>