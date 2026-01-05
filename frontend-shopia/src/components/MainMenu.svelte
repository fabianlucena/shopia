<script>
  import { writable } from 'svelte/store';
  import Menu from '$components/Menu.svelte';
  import { showMainMenu } from '$stores/session.js';
  import { permissions } from '$stores/session.js';

  const allOptions = [
    {
      name: 'home',
      label: 'Inicio',
      path: '/',
    },
    {
      name: 'items',
      label: 'Mis artículos',
      path: '/items',
    },
    {
      name: 'commerces',
      label: 'Mis comercios',
      path: '/commerces',
    },
    {
      name: 'about',
      label: 'Acerca de',
      path: '/about',
      condition: () => $permissions.includes('item.get'),
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
