<script>
  import ButtonIcon from '$components/controls/ButtonIcon.svelte';
  import AddImageIcon from '$icons/add-image.svelte';

  let {
    icon = {},
    ...restProps
  } = $props();
</script>

<ButtonIcon {...restProps} >
  <AddImageIcon {...icon}/>
</ButtonIcon>