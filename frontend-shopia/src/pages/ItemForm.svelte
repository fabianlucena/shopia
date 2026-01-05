<script>
  import { writable } from 'svelte/store';
  import { getSingleForUuid } from '$services/itemService.js';
  import * as categoryService from '$services/categoryService.js';
  import * as storeService from '$services/storeService.js';
  import Form from '$components/Form.svelte';
  import TextField from '$components/TextField.svelte';
  import SelectField from '$components/SelectField.svelte';

  let {uuid, ...allProps} = $props();

  let data = writable({
    name: '',
    description: '',
    categoryUuid: '',
    storeUuid: '',
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

  <SelectField
    label="Categoría"
    bind:value={$data.categoryUuid}
    service={categoryService.getAllForSelect}
    required={true}
  />

  <SelectField
    label="Local"
    bind:value={$data.storeUuid}
    service={storeService.getAllForSelect}
    required={true}
  />
</Form>