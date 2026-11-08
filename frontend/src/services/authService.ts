// Login API
export async function loginUser(email: string, password: string) {
  const response = await fetch("/api/Auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      email: email.trim(),
      password: password,
    }),
  });

  return response;
}

// Register API
export async function registerUser(
  companyName: string,
  email: string,
  password: string,
  confirmPassword: string,
) {
  const response = await fetch("/api/Auth/register", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      photographyCompanyName: companyName,
      email: email,
      password: password,
      confirmPassword: confirmPassword,
    }),
  });

  return response;
}
