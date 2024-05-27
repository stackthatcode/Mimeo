import axios from 'axios';
import { AxiosResponse, AxiosRequestConfig } from 'axios';
import { useMainStore } from "@/stores/mainStore";


const optionsFactory = (method: string, url: string,  params: any) : AxiosRequestConfig => {
  return {
    headers: {
      'X-RapidAPI-Key': '464fa87404mshc372dcf1912bac8p1f17b8jsnb2be4527daf6',
      'X-RapidAPI-Host': 'famous-quotes4.p.rapidapi.com',
    },
    baseURL: 'https://famous-quotes4.p.rapidapi.com',
    method: method,
    params: params,
    url: url,
  }
};

const mainStore = useMainStore();

axios.interceptors.request.use((config) => {
  mainStore.spinnerOn();
  return config;
}, (error) => {
  mainStore.spinnerOff();
  return Promise.reject(error);
});

axios.interceptors.response.use((config) => {
  mainStore.spinnerOff();
  return config;
}, (error) => {
  mainStore.spinnerOff();
  return Promise.reject(error);
});




const invokeApi = function<T>(method: string, url: string, params: any) : Promise<T> {
  const config : AxiosRequestConfig = optionsFactory(method, url, params);

  return axios
    .request(config)
    .then(( { data } : { data : T } ) => {
      // throw "Oh noes!";
      //
      return data;
    })
    .catch(exception => {
      console.error(exception);
      alert("An error has occured")
      return (null as unknown as T);
    });
}


export interface QuoteApiResponse {
	text: string;
	author: string;
	category: string;
	id: number;
}

export interface QuoteApiParams {
  category: string,
  count: number,
}

export default {
  getQuote: function(params: QuoteApiParams): Promise<QuoteApiResponse> {
    return invokeApi<QuoteApiResponse>("GET", "/random", params);
  }
}

