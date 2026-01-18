<script>
  import { writable } from 'svelte/store';
  import Menu from '$components/Menu.svelte';
  import { showMainMenu, permissions, mySelectedCommerceUuid, myCommerces } from '$stores/session.js';
  import Select from './controls/Select.svelte';

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
      name: 'mySelectedCommerce',
      snippet: MyCommerceSelector,
      condition: () => $permissions.includes('commerce.get'),
    },
    {
      name: 'commerces',
      label: 'Mis comercios',
      path: '/commerces-list',
      condition: () => $permissions.includes('commerce.get'),
    },
    {
      name: 'stores',
      label: 'Mis locales',
      path: '/stores-list',
      condition: () => $permissions.includes('store.get')
        && $mySelectedCommerceUuid,
    },
    {
      name: 'items',
      label: 'Mis artículos',
      path: '/items-list',
      condition: () => $permissions.includes('item.get')
        && $mySelectedCommerceUuid,
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

{#snippet MyCommerceSelector()}
  <Select
    bind:value={$mySelectedCommerceUuid}
    options={$myCommerces?.map(c => ({ label: c.name, value: c.uuid }))}
    placeholder="Seleccionar comercio"
  />
{/snippet}

<Menu
  onclose={() => $showMainMenu = false}
  items={$options}
/>
