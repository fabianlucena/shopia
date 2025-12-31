<script>
  let {
    for: forAttr = '',
    label = '',
    children,
  } = $props();
</script>

<div class="field">
  <label for={forAttr}>
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

  :global(.field input) {
    font-size: 110%;
    background-color: transparent;
    border: none;
  }

  :global(.field input:focus) {
    outline: none;
  }
</style>