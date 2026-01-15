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