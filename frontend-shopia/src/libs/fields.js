export function getDataForSend({ fields, data, defaultData = {} }) {
  let sendData = {};
  let formData = null;

  for (var name in data) {
    const field = fields.find(f => f.name === name);
    if (!field)
      continue;

    const value = data[name];
    if (JSON.stringify(value) === JSON.stringify(defaultData[name])) {
      continue;
    }

    if (field.type === 'imageGallery' && value?.length) {
      const rootName = name + '_';
      const deleted = [];
      for (var index in value) {
        const image = value[index];
        const url = image.url;
        if (!url)
          continue;

        if (image.added) {
          formData ??= new FormData();
          formData.append(rootName + index, image.blob, image.name);
        } else if (image.deleted) {
          if (image.uuid) {
            deleted.push(image.uuid);
          }
        }
      }

      if (deleted.length) {
        sendData[field.deleteFieldName ?? 'delete' + name] = deleted;
      }
    } else {
      sendData[name] = value;
    }
  }

  if (formData) {
    for (var name in sendData) {
      let value = sendData[name];
      if (Array.isArray(value) || typeof value === 'object') {
        value = JSON.stringify(value);
      }

      formData.append(name, value);
    }

    sendData = formData;
  }

  return sendData;
}