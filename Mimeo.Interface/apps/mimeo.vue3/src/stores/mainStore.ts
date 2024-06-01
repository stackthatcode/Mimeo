import { defineStore } from 'pinia'


export interface mainStoreState {
  ajaxVisible: Boolean,
  testDialogVisible: Boolean,
  name: String,
}

export const useMainStore = defineStore('mainStore', {
  state: (): mainStoreState => {
    return {
      ajaxVisible: false,
      testDialogVisible: false,
      name: 'Eduardo'
    };
  },

  getters: {
    spinnerVisible: (state) => state.ajaxVisible,
    //testDialogVisible: (state) => state.testDialogVisible,
  },

  actions: {
    spinnerOn(): void {
      console.log("spinnerOn");
      this.ajaxVisible = true;
    },

    spinnerOff(): void {
      console.log("spinnerOff");
      this.ajaxVisible = false;
    },

    showTestDialog(): void {
      console.log("showTestDialog");
      this.testDialogVisible = true;
    },

    hideTestDialog(): void {
      console.log("hideTestDialog");
      this.testDialogVisible = false;
    }
  },
})

