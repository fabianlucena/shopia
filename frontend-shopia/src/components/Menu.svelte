<script>
  import { navigate } from '$libs/router.js';

  let {
    items = [],
    onclose = null,
    ...restProps
  } = $props();

  function handleClose(evt) {
    onclose?.();
  }

  $effect(() => {
    window.addEventListener('click', handleClose, true);

    return () => window.removeEventListener('click', handleClose, true);
  });

  function handleClick(itemName) {
    const item = items.find(i => i.name === itemName);
    if (!item) {
      console.error(`Menu item with name "${itemName}" not found.`);
      return;
    }

    if (item.method) {
      item.method();
    }

    if (item.path) {
      navigate(item.path);
    }

    onclose?.();
  }
</script>

<nav
  {...restProps}
>
  {#each items as item}
    <button
      onclick={() => handleClick(item.name)}
    >
      {item.label}
    </button>
  {/each}
</nav>

<style>
  nav {
    position: absolute;
    z-index: 2;
    top: 3rem;
    left: 0;
    display: flex;
    flex-direction: column;
    background-color: var(--menu-background-color);
  }

  button {
    background-color: var(--menu-item-background-color);
    color: var(--menu-item-color);
    border: none;
    border-radius: 0;
    padding: .5em 1em;
    margin: 0;
    text-align: left;
    cursor: pointer;
    font-size: 1rem;
  }

  button:hover {
    background-color: var(--menu-item-hover-background-color);
  }
</style>