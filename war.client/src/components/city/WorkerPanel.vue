<script setup lang="js">
import { ref, computed } from 'vue';
import { useItemStore } from '@/stores/item';
import axios from 'axios';
import { useNotificationStore } from '@/stores/notification'

const notificationStore = useNotificationStore();

const props = defineProps({
    city: Object,
    buildingContainerId: String
});

const emits = defineEmits('cityChanged');

const buildingContainer = computed(() => {
    var result = props.city.buildings.find(x => x.id == props.buildingContainerId);
    result.building = itemStore.find(result.buildingKey);
    return result;
})

const itemStore = useItemStore();
const workers = ref(buildingContainer.value.workers);

function setWorkers() {
    axios.post(`city/${props.city.id}/setWorkers`, {
        buildingContainer: buildingContainer.value.id,
        workers: workers.value
    })
        .then(x => {
            emits('cityChanged', x.data);
        })
        .catch(notificationStore.addResponse);
}
</script>
<template>
    <div class="row" v-if="buildingContainer.building.workablePeople.length">
        <h6>Workers:</h6>
        <div class="col">
            <table>
                <tr v-for="wp in buildingContainer.building.workablePeople" :key="wp">
                    <td>{{ itemStore.find(wp).name }}</td>
                    <td><input type="number" min="0" :max="15" v-model="workers[wp]" /></td>
                </tr>
            </table>
            <button type="button" @click="setWorkers">Set Workers</button>
        </div>
    </div>
</template>