import { Api } from './api';
import { isLoggedIn, permissions } from '$stores/session.js';

export async function autoLogin() {
  const autoLoginToken = localStorage.getItem('autoLoginToken');
  if (!autoLoginToken) {
    return false;
  }

  await _login('/v1/auto-login', {autoLoginToken});
  return true;
}

export async function login(body) {
  return await _login('/v1/login', body);
}

export async function _login(url, body) {
  const deviceToken = localStorage.getItem('deviceToken');
  if (deviceToken) {
    body.deviceToken = deviceToken;
  }

  try {
    const data = await Api.postJson(url, { body });
    if (!data?.authorizationToken) {
      isLoggedIn.set(false);
      return;
    }

    Api.headers.Authorization = 'Bearer ' + data.authorizationToken;
    isLoggedIn.set(true);

    if (data.deviceToken) {
      localStorage.setItem('deviceToken', data.deviceToken);
    }
    
    if (data.autoLoginToken) {
      localStorage.setItem('autoLoginToken', data.autoLoginToken);
    }

    if (data.attributes?.permissions) {
      permissions.set(data.attributes?.permissions);
    }

    return true;
  } catch(err) {
    console.error(err);
    throw err;
  }
}

export function logout() {
  localStorage.removeItem('autoLoginToken');
  isLoggedIn.set(false);
  
  try {
    Api.postJson('/v1/logout');
  } catch(err) {
    console.error(err);
  }
  delete Api.headers.Authorization;
}
