<script>
  import { onMount } from 'svelte';
  import Header from '$components/Header.svelte';
  import MainMenu from '$components/MainMenu.svelte';
  import ConfirmModal from '$components/ConfirmModal.svelte';
  import Routes from '$libs/Routes.svelte';
  import NotificationContainer from '$components/NotificationContainer.svelte';
  import { showMainMenu } from '$stores/session.js';
  import { autoLogin } from '$services/loginService.js';

  onMount(() => autoLogin());
</script>

<main>
  <NotificationContainer />
  <ConfirmModal />
  <Header />
  {#if $showMainMenu}
    <MainMenu />
  {/if}
  <section class="content" >
    <Routes />
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
    align-items: center;
  }
</style>
