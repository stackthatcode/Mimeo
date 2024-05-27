<template>
  <v-container>
    <h2 class="mt-10">Main Dashboard (TEST)</h2>

    <v-btn class="mt-5 mr-5" color="primary" @click="buttonCallback">
      AJAX Test!
    </v-btn>

    <v-btn class="mt-5 mr-5" color="secondary" @click="buttonCallback2">
      Spinner Test
    </v-btn>

    <div class="mt-10">Spinner Visible: {{ mainStore.spinnerVisible }}</div>

    <div v-if="mainStore.spinnerVisible">
      <v-progress-circular indeterminate></v-progress-circular>
    </div>
  </v-container>
</template>

<script lang="ts" setup>
import QuotesApi from "@/services/QuotesApi";
import { QuoteApiResponse, QuoteApiParams } from "@/services/QuotesApi";
import { useMainStore } from "@/stores/mainStore";

const mainStore = useMainStore();

const buttonCallback2 = function (): void {
  if (mainStore.spinnerVisible) {
    mainStore.spinnerOff();
  } else {
    mainStore.spinnerOn();
  }
};

const buttonCallback = function (): Promise<QuoteApiResponse> {
  let parameters: QuoteApiParams = {
    category: "all",
    count: 5,
  };

  // This is an example of consuming AJAX method using with encapsulation and Typescript type-safety
  //
  return QuotesApi.getQuote(parameters).then((result) => {
    let test: QuoteApiResponse = result;
    console.log(test);
    //alert("completed");
    return test;
  });
};
</script>

<style></style>
