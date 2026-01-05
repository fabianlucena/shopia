<script>
  import TextField from '$components/fields/TextField.svelte';
  
  export let type = 'password';
  export let value = '';
</script>

<TextField
  {type}
  bind:value
  {...$$restProps}
>
</TextField>
