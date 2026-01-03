<script>
  import Menu from '$components/Menu.svelte';
  import { logout } from '$services/loginService';
  import { isLoggedIn, showUserMenu } from '$stores/session.js';

  $: items = [
    {
      name: 'logout',
      label: 'Salir',
      path: '/',
      method: logout,
      condition: $isLoggedIn,
    },
  ].filter(r => r.condition !== false);
</script>

<Menu
  {items}
  onclose={() => $showUserMenu = false}
  style="right: 0; left: auto;"
/>
