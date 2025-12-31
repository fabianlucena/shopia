import { writable } from 'svelte/store';

export const isLoggedIn = writable(null);
export const permissions = writable(null);