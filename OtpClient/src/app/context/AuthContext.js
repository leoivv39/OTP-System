"use client";

import React, { createContext, useContext } from "react";
import AuthService from "../services/AuthService";
import { useState } from "react";
import { useEffect } from "react";
import Cookies from "js-cookie";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [authToken, setAuthToken] = useState(null);
  const [otpToken, setOtpToken] = useState(null);
  const [authContextIsLoading, setAuthContextIsLoading] = useState(true);

  useEffect(() => {
    setStoredToken("auth_token", setAuthToken);
    setStoredToken("otp_token", setOtpToken);
    setAuthContextIsLoading(false);
  }, []);

  const setStoredToken = (tokenName, setToken) => {
    const storedToken = Cookies.get(tokenName);
    if (storedToken) {
      setToken(storedToken);
    }
  };

  const onLogin = async (username, password) => {
    const response = await AuthService.login(username, password);
    if (response.status != 200) {
      return response;
    }

    const token = response.data;
    saveTokenToCookie("auth_token", token);
    setAuthToken(token);
    return response;
  };

  const onGenerateOtp = async () => {
    return await AuthService.generateOtp(authToken);
  };

  const onLoginOtp = async (otp) => {
    const response = await AuthService.loginOtp(otp, authToken);
    if (response.status != 200) {
      return response;
    }

    const token = response.data;
    saveTokenToCookie("otp_token", token);
    setAuthToken(token);
    return response;
  };

  const saveTokenToCookie = (tokenName, token) => {
    Cookies.set(tokenName, token, {
      expires: 1,
      path: "/",
      sameSite: "strict",
    });
  };

  const onLogout = () => {
    Cookies.remove("auth_token");
    Cookies.remove("otp_token");
    setAuthToken(null);
    setOtpToken(null);
  };

  const onRegister = async (username, password, email) => {
    return await AuthService.register(username, password, email);
  };

  return (
    <AuthContext.Provider
      value={{
        authContextIsLoading,
        onLogin,
        onRegister,
        onLogout,
        onGenerateOtp,
        onLoginOtp,
        authToken,
        otpToken,
        setAuthToken,
        setOtpToken,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};
