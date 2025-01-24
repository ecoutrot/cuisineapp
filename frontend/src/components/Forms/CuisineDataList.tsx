import { useState } from "react";
import { Item } from "../../types/Item";
import Select from "../CustomSelect";
import { SelectValue } from "../CustomSelect/components/type";

function CuisineDataList({
  items,
  selectedValue,
  name,
  onChange,
  label,
  placeholder,
  isSearchable,
  classNames,
  disabled,
}: {
  items: Item[];
  selectedValue: string | null;
  name: string;
  onChange?: (value: string | null) => void;
  label: string | null;
  placeholder?: string | null;
  isSearchable: boolean;
  classNames?: string | null;
  disabled?: boolean | null;
}) {
  const [isItemSelected, setIsItemSelected] = useState(false);
  const item = items.find((item) => item.value === selectedValue);
  const isDisabled = disabled ?? false;

  const handleChange = (selectValue: SelectValue) => {
    if (selectValue && typeof selectValue === "object" && "value" in selectValue) {
      if (onChange) {
        onChange(selectValue.value);
        setIsItemSelected(true);
      }
    } else {
      if (onChange) {
        onChange(null);
        setIsItemSelected(false);
      }
    }
  };

  return (
    <>
      <label className={`relative my-4 block rounded-md border border-gray-200 shadow-sm focus-within:border-blue-600 focus-within:ring-1 focus-within:ring-blue-600 ${classNames}`}>
        <Select
          isDisabled={isDisabled}
          primaryColor="blue"
          placeholder={placeholder ?? ""}
          isSearchable={isSearchable}
          value={item ?? null}
          onChange={handleChange}
          options={items}
          searchInputPlaceholder="Rechercher..."
        />
        <input type="hidden" name={name} value={selectedValue ?? ""} autoComplete="off" />
        {label && (
          <span
            className={`pointer-events-none absolute start-2.5 top-0 -translate-y-1/2 ${isDisabled ? "bg-gray-200" : "bg-white"} p-0.5 text-xs text-gray-700 transition-all  ${
              (item || isItemSelected) && !isDisabled ? "top-0 text-xs" : "top-1/2 text-sm"
            }`}
          >
            {label}
          </span>
        )}
      </label>
    </>
  );
}

export default CuisineDataList;
