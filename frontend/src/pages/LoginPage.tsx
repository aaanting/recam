import { Link } from "react-router";
import "./LoginPage.css";
import { useState } from "react";
import { loginUser } from "../services/AuthService";

type LoginPageProps = {
  onLogin: () => void;
};

function LoginPage({ onLogin }: LoginPageProps) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");

  async function handleLogin() {
    // Check inputs
    if (email.trim() === "" || password === "") {
      setError("Please enter your email and password.");
      return;
    }

    setSubmitting(true);
    setError("");

    try {
      const response = await loginUser(email, password);

      if (response.status === 401) {
        setError("Incorrect email or password.");
        return;
      }

      if (response.status === 400) {
        setError("Enter a valid email address and password.");
        return;
      }

      if (!response.ok) {
        setError("Login failed.");
        return;
      }

      const result = await response.json();

      if (!result.token || typeof result.token !== "string") {
        setError("The server did not return a sign-in token.");
        return;
      }

      sessionStorage.setItem("token", result.token);

      setPassword("");
      onLogin();
    } catch {
      setError("Sign in failed.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <main className="login-page">
      <div className="login-heading">
        <h1>Sign in to your account</h1>

        <p>
          Don't have an account? <Link to="/register">Register</Link>
        </p>
      </div>

      <form
        className="login-form"
        onSubmit={(e) => {
          e.preventDefault();
          handleLogin();
        }}
      >
        <label className="login-field">
          Email address
          <input
            id="email"
            name="email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            autoComplete="username"
            required
          />
        </label>

        <label className="login-field">
          Password
          <input
            id="password"
            name="password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            autoComplete="current-password"
            required
          />
        </label>

        {error && <p className="errorMsg">{error}</p>}

        <button className="login-button" type="submit" disabled={submitting}>
          {submitting ? "Signing in..." : "Sign in"}
        </button>
      </form>
    </main>
  );
}

export default LoginPage;
