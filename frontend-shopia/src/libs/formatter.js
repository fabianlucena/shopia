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

export function number(
  value,
  {
    style = 'decimal',
    locale = 'es-AR',
    fractionDigits = null,
    ifIsNaN = undefined,
  } = {}
) {
  let num = Number(value);
  if (Number.isNaN(num)) {
    return ifIsNaN !== undefined ? ifIsNaN : value;
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

  let result = new Intl.NumberFormat(locale, {
    // @ts-ignore
    style,
    minimumFractionDigits: fractionDigits,
    maximumFractionDigits: fractionDigits,
  }).format(num);

  return result;
}

export function percent(value, options = {}) {
  let num = Number(value);
  if (Number.isNaN(num)) {
    // @ts-ignore
    return options.ifIsNaN !== undefined ? options.ifIsNaN : value;
  }

  options.style ??= 'percent';

  // @ts-ignore
  if (options.fractionDigits === undefined) {
    if (num > .1) {
      // @ts-ignore
      options.fractionDigits = 0;
    } else if (num > .01) {
      // @ts-ignore
      options.fractionDigits = 1;
    } else {
      // @ts-ignore
      options.fractionDigits = 2;
    }
  }

  return number(value, options);
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

export function unit(
  value,
  {
    multiplier = 1000,
    unit = '',
    ...options
  } = {}
) {
  let num = Number(value);
  if (Number.isNaN(num)) {
    // @ts-ignore
    return options.ifIsNaN !== undefined ? options.ifIsNaN : value;
  }

  let sign = '';
  if (num < 0)
  {
    sign = '-';
    num = -num;
  }

  let units = [''];
  let unitIndex = 0;
  if (num >= 1) {
    units = ['', 'K', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y'];
    while (num >= multiplier && unitIndex < units.length - 1) {
      num /= multiplier;
      unitIndex++;
    }
  } else if (num > 0) {
    units = ['', 'm', 'µ', 'n', 'p', 'f', 'a', 'z', 'y'];
    while (num < 1 && unitIndex < units.length - 1) {
      num *= multiplier;
      unitIndex++;
    }
  }

  // @ts-ignore
  if (options.fractionDigits === undefined) {
    if (num >= 100) {
      // @ts-ignore
      options.fractionDigits = 0;
    } else if (num > 10) {
      // @ts-ignore
      options.fractionDigits = 1;
    } else {
      // @ts-ignore
      options.fractionDigits = 2;
    }
  }

  let result = number(num, options);

  return `${result} ${sign}${units[unitIndex]}${unit}`;
}

export function bytes(
  value,
  options
) {
  return unit(value, { ...options, multiplier: 1000, unit: 'B' });
}

export function ibytes(
  value,
  options
) {
  return unit(value, { ...options, multiplier: 1024, unit: 'iB' });
}