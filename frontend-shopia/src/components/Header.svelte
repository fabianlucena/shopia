<script>
  import { showMainMenu, isLoggedIn, showUserMenu } from '$stores/session.js';
  import { navigate } from '$libs/router.js';
  import MenuButton from '$components/buttons/Menu.svelte';
  import UserButton from '$components/buttons/User.svelte';
  import LoginButton from '$components/buttons/Login.svelte';
  import UserMenu from './UserMenu.svelte';
</script>

<header>
  <MenuButton
    onclick={() => $showMainMenu = !$showMainMenu}
  />
  <span
    style="flex: 1; text-align: center;"
  >
    Shopia
  </span>
  {#if $isLoggedIn}
    <UserButton
      onclick={() => $showUserMenu = !$showUserMenu}
    />
    {#if $showUserMenu}
      <UserMenu />
    {/if}
  {:else}
    <LoginButton
      class="icon"
      onclick={() => navigate('/login')}
    />
  {/if}
</header>

<style>
  header {
    display: flex;
    align-items: center;
    font-size: 1.5em;
    margin: 0;
    padding: 0.5em;
    background-color: var(--header-background-color);
    color: var(--header-text-color);
  }
</style>
