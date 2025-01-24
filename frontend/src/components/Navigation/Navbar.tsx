import { useState } from "react";
import { FaBars, FaChevronDown, FaPlus, FaTimes, FaUser } from "react-icons/fa";
import { GiCook } from "react-icons/gi";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../Auth/AuthProvider";
import { fetchForLogout } from "../../services/Auth";
import NavbarIcon from "./NavbarIcon";
import NavbarLink from "./NavbarLink";

function Navbar() {
  const { token, logout } = useAuth();
  const navigate = useNavigate();

  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isManageOpen, setIsManageOpen] = useState(false);
  const [isCompteOpen, setIsCompteOpen] = useState(false);
  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };
  const toggleManage = () => {
    setIsManageOpen(!isManageOpen);
    setIsCompteOpen(false);
  };
  const toggleCompte = () => {
    setIsCompteOpen(!isCompteOpen);
    setIsManageOpen(false);
  };
  const setAllFalse = () => {
    setIsMenuOpen(false);
    setIsManageOpen(false);
    setIsCompteOpen(false);
  };

  const location = useLocation();

  const handleLogout = async (e: { preventDefault: () => void; }) => {
    e.preventDefault();
    await fetchForLogout();
    logout();
    void navigate("/");
  };

  return (
    <div className="bg-indigo-500">
      <nav className="relative flex items-center justify-between bg-white p-4">
        <Link to="/" className="text-3xl font-bold leading-none">
          <div className="flex items-center gap-4 text-indigo-600">
            <GiCook className="size-7 xl:size-8" />
            <span className="text-2xl xl:text-3xl">Livre de cuisine</span>
          </div>
        </Link>
          {!token ? (
            <>
                <Link className="p-3 text-indigo-600" to="/login">
                  <FaUser className="size-6"/>
                </Link>
            </>
          ) : (
            <>
            <button className="p-3 text-indigo-600 lg:hidden" onClick={toggleMenu}>
              {isMenuOpen ? <FaTimes className="size-6" /> : <FaBars className="size-6" />}
            </button>
            <ul
              className={`absolute left-0 top-16 w-full bg-white pr-5 text-right transition-transform duration-300 ease-in-out lg:static lg:flex lg:w-auto lg:items-center lg:space-x-6 lg:bg-transparent lg:pr-0 ${
                isMenuOpen ? "z-50" : "hidden"
              }`}
            >
                  <li className="mt-3 flex items-center gap-0 lg:mt-0" onClick={setAllFalse}>
                    <NavbarLink path="/recipes" label="Recettes" isActive={location.pathname.startsWith("/recipes")} />
                    <NavbarIcon path="/recipes/create" icon={FaPlus} isActive={location.pathname.startsWith("/recipes/create")} />
                  </li>
                  <li className="group relative">
                    <button onClick={toggleManage} className="ml-auto mr-0 flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-800">
                      Gestion <FaChevronDown className={`transition-transform ${isManageOpen ? "rotate-180" : ""}`} />
                    </button>
                    {isManageOpen && (
                      <ul className="absolute right-0 z-20 mt-2 w-48 rounded-md bg-white shadow-lg">
                        <li className="flex items-center justify-between px-4 py-2 hover:bg-gray-100" onClick={setAllFalse}>
                          <NavbarLink path="/ingredients" label="Ingrédients" isActive={location.pathname.startsWith("/ingredients")} />
                          <NavbarIcon path="/ingredients/create" icon={FaPlus} isActive={location.pathname.startsWith("/ingredients/create")} />
                        </li>
                        <li className="flex items-center justify-between px-4 py-2 hover:bg-gray-100" onClick={setAllFalse}>
                          <NavbarLink path="/recipeCategories" label="Catégories" isActive={location.pathname.startsWith("/recipeCategories")} />
                          <NavbarIcon path="/recipeCategories/create" icon={FaPlus} isActive={location.pathname.startsWith("/recipeCategories/create")} />
                        </li>
                        <li className="flex items-center justify-between px-4 py-2 hover:bg-gray-100" onClick={setAllFalse}>
                          <NavbarLink path="/units" label="Unités" isActive={location.pathname.startsWith("/units")} />
                          <NavbarIcon path="/units/create" icon={FaPlus} isActive={location.pathname.startsWith("/units/create")} />
                        </li>
                      </ul>
                    )}
                  </li>
                  <li className="group relative">
                    <button onClick={toggleCompte} className="ml-auto mr-0 flex items-center gap-2 px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-800">
                      Compte <FaChevronDown className={`transition-transform ${isCompteOpen ? "rotate-180" : ""}`} />
                    </button>
                    {isCompteOpen && (
                      <ul className="absolute right-0 z-20 mt-2 w-60 rounded-md bg-white shadow-lg">
                        <li className="flex items-center justify-between px-4 py-2 hover:bg-gray-100" onClick={setAllFalse}>
                          <Link className="ml-auto mr-0 block px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-800" to="/password-change">
                            Changer de mot de passe
                          </Link>
                        </li>
                        <li className="flex items-center justify-between px-4 py-2 hover:bg-gray-100" onClick={setAllFalse}>
                          <Link className="ml-auto mr-0 block px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-800" onClick={(e) => { void handleLogout(e); }} to="#">
                            Deconnexion
                          </Link>
                        </li>
                      </ul>
                    )}
                  </li>
            </ul>
            </>
          )}
      </nav>
      {isMenuOpen && <div className="fixed inset-0 z-10 bg-gray-800/25" onClick={toggleMenu}></div>}
    </div>
  );
}

export default Navbar;
