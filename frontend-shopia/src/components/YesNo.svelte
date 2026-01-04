<script>
  let {
    checked,
    labelOn = 'Sí',
    labelOff = 'No',
    onChange = null,
  } = $props();

  let isOn = $derived(!!checked);

  function handleChange(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    
    if (onChange) {
      onChange(evt.target.checked);
    } else {
      checked = evt.target.checked;
    }
  }
</script>

<label class="toggle">
  <input
    type="checkbox"
    checked={isOn}
    onchange={handleChange}
  />

  <span class="track">
    <span class="thumb"></span>
    <span class="text">
      {isOn ? labelOn : labelOff}
    </span>
  </span>
</label>

<style>
  .toggle {
    display: inline-flex;
    align-items: center;
    cursor: pointer;
    user-select: none;
    vertical-align: bottom;
  }

  .toggle input {
    display: none;
  }

  .track {
    position: relative;
    display: inline-block;
    align-items: center;
    background: #ccc;
    border-radius: 1em;
    padding: .1em;
    width: 2em;
    height: .9em;
    transition: background 0.2s ease;
    color: #fff;
  }

  .thumb {
    position: absolute;
    display: block;
    width: .8em;
    height: .8em;
    border-radius: 50%;
    background: #fff;
    box-shadow: 0 .1em .3em rgba(0,0,0,0.3);
    top: .13em;
    left: .15em;
    transition: left 0.2s ease;
  }

  .toggle input:checked + .track {
    background: #16a34a;
  }

  .toggle input:checked + .track .thumb {
    left: 1.2em;
    transition: left 0.2s ease;
  }

  .text {
    position: absolute;
    font-size: .7em;
    left: 1.5em;
    top: .14em;
    text-align: center;
    color: #444444;
    transition: left 0.2s ease;
  }

  .toggle input:checked + .track .text {
    transform: translateX(0);
    color: #e4e4e4;
    top: .1em;
    left: .4em;
    transition: top 0.2s ease;
    transition: left 0.2s ease;
  }
</style>
