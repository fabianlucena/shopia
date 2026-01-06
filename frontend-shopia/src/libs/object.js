export function getValue({ data, field, defaultValue = undefined }) {
  const keys = field.split('.');
  let result = data;
  for (const key of keys) {
    if (result && Object.prototype.hasOwnProperty.call(result, key)) {
      result = result[key];
    } else {
      return defaultValue;
    }
  }
  return result;
}