<template lang="">
  <v-card height="200" class="pa-3 ma-3">
    <template v-slot:title> This is a TEST </template>

    <template v-slot:subtitle> This is a subtitle </template>

    <template v-slot:text>
      This is content - value: <strong>{{ value }}</strong>
      <br />

      Description: <strong>{{ description }}</strong>
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

export interface TestClickEvent {
  eventId: Number;
  metadata: String;
}

// defineProps will expose the props (parameter) interface to consumers of Test.vue
//
// ^^^ Presumably the props constant is a pointer that can be referenced within the body of this script tag, no?
//
const props = defineProps<Payload>();

// defineEmits  Events - metadata specifies the type of object that is passed by the event emission
// ... (must it be an interface?) <= ask yourself why that would be necessary, eh?
//
const emit = defineEmits<{ (e: "click", metadata: TestClickEvent): void }>();

// Homework assignment
//
const buttonCallback = function () {
  // You may be surprised to find that a magic string ("click") is actually compiler checked due to the
  // ... defineEmits invocation above
  //
  emit("click", { eventId: 666, metadata: "Hail Thy Lord!" });
};
</script>

<style lang=""></style>
