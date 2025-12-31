import { route } from '$stores/route.js';

export function navigate(path) {
  history.pushState({}, '', path);
  route.set(path);
}

window.addEventListener('popstate', () => route.set(window.location.pathname));