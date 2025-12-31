import { Toaster, toast } from 'svelte-sonner';

export function pushNotification(message, type = 'default') {
  toast(message, { type });
}

export const NotificationContainer = Toaster;