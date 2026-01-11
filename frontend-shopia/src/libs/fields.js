export function getDataForSend({ fields, data, defaultData = {} }) {
  let sendData = {};
  let formData = null;
  for (var name in data) {
    const field = fields.find(f => f.name === name);
    if (!field)
      continue;

    const value = data[name];
    if (value === defaultData[name]) {
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
        sendData[field.deleteFieldName ?? 'delete' + name] = JSON.stringify(deleted);
      }
    } else {
      sendData[name] = value;
    }
  }

  if (formData) {
    for (var name in sendData) {
      formData.append(name, sendData[name]);
    }
  }

  return sendData;
}