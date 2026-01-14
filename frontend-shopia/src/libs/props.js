export function styleObjectToString(obj) {
  return Object.entries(obj)
    .map(([k, v]) =>
      `${k.replace(/[A-Z]/g, m => '-' + m.toLowerCase())}:${v}`
    )
    .join(';');
}

export function arrangeProps(
  props,
  { defaultValues, addValues } = { defaultValues: null, addValues: null }
) {
  props = { ...defaultValues, ...props };

  for (const [key, value] of Object.entries(addValues || {})) {
    if (typeof key === 'string') {
      props[key] = ((value ?? '') + ' ' + (props[key] ?? '')).trim();
    } else if (Array.isArray(key)) {
      props[key] = [...(value ?? []), ...(props[key] ?? [])];
    } else if (typeof key === 'object') {
      props[key] = { ...(value ?? {}), ...(props[key] ?? {}) };
    }
  }

  if (props.style && typeof props.style !== 'string')
    props.style = styleObjectToString(props.style ?? {});

  return props;
}