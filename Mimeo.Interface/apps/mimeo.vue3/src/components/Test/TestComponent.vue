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

// Props
//
export interface TestProps {
  value: Number;
  description: String;
}

const props = defineProps<TestProps>();

// Events
//
export interface TestClickEventArgs {
  eventId: Number;
  payload: String;
}

export interface TestBeepEventArgs {
  eventId: Number;
  payload: String;
}
const emit = defineEmits<{
  (e: "click", metadata: TestClickEventArgs): void;
  (e: "beep", metadata: TestBeepEventArgs): void;
}>();

const buttonCallback = function (): void {
  emit("click", {
    eventId: props.value,
    payload: "My current props " + props.value,
  });
};
</script>

<style lang=""></style>
