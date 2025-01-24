import { ChangeEvent, useState } from "react";
import { MdUploadFile } from "react-icons/md";
import { fileSizeFormat } from "../../Helpers/Parsers";

function CuisineInputFile({
  defaultValue,
  name,
  accept,
  onChange,
  label,
  placeholder,
  classNames,
  disabled,
}: {
  defaultValue: string | null;
  name: string;
  accept?: string | null;
  onChange?: (value: File | null) => void;
  label: string | null;
  placeholder?: string | null;
  classNames?: string | null;
  disabled?: boolean | null;
}) {
  const [fileName, setFileName] = useState<string | null>(null);
  const [fileSize, setFileSize] = useState<string | null>(null);
  const isDisabled = disabled ?? false;
  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      onChange?.(e.target.files[0]);
      setFileName(e.target.files[0].name);
      setFileSize(fileSizeFormat(e.target.files[0].size));
    }
  };

  return (
    <>
      <div className="relative">
        <label
          title="Click to upload"
          htmlFor={name}
          className={`group flex cursor-pointer items-center gap-4 px-6 py-4 before:absolute before:inset-0 before:rounded-3xl before:border before:border-dashed before:border-gray-400/60 before:bg-gray-100 before:transition-transform before:duration-300 hover:before:border-indigo-500 active:duration-75 active:before:scale-95 ${classNames}`}
        >
          <div className="relative w-max">
            <MdUploadFile className="size-16 text-indigo-500" />
          </div>
          <div className="relative">
            <span className="relative block text-base font-semibold text-indigo-500">{fileName ? fileName : label}</span>
            <span className="mt-0.5 block text-sm text-gray-500">{fileSize}</span>
          </div>
        </label>
        <input
          hidden
          type="file"
          accept={accept ?? ""}
          defaultValue={defaultValue ?? ""}
          name={name}
          id={name}
          onChange={handleFileChange}
          className="peer w-full border-none bg-transparent placeholder:text-transparent focus:border-transparent focus:outline-none focus:ring-0"
          placeholder={placeholder ?? ""}
          disabled={isDisabled}
        />
        <input hidden type="file" name="button2" id="button2" />
      </div>
    </>
  );
}

export default CuisineInputFile;
