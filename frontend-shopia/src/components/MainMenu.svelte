<script>
  import Menu from '$components/Menu.svelte';
  import { showMainMenu } from '$stores/session.js';

  $: items = [
    {
      name: 'home',
      label: 'Inicio',
      path: '/',
    },
    {
      name: 'about',
      label: 'Acerca de',
      path: '/about',
    },
  ].filter(r => r.condition !== false);
</script>

<Menu
  onclose={() => $showMainMenu = false}
  {items}
/>
