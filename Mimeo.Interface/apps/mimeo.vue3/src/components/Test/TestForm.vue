<template>
  <v-container class="fill-height">
    <v-responsive class="d-flex align-center text-center fill-height">
      <h4 class="text-h6 font-weight-bold">
        (Demo v-responsive tag with fill-height enabled)
      </h4>

      <div class="py-3" />

      <v-row class="d-flex align-center justify-center">
        <v-col cols="auto">
          <TestComponent
            :value="myValue"
            :description="myDescription"
            @click="testClick"
          />
        </v-col>

        <!--
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
        -->

        <v-col cols="auto"> </v-col>
      </v-row>
    </v-responsive>
  </v-container>
</template>

<script lang="ts" setup>
import { ref, computed, reactive } from "vue";
import TestComponent, { TestClickEventArgs } from "./TestComponent.vue";
import { useAppStore } from "@/stores/app";

// Ultra-basic, albeit useful type declaration
//
type DataType666 = {
  a: Number;
  b: Number;
};

const myValue: Number = 12;
const myDescription: String = "This is my description";

// Using the ref method, with its generic argument passed thereto... (?)
//
const testArray: Array<DataType666> = [{ a: 300, b: 900 }];
const appStore = useAppStore();

let arrayData = ref<Array<DataType666>>(testArray);
let counter = ref(10);

// Callback method for Click Event
//
const testClick = function (arg: TestClickEventArgs): number {
  alert(arg.eventId + " " + arg.payload);
  return 3;
};

// Methods are added by simply declaring them in the body of this script tag block;
// ... both the declaration of a variable and the terser "function" both work, obviously
//
function buttonClick() {
  // Aha! Observe, that using ref constrains us to use value.
  //
  arrayData.value.push({ a: 909, b: 233 });
}

function buttonClick2() {
  alert("wait!");
  appStore.increment();
  alert(
    "yo! " + appStore.count + " " + appStore.doubleCount + " " + appStore.name
  );
}

const computedTest = computed(() => {
  return arrayData.value.length + counter.value;
});
</script>
