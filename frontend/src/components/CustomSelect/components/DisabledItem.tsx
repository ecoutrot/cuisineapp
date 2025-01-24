import React, { JSX, useContext } from "react";

import { SelectContext } from "./SelectProvider";

interface DisabledItemProps {
  children: JSX.Element | string;
}

const DisabledItem: React.FC<DisabledItemProps> = ({ children }) => {
  const { classNames } = useContext(SelectContext);
  return <div className={classNames && classNames.listDisabledItem ? classNames.listDisabledItem : "cursor-not-allowed select-none truncate p-2 text-gray-400"}>{children}</div>;
};

export default DisabledItem;
