export function getValue(obj, path, defaultValue = undefined) {
  const keys = path.split('.');
  let result = obj;
  for (const key of keys) {
    if (result && Object.prototype.hasOwnProperty.call(result, key)) {
      result = result[key];
    } else {
      return defaultValue;
    }
  }
  return result;
}