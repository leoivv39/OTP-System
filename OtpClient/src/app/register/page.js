"use client";

import React from "react";
import { useState } from "react";
import "../styles/globals.css";
import { useAuth } from "../context/AuthContext";
import Link from "next/link";
import "../styles/RegisterPage.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";

export default function RegisterPage() {
  const { onRegister } = useAuth();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [registerInfo, setRegisterInfo] = useState({
    success: true,
    message: "",
  });

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!username || !email || !password) {
      setRegisterInfo({
        success: false,
        message: "Please complete all the fields.",
      });
      return;
    }

    const response = await onRegister(username, password, email);
    if (response.status == 201) {
      setRegisterInfo({
        success: true,
        message: "Registration successful!",
      });
      return;
    }

    if (response.status == 400) {
      const errors = response.data.errors;
      if (errors != undefined) {
        setRegisterInfo({
          success: false,
          message: [errors.Username, errors.Password, errors.Email].join("\n"),
        });
      } else {
        setRegisterInfo({
          success: false,
          message: response.data.message,
        });
      }
    }
  };

  return (
    <div className="center-container">
      <div className="form-container">
        <Link href="/login" className="back-link">
          <FontAwesomeIcon icon={faArrowLeft} />
        </Link>

        <form className="form" onSubmit={handleSubmit}>
          <h2>Create a new account</h2>
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
            <div className="form-group">
              <div>
                <label>Email</label>
              </div>
              <input
                type="text"
                id="email"
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
          </div>
          {registerInfo.message.length > 0 && (
            <h2
              className={
                registerInfo.success ? "success-message" : "error-message"
              }
            >
              {registerInfo.message}
            </h2>
          )}
          <button type="submit" className="btn">
            Register
          </button>
        </form>
      </div>
    </div>
  );
}
