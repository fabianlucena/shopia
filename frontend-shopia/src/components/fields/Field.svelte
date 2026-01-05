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
  {@render children()}
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
    font-size: 80%;
    opacity: .6;
    margin-bottom: 0.1em;
  }

  :global(.required) {
    color: red;
    font-weight: bold;
  }

  :global(.field input) {
    font-size: 110%;
    background-color: transparent;
    border: none;
  }

  :global(.field input:focus) {
    outline: none;
  }
</style>