<template>
  <v-container>
    <h4 class="text-h6 font-weight-bold">
      (Demo v-responsive tag with fill-height enabled)
    </h4>

    <div class="py-3" />

    <v-row class="d-flex align-center justify-center">
      <v-col cols="4" alignSelf="start">
        <TestComponent
          :value="componentCounter"
          :description="componentDesc"
          @click="testComponentClick"
        />

        <div class="ml-5 mr-5 pl-5 pr-5">
          <v-btn
            color="secondary"
            prepend-icon="mdi-account-multiple-outline"
            variant="flat"
            @click="testDialogClick"
          >
            <div class="text-none font-weight-regular">LAUNCH DIALOG</div>
          </v-btn>
        </div>

        <TestDialog :visible="dialogVisible" @close="closeTestDialog" />
      </v-col>

      <v-col cols="8">
        <v-data-table
          :items="arrayData"
          :headers="headers"
          density="compact"
        ></v-data-table>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { DataTableHeader } from "vuetify";
import { ref, reactive } from "vue";
import TestComponent, { TestClickEventArgs } from "./TestComponent.vue";
import TestDialog, { TestDialogCloseArgs } from "./TestDialog.vue";
import { useMainStore } from "@/stores/mainStore";

type TestRecords = {
  name: String;
  address: String;
  quantity: Number;
};

// Constants and local properties
//
const componentDesc: String = "This is my description";
let componentCounter = ref<number>(10);
let dialogVisible = ref<boolean>(false);

let arrayData = reactive<Array<TestRecords>>([
  { name: "My Test", address: "123 Test Street, Test City", quantity: 2 },
]);

const headers: Array<DataTableHeader> = [
  { value: "name", title: "Name", align: "start", width: "400" },
  { value: "address", title: "Address", align: "start", width: "300" },
  { value: "quantity", title: "Quantity", align: "end", width: "300" },
];

// Events
//
const testComponentClick = function (arg: TestClickEventArgs): void {
  arrayData.push({
    name: "[component]",
    address: arg.eventId + " Test Blvd",
    quantity: arg.eventId,
  });

  componentCounter.value += 1;
};

const testDialogClick = function (): void {
  dialogVisible.value = true;
};

const closeTestDialog = function (args: TestDialogCloseArgs | null): void {
  if (args) {
    alert("Bing! " + args.message);
  } else {
    alert("Meeza no args - getting alza this codsa!");
  }
  dialogVisible.value = false;
};

// Computed
//
// const computedTest = computed(() => {
//   return arrayData.value.length + counter.value;
// });
</script>

<style>
tbody tr:nth-of-type(even) {
  background-color: rgba(0, 0, 0, 0.05);
}
</style>
