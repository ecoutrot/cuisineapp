export const toStringOrNull = (value: string | null) => {
  return value === "" ? null : value;
};

export const toIntOrNull = (value: string | null) => {
  const parse = parseInt(value as string, 10);
  return isNaN(parse) ? null : parse;
};

export const toFloatOrNull = (value: string | null) => {
  const parse = parseFloat(value as string);
  return isNaN(parse) ? null : parse;
};

export const fileSizeFormat = (fileSize: number) => {
  const fileSizeFormat = new Intl.NumberFormat("fr-FR", { maximumFractionDigits: 2 });
  if (fileSize > 1073741824) {
    return `${fileSizeFormat.format(fileSize / 1073741824)} Go`;
  } else if (fileSize > 1048576) {
    return `${fileSizeFormat.format(fileSize / 1048576)} Mo`;
  } else if (fileSize > 1024) {
    return `${fileSizeFormat.format(fileSize / 1024)} Ko`;
  }
  return `${fileSizeFormat.format(fileSize)} o`;
};
