import axios from 'axios';
import { AxiosRequestConfig } from 'axios';
import { useMainStore } from "@/stores/mainStore";


export interface AxiosRequestFactory { (method: string, url: string,  params: any): AxiosRequestConfig }

// const exampleRequestFactory: AxiosRequestFactory = (method: string, url: string,  params: any) : AxiosRequestConfig => {
//   return {
//     baseURL: 'https://localhost',
//     method: method,
//     params: params,
//     url: url,
//   }
// };

const mainStore = useMainStore();

export class AjaxInvoke {

  private RequestFactory: AxiosRequestFactory;
  private SpinnerOnOverride?: { (): void };
  private SpinnerOffOverride?: { (): void };

  constructor(
      requestConfigFactory: AxiosRequestFactory,
      spinnerOnOverride?: () => void,
      spinnerOffOverride?: () => void) {

    this.RequestFactory = requestConfigFactory;
    this.SpinnerOnOverride = spinnerOnOverride;
    this.SpinnerOffOverride = spinnerOffOverride;
  }

  SpinnerOn() {
    this.SpinnerOnOverride?.();
    if (this.SpinnerOnOverride === undefined) {
      mainStore.globalSpinnerShow();
    }
  }

  SpinnerOff() {
    this.SpinnerOffOverride?.();
    if (this.SpinnerOffOverride === undefined) {
      mainStore.globalSpinnerHide();
    }
  }

  Call<T>(method: string, url: string, params: any) : Promise<T> {
    const config : AxiosRequestConfig = this.RequestFactory(method, url, params);
    this.SpinnerOn();

    return axios
      .request(config)
      .then(( { data } : { data : T } ) => {

        this.SpinnerOff();
        return data;
      })
      .catch(exception => {
        this.SpinnerOff();
        console.error(exception);

        // ## TODO - we can filter through handling 401's, 403's, and 500's separately i.e. when they time for UX.
        //
        mainStore.errorPopupShow(exception);
        return (null as unknown as T);
      });
  }
}

