<script>
  import { writable } from 'svelte/store';
  import { getSingleForUuid } from '$services/itemService.js';
  import Form from '$components/Form.svelte';
  import TextField from '$components/TextField.svelte';

  let {uuid, ...allProps} = $props();

  let data = writable({
    name: '',
    description: '',
  });
  
  $effect(() => {
    if (uuid !== 'new') {
      getSingleForUuid(uuid)
        .then(resData => data.set(resData));
    }
  });
</script>

<Form
  header="Artículo"
>
  <TextField
    label="Nombre"
    bind:value={$data.name}
    required={true}
  />

  <TextField
    label="Descripción"
    bind:value={$data.description}
    required={true}
  />
</Form>