
import { defineStore } from 'pinia'


interface mainStoreState {
  ajaxVisible: boolean,
  name: string
};

export const useMainStore = defineStore('mainStore', {
  state: (): mainStoreState => {
    return { ajaxVisible: false, name: 'Eduardo' };
  },

  getters: {
    spinnerVisible: (state) => state.ajaxVisible,
  },

  actions: {
    spinnerOn() {
      console.log("spinnerOn");
      this.ajaxVisible = true;
    },

    spinnerOff() {
      console.log("spinnerOff");
      this.ajaxVisible = false;
    },
  },
})

