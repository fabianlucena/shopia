<script>
  import { navigate } from '$libs/router.js';
  import { showMenu } from '$stores/showMenu.js';
  import { logout } from '$services/loginService';
  import { isLoggedIn } from '$stores/session.js';

  $: menuItems = [
    {
      name: 'home',
      label: 'Inicio',
      path: '/',
    },
    {
      name: 'login',
      label: 'Ingresar',
      path: '/login',
      condition: !$isLoggedIn,
    },
    {
      name: 'logout',
      label: 'Salir',
      path: '/',
      method: logout,
      condition: $isLoggedIn,
    },
    {
      name: 'about',
      label: 'Acerca de',
      path: '/about',
    },
  ].filter(r => r.condition !== false);

  function handleClick(itemName) {
    const item = menuItems.find(i => i.name === itemName);
    if (!item) {
      console.error(`Menu item with name "${itemName}" not found.`);
      return;
    }

    if (item.method) {
      item.method();
    }

    if (item.path) {
      navigate(item.path);
    }

    $showMenu = false;
  }
</script>

{#if $showMenu}
  <nav>
    {#each menuItems as item}
      <button onclick={() => handleClick(item.name)}>{item.label}</button>
    {/each}
  </nav>
{/if}

<style>
  nav {
    position: absolute;
    z-index: 2;
    top: 3em;
    display: flex;
    flex-direction: column;
    background-color: var(--menu-background-color);
  }

  button {
    background-color: var(--menu-item-background-color);
    color: var(--button-text-color);
    border: none;
    border-radius: 0;
    padding: .5em 1em;
    margin: 0;
    text-align: left;
    cursor: pointer;
    font-size: 1em;
  }

  button:hover {
    background-color: var(--menu-item-hover-background-color);
  }
</style>