<script>
  import Header from '$components/Header.svelte';
  import Menu from '$components/Menu.svelte';
  import Routes from '$libs/Routes.svelte';
  import Login from '$pages/Login.svelte';
  import { isLoggedIn } from '$stores/session.js';
  import { NotificationContainer } from '$libs/notification.js';
</script>

<main>
  <NotificationContainer />
  <Header />
  {#if $isLoggedIn}
    <Menu />
  {/if}
  <section class="content" >
    {#if $isLoggedIn}
      <Routes />
    {:else}
      <Login />
    {/if}
  </section>
</main>

<style>
  main {
    flex: 1;
    overflow: auto;
    display: flex;
    flex-direction: column;
    background-color: var(--app-background-color);
  }

  .content {
    flex: 1;
    overflow: auto;
    display: flex;
    flex-direction: column;
  }
</style>
