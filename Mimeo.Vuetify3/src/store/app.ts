// Utilities
//
import { defineStore } from 'pinia'
import { ref, computed, reactive } from "vue";

// #1 - this is our first go at it
//
// export const useAppStore = defineStore('app', () => {
//   const count = ref(123);
//   const doubleCount = computed(() => {
//     return count.value * 2;
//   });
//
// Action
//   function increment() {
//     count.value++;
//   }
// });


// #2 - straight from the examples.
//
// ... Uh, okay, why did I lose Intellisense. Maybe because the lambda method does not yet have Intellisense support
// ... in Visual Studio Code?
//
export const useAppStore = defineStore('app', {
  state: () => ({ count: 0, name: 'Eduardo' }),
  getters: {
    doubleCount: (state) => state.count * 2,
  },
  actions: {
    increment() {
      this.count++
    },
  },
})

