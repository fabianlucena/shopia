<script>
  import { route } from '$stores/route.js';
  import { isLoggedIn, permissions } from '$stores/session.js';
  import Home from '$pages/Home.svelte';
  import About from '$pages/About.svelte';
  import Login from '$pages/Login.svelte';
  import NotFound from '$pages/NotFound.svelte';
  import PasswordRecovery from '$pages/PasswordRecovery.svelte';
  import ItemsList from '$pages/ItemsList.svelte';
  import ItemForm from '$pages/ItemForm.svelte';
  import CommercesList from '$pages/CommercesList.svelte';
  import CommerceForm from '$pages/CommerceForm.svelte';
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
      page: ItemsList,
      condition: () => $permissions.includes('item.get'),
    },
    {
      path: '/item/:uuid',
      page: ItemForm,
      condition: () => $permissions.includes('item.get'),
    },
    
    {
      path: '/commerces',
      page: CommercesList,
      condition: () => $permissions.includes('commerce.get'),
    },
    {
      path: '/commerce/:uuid',
      page: CommerceForm,
      condition: () => $permissions.includes('commerce.get'),
    },
  ];
  
  let routes = writable([]);
  $effect(() => {
    routes.set(allRoutes.filter(r =>
      typeof r.condition === 'function' ? r.condition() : r.condition !== false
    ));
  });

  let Component = $state(null);
  let params = writable({});
  $effect(() => {
    let theRoute = $routes.find(r => r.path === $route);
    if (!theRoute) {
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
              theRoute = r;
              break;
            }
          }
        }
      }
    }

    if (theRoute?.page) {
      Component = theRoute.page;
        params.set({});
      if (theRoute.path.includes(':')) {
        const routeParts = theRoute.path.split('/');
        const pathParts = $route.split('/');
        routeParts.forEach((part, index) => {
          if (part.startsWith(':')) {
            const paramName = part.slice(1);
            params.update(currentParams => {
              currentParams[paramName] = pathParts[index];
              return currentParams;
            });
          }
        });
      }
    } else {
      Component = null;
    }
  });

</script>

{#if Component}
  <Component {...$params}/>
{:else}
  <NotFound />
{/if}
