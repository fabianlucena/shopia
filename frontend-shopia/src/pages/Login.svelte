<script>
  import Form from '$components/Form.svelte';
  import TextField from '$components/TextField.svelte';
  import PasswordField from '$components/PasswordField.svelte';
  import Button from '$components/Button.svelte';
  import { login } from '$services/loginService.js';
  import { pushNotification } from '$libs/notification.js';

  let data = {
    username: '',
    password: '',
  };

  async function handleSubmit(evt) {
    evt.preventDefault();
    try {
      const res = await login(data);
    } catch (error) {
      pushNotification('Error de inicio de sesión', 'error');
      return;
    }
    
    pushNotification('Inicio de sesión exitoso', 'success');
  }
</script>

<Form
  header="Login"
  onsubmit={handleSubmit}
  submitLabel="Ingresar"
  submitPosition="last"
>
  <TextField
    label="Nombre de usuario"
    bind:value={data.username}
  />

  <PasswordField
    label="Contraseña"
    bind:value={data.password}
  />

  {#snippet footer()}
    <Button>Registrarse</Button>
  {/snippet}
</Form>
