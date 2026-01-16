import { Api } from './api';
import { isLoggedIn, permissions, showUserMenu } from '$stores/session.js';

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
  showUserMenu.set(false);

  const deviceToken = localStorage.getItem('deviceToken');
  if (deviceToken) {
    body.deviceToken = deviceToken;
  }

  try {
    const data = await Api.postJson(url, { body });
    return setLogin(data);
  } catch(err) {
    console.error(err);
    throw err;
  }
}

export function setLogin(data) {
  if (!data?.authorizationToken) {
    isLoggedIn.set(false);
    permissions.set([]);
    localStorage.removeItem('autoLoginToken');
    return;
  }

  Api.headers.Authorization = 'Bearer ' + data.authorizationToken;
  isLoggedIn.set(true);

  if (data.deviceToken) {
    localStorage.setItem('deviceToken', data.deviceToken);
  }
  
  if (data.autoLoginToken) {
    localStorage.setItem('autoLoginToken', data.autoLoginToken);
  } else {
    localStorage.removeItem('autoLoginToken');
  }

  if (data.attributes?.permissions) {
    permissions.set(data.attributes?.permissions ?? []);
  }

  return true;
}

export function logout() {
  localStorage.removeItem('autoLoginToken');
  isLoggedIn.set(false);
  showUserMenu.set(false);
  permissions.set([]);
  
  try {
    Api.postJson('/v1/logout');
  } catch(err) {
    console.error(err);
  }
  delete Api.headers.Authorization;
}
