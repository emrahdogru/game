<script setup lang="js">
import Login from './components/UserLogin.vue'
import { useUserStore } from './stores/user';
import UserLobby from './components/UserLobby.vue';
const userStore = useUserStore();
</script>

<template>
  <header>

  </header>

  <main>
    <div class="wrapper">
      <Login v-if="!userStore.$state.key"></Login>
      <div v-else style="border:1px solid red;">
        <UserLobby></UserLobby>
        {{ userStore.$state }}
      </div>
    </div>
  </main>
</template>

<style scoped></style>
