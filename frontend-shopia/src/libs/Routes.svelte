<script>
  import { route } from '$stores/route.js';
  import { isLoggedIn, permissions } from '$stores/session.js';
  import Home from '$pages/Home.svelte';
  import About from '$pages/About.svelte';
  import Login from '$pages/Login.svelte';
  import NotFound from '$pages/NotFound.svelte';
  import PasswordRecovery from '$pages/PasswordRecovery.svelte';
  import Items from '$pages/Items.svelte';
  import Item from '$pages/Item.svelte';
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
    {
      path: '/item/:uuid',
      page: Item,
      condition: () => $permissions.includes('item.get'),
    },
  ];
  
  let params = {};
  let routes = writable([]);
  $effect(() => {
    routes.set(allRoutes.filter(r =>
      typeof r.condition === 'function' ? r.condition() : r.condition !== false
    ));
  });

  let theRoute;
  let Component = $state(Home);
  $effect(() => {
    let newRoute = $routes.find(r => r.path === $route);
    if (!newRoute) {
      // Check for dynamic routes
      for (let r of $routes) {
        if (r.path.includes(':')) {
          const routeParts = r.path.split('/');
          const pathParts = $route.split('/');
          if (routeParts.length === pathParts.length) {
            let isMatch = true;
            for (let i = 0; i < routeParts.length; i++) {
              if (!routeParts[i].startsWith(':') && routeParts[i] !== pathParts[i]) {
                isMatch = false;
                break;
              }
            }
            if (isMatch) {
              newRoute = r;
              break;
            }
          }
        }
      }
    }

    if (newRoute !== theRoute) {
      theRoute = newRoute;
    }

    if (theRoute?.page) {
      Component = theRoute.page;
      if (theRoute.path.includes(':')) {
        const routeParts = theRoute.path.split('/');
        const pathParts = $route.split('/');
        params = {};
        routeParts.forEach((part, index) => {
          if (part.startsWith(':')) {
            const paramName = part.slice(1);
            params[paramName] = pathParts[index];
          }
        });
      } else {
        params = {};
      }
    } else {
      Component = null;
    }
  });

</script>

{#if Component}
  <Component {...params}/>
{:else}
  <NotFound />
{/if}
