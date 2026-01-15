import { Api } from './api';

export async function get(query, options) {
  options = {...options, query: {...options?.query, ...query}};
  var data = await Api.getJson('/v1/plan', options)
    if (!Array.isArray(data.rows)) {
      data.rows = [];
    }
        
  return data;
}

export async function getSingleForUuid(uuid, options) {
  var data = await get(null, {...options, path: uuid});
  if (!data?.rows?.length) {
    throw new Error('No existe el plan.');
  }

  if (data.rows.length > 1) {
    throw new Error(`Hay muchos planes ${data.rows.length}.`);
  }
  
  const row = data.rows[0];
  
  return row;
}