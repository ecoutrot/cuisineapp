import { useEffect } from "react";
import { useLocation } from "react-router-dom";

const ScrollToHashElement = () => {
  const location = useLocation();
  useEffect(() => {
    if (location.hash) {
      const elementId = location.hash.slice(1);
      const observer = new MutationObserver(() => {
        const element = document.getElementById(elementId);
        if (element) {
          element.scrollIntoView({
            behavior: "smooth",
            inline: "nearest",
          });
          observer.disconnect();
        }
      });
      observer.observe(document.body, { childList: true, subtree: true });
      return () => observer.disconnect();
    }
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, [location]);

  return null;
};

export default ScrollToHashElement;
