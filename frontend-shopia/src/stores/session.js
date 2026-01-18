import { writable, get } from 'svelte/store';
import * as commerceService from '$services/commerceService.js';

export const isLoggedIn = writable(false);
export const showUserMenu = writable(false);
export const showMainMenu = writable(false);
export const permissions = writable([]);
export const myCommerces = writable([]);
export const selectedMyCommerce = writable(null);
export const selectedMyCommerceUuid = writable(null);
export function reloadMyCommerces() {
  commerceService.getMyCommerces()
    .then(commerces => {
      myCommerces.set(commerces.rows);

      if (commerces.rows.length === 1) {
        selectedMyCommerce.set(commerces.rows[0]);
        selectedMyCommerceUuid.set(commerces.rows[0].uuid);
      } else {
        selectedMyCommerce.set(null);
        selectedMyCommerceUuid.set(null);
      }
    });
}

selectedMyCommerceUuid.subscribe(uuid => {
  if (uuid) {
    const commerce = get(myCommerces).find(c => c.uuid === uuid);
    selectedMyCommerce.set(commerce);
  } else {
    selectedMyCommerce.set(null);
  }
});

isLoggedIn.subscribe(async (value) => {
  if (value) {
    reloadMyCommerces();
  } else {
    myCommerces.set([]);
    selectedMyCommerce.set(null);
    selectedMyCommerceUuid.set(null);
  }
});
