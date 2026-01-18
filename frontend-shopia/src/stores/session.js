import { writable, get } from 'svelte/store';
import * as commerceService from '$services/commerceService.js';
import * as storeService from '$services/storeService.js';

export const isLoggedIn = writable(false);
export const showUserMenu = writable(false);
export const showMainMenu = writable(false);
export const permissions = writable([]);
export const myCommerces = writable([]);
export const mySelectedCommerce = writable(null);
export const mySelectedCommerceUuid = writable(null);

const mySelectedCommerceStores = writable(null);

export async function getMySelectedCommerceStores() {
  let result = get(mySelectedCommerceStores);
  if (result)
    return result;

  const commerceUuid = get(mySelectedCommerceUuid);
  if (!commerceUuid) {
    mySelectedCommerceStores.set([]);
    return [];
  }

  const data = await storeService.getForCommerceUuid(commerceUuid);
  result = data.rows;
  mySelectedCommerceStores.set(result);

  return result;
}

mySelectedCommerceUuid.subscribe(uuid => {
  if (uuid) {
    const commerce = get(myCommerces).find(c => c.uuid === uuid);
    mySelectedCommerce.set(commerce);
  } else {
    mySelectedCommerce.set(null);
  }

  mySelectedCommerceStores.set(null);
});

isLoggedIn.subscribe(async (value) => {
  if (value) {
    loadMyCommerces();
  } else {
    myCommerces.set([]);
    mySelectedCommerce.set(null);
    mySelectedCommerceUuid.set(null);
  }
});

export function loadMyCommerces() {
  commerceService.getMyCommerces()
    .then(commerces => {
      myCommerces.set(commerces.rows);

      if (commerces.rows.length === 1) {
        mySelectedCommerce.set(commerces.rows[0]);
        mySelectedCommerceUuid.set(commerces.rows[0].uuid);
      } else {
        const newMyCommerceSelected = commerces.rows.find(c => c.uuid === get(mySelectedCommerceUuid));
        if (newMyCommerceSelected) {
          mySelectedCommerce.set(newMyCommerceSelected);
          mySelectedCommerceUuid.set(newMyCommerceSelected.uuid);
        } else {
          mySelectedCommerce.set(null);
          mySelectedCommerceUuid.set(null);
        }
      }
    });
}