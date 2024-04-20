"use client";

import React from "react";
import { useAuth } from "../context/AuthContext";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import "../styles/globals.css";

export default function LoggedInPage() {
  const { authToken, otpToken, authContextIsLoading, onLogout } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (!authContextIsLoading && (!authToken || !otpToken)) {
      router.push("/unauthorized");
    }
  }, [authContextIsLoading]);

  const handleLogout = (e) => {
    onLogout();
    router.push("/login");
  };

  return (
    <div className="center-container">
      <h2>You are now logged in!</h2>
      <button className="btn" style={{ width: 100 }} onClick={handleLogout}>
        Log Out
      </button>
    </div>
  );
}
