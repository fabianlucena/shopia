<script>
  import { writable } from 'svelte/store';
  import * as myPlanService from '$services/myPlanService.js';
  import { money, percent } from '$libs/formatter.js';

  const limits = [
    {
      label: 'Comercios totales',
      limitKey: 'maxTotalCommerces',
      usedKey: 'totalCommercesCount',
    },
    {
      label: 'Comercios habilitados',
      limitKey: 'maxEnabledCommerces',
      usedKey: 'enabledCommercesCount',
    },
    {
      label: 'Tamaño máximo de imagen para comercios',
      limitKey: 'maxCommerceImageSize',
    },
    {
      label: 'Imágenes totales por comercio',
      limitKey: 'maxTotalImagesPerSingleCommerce',
    },
    {
      label: 'Imágenes totales de comercios',
      limitKey: 'maxTotalCommercesImages',
      usedKey: 'totalCommercesImagesCount',
    },
    {
      label: 'Imágenes habilitadas de comercios',
      limitKey: 'maxEnabledCommercesImages',
      usedKey: 'enabledCommercesImagesCount',
    },
    {
      label: 'Tamaño total de imágenes de comercios',
      limitKey: 'maxCommercesImagesAggregatedSize',
      usedKey: 'commercesImagesAggregatedSize',
    },
    {
      label: 'Tamaño total de imágenes habilitadas de comercios',
      limitKey: 'maxEnabledCommercesImagesAggregatedSize',
      usedKey: 'enabledCommercesImagesAggregatedSize',
    },
    {
      label: 'Locales totales',
      limitKey: 'maxTotalStores',
      usedKey: 'totalStoresCount',
    },
    {
      label: 'Locales habilitados',
      limitKey: 'maxEnabledStores',
      usedKey: 'enabledStoresCount',
    },
    {
      label: 'Artículos totales',
      limitKey: 'maxTotalItems',
      usedKey: 'totalItemsCount',
    },
    {
      label: 'Artículos habilitados',
      limitKey: 'maxEnabledItems',
      usedKey: 'enabledItemsCount',
    },
    {
      label: 'Tamaño máximo de imagen para artículos',
      limitKey: 'maxItemImageSize',
    },
    {
      label: 'Imágenes totales por artículo',
      limitKey: 'maxTotalImagesPerSingleItem',
    },
    {
      label: 'Imágenes totales de artículos',
      limitKey: 'maxTotalItemsImages',
      usedKey: 'totalItemsImagesCount',
    },
    {
      label: 'Imágenes habilitadas de artículos',
      limitKey: 'maxEnabledItemsImages',
      usedKey: 'enabledItemsImagesCount',
    },
    {
      label: 'Tamaño total máximo de imágenes de artículos',
      limitKey: 'maxItemsImagesAggregatedSize',
      usedKey: 'itemsImagesAggregatedSize',
    },
    {
      label: 'Tamaño total máximo de imágenes habilitadas de artículos',
      limitKey: 'maxEnabledItemsImagesAggregatedSize',
      usedKey: 'enabledItemsImagesAggregatedSize',
    }
  ];

  let data = writable({
    name: '',
    description: '',
    price: 0,
    limits: {},
    used: {},
  });

  $effect(() => {
    myPlanService.getSingle()
      .then(resData => {
        data.update(d => ({ ...d, ...resData }));
      });
  });
</script>

<div class="name">{$data.name}</div>
<div class="description">{$data.description}</div>
<div class="price">Precio: {money($data.price)}</div>
<div
  class="limits"
>
  {#each limits as limit (limit.limitKey) }
    <div class="limit">
      <div class="label">{limit.label}</div>
      <div class="value">
        {#if limit.usedKey}
          <span class="used">{$data.used[limit.usedKey] ?? '...'}</span>
          /
        {/if}
        <span class="max">{$data.limits[limit.limitKey] ?? '...'}</span>
      </div>
      {#if limit.usedKey}
      <div class="value">
          {percent($data.used[limit.usedKey] / $data.limits[limit.limitKey])}
      </div>
      {/if}
    </div>
  {/each}
</div>

<style>
  .name {
    font-weight: bold;
    font-size: 1.5em;
    margin-bottom: 0.2em;
  }

  .description {
    font-style: italic;
    margin-bottom: 1em;
  }

  .limits {
    display: flex;
    flex-wrap: wrap;
    gap: 1em;
    justify-content: center;
    overflow: auto;
  }

  .limit {
    border: .15em solid var(--border-color);
    border-radius: 0.5em;
    margin-bottom: 0.5em;
    width: 8em;
    padding: 0.5em;
  }

  .label {
    font-weight: bold;
    font-size: .8em;
    text-align: center;
  }

  .value {
    font-size: .8em;
    text-align: center;
    display: block;
    margin-top: 0.3em;
  }
</style>