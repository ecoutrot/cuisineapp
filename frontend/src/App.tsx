import { Outlet } from "react-router-dom";
import "./App.css";
import Navbar from "./components/Navigation/Navbar";
import AuthProvider from "./components/Auth/AuthProvider";
import ScrollToHashElement from "./components/Navigation/ScrollToHashElement";

function App() {
  return (
    <>
      <AuthProvider>
        <ScrollToHashElement />
        <Navbar />
        <Outlet />
      </AuthProvider>
    </>
  );
}

export default App;
