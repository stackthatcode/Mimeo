<template lang="">
  <v-card height="200" class="pa-3 ma-3" variant="flat" color="surface-variant">
    <template v-slot:title> Test Component </template>

    <template v-slot:subtitle> This is a subtitle </template>

    <template v-slot:text>
      Value: <strong>{{ value }}</strong>
      <br />

      <p>{{ description }}</p>
      <div class="py-3"></div>

      <v-btn color="primary" @click="buttonCallback"> Another button! </v-btn>
    </template>
  </v-card>
</template>

<script lang="ts" setup>
//
// These export statements make Payload and TestClickEvent available to consumers of Test.vue
//
export interface Payload {
  value: Number;
  description: String;
}

// defineProps will expose the props (parameter) interface to consumers of Test.vue
//
// ^^^ Presumably the props constant is a pointer that can be referenced within the body of this script tag, no?
//
const props = defineProps<Payload>();

export interface TestClickEventArgs {
  eventId: Number;
  payload: String;
}

// defineEmits -> Events - metadata specifies the type of object that is passed by the event emission
// The Vue3 convention is for the first argument of the lambda type to a Const String (e.g. "click") to signal to
// ... the parent component the name of the event for binding
//
const emit = defineEmits<{
  (e: "click", metadata: TestClickEventArgs): void;
}>();

// Homework assignment
//
const buttonCallback = function () {
  // You may be surprised to find that a magic string ("click") is actually compiler checked due to the
  // ... defineEmits invocation above
  //
  emit("click", { eventId: 666, payload: "Hail Thy Lord!" });
};
</script>

<style lang=""></style>
