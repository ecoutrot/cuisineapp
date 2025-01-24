import { IconType } from "react-icons/lib";
import { Link } from "react-router-dom";

function NavbarIcon({ path, isActive, icon: Icon, color }: { path: string; isActive: boolean; icon: IconType; color?: string }) {
  const className = isActive ? "mr-4 bg-" + (color ?? "indigo-600") + " text-white p-1 rounded-full" : "mr-4 bg-gray-500 text-white p-1 rounded-full hover:bg-gray-700";
  return (
    <Link to={path} className={className}>
      <Icon className="size-3" />
    </Link>
  );
}

export default NavbarIcon;
