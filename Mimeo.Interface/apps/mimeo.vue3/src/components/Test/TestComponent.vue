<template lang="">
  <v-card height="200" class="pa-3 ma-3" variant="flat">
    <template v-slot:title> Test Component </template>
    <template v-slot:subtitle>
      Example of strongly-typed props and events
    </template>

    <template v-slot:text>
      <p align="left">
        value (prop): <strong>{{ value }}</strong>
        <br />
        description (prop): <strong>{{ description }}</strong>
      </p>
      <div class="py-3"></div>

      <v-btn color="primary" @click="buttonCallback"> Another button! </v-btn>
    </template>
  </v-card>
</template>

<script lang="ts" setup>
import { defineProps, defineEmits } from "vue";

export interface TestProps {
  value: Number;
  description: String;
}
export interface TestClickEventArgs {
  eventId: Number;
  payload: String;
}

const props = defineProps<TestProps>();

const emit = defineEmits<{
  (e: "click", metadata: TestClickEventArgs): void;
  (e: "beep", metadata: TestClickEventArgs): void;
}>();

const buttonCallback = function () {
  emit("click", { eventId: 666, payload: "Hail Thy Lord!" });
};
</script>

<style lang=""></style>
