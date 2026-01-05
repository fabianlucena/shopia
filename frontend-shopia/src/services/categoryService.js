import { Api } from './api.js';

export async function get(query, options) {
  options = {...options, query: {...options?.query, ...query}};
  var data = await Api.getJson('/v1/category', options)
    if (!Array.isArray(data.rows)) {
      data.rows = [];
    }
        
  return data;
}

export async function getAllForSelect(query, options) {
  const data = await get(query, options);
  return data.rows.map(row => ({
    label: row.name,
    value: row.uuid,
  }));
}