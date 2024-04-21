<template>
  <v-container :fluid="true">
    <router-view />
  </v-container>
</template>

<script lang="ts" setup></script>

<style>
.v-container--fluid {
  padding: 0 16px 0 16px !important;
}
</style>
