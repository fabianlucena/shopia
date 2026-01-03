import { writable } from 'svelte/store';

const defaultData =  {
  header: null,
  headerText: 'Confirmar acción',
  body: null,
  message: '¿Estás seguro de que deseas continuar con esta acción?',
  confirmText: 'Confirmar',
  cancelText: 'Cancelar',
  onConfirm: () => {},
  onClose: () => {},
};

export const confirmModal = writable({
  open: false,
  header: null,
  headerText: 'Confirmar acción',
  body: null,
  message: '¿Estás seguro de que deseas continuar con esta acción?',
  confirmText: 'Confirmar',
  cancelText: 'Cancelar',
  onConfirm: () => {},
  onClose: () => {},
});

export async function confirm(data) {
  confirmModal.update(d => ({
    ...d,
    ...defaultData,
    ...data,
    open: true,
  }));
}