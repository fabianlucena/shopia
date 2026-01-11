<script>
  import OkButton from '$components/buttons/Ok.svelte';
  import CancelButton from '$components/buttons/Cancel.svelte';

  let {
    image = null,
    onOk = null,
    onCancel = null,
    hotPlace = .2,
    aspectRatio = 0,
    defaultSelSize = .7,
    class : theClass = '',
    maxWidth = 0,
    maxHeight = 0,
    ...restProps
  } = $props();

  let name;
  let canvas;
  let ctx;
  let draw = { x: 0, y: 0, w: 0, h: 0, scale: 1 };
  let sel = { x: 50, y: 50, w: 200, h: 150 };
  let imageLoaded = false;
  let hotPlace1 = $derived(1 - hotPlace);

  $effect(() => {
    window.addEventListener('resize', windowResize);
    
    if (canvas) {
      canvas.width = canvas.clientWidth;
      canvas.height = canvas.clientHeight;

      if (!ctx) {
        ctx = canvas?.getContext('2d');
        if (!ctx) {
          return;
        }
      }

      adjustCanvas();
      if (image) {
        image.onload = () => imageLoadHandler();
        if (image.complete && image.naturalWidth > 0) {
          imageLoadHandler();
        }
      }

      canvas.addEventListener('pointerdown', pointerDownHandler);
      canvas.addEventListener('pointermove', pointerMoveHandler);
      canvas.addEventListener('pointerup', pointerUpHandler);
    }

    return () => {
      image && (image.onload = null);
      window.removeEventListener('resize', windowResize);
      if (canvas) {
        canvas.removeEventListener('pointerdown', pointerDownHandler);
        canvas.removeEventListener('pointermove', pointerMoveHandler);
        canvas.removeEventListener('pointerup', pointerUpHandler);
      }
    };
  });

  function windowResize() {
    adjustCanvas();

    if (imageLoaded) {
      const center = { x: sel.x + sel.w / 2, y: sel.y + sel.h / 2 };
      const rel = {
        cx: (center.x - draw.x) / draw.w,
        cy: (center.y - draw.y) / draw.h,
        rw: sel.w / draw.w,
        rh: sel.h / draw.h
      };

      adjustCanvas();
      fitImage();

      sel.w = rel.rw * draw.w;
      sel.h = rel.rh * draw.h;
      sel.x = draw.x + rel.cx * draw.w - sel.w / 2;
      sel.y = draw.y + rel.cy * draw.h - sel.h / 2;

      clampSelectionToImage();
    }

    drawScene();
  }

  function adjustCanvas() {
    if (!canvas)
      return;

    const rect = canvas.getBoundingClientRect();
    const dpr = window.devicePixelRatio || 1;
    canvas.width = Math.max(1, Math.floor(rect.width * dpr));
    canvas.height = Math.max(1, Math.floor((rect.width * 0.6) * dpr));

    if (!ctx)
      return;

    ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
  }

  function imageLoadHandler() {
    name = image.dataset?.['name'];
    imageLoaded = true;
    adjustCanvas();
    fitImage();
    clampSelectionToImage();
    drawScene();
  }

  function fitImage() {
    if (!imageLoaded || !image)
      return;

    const cw = canvas.getBoundingClientRect().width;
    const ch = canvas.getBoundingClientRect().height;

    const s = Math.min(cw / image.width, ch / image.height);
    const w = image.width * s;
    const h = image.height * s;
    const x = (cw - w) / 2;
    const y = (ch - h) / 2;

    draw = { x, y, w, h, scale: s };
    
    const currSelSize = defaultSelSize > 0 ? defaultSelSize : 0.7;
    sel.w = Math.min(300, w * currSelSize);
    sel.h = Math.min(200, h * currSelSize);
    clampSelectionToImage()
    sel.x = x + (w - sel.w) / 2;
    sel.y = y + (h - sel.h) / 2;
  }
  
  function clampSelectionToImage() {
    const minX = draw.x;
    const minY = draw.y;
    const maxX = draw.x + draw.w;
    const maxY = draw.y + draw.h;

    if (sel.w < 1) sel.w = 1;
    if (sel.h < 1) sel.h = 1;

    if (sel.x < minX) sel.x = minX;
    if (sel.y < minY) sel.y = minY;

    if (sel.x + sel.w > maxX) sel.x = maxX - sel.w;
    if (sel.y + sel.h > maxY) sel.y = maxY - sel.h;

    if (aspectRatio > 0) {
      const targetH = sel.w / aspectRatio;
      if (targetH > sel.h) {
        sel.h = targetH;
        if (sel.y + sel.h > maxY) {
          sel.h = maxY - sel.y;
          sel.w = sel.h * aspectRatio;
        }
      } else {
        const targetW = sel.h * aspectRatio;
        if (targetW > sel.w) {
          sel.w = targetW;
          if (sel.x + sel.w > maxX) {
            sel.w = maxX - sel.x;
            sel.h = sel.w / aspectRatio;
          }
        }
      }
    }
  }

  function drawScene() {
    if (!ctx)
      return;

    const cw = canvas.getBoundingClientRect().width;
    const ch = canvas.getBoundingClientRect().height;

    ctx.clearRect(0, 0, cw, ch);

    if (!imageLoaded)
      return;

    ctx.drawImage(image, draw.x, draw.y, draw.w, draw.h);

    ctx.save();
    ctx.fillStyle = 'rgba(0,0,0,0.55)';
    ctx.fillRect(0, 0, cw, ch);
    ctx.beginPath();
    ctx.rect(sel.x, sel.y, sel.w, sel.h);
    ctx.clip();
    ctx.clearRect(sel.x, sel.y, sel.w, sel.h);
    ctx.drawImage(image, draw.x, draw.y, draw.w, draw.h);
    ctx.restore();

    ctx.save();
    ctx.strokeStyle = 'rgba(240,240,240,0.75)';
    ctx.lineWidth = 2;
    ctx.strokeRect(sel.x, sel.y, sel.w, sel.h);
    ctx.beginPath();
    ctx.moveTo(sel.x + sel.w * hotPlace, sel.y);
    ctx.lineTo(sel.x + sel.w * hotPlace, sel.y + sel.h);
    ctx.moveTo(sel.x + sel.w * hotPlace1, sel.y);
    ctx.lineTo(sel.x + sel.w * hotPlace1, sel.y + sel.h);
    ctx.moveTo(sel.x, sel.y + sel.h * hotPlace);
    ctx.lineTo(sel.x + sel.w, sel.y + sel.h * hotPlace);
    ctx.moveTo(sel.x, sel.y + sel.h * hotPlace1);
    ctx.lineTo(sel.x + sel.w, sel.y + sel.h * hotPlace1);
    ctx.stroke();
    ctx.restore();
  }

  let dragging = false;
  let dragMode = { new: true };
  let start = { x: 0, y: 0 };
  let selStart = { x: 0, y: 0, w: 0, h: 0 };
  let offset = { x: 0, y: 0 };

  function getDragMode(px, py) {
    if (!imageLoaded
      || sel.w <= 0 || sel.h <= 0
    )
      return { new: true };

    let x = px - sel.x;
    if (x < 0 || x > sel.w)
      return { new: true };

    let y = py - sel.y;
    if (y < 0 || y > sel.h)
      return { new: true };

    if (x <= sel.w * hotPlace) {
      if (y <= sel.h * hotPlace) {
        return { left: true, top: true };
      }

      if (y >= sel.h * hotPlace1) {
        return { left: true, bottom: true };
      }

      return { left: true, center: true };
    }
    
    if (x <= sel.w * hotPlace1) {
      if (y <= sel.h * hotPlace) {
        return { center: true, top: true };
      }

      if (y >= sel.h * hotPlace1) {
        return { center: true, bottom: true };
      }

      return { move: true };
    }

    if (y <= sel.h * hotPlace) {
      return { right: true, top: true };
    }

    if (y >= sel.h * hotPlace1) {
      return { right: true, bottom: true };
    }

    return { right: true, center: true };
  }

  function getPointerPos(evt) {
    const r = canvas.getBoundingClientRect();
    const x = (evt.clientX < r.left)? 0 : (evt.clientX > r.right)? r.width : (evt.clientX - r.left);
    const y = (evt.clientY < r.top)? 0 : (evt.clientY > r.bottom)? r.height : (evt.clientY - r.top);
    return { x, y };
  }

  function pointerDownHandler(evt) {
    if (!imageLoaded)
      return;

    canvas.setPointerCapture(evt.pointerId);

    const p = getPointerPos(evt);
    start = p;
    selStart = {...sel};

    // @ts-ignore
    dragMode = getDragMode(p.x, p.y);
    dragging = true;

    offset.x = p.x - sel.x;
    offset.y = p.y - sel.y;

    drawScene();
  }

  function pointerMoveHandler(evt) {
    if (!imageLoaded)
      return;

    const p = getPointerPos(evt);
    const dm = getDragMode(p.x, p.y);

    if (!dragging) {
      // @ts-ignore
      const cursor = dm.move? 'move'
        : dm.new? 'crosshair'
        : dm.left && dm.top? 'nwse-resize'
        : dm.left && dm.bottom? 'nesw-resize'
        : dm.right && dm.top? 'nesw-resize'
        : dm.right && dm.bottom? 'nwse-resize'
        : dm.left || dm.right? 'ew-resize'
        : dm.top || dm.bottom? 'ns-resize'
        : 'default';

      canvas.style.cursor = cursor;
      return;
    }

    if (!dragging)
      return;

    if (dragMode.move) {
      sel.x = p.x - offset.x;
      sel.y = p.y - offset.y;
    } else if (dragMode.new) {
      sel.x = Math.min(start.x, p.x);
      sel.y = Math.min(start.y, p.y);
      sel.w = Math.abs(p.x - start.x);
      sel.h = Math.abs(p.y - start.y);
    } else {
      if (dragMode.left) {
        sel.x = p.x - offset.x;
        if (sel.x < draw.x) {
          sel.x = draw.x;
        }

        sel.w = selStart.w - (sel.x - selStart.x);
        if (sel.w < 1) {
          sel.w = 1;
        }
      } else if (dragMode.right) {
        sel.w = selStart.w - (start.x - p.x);
        const maxW = draw.w + draw.x - sel.x;
        if (sel.w > maxW)
          sel.w = maxW;
      }

      if (dragMode.top) {
        sel.y = p.y - offset.y;
        if (sel.y < draw.y) {
          sel.y = draw.y;
        }

        sel.h = selStart.h - (sel.y - selStart.y);
        if (sel.h < 1) {
          sel.h = 1;
        }
      } else if (dragMode.bottom) {
        sel.h = selStart.h - (start.y - p.y);
        const maxH = draw.h + draw.y - sel.y;
        if (sel.h > maxH)
          sel.h = maxH;
      }
    }

    clampSelectionToImage();
    drawScene();
  }

  function pointerUpHandler(evt) {
    dragging = false;
  }

  function okHandler(evt) {
    evt.stopPropagation();
    evt.preventDefault();

    if (!imageLoaded)
      return;

    // Convertir selección (canvas) -> coordenadas en la imagen original
    // La imagen se dibuja escalada en draw.{x,y,w,h} con escala draw.scale
    const sx = (sel.x - draw.x) / draw.scale;
    const sy = (sel.y - draw.y) / draw.scale;
    const sw = sel.w / draw.scale;
    const sh = sel.h / draw.scale;

    // Clamp por seguridad
    const cx = Math.max(0, Math.min(image.width, sx));
    const cy = Math.max(0, Math.min(image.height, sy));
    const cw = Math.max(1, Math.min(image.width - cx, sw));
    const ch = Math.max(1, Math.min(image.height - cy, sh));
    let ow = cw;
    let oh = ch;

    if (maxWidth > 0 && cw > maxWidth) {
      const ratio = maxWidth / ow;
      ow *= ratio;
      oh *= ratio;
    }
    
    if (maxHeight > 0 && ch > maxHeight) {
      const ratio = maxHeight / oh;
      ow *= ratio;
      oh *= ratio;
    }

    const out = document.createElement('canvas');
    out.width = Math.floor(ow);
    out.height = Math.floor(oh);
    out.getContext('2d')
      .drawImage(image, cx, cy, cw, ch, 0, 0, out.width, out.height);

    out.toBlob(blob => {
      if (!blob)
        return;

      onOk?.({
        blob,
        name: name.includes('.')?
          name.substring(0, name.lastIndexOf('.')) + '.png' :
          name + '.png',
        type: 'image/png',
      });
    }, 'image/png');
  }
  
  function cancelHandler(evt) {
    evt.stopPropagation();
    evt.preventDefault();

    onCancel?.(null);
  }
</script>

<div
  class={`${theClass} edit-image-control`}
  {...restProps}
>
  <canvas
    bind:this={canvas}
    style="border:1px solid #ccc; width:100%; touch-action:none;"
  ></canvas>
  <div
    class="button-bar"
  >
    <CancelButton
      class="edit-image-button"
      onClick={cancelHandler}
    />
    <OkButton
      class="edit-image-button"
      onClick={okHandler}
    />
  </div>
</div>

<style>
  .edit-image-control {
    position: relative;
  }

  .button-bar {
    position: absolute;
    top: .2em;
    right: .2em;
    font-size: 1.2em;
  }

  :global(button.edit-image-button) {
    height: 1.5em !important;
    background-color: rgba(192, 192, 192, 0.5) !important;
    border-radius: 0.5em !important;
    padding: 0.2em !important;
  }
</style>