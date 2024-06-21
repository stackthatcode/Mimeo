<template>
  <div>
    <v-app>
      <v-app-bar density="compact" flat>
        <template v-slot:prepend>
          <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
        </template>

        <v-app-bar-title>
          <div class="app-title">Olympus</div>
        </v-app-bar-title>
      </v-app-bar>

      <v-navigation-drawer v-model="drawer" location="start" temporary>
        <v-list color="primary" variant="plain" density="compact" class="mt-5">
          <v-list-item
            v-for="(item, i) in items"
            :key="i"
            :value="item"
            color="primary"
            variant="plain"
            :prepend-icon="item.icon"
            :title="item.title"
            :to="item.route"
          >
          </v-list-item>
        </v-list>
      </v-navigation-drawer>

      <v-main>
        <router-view />
      </v-main>
    </v-app>

    <!-- 6/21/2024 Ignore this compiler error - due to Vuetify quirks, we need to use the v-model binding attribute
        in lieu of the :value binding -->
    <v-overlay
      v-model="mainStore.globalSpinnerVisible"
      opacity="0.07"
      width="100%"
      height="100%"
    >
      <v-container class="fill-height" fluid>
        <v-row class="fill-height" align="center" justify="center">
          <v-col cols="auto">
            <v-progress-circular indeterminate size="64"></v-progress-circular>
          </v-col>
        </v-row>
      </v-container>
    </v-overlay>

    <v-dialog
      v-model="mainStore.errorDialogVisible"
      @click:outside="closeErrorDialog"
      max-width="600"
      persistent
    >
      <v-card rounded="lg">
        <v-card-title class="d-flex justify-space-between align-center">
          <div class="text-h6 ps-2">System Error Encountered</div>

          <v-btn
            icon="mdi-close"
            variant="text"
            @click="closeErrorDialog"
          ></v-btn>
        </v-card-title>

        <v-divider class="mb-4"></v-divider>

        <v-card-text>
          <div class="text-medium-emphasis mb-4">
            Please contact technical support with the following details:
          </div>
          <v-textarea
            class="mb-2"
            rows="3"
            variant="outlined"
            v-model="mainStore.errorMessage"
            readonly
          ></v-textarea>
        </v-card-text>

        <v-divider class="mt-2"></v-divider>

        <v-card-actions class="my-2 d-flex justify-end">
          <v-btn
            color="primary"
            text="Close"
            variant="flat"
            @click="closeErrorDialog"
            class="pl-5 pr-5"
          ></v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { RouteLocationRaw } from "vue-router";
import { useMainStore } from "@/stores/mainStore";

const mainStore = useMainStore();
const closeErrorDialog = function (): void {
  mainStore.errorPopupHide();
};

let drawer = ref<boolean>(false);

type NavigationItem = {
  icon: string;
  title: string;
  route: RouteLocationRaw;
};

let items = ref<Array<NavigationItem>>([
  {
    icon: "mdi-view-dashboard",
    title: "Main Dashboard",
    route: {
      name: "main-dashboard",
    },
  },
  {
    icon: "mdi-flask",
    title: "Test Page",
    route: {
      name: "home-view",
    },
  },
]);
</script>

<style>
.app-title {
  font-size: 0.7em !important;
  text-transform: uppercase;
  letter-spacing: 0.25em !important;
  font-weight: 700;
}

.v-toolbar__content {
  border-bottom: 1px solid rgba(0, 0, 0, 0.12); /* Adjust color and opacity as needed */
}
</style>
