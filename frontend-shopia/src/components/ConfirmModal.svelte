<script>
  import Modal from '$components/Modal.svelte';
  import Button from '$components/controls/Button.svelte';
  import ButtonClose from '$components/buttons/Close.svelte';
  import { confirmModal } from '$libs/confirm.js';

  function confirm() {
    $confirmModal.onConfirm?.();
    close();
  }

  function close() {
    $confirmModal.onClose?.();
    confirmModal.update(d => ({ ...d, open: false }));
  }

</script>

<Modal
  open={$confirmModal.open}
>
  {#snippet header()}
    {#if $confirmModal.header}
      {@render $confirmModal.header()}
    {:else}
      {$confirmModal.headerText}
      <ButtonClose onclick={close} />
    {/if}
  {/snippet}

  {#if $confirmModal.body}
    {@render $confirmModal.body()}
  {:else}
    <p>{$confirmModal.message}</p>
  {/if}

  {#snippet footer()}
    <Button variant="danger" onclick={confirm}>{$confirmModal.confirmText}</Button>
    <Button onclick={close}>{$confirmModal.cancelText}</Button>
  {/snippet}
</Modal>