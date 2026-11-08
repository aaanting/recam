import { useNavigate } from "react-router";
import logo from "../assets/logo.svg";
import logoutIcon from "../assets/logout.svg";
import "./Header.css";

type HeaderProps = {
  signedIn: boolean;
  onLogout: () => void;
};

function Header({ signedIn, onLogout }: HeaderProps) {
  const navigate = useNavigate();

  function handleLogout() {
    onLogout();
    navigate("/login", { replace: true });
  }

  return (
    <header className="header">
      <img src={logo} alt="RECAM" className="header-logo" />
      {signedIn && (
        <button
          className="header-logout"
          type="button"
          aria-label="Sign out"
          onClick={handleLogout}
        >
          <img src={logoutIcon} alt="logout" className="logout-btn" />
        </button>
      )}
    </header>
  );
}

export default Header;
