<script>
  import { route } from '$stores/route.js';
  import { isLoggedIn, permissions } from '$stores/session.js';
  import Home from '$pages/Home.svelte';
  import About from '$pages/About.svelte';
  import Login from '$pages/Login.svelte';
  import NotFound from '$pages/NotFound.svelte';
  import PasswordRecovery from '$pages/PasswordRecovery.svelte';
  import Items from '$pages/items.svelte';
  import { writable } from 'svelte/store';

  const allRoutes = [
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
    {
      path: '/items',
      page: Items,
      condition: () => $permissions.includes('item.get'),
    },
  ];
  
  let routes = writable([]);
  $effect(() => {
    routes.set(allRoutes.filter(r =>
      typeof r.condition === 'function' ? r.condition() : r.condition !== false
    ));
  });

  let Component = $state(Home);
  $effect(() => {
    let theRoute = $routes.find(r => r.path === $route);
    if (theRoute?.page) {
      Component = theRoute.page;
    } else {
      Component = null;
    }
  });

</script>

{#if Component}
  <Component />
{:else}
  <NotFound />
{/if}
