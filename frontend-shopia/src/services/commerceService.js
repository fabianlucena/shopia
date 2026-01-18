import { Api } from './api';

export async function add(data, options) {
  return await Api.postJson('/v1/commerce', {...options, body: data});
}

export async function get(query, options) {
  options = {...options, query: {...options?.query, ...query}};
  var data = await Api.getJson('/v1/commerce', options)
  if (!Array.isArray(data.rows))
    data.rows = [];
        
  return data;
}

export async function getAllForSelect(query, options) {
  const data = await get(query, options);
  return data.rows.map(row => ({
    label: row.name,
    value: row.uuid,
  }));
}

export async function getMyCommerces(options) {
  return await get({mine: true}, options);
}

export async function getSingleForUuid(uuid, options) {
  var data = await get(null, {...options, path: uuid});
  if (!data?.rows?.length) {
    throw new Error('No existe el comercio.');
  }

  if (data.rows.length > 1) {
    throw new Error(`Hay muchos comercios ${data.rows.length}.`);
  }
  
  const row = data.rows[0];
  
  return row;
}

export async function updateForUuid(uuid, data, options) {
  return await Api.patchJson('/v1/commerce', {...options, path: uuid, body: data});
}

export async function deleteForUuid(uuid, options) {
  return await Api.deleteJson('/v1/commerce', {...options, path: uuid});
}
