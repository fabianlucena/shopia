import { writable } from 'svelte/store';

export const isLoggedIn = writable(false);
export const showUserMenu = writable(false);
export const showMainMenu = writable(false);
export const permissions = writable([]);