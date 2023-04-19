import axios from "axios";

const HttpMethods = {
  GET: "GET",
  POST: "POST",
  DELETE: "DELETE",
};
const _axios = axios.create();

const configure = (token?: string) => {
  _axios.interceptors.request.use((config) => {
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });
};

const getAxiosClient = () => _axios;

const HttpService = {
  HttpMethods,
  configure,
  getAxiosClient,
};

export default HttpService;
