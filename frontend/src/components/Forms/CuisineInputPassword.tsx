function CuisineInputPassword({
  name,
  onChange,
  label,
  placeholder,
  classNames,
  disabled,
}: {
  name: string;
  onChange?: (value: string) => void;
  label: string | null;
  placeholder?: string | null;
  classNames?: string | null;
  disabled?: boolean | null;
}) {
  const isDisabled = disabled ?? false;

  return (
    <>
      <label className={`relative my-4 block rounded-md border border-gray-200 shadow-sm focus-within:border-blue-600 focus-within:ring-1 focus-within:ring-blue-600 ${classNames}`}>
        <input
          type="password"
          name={name}
          onBlur={(e) => onChange && onChange(e.target.value)}
          className="peer w-full border-none bg-transparent p-2 placeholder:text-transparent focus:border-transparent focus:outline-none focus:ring-0"
          placeholder={placeholder ?? ""}
          disabled={isDisabled}
        />
        {label && (
          <span
            className={`pointer-events-none absolute start-2.5 top-0 -translate-y-1/2 ${
              isDisabled ? "bg-gray-100" : "bg-white"
            } p-0.5 text-xs text-gray-700 transition-all peer-placeholder-shown:top-1/2 peer-placeholder-shown:text-sm peer-focus:top-0 peer-focus:text-xs`}
          >
            {label}
          </span>
        )}
      </label>
    </>
  );
}

export default CuisineInputPassword;
