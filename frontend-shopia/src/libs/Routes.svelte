<script>
  import { route } from '$stores/route.js';
  import Home from '$pages/Home.svelte';
  import About from '$pages/About.svelte';

  const routes = {
    '/': Home,
    '/about': About
  };

  $: Component = routes[$route] || Home;
</script>

<svelte:component this="{Component}" />