import logo from '../assets/logo.svg'
import './Header.css'

function Header() {
  return (
    <header className="header">
      <img src={logo} alt="RECAM" className="header-logo" />
    </header>
  )
}

export default Header
