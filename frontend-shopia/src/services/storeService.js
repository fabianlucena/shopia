import { Api } from './api';

export async function add(data, options) {
  return await Api.postJson('/v1/store', {...options, body: data});
}

export async function get(query, options) {
  options = {...options, query: {...options?.query, ...query}};
  var data = await Api.getJson('/v1/store', options)
    if (!Array.isArray(data.rows)) {
      data.rows = [];
    }
        
  return data;
}

export async function getForCommerceUuid(commerceUuid, options) {
  return await get({ commerceUuid }, options);
}

export async function getSingleForUuid(uuid, options) {
  var data = await get(null, {...options, path: uuid});
  if (!data?.rows?.length) {
    throw new Error('No existe el local.');
  }

  if (data.rows.length > 1) {
    throw new Error(`Hay muchos locales ${data.rows.length}.`);
  }
  
  const row = data.rows[0];
  
  return row;
}

export async function updateForUuid(uuid, data, options) {
  return await Api.patchJson('/v1/store', {...options, path: uuid, body: data});
}

export async function deleteForUuid(uuid, options) {
  return await Api.deleteJson('/v1/store', {...options, path: uuid});
}
