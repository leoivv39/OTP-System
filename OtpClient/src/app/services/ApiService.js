import axios from "axios";

export default class ApiService {
  static async sendRequest(url, data = null, method) {
    const apiClient = axios.create({
      headers: {
        "Content-Type": "application/json",
      },
    });
    return await this.sendRequestWithApiClient(url, data, method, apiClient);
  }

  static async sendRequestWithToken(url, token, data = null, method) {
    const apiClient = axios.create({
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });
    return await this.sendRequestWithApiClient(url, data, method, apiClient);
  }

  static async sendRequestWithApiClient(url, data = null, method, apiClient) {
    try {
      let response;
      switch (method.toUpperCase()) {
        case "POST":
          response = await apiClient.post(url, data);
          break;
        case "PUT":
          response = await apiClient.put(url, data);
          break;
        case "DELETE":
          response = await apiClient.delete(url, data);
          break;
        case "GET":
        default:
          response = await apiClient.get(url, data);
          break;
      }

      return {
        status: response.status,
        data: response.data,
      };
    } catch (error) {
      return {
        status: error.response.status,
        data: error.response.data,
      };
    }
  }
}
