import { Link } from "react-router";
import "./LoginPage.css";
import { useState } from "react";
import { registerUser } from "../services/AuthService";

function RegisterPage() {
  // Form inputs
  const [companyName, setCompanyName] = useState("");
  const [email, setEmail] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [acceptTerms, setAcceptTerms] = useState(false);

  // Form status
  const [registered, setRegistered] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");

  // Handle registration
  async function handleRegister() {
    // Check passwords
    if (password !== confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    // Check terms
    if (!acceptTerms) {
      setError("Please accept the Terms and Conditions.");
      return;
    }

    setSubmitting(true);
    setError("");

    try {
      const response = await registerUser(
        companyName,
        email,
        password,
        confirmPassword,
      );

      if (response.status === 201) {
        setRegistered(true);
      } else if (response.status === 400) {
        const errors = await response.json().catch(() => []);
        let errorMessage = "";

        if (Array.isArray(errors)) {
          for (const error of errors) {
            if (
              error.code === "DuplicateEmail" ||
              error.code === "DuplicateUserName"
            ) {
              errorMessage = "This email is already registered.";
              break;
            }

            if (error.errorMessage) {
              errorMessage = error.errorMessage;
              break;
            }

            if (error.description) {
              errorMessage = error.description;
              break;
            }
          }
        }

        setError(
          errorMessage || "Registration failed. Please check your details.",
        );
      } else {
        setError("Registration is unavailable. Please try again.");
      }
    } catch {
      setError("Server error.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <main className="login-page">
      <div className="login-heading">
        <h1>Signup Photography Company</h1>

        <p>
          Already have an account? <Link to="/login">Sign in</Link>
        </p>
      </div>

      {registered ? (
        <section className="login-form registration-success">
          <span className="registration-check">✓</span>

          <h2>Registration successful!</h2>

          <p>
            Your admin account has been created successfully. You can now sign
            in.
          </p>

          <Link className="login-button registration-sign-in" to="/login">
            Sign in
          </Link>
        </section>
      ) : (
        <form
          className="login-form"
          onSubmit={(e) => {
            e.preventDefault();
            handleRegister();
          }}
        >
          <label className="login-field">
            Company Name
            <input
              type="text"
              value={companyName}
              onChange={(e) => setCompanyName(e.target.value)}
            />
          </label>

          <label className="login-field">
            Email address
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </label>

          <label className="login-field">
            Phone Number
            <input
              type="tel"
              value={phoneNumber}
              onChange={(e) => setPhoneNumber(e.target.value)}
            />
          </label>

          <label className="login-field">
            Password
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              autoComplete="new-password"
              required
            />
          </label>

          <label className="login-field">
            Confirm password
            <input
              type="password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              autoComplete="new-password"
              required
            />
          </label>

          <label className="registration-terms">
            <input
              type="checkbox"
              checked={acceptTerms}
              onChange={(e) => setAcceptTerms(e.target.checked)}
              required
            />
            I agree to the Terms and Conditions
          </label>

          {error && <p className="errorMsg">{error}</p>}

          <button className="login-button" type="submit" disabled={submitting}>
            {submitting ? "Creating account..." : "Create Account"}
          </button>
        </form>
      )}
    </main>
  );
}

export default RegisterPage;
