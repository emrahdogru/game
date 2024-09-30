<script setup lang="js">
import axios from 'axios';
import { onMounted, ref, computed } from 'vue';
import BuildingList from './BuildingList.vue';
import CityResources from './CityResources.vue';
import ConstructBuilding from './ConstructBuilding.vue';
import BuildingDetail from './BuildingDetail.vue';
import { useNotificationStore } from '@/stores/notification'

const notificationStore = useNotificationStore();

const props = defineProps({
    id: String
});

const city = ref(null);
const commands = ref({
    newBuildingPanel: false,
    selectedBuildingId: null
});

const selectedBuilding = computed(() => {
    return city.value.buildings.find(x => x.id == commands.value.selectedBuildingId)
})

const update = () => {
    axios.get('city/' + props.id)
        .then(x => {
            if (x.data != null)
                x.data.requestDate = new Date();
            city.value = x.data;
        })
        .catch(notificationStore.addResponse);
}

function cityChanged(newCity) {
    if (newCity == null) {
        update();
    } else {
        if (newCity != null)
            newCity.requestDate = new Date();
        city.value = newCity;
    }
}

function onBuildingSelect(building) {
    commands.value.selectedBuildingId = building.id;
    commands.value.newBuildingPanel = false;
}

onMounted(() => {
    update();
})
</script>
<template>
    <form>
        <div v-if="city != null">
            <div class="row">
                <div class="col-md-12">
                    <button type="button" @click="update">Refresh city</button>
                    <h1>{{ city.name }}</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    Kaynaklar
                    <CityResources @cityChanged="cityChanged" :city="city"></CityResources>
                </div>
                <div class="col-md-3">
                    <button type="button"
                        @click="commands.newBuildingPanel = true; commands.selectedBuildingId = null;">Construct a new
                        building</button>

                    <BuildingList @cityChanged="cityChanged" :city="city" @buildingSelect="onBuildingSelect">
                    </BuildingList>
                </div>
                <div class="col-md-6">
                    <ConstructBuilding @cityChanged="cityChanged" v-if="commands.newBuildingPanel" :city="city">
                    </ConstructBuilding>
                    <BuildingDetail v-else-if="selectedBuilding != null" :city="city" @cityChanged="cityChanged"
                        :key="selectedBuilding.id" :building-container-id="selectedBuilding.id">
                    </BuildingDetail>
                </div>
            </div>
        </div>
    </form>
</template>
<style lang="css"></style>