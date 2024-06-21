import axios from 'axios';
import { AxiosResponse, AxiosRequestConfig } from 'axios';
import { useMainStore } from "@/stores/mainStore";

// v0.9 AJAX
//

// MainStore reference
//
const mainStore = useMainStore();



// This is a static / global reference to axios. This is poorly designed, as it only accounts for one AJAX request,
// which may be a safe assumption if I continue to abide with my practice of one AJAX call at a time i.e. none running
// in parallel.
//

axios.interceptors.request.use((config) => {
  mainStore.globalSpinnerShow();
  return config;
}, (error) => {
  mainStore.globalSpinnerHide();
  return Promise.reject(error);
});

axios.interceptors.response.use((config) => {
  mainStore.globalSpinnerHide();
  return config;
}, (error) => {
  mainStore.globalSpinnerHide();
  return Promise.reject(error);
});



// Saving API Keys on the client side. Shame!
//
const requestFactory = (method: string, url: string,  params: any) : AxiosRequestConfig => {
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


// TODO - replace with our centralized AJAX Service
//
const invokeApi = function<T>(method: string, url: string, params: any) : Promise<T> {
  const config : AxiosRequestConfig = requestFactory(method, url, params);

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

