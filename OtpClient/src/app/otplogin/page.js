"use client";

import "../styles/globals.css";
import "../styles/OtpLoginPage.css";
import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import { useRef } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Mutex } from "async-mutex";

const mutex = new Mutex();

export default function OtpLoginPage() {
  const {
    authToken,
    authContextIsLoading,
    onGenerateOtp,
    onLoginOtp,
    setOtpToken,
  } = useAuth();
  const router = useRouter();
  const [otp, setOtp] = useState("");
  const [otpLoginInfo, setOtpLoginInfo] = useState({
    success: false,
    message: "",
  });
  const otpWasGenerated = useRef(false);

  useEffect(() => {
    if (!authContextIsLoading && !authToken) {
      router.push("/unauthorized");
    }
  }, [authContextIsLoading, authToken, router]);

  useEffect(() => {
    mutex
      .runExclusive(async () => {
        if (authContextIsLoading || otpWasGenerated.current) {
          return;
        }
        const response = await onGenerateOtp(authToken);
        if (response.status === 201) {
          toast.info(`Here is your password: ${response.data.otp}`);
          otpWasGenerated.current = true;
        } else {
          toast.error("Something went wrong.");
        }
      })
      .catch(console.error);
  }, [authContextIsLoading]);

  const onSendOtp = async (e) => {
    if (!otp) {
      setOtpLoginInfo({
        success: false,
        message: "Please enter the password.",
      });
      return;
    }

    const response = await onLoginOtp(otp);
    if (response.status == 400) {
      setOtpLoginInfo({
        success: false,
        message: getErrorMessage(response.data.code),
      });
    } else if (response.status == 200) {
      setOtpLoginInfo({
        success: true,
        message: "Login was successful.",
      });
      setOtpToken(response.data);
      router.push("/loggedin");
    } else {
      setOtpLoginInfo({
        success: false,
        message: "Something went wrong.",
      });
    }
  };

  const getErrorMessage = (responseCode) => {
    switch (responseCode) {
      case "invalid.otp":
        return "The password is invalid";
      case "expired.otp":
        return "The password has expired";
      case "consumed.otp":
        return "The password has already been consumed";
    }
  };

  if (authContextIsLoading) {
    return (
      <div className="center-container">
        <div className="loading-spinner"></div>
      </div>
    );
  }

  return (
    <div>
      <div className="center-container">
        <div className="form">
          <p>We've sent you a One Time Password. Please enter it below.</p>
          <input
            type="text"
            value={otp}
            onChange={(e) => setOtp(e.target.value)}
          />
          {otpLoginInfo.message.length > 0 && (
            <h2
              className={
                otpLoginInfo.success ? "success-message" : "error-message"
              }
            >
              {otpLoginInfo.message}
            </h2>
          )}
          <button className="btn" onClick={onSendOtp}>
            Send
          </button>
        </div>
      </div>
      <ToastContainer
        position="bottom-right"
        autoClose={false}
        hideProgressBar={true}
        closeOnClick={true}
        pauseOnHover={true}
        draggable={true}
        theme="light"
      />
    </div>
  );
}
