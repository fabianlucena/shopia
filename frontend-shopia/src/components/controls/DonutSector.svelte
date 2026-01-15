<script>
    import { writable } from "svelte/store";

  let {
    ratio = 0.65,
    size = 120,
    stroke = .20,
    color = '#4caf50',
    bg = 'rgba(221, 221, 221, 0.5)',
    getColor = null,
    getBGColor = null,
    ...restProps
  } = $props();

  let r = writable(0);
  let c = writable(0);

  $effect(() => {
    const newR = size * (1 - stroke) / 2;
    r.set(newR);
    c.set(2 * Math.PI * newR);
  });
</script>

<svg
  width={size}
  height={size}
  viewBox={`0 0 ${size} ${size}`}
  {...restProps}
>
  <!-- Fondo del anillo -->
  <circle
    cx={size/2}
    cy={size/2}
    r={$r}
    stroke={getBGColor?.(ratio) ?? bg}
    stroke-width={size * stroke}
    fill="none"
  />

  <!-- Sector del porcentaje -->
  <circle
    cx={size/2}
    cy={size/2}
    r={$r}
    stroke={getColor?.(ratio) ?? color}
    stroke-width={size * stroke}
    fill="none"
    stroke-linecap="round"
    transform={`rotate(-90 ${size/2} ${size/2})`}
    stroke-dasharray={$c}
    stroke-dashoffset={$c * (1 - ratio)}
  />
</svg>