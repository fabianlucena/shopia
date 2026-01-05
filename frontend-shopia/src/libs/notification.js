import { toast } from 'svelte-sonner';

export function pushNotification(message, type = 'default') {
  if (type === 'success') {
    console.log(message);
    toast.success(message);
  } else if (type === 'error') {
    console.error(message);
    toast.error(message);
  } else if (type === 'info') {
    console.log(message);
    toast.info(message, { duration: 4000 });
  } else if (type === 'warning') {
    console.warn(message);
    toast.warning(message, { duration: 4000 });
  } else {
    console.log(message);
    toast(message);
  }
}