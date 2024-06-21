<template>
  <div class="pa-3 ma-3">
    <v-dialog
      v-model="visibleDialog"
      max-width="500"
      @click:outside="cancel"
      persistent
    >
      <v-card rounded="lg">
        <v-card-title class="d-flex justify-space-between align-center">
          <div class="text-h5 ps-2">Invite Gus to connect</div>

          <v-btn icon="mdi-close" variant="text" @click="cancel"></v-btn>
        </v-card-title>

        <v-divider class="mb-4"></v-divider>

        <v-card-text>
          <div class="text-medium-emphasis mb-4">
            Invite collaborators to your network and grow your connections.
          </div>

          <div class="mb-2">Message (optional)</div>

          <v-textarea
            :counter="300"
            class="mb-2"
            rows="2"
            variant="outlined"
            persistent-counter
            v-model="messageText"
          ></v-textarea>

          <!---
            <div class="text-overline mb-2">💎 PREMIUM</div>

            <div class="text-medium-emphasis mb-1">
              Share with unlimited people and get more insights about your
              network. Try Premium Free for 30 days.
            </div>
          -->

          <!-- Nice styling, but, leave this out
                <v-btn
                  class="text-none font-weight-bold ms-n4"
                  color="primary"
                  text="Retry Premium Free"
                  variant="text"
                ></v-btn>-->
        </v-card-text>

        <v-divider class="mt-2"></v-divider>

        <v-card-actions class="my-2 d-flex justify-end">
          <v-btn class="text-none" text="Cancel" @click="cancel"></v-btn>

          <v-btn
            class="text-none"
            color="primary"
            text="Send"
            variant="flat"
            @click="submit"
          ></v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts" setup>
import { defineProps, defineEmits, ref, watch } from "vue";

// Props configuration
//
export interface TestDialogProps {
  visible: Boolean;
}
const props = defineProps<TestDialogProps>();
watch(
  () => props.visible,
  (newVal) => {
    //console.log("detect");
    visibleDialog.value = newVal;
  }
);

// Local properties
//
const visibleDialog = ref<Boolean>(false);
const messageText = ref<String>("");

// Events
//
export interface TestDialogCloseArgs {
  value: Number;
  message: String;
}

const emit = defineEmits<{
  (e: "close", metadata: TestDialogCloseArgs | null): void;
}>();

const cancel = function (): void {
  visibleDialog.value = false;
  emit("close", null);
};

const submit = function (): void {
  visibleDialog.value = false;
  emit("close", {
    value: 66,
    message: messageText.value,
  });
};
</script>
