<script>
  import { showMenu } from '$stores/showMenu.js';
  import { isLoggedIn } from '$stores/session.js';
</script>

<h1>
  {#if $isLoggedIn}
    <button
      onclick={() => $showMenu = !$showMenu}
    >
      &#9776;
    </button>
  {/if}
  Shopia
</h1>

<style>
  h1 {
    display: flex;
    align-items: center;
    font-size: 1.5em;
    margin: 0;
    padding: 0.5em;
    background-color: var(--header-background-color);
    color: var(--header-text-color);
  }

  button {
    font-size: 1em;
    margin: 0 .5em 0 0;
    padding: 0;
    border-radius: 0;
    font-weight: bold;
  }
</style>
