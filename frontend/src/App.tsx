import { useState } from "react";
import { BrowserRouter, Navigate, Route, Routes } from "react-router";
import DashboardPage from "./pages/DashboardPage";
import Header from "./components/Header";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";

function App() {
  const [signedIn, setSignedIn] = useState(() => Boolean(sessionStorage.getItem("token")));

  function handleLogout() {
    sessionStorage.removeItem("token");
    setSignedIn(false);
  }

  const loginPage = signedIn
    ? <Navigate to="/dashboard" replace />
    : <LoginPage onLogin={() => setSignedIn(true)} />;

  return (
    <BrowserRouter>
      <Header signedIn={signedIn} onLogout={handleLogout} />
      <Routes>
        <Route path="/" element={loginPage} />
        <Route path="/login" element={loginPage} />
        <Route
          path="/dashboard"
          element={signedIn ? <DashboardPage /> : <Navigate to="/login" replace />}
        />
        <Route path="/register" element={<RegisterPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
