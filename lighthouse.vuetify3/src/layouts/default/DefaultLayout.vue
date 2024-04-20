<template>
  <v-app>
    <v-app-bar density="compact" elevation="1">
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
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { RouteLocationRaw } from "vue-router";

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
    icon: "mdi-playlist-edit",
    title: "Stock Configuration",
    route: {
      name: "stock-config",
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
</style>
