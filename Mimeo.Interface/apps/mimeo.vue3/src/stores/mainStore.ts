import { defineStore } from 'pinia'


interface mainStoreState {
  globalSpinnerVisible: Boolean,
  errorMessage: String | null,
  name: String,
}

export const useMainStore = defineStore('mainStore', {
  state: (): mainStoreState => {
    return {
      globalSpinnerVisible: false,
      errorMessage: null,
      name: 'Man With No Name'
    };
  },

  getters: {
    // Add computed or object graph-nested properties
    //
    errorDialogVisible: (state) => state.errorMessage && true,
  },

  actions: {
    globalSpinnerShow(): void {
      console.log("globalSpinnerShow");
      this.globalSpinnerVisible = true;
    },

    globalSpinnerHide(): void {
      console.log("globalSpinnerHide");
      this.globalSpinnerVisible = false;
    },

    errorPopupShow(errorMessage: String | null): void {
      console.log("errorPopupShow");
      this.errorMessage = errorMessage;

    },
    errorPopupHide(): void {
      console.log("errorPopupHide");
      this.errorMessage = null;
    },
  },
})

