<script>
  import Form from '$components/Form.svelte';
  import TextField from '$components/fields/TextField.svelte';
  import PasswordField from '$components/fields/PasswordField.svelte';
  import LinkField from '$components/fields/LinkField.svelte';
  import Button from '$components/controls/Button.svelte';
  import { login } from '$services/loginService.js';
  import { pushNotification } from '$libs/notification.js';
  import { navigate } from '$libs/router';
  import AccederConGoogle from '$components/buttons/AccederConGoogle.svelte';
  import { writable } from 'svelte/store';
  import { getProviders } from '$services/oAuth2Service.js';

  let data = writable({
    username: '',
    password: '',
  });

  let providers = writable([]);

  $effect(() => {
    getProviders()
      .then(result => providers.set(result))
      .catch(err => {
        pushNotification('Error al cargar proveedores de inicio de sesión', 'error');
      });
  });

  function getState() {
    let state = location.pathname;

    if (location.search)
      state += location.search;

    if (location.hash)
      state += location.hash;

    return state;
  }

  async function handleSubmit(evt) {
    evt.preventDefault();
    
    try {
      await login($data);
    } catch (error) {
      pushNotification('Error de inicio de sesión', 'error');
      return;
    }
    
    navigate('/');
    pushNotification('Inicio de sesión exitoso', 'success');
  }
</script>

<Form
  header="Login"
  onSubmit={handleSubmit}
  submitLabel="Ingresar"
  submitPosition="last"
>
  <TextField
    label="Nombre de usuario"
    bind:value={$data.username}
    required={true}
  />

  <PasswordField
    label="Contraseña"
    bind:value={$data.password}
    required={true}
  />
</Form>

{#each $providers as provider}
  {#if provider.name === 'google'}
    <AccederConGoogle
      class="oauth-provider-google"
      onclick={evt => {
        evt.preventDefault();
        evt.stopPropagation();
        const url = provider.url + '&state=' + encodeURIComponent(getState());
        window.location.href = url;
      }}
    />
  {:else}
    <Button
      class={"oauth-provider-" + provider.name}
      on:click={evt => {
        evt.preventDefault();
        evt.stopPropagation();
        const url = provider.url + '&state=' + encodeURIComponent(getState());
        window.location.href = url;
      }}
    >
      {provider.label}
    </Button>
  {/if}
{/each}

<style>
  :global(button.oauth-provider-google) {
    margin: .5em auto;
  }
</style>