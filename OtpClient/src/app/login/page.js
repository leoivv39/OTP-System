"use client";

import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import Link from "next/link";
import "../styles/LoginPage.css";
import { useRouter } from "next/navigation";

export default function LoginPage() {
  const { onLogin } = useAuth();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const router = useRouter();
  const [errorMessage, setErrorMessage] = useState("");

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!username || !password) {
      setErrorMessage("Please enter both username and password");
      return;
    }

    const response = await onLogin(username, password);
    if (response.status == 404) {
      setErrorMessage("Invalid username and/or password");
    } else if (response.status == 200) {
      setErrorMessage("");
      router.push("/otplogin");
    } else {
      setErrorMessage("Something went wrong.");
    }
  };

  return (
    <div className="center-container">
      <form className="form" onSubmit={handleSubmit}>
        <h2>Log in to your account</h2>
        <div className="form-group-container">
          <div className="form-group">
            <div>
              <label>Username</label>
            </div>
            <input
              type="text"
              id="username"
              onChange={(e) => setUsername(e.target.value)}
            />
          </div>
          <div className="form-group">
            <div>
              <label>Password</label>
            </div>
            <input
              type="password"
              id="password"
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
        </div>
        {errorMessage.length > 0 && (
          <h2 className="error-message">{errorMessage}</h2>
        )}
        <button type="submit" className="btn">
          Login
        </button>
        <div className="sign-up-text">
          Don't have an account?
          <Link href="/register" className="sign-up-link">
            {" "}
            Sign up
          </Link>
        </div>
      </form>
    </div>
  );
}
