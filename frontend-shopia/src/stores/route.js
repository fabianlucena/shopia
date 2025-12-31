import { writable } from 'svelte/store';

export const route = writable(window.location.pathname);