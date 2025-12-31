<script>
  import { route } from '$stores/route.js';
  import { isLoggedIn } from '$stores/session.js';
  import Home from '$pages/Home.svelte';
  import About from '$pages/About.svelte';
  import Login from '$pages/Login.svelte';
  import NotFound from '$pages/NotFound.svelte';
  import PasswordRecovery from '$pages/PasswordRecovery.svelte';

  const routes = [
    {
      path: '/',
      page: Home,
    },
    {
      path: '/about',
      page: About,
    },
    {
      path: '/login',
      page: Login,
      condition: !$isLoggedIn,
    },
    {
      path: '/password-recovery',
      page: PasswordRecovery,
      condition: !$isLoggedIn,
    },
  ].filter(r => r.condition !== false);

  let Component = $state(Home);
  $effect(() => {
    let theRoute = routes.find(r => r.path === $route);
    if (theRoute?.page) {
      Component = theRoute.page;
    } else {
      Component = NotFound;
    }
  });

</script>

<Component />