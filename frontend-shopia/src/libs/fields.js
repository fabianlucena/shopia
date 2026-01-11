export function getDataForSend({ fields, data, defaultData = {} }) {
  let sendData;
  if (fields.some(f => f.type === 'imageGallery' && data[f.name]?.length )) {
    sendData = new FormData();
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
            const thisName = rootName + index;
            sendData.append(thisName, image.blob, image.name);
          } else if (image.deleted) {
            if (image.uuid) {
              deleted.push(image.uuid);
            }
          }
        }

        if (deleted.length) {
          sendData.append(field.deleteFieldName ?? 'delete' + name, JSON.stringify(deleted));
        }
      } else {
        sendData.append(name, value);
      }
    }
  } else {
    for (var name in data) {
      const value = data[name];
      if (value !== defaultData[name]) {
        sendData = data;
      }
    }
  }

  return sendData;
}