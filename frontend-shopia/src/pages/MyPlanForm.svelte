<script>
  import { writable } from 'svelte/store';
  import * as myPlanService from '$services/myPlanService.js';
  import { money, percent, bytes } from '$libs/formatter.js';
  import DonutSector from '$components/controls/DonutSector.svelte';

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
      formatter: bytes,
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
      formatter: bytes,
    },
    {
      label: 'Tamaño total de imágenes habilitadas de comercios',
      limitKey: 'maxEnabledCommercesImagesAggregatedSize',
      usedKey: 'enabledCommercesImagesAggregatedSize',
      formatter: bytes,
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
      formatter: bytes,
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
      formatter: bytes,
    },
    {
      label: 'Tamaño total máximo de imágenes habilitadas de artículos',
      limitKey: 'maxEnabledItemsImagesAggregatedSize',
      usedKey: 'enabledItemsImagesAggregatedSize',
      formatter: bytes,
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
        data.set(resData);
      });
  });

  function getColor(ratio) {
    if (ratio < .5)
      return '#4caf50';

    if (ratio < .8)
      return '#ff9800';

    return '#f44336';
  }

  function getBGColor(ratio) {
    if (ratio < .5)
      return 'rgba(76, 175, 80, 0.3)';

    if (ratio < .8)
      return 'rgba(255, 152, 0, 0.3)';

    return 'rgba(244, 67, 54, 0.3)';
  }
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
      {#if limit.usedKey}
        <div class="percent">
          <div class="chart">
            <DonutSector
              ratio={($data.used[limit.usedKey] ?? 0) / ($data.limits[limit.limitKey] ?? 1)}
              size={80}
              stroke={.15}
              getColor={getColor}
              getBGColor={getBGColor}
            />
          </div>
          <div class="values">
            <div class="value">
              <span class="used">{limit.formatter?.($data.used[limit.usedKey]) ?? ($data.used[limit.usedKey] ?? '...')}</span>
              /
              <span class="max">{limit.formatter?.($data.limits[limit.limitKey]) ?? ($data.limits[limit.limitKey] ?? '...')}</span>
            </div>
            <div class="value">
              {percent($data.used[limit.usedKey] / $data.limits[limit.limitKey], { ifIsNaN: '...' })}
            </div>
          </div>
        </div>
      {:else}
        <div class="value">
          <span class="max">{limit.formatter?.($data.limits[limit.limitKey]) ?? ($data.limits[limit.limitKey] ?? '...')}</span>
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
    display: flex;
    flex-direction: column;
    align-items: center;
  }

  .label {
    font-weight: bold;
    font-size: .8em;
    text-align: center;
    flex: 1;
  }

  .value {
    font-size: .8em;
    text-align: center;
    display: block;
    margin-top: 0.3em;
  }

  .percent {
    position: relative;
    height: 5em;
    text-align: center;
    margin-top: .5em;
  }

  .chart {
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%, -50%);
  }

  .values {
    z-index: 1;
    position: relative;
    margin-top: 1.3em;
  }

  .value {
    font-size: .7em;
    font-weight: bold;
  }
</style>