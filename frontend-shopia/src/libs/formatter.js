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

export function getFormattedValue(data, property) {
  let value;
  if (property.getValue) {
    try {
      value = property.getValue(data, property);
    } catch (e) {
      console.error('Error in getValue for property', property, 'with row', data, e);
    }
  } else if (property.field) {
    try {
      value = getValue(data, property.field);
    } catch (e) {
      console.error('Error getting field value for property', property, 'with row', data, e);
    }
  }

  if (property.formatter) {
    try {
      value = property.formatter(value, data, property);
    } catch (e) {
      console.error('Error in formatter for property', property, 'with value', value, e);
    }
  }

  return value;
}