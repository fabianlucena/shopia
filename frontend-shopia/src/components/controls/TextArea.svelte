<script>
  let {
    value = $bindable(''),
    ...props
  } = $props();

  function inputHandler(event) {
    props.oninput?.(event);
    props.onInput?.(event);

    if (!event.defaultPrevented) {
      value = event.target.value;
    }
  }
</script>

<textarea
  oninput={inputHandler}
  {...props}
>{value}</textarea>
