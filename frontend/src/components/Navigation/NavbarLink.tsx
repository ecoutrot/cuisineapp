import { Link } from "react-router-dom";

function NavbarLink({ path, label, isActive, color }: { path: string; label: string; isActive: boolean; color?: string }) {
  const className = isActive
    ? "ml-auto mr-0 block py-2 px-4 text-sm text-" + (color ?? "indigo-600") + " font-bold"
    : "ml-auto mr-0 block py-2 px-4 text-sm font-medium text-gray-600 hover:text-gray-800";
  return (
    <Link className={className} to={path}>
      {label}
    </Link>
  );
}

export default NavbarLink;
