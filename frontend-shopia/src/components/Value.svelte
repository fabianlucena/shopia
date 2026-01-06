<script>
  import Switch from './controls/Switch.svelte';
  import TagsView from './controls/TagsView.svelte';

  let {
    data,
    options,
    getValue,
    onChange = null,
  } = $props();
</script>

{#snippet control({ data, options, value })}
  {#if options.control === 'switch'}
    <Switch
      {value}
      onChange={value => {
        onChange?.({
          data,
          options,
          value,
        });
      }}
    />
  {:else if options.control === 'tagsView'}
    <TagsView
      {value}
    />
  {:else}
    <strong>No {options.control} Control</strong>
  {/if}
{/snippet}

{#if options.render}
  {@render options.render({ data, options, value: getValue({ data, options }) })}
{:else if options.control}
  {@render control({ data, options, value: getValue({ data, options }) })}
{:else}
  {getValue({ data, options })}
{/if}