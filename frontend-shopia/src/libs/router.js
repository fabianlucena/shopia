import { route } from '$stores/route.js';

export function navigate(path) {
  if (!isNaN(path)) {
    window.history.go(path);
    return;
  }

  history.pushState({}, '', path);
  route.set(path);
}

window.addEventListener('popstate', () => route.set(window.location.pathname));