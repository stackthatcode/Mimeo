<template>
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
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { RouteLocationRaw } from "vue-router";

// Vue 3 Composition API "ref" basic function enables reactivity
//
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
