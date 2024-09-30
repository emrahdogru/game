<script setup lang="js">
import CityManagement from './city/Index.vue';

defineProps({
    gameBoardId: String
});
</script>
<template>
    <div>

    </div>
    <CityManagement v-if="selectedCityId" :id="selectedCityId"></CityManagement>
</template>