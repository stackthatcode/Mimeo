<template>
  <v-container class="fill-height">
    <v-responsive class="d-flex align-center text-center fill-height">
      <v-img contain height="100" src="@/assets/Function_fx_Logo.jpg" />

      <h4 class="text-h6 font-weight-bold">
        (Demonstration of v-responsive tag with fill-height enabled)
      </h4>

      <div class="py-3" />

      <v-row class="d-flex align-center justify-center">
        <v-col cols="auto">
          <Test
            :value="myValue"
            :description="myDescription"
            @click="testClick"
          />
        </v-col>

        <v-col cols="auto">
          <h1 class="mt-4">Hello World Data {{ computedTest }}</h1>

          <v-btn color="primary" class="mb-3" @click="buttonClick">
            This is a test
          </v-btn>
          <v-btn color="secondary" class="mb-3 ml-3" @click="buttonClick2">
            Another Test
          </v-btn>

          <v-timeline density="compact" align="start">
            <v-timeline-item
              v-for="row in arrayData"
              :key="row.a"
              dot-color="red"
              size="x-small"
            >
              <div class="mb-4">
                <div class="font-weight-normal">
                  <strong>Typescript is da bomb, yo!</strong>
                </div>

                <div>Static type checking: {{ row.a }} {{ row.b }}</div>
              </div>
            </v-timeline-item>
          </v-timeline>
        </v-col>

        <v-col cols="auto"> </v-col>
      </v-row>
    </v-responsive>
  </v-container>
</template>

<script lang="ts" setup>
//
// Ok, the setup function is implied by the below "setup" attribute
//
import { ref, computed, reactive } from "vue";

// Observe how the import of a Component includes an "named object destructuring"
// ... that provides access to the exported interface type TestClickEvent
//
import Test, { TestClickEventArgs } from "./Test.vue";

import { useAppStore } from "@/store/app";

// Ultra-basic, albeit useful type declaration
//
type DataType666 = {
  a: Number;
  b: Number;
};

// Using the ref method, with its generic argument passed thereto... (?)
//
const testArray: Array<DataType666> = [{ a: 300, b: 900 }];

const appStore = useAppStore();

let arrayData = ref<Array<DataType666>>(testArray);
let counter = ref(10);

// Methods are added by simply declaring them in the body of this script tag block;
// ... both the declaration of a variable and the terser "function" both work, obviously
//
function buttonClick() {
  // Aha! Observe, that using ref constrains us to use value.
  //
  arrayData.value.push({ a: 909, b: 233 });
}

function buttonClick2() {
  appStore.increment();
  alert(
    "yo! " + appStore.count + " " + appStore.doubleCount + " " + appStore.name
  );
}

// Initialize the data to be pushed into the Test component
//
const valueFactory = function (initialValue: number): number {
  return initialValue % 6;
};
const myValue: Number = valueFactory(89);
const myDescription: String = "This is my description";

// Basic functional programming approach to introducing computed variables
//
const computedTest = computed(() => {
  return arrayData.value.length + counter.value;
});

// Callback method for Click Event
//
const testClick = function (arg: TestClickEventArgs) {
  alert(arg.eventId + " " + arg.payload);
};
</script>
