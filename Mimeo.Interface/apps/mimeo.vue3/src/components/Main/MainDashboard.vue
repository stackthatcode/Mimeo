<template>
  <v-container>
    <h2 class="mt-10">Main Dashboard (TEST)</h2>

    <v-btn class="mt-5 mr-5" color="primary" @click="buttonCallback">
      AJAX Test!
    </v-btn>

    <v-btn class="mt-5 mr-5" color="secondary" @click="buttonCallback2">
      Spinner Test
    </v-btn>

    <v-btn class="mt-5 mr-5" color="orange" @click="buttonCallback3">
      Error Test
    </v-btn>

    <div class="mt-10">
      Global Spinner Visible: {{ mainStore.globalSpinnerVisible }}
    </div>

    <div v-if="mainStore.globalSpinnerVisible">
      <v-progress-circular indeterminate></v-progress-circular>
    </div>
  </v-container>
</template>

<script lang="ts" setup>
import QuotesApi from "@/services/QuotesApi";
import { QuoteApiResponse, QuoteApiParams } from "@/services/QuotesApi";
import { useMainStore } from "@/stores/mainStore";

const mainStore = useMainStore();

const buttonCallback3 = function (): void {
  try {
    throw new Error("Example exception");
  } catch (exception) {
    console.error(exception);
    mainStore.errorPopupShow(
      "AJAX error - please open dev tools to view the console."
    );
  }
};

const buttonCallback2 = function (): void {
  if (mainStore.globalSpinnerVisible) {
    mainStore.globalSpinnerHide();
  } else {
    mainStore.globalSpinnerShow();
  }
};

const buttonCallback = function (): Promise<QuoteApiResponse> {
  let parameters: QuoteApiParams = {
    category: "all",
    count: 5,
  };

  // ### this is an example of consuming AJAX method using with encapsulation and Typescript type-safety
  //
  return QuotesApi.getQuote(parameters).then((result) => {
    let test: QuoteApiResponse = result;
    console.log(test);
    return test;
  });
};
</script>

<style></style>
