import axios from "axios";

let errorHandler:(errorMsg: string) => void = () => {};

export const setErrorHandler = (handler:(errorMsg: string) => void) => {
  errorHandler = handler;
};


export const api = axios.create({

	baseURL: "http://localhost:5103/api",
	timeout: 3000,
	headers: {
		"Content-Type": "application/json"
	}
})

api.interceptors.request.use(
	(config) => {
		const token = localStorage.getItem("token");
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	},
	(error) => {
		return Promise.reject(error);
	}
);

api.interceptors.response.use(
	(response) => {

		return response;
	},
	(error) => {
		if( error.status == 400){
			errorHandler(error.response.data.errorMessage);
		}
		if( error.status == 401){
			errorHandler("Hozzáférés megtagadva");
		}
		return Promise.reject(error);
	}
)


