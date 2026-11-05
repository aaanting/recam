import { Link } from "react-router";
import "./LoginPage.css";

function RegisterPage() {
  return (
    <main className="login-page">
      <div className="login-heading">
        <h1>Signup Photography Company</h1>
        <p>
          Already have an account? <Link to="/login">Sign in</Link>
        </p>
      </div>

      <form className="login-form">
        <label className="login-field">
          Company Name
          <input name="photographyCompanyName" />
        </label>

        <label className="login-field">
          Email address
          <input name="email" type="email" />
        </label>

        <label className="login-field">
          Phone Number
          <input name="phoneNumber" type="tel" />
        </label>

        <label className="login-field">
          Password
          <input
            name="password"
            type="password"
            autoComplete="new-password"
            required
          />
        </label>

        <label className="login-field">
          Confirm password
          <input name="confirmPassword" type="password" required />
        </label>

        <label className="registration-terms">
          <input name="acceptTerms" type="checkbox" required />I agree to the
          Terms and Conditions
        </label>

        <button className="login-button" type="submit">
          Create Account
        </button>
      </form>
    </main>
  );
}

export default RegisterPage;
