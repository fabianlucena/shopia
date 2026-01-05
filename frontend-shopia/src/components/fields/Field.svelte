<script>
  import RequiredIcon from '$icons/required.svelte';

  let {
    for: forAttr = '',
    label = '',
    required = false,
    children,
  } = $props();
</script>

<div class="field">
  <label for={forAttr}>
    {#if required}
      <RequiredIcon class="required" />
    {/if}
    {label}
  </label>
  <span class="field-control">
    {@render children()}
  </span>
</div>

<style >
  .field {
    display: flex;
    flex-direction: column;
    padding: .5em .5em 0;
    background-color: var(--input-background-color);
    border-bottom: .15em solid var(--border-color);
  }

  label {
    font-size: 75%;
    opacity: .6;
    margin-bottom: 0.1em;
  }

  .field-control {
    width: 100%;
    display: flex;
    align-items: center;
    gap: .3em;
  }

  :global(.required) {
    color: red;
    font-weight: bold;
  }

  :global(
    .field input,
    .field textarea,
    .field select
  ) {
    flex: 1;
    font-family: inherit;
    font-size: 100%;
    background-color: transparent;
    border: none;
  }

  :global(
    .field input:focus,
    .field textarea:focus,
    .field select:focus
  ) {
    outline: none;
  }

  :global(.field textarea) {
    resize: vertical;
  }
</style>