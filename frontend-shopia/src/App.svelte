<script>
  import Header from '$components/Header.svelte';
  import Menu from '$components/Menu.svelte';
  import Routes from '$libs/Routes.svelte';
  import { NotificationContainer } from '$libs/notification.js';
</script>

<main>
  <NotificationContainer />
  <Header />
  <Menu />
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
