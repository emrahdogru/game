<script setup lang="js">
import { useNotificationStore } from '@/stores/notification'

const notificationStore = useNotificationStore();
</script>
<template>
    <div class="notification-container">
        <div v-for="x in notificationStore.notifications" :class="['text-bg-' + x.type]" :key="x" class="notification">
            {{ x.message }}</div>
    </div>
</template>
<style lang="css" scoped>
.notification-container {
    position: fixed;
    top: 50px;
    right: 0px;
    list-style-type: none;
}

.notification {
    padding: 1px 15px;
}
</style>