import { Toaster, toast } from 'svelte-sonner';

export function pushNotification(message, type = 'default') {
  console.log(message);
  // @ts-ignore
  toast(message, { type });
}

export const NotificationContainer = Toaster;