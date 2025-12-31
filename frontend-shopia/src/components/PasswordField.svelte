<script>
  import TextField from '$components/TextField.svelte';
  
  export let type = 'password';
  export let value = '';
</script>

<TextField
  {type}
  bind:value
  {...$$restProps}
>
</TextField>
