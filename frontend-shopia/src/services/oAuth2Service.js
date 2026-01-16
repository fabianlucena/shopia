import { Api } from './api';

export async function getProviders(query, options) {
  options = { ...options, query: {...options?.query, ...query}};
  var data = await Api.getJson('/v1/oauth2/providers', options)
  if (!Array.isArray(data.rows)) {
    data.rows = [];
  }
        
  return data;
}

export async function callback(name, action, query, options) {
  options = { ...options, query };
  return await Api.getJson(`/v1/oauth2/callback/${name}/${action}`, { query });
}