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
// "export" makes Payload and TestClickEvent available to consumers of Test.vue
//
export interface Payload {
  value: Number;
  description: String;
}

// defineProps exposes the props (component parameter) interface to consumers of Test.vue
//
// ^^^ Presumably the props constant is a pointer that can be referenced within the body of this script tag, no?
//
const props = defineProps<Payload>();

// Again making the interface available to consumers of this component
//
export interface TestClickEventArgs {
  eventId: Number;
  payload: String;
}

// 5/25/2024 - here's what's bothering you - this is seriously fucking pimp tight: it's using function overload types
// ... especially "e" to create compiler safety when we invoke defineEmits. It damn near appears that the Typescript
// ... language design people *wanted* for this kind of certainty given Javascript's heavy reliance on events.
//
const emit = defineEmits<{
  (e: "click", metadata: TestClickEventArgs): void;
  (e: "beep", metadata: TestClickEventArgs): void;
}>();

// You wanted to know how to use this
//
type EmitType = {
  (e: "click", metadata: TestClickEventArgs): void;
  (e: "beep", metadata: TestClickEventArgs): void;
};

const buttonCallback = function () {
  emit("click", { eventId: 666, payload: "Hail Thy Lord!" });
};
</script>

<style lang=""></style>
