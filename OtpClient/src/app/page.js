"use client";

import { useRouter } from "next/navigation";
import { useEffect } from "react";
import { useAuth } from "./context/AuthContext";

export default function Home() {
  const { authToken, otpToken, authContextIsLoading } = useAuth();
  const router = useRouter();

  useEffect(() => {
    console.log(authToken);
    if (authToken && otpToken) {
      router.push("/loggedin");
    } else if (authToken) {
      router.push("/otplogin");
    } else {
      router.push("/login");
    }
  }, [authContextIsLoading]);

  return (
    <div className="center-container">
      <div className="loading-spinner"></div>
    </div>
  );
}
