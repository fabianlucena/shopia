import { getValue } from '$libs/object.js';
  
export function money(value, { currency = 'ARS', locale = 'es-AR' } = {}) {
  let result = Number(value);
  if (Number.isNaN(result)) {
    return value;
  }
  
  // @ts-ignore
  result = new Intl.NumberFormat(locale, {
    style: 'currency',
    currency,
  }).format(result);

  return result;
}

export function percent(value, { locale = 'es-AR', fractionDigits = null } = {}) {
  let result = Number(value);
  if (Number.isNaN(result)) {
    return value;
  }

  if (fractionDigits === null) {
    if (value >= 1) {
      fractionDigits = 0;
    } else if (value < 0.1) {
      fractionDigits = 2;
    } else {
      fractionDigits = 1;
    }
  }
  
  // @ts-ignore
  result = new Intl.NumberFormat(locale, {
    style: 'percent',
    minimumFractionDigits: fractionDigits,
    maximumFractionDigits: fractionDigits,
  }).format(result);

  return result;
}

export function yesNo(value, { yes = 'Sí', no = 'No' } = {}) {
  if (value === true) {
    return yes;
  } else if (value === false) {
    return no;
  }

  return '';
}

export function getFormattedValue({data, options}) {
  let value;
  if (options.getValue) {
    try {
      value = options.getValue({
        data,
        field: options.field,
        value: data[options.field],
        options,
      });
    } catch (e) {
      console.error('Error in getValue for field', options, 'with row', data, e);
    }
  } else if (options.field) {
    try {
      value = getValue({data, field: options.field});
    } catch (e) {
      console.error('Error getting field value for field', options, 'with row', data, e);
    }
  }

  if (options.formatter) {
    try {
      value = options.formatter(value, data, options);
    } catch (e) {
      console.error('Error in formatter for field', options, 'with value', value, e);
    }
  }

  return value;
}

export function bytes(value, { locale = 'es-AR', fractionDigits = 2 } = {}) {
  let result = Number(value);
  if (Number.isNaN(result)) {
    return value;
  }

  const units = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
  let unitIndex = 0;
  while (result >= 1024 && unitIndex < units.length - 1) {
    result /= 1024;
    unitIndex++;
  }
  // @ts-ignore
  result = result.toFixed(fractionDigits);

  // @ts-ignore
  result = new Intl.NumberFormat(locale, {
    minimumFractionDigits: fractionDigits,
    maximumFractionDigits: fractionDigits,
  }).format(result);

  return `${result} ${units[unitIndex]}`;
}