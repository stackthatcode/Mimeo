import axios from 'axios';
import { AxiosResponse, AxiosRequestConfig } from 'axios';
import { useMainStore } from "@/stores/mainStore";



export default function<T>(
    method: string,
    url: string,
    params: any,
    spinnerOnOverride?: () => void,
    spinnerOffOverride?: () => void,
  ) : Promise<T> {

  const config : AxiosRequestConfig = requestFactory(method, url, params);
  const mainStore = useMainStore();

  spinnerOnOverride?.();
  if (spinnerOnOverride === undefined) {
    mainStore.globalSpinnerShow();
  }

  return axios
    .request(config)
    .then(( { data } : { data : T } ) => {

      throw "Oh noes!";

      spinnerOffOverride?.();
      if (spinnerOffOverride === undefined) {
        mainStore.globalSpinnerShow();
      }

      return data;
    })
    .catch(exception => {
      spinnerOffOverride?.();
      if (spinnerOffOverride === undefined) {
        mainStore.globalSpinnerShow();
      }

      console.error(exception);

      // TODO - we can filter through handling 401's, 403's, and 500's separately i.e. when they time for UX.
      //
      mainStore.errorPopupShow(exception);
      return (null as unknown as T);
    });
}

const requestFactory = (method: string, url: string,  params: any) : AxiosRequestConfig => {
  return {
    baseURL: 'https://localhost',
    method: method,
    params: params,
    url: url,
  }
};

