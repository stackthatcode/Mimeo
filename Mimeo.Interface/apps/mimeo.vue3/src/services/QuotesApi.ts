import { AjaxInvoke, AxiosRequestFactory } from '@/services/AjaxInvoke';
import { AxiosRequestConfig } from 'axios';


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

// NOTE: Saving API Keys on the client side he is so bad. Shame!
//
const requestFactory: AxiosRequestFactory = (method: string, url: string,  params: any) : AxiosRequestConfig => {
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

const ajaxInvoke: AjaxInvoke = new AjaxInvoke(requestFactory);

export default {
  getQuote: function(params: QuoteApiParams): Promise<QuoteApiResponse> {
    //return ajaxInvoke.Call<QuoteApiResponse>("GET", "/random", params);

    return ajaxInvoke.Call<QuoteApiResponse>("GET", "/random", params);

  }
}

