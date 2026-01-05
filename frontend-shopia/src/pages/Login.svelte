<script>
  import Form from '$components/Form.svelte';
  import TextField from '$components/fields/TextField.svelte';
  import PasswordField from '$components/fields/PasswordField.svelte';
  import LinkField from '$components/fields/LinkField.svelte';
  import Button from '$components/controls/Button.svelte';
  import { login } from '$services/loginService.js';
  import { pushNotification } from '$libs/notification.js';
  import { navigate } from '$libs/router';

  let data = {
    username: '',
    password: '',
  };

  async function handleSubmit(evt) {
    evt.preventDefault();
    
    try {
      await login(data);
    } catch (error) {
      pushNotification('Error de inicio de sesión', 'error');
      return;
    }
    
    navigate('/');
    pushNotification('Inicio de sesión exitoso', 'success');
  }

  function passwordRecoveryHandler(evt) {
    evt.preventDefault();
    navigate('/password-recovery');
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
    bind:value={data.username}
    required={true}
  />

  <PasswordField
    label="Contraseña"
    bind:value={data.password}
    required={true}
  />

  <LinkField
    onclick={passwordRecoveryHandler}
  >
    Recuperar contraseña
  </LinkField>

  {#snippet footer()}
    <Button>Registrarse</Button>
  {/snippet}
</Form>
