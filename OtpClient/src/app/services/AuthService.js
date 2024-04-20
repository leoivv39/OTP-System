import ApiService from "./ApiService";
import configData from "../../../config.json";

const BASE_URL = configData.AUTH_URL;

class AuthService {
  static async login(username, password) {
    return await ApiService.sendRequest(
      `${BASE_URL}/login`,
      {
        username,
        password,
      },
      "POST"
    );
  }

  static async register(username, password, email) {
    return await ApiService.sendRequest(
      `${BASE_URL}/`,
      {
        username,
        password,
        email,
      },
      "POST"
    );
  }

  static async loginOtp(otp, authToken) {
    return await ApiService.sendRequestWithToken(
      `${BASE_URL}/otp/login`,
      authToken,
      {
        otp,
      },
      "POST"
    );
  }

  static async generateOtp(authToken) {
    return await ApiService.sendRequestWithToken(
      `${BASE_URL}/otp`,
      authToken,
      null,
      "POST"
    );
  }
}

export default AuthService;
