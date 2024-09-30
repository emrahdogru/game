<script setup lang="js">
import axios from 'axios';
import { computed, onMounted, ref } from 'vue';
import CityManagement from './city/Index.vue';
import LocationLabel from './tools/LocationLabel.vue';
import NotificationList from './NotificationList.vue'


defineProps({
    id: String
});

const cities = ref([]);
const selectedCityId = ref(null);
const items = ref({});

const currentTab = computed(() => {
    if (selectedCityId.value != null)
        return "city";

    return "default";
})

onMounted(async () => {
    axios.get('city/usercities')
        .then(x => {
            cities.value = x.data;
        });

    await axios.get('item')
        .then(x => {
            items.value = x.data;
        });
})

</script>
<template>

    <div class="game-board-container">
        <header class="game-board-header">
            currentTab: {{ currentTab }}
        </header>
        <div class="game-board-content">
            <h3>GAME BOARD CONTENT</h3>
            <div v-if="currentTab == 'default'">
                <h4>ŞEHİRLER</h4>
                <ul style="border:1px dotted blue">
                    <li v-for="city in cities" :key="city.id" v-on:click="selectedCityId = city.id">
                        {{ city.name }}
                        <LocationLabel :point="city.location" />
                    </li>
                </ul>
            </div>
            <CityManagement v-if="currentTab == 'city'" :id="selectedCityId"></CityManagement>
        </div>
    </div>
    <NotificationList></NotificationList>
</template>
<style lang="css">
.game-board-container {
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    background-color: #ebebeb;
}

.game-board-header {
    position: fixed;
    top: 0;
    right: 0;
    left: 0;
    height: 50px;
    background-color: #e0e0e0;
    border-bottom: 1px solid #5e5e5e;
}

.game-board-content {
    position: fixed;
    top: 50px;
    right: 0px;
    bottom: 0px;
    left: 0px;
    overflow-y: auto;
}
</style>