<script>
  let {
    open = false,
    children = null,
    header = null,
    footer = null,
  } = $props();
</script>

{#if open}
  <div class="backdrop">
    <div class="dialog">
      {#if header}
        <div class="header">
          {@render header?.()}
        </div>
      {/if}
      <div class="body">
        {@render children?.()}
      </div>
      {#if footer}
        <div class="footer">
          {@render footer?.()}
        </div>
      {/if}
    </div>
  </div>
{/if}

<style>
  .backdrop {
    z-index: 3;
    position: fixed;
    inset: 0;
    background: rgba(32,32,32,0.5);
    backdrop-filter: blur(.25em);
  }

  .dialog {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background: var(--background-color);
    border: .25em solid var(--header-background-color);
    border-radius: 1em;
    min-width: 10em;
    width: 30em;
    max-width: 90vw;
  }

  .header {
    background: var(--header-background-color);
    color: var(--header-text-color);
    padding: 0.75rem 1rem;
    font-size: 1.25rem;
    font-weight: bold;
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-top: 0;
  }

  .body {
    padding: 1.5rem;
  }

  .footer {
    padding: .5em 1em;
    background: var(--input-background-color);
    display: flex;
    justify-content: flex-end;
    gap: .5em;
    border-bottom-left-radius: 1em;
    border-bottom-right-radius: 1em;
  }
</style>
