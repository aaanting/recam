import "./LoginPage.css";

function LoginPage() {
  return (
    <main className="login-page">
      <div className="login-heading">
        <h1>Sign in to your account</h1>
        <div>Don't have and account? Register</div>
      </div>

      <form className="login-form" onSubmit={(event) => event.preventDefault()}>
        <label className="login-field">
          Email address
          <input id="email" name="email" type="email" required />
        </label>

        <label className="login-field">
          Password
          <input id="password" name="password" type="password" required />
        </label>

        <button className="login-button" type="submit">
          Sign in
        </button>
      </form>
    </main>
  );
}

export default LoginPage;
