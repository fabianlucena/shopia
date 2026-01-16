<script>
  import { writable } from 'svelte/store';
  import Menu from '$components/Menu.svelte';
  import { showMainMenu } from '$stores/session.js';
  import { permissions } from '$stores/session.js';

  const allOptions = [
    {
      name: 'explore',
      label: 'Explorar',
      path: '/explore',
    },
    {
      name: 'home',
      label: 'Inicio',
      path: '/',
    },
    {
      name: 'items',
      label: 'Mis artículos',
      path: '/items',
      condition: () => $permissions.includes('item.get'),
    },
    {
      name: 'stores',
      label: 'Mis locales',
      path: '/stores',
      condition: () => $permissions.includes('store.get'),
    },
    {
      name: 'commerces',
      label: 'Mis comercios',
      path: '/commerces',
      condition: () => $permissions.includes('commerce.get'),
    },
    {
      name: 'plan',
      label: 'Mi plan',
      path: '/my-plan',
      condition: () => $permissions.includes('my-plan.get'),
    },
    {
      name: 'about',
      label: 'Acerca de',
      path: '/about',
    },
  ];

  let options = writable([]);
  $effect(() => {
    options.set(allOptions.filter(r =>
      // @ts-ignore
      typeof r.condition === 'function' ? r.condition() : r.condition !== false
    ));
  });
</script>

<Menu
  onclose={() => $showMainMenu = false}
  items={$options}
/>
