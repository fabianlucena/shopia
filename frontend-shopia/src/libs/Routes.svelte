<script>
  import { route } from '$stores/route.js';
  import { isLoggedIn, permissions, mySelectedCommerceUuid } from '$stores/session.js';
  import Home from '$pages/Home.svelte';
  import Explore from '$pages/Explore.svelte';
  import Item from '$pages/Item.svelte';
  import Store from '$pages/Store.svelte';
  import Commerce from '$pages/Commerce.svelte';
  import About from '$pages/About.svelte';
  import Login from '$pages/Login.svelte';
  import OAuth2Callback from '$pages/OAuth2Callback.svelte';
  import NotFound from '$pages/NotFound.svelte';
  import PasswordRecovery from '$pages/PasswordRecovery.svelte';
  import ItemsList from '$pages/ItemsList.svelte';
  import ItemForm from '$pages/ItemForm.svelte';
  import CommercesList from '$pages/CommercesList.svelte';
  import CommerceForm from '$pages/CommerceForm.svelte';
  import StoresList from '$pages/StoresList.svelte';
  import StoreForm from '$pages/StoreForm.svelte';
  import MyPlanForm from '$pages/MyPlanForm.svelte';
  import { writable } from 'svelte/store';

  const allRoutes = [
    {
      path: '/',
      page: Home,
    },
    {
      path: '/explore',
      page: Explore,
    },
    {
      path: '/item/:uuid',
      page: Item,
    },
    {
      path: '/store/:uuid',
      page: Store,
    },
    {
      path: '/commerce/:uuid',
      page: Commerce,
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
      path: '/oauth2callback/*',
      page: OAuth2Callback,
    },
    {
      path: '/password-recovery',
      page: PasswordRecovery,
      condition: !$isLoggedIn,
    },
    {
      path: '/commerces-list',
      page: CommercesList,
      condition: () => $permissions.includes('commerce.get'),
    },
    {
      path: '/commerce-form/:uuid',
      page: CommerceForm,
      condition: () => $permissions.includes('commerce.get'),
    },
    {
      path: '/stores-list',
      page: StoresList,
      condition: () => $permissions.includes('store.get')
        && $mySelectedCommerceUuid,
    },
    {
      path: '/store-form/:uuid',
      page: StoreForm,
      condition: () => $permissions.includes('store.get')
        && $mySelectedCommerceUuid,
    },
    {
      path: '/items-list',
      page: ItemsList,
      condition: () => $permissions.includes('item.get')
        && $mySelectedCommerceUuid,
    },
    {
      path: '/item-form/:uuid',
      page: ItemForm,
      condition: () => $permissions.includes('item.get')
        && $mySelectedCommerceUuid,
    },
    {
      path: '/my-plan',
      page: MyPlanForm,
      condition: () => $permissions.includes('my-plan.get'),
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
    let path = $route;
    if (path.includes('?')) {
      path = path.split('?')[0];
    }
    
    let theRoute = $routes.find(r => r.path === path);
    if (!theRoute) {
      // Check for dynamic routes
      for (let r of $routes) {
        if (r.path.includes(':')) {
          const routeParts = r.path.split('/');
          const pathParts = path.split('/');
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

        if (r.path.endsWith('/*')) {
          const basePath = r.path.slice(0, -2);
          if (path.startsWith(basePath)) {
            theRoute = r;
            break;
          }
        }
      }
    }

    if (theRoute?.page) {
      Component = theRoute.page;
        params.set({});
      if (theRoute.path.includes(':')) {
        const routeParts = theRoute.path.split('/');
        const pathParts = path.split('/');
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
{:else if !$isLoggedIn}
  <Login />
{:else}
  <NotFound />
{/if}
