import React, { forwardRef, useContext } from "react";

import { SearchIcon } from "./Icons";
import { SelectContext } from "./SelectProvider";

interface SearchInputProps {
  placeholder?: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  name?: string;
}

const SearchInput = forwardRef<HTMLInputElement, SearchInputProps>(function SearchInput({ placeholder = "", value = "", onChange, name = "" }, ref) {
  const { classNames } = useContext(SelectContext);
  return (
    <div className={classNames && classNames.searchContainer ? classNames.searchContainer : "relative px-2.5 py-1"}>
      <SearchIcon className={classNames && classNames.searchIcon ? classNames.searchIcon : "absolute ml-2 mt-2.5 size-5 pb-0.5 text-gray-500"} />
      <input
        autoComplete="off"
        ref={ref}
        className={
          classNames && classNames.searchBox
            ? classNames.searchBox
            : "w-full rounded border border-gray-200 bg-gray-100 py-2 pl-8 text-sm text-gray-500 focus:border-gray-200 focus:outline-none focus:ring-0"
        }
        type="search"
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        name={name}
      />
    </div>
  );
});

export default SearchInput;
