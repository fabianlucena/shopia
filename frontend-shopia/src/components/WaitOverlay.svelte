<script>
  import LoadingIcon from '$icons/loading.svelte';
  import { styleObjectToString } from '$libs/props.js';

  let {
    class : theClass = '',
    style = {},
    show = true,
    children = null,
    ...restProps
  } = $props();
</script>

<div 
  {...restProps}
  class={'overlay ' + theClass}
  style={styleObjectToString({
    display: show ? '' : 'none',
    ...style
  })}
>
  <LoadingIcon />
  {#if children}
    <div class="message">
      {@render children?.()}
    </div>
  {/if}
</div>

<style>
  .overlay {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(128, 128, 128, 0.3);
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    gap: 2em;
    backdrop-filter: blur(2px);
    font-size: 1.5em;
    color: var(--text-color);
    z-index: 3;
  } 
</style>