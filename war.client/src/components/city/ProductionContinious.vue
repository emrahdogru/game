<script setup lang="js">
import { computed, ref } from 'vue';
import { useItemStore } from '@/stores/item';
import { useNotificationStore } from '@/stores/notification'
import ProducibleItemList from './ProducibleItemList.vue';
import axios from 'axios';

const props = defineProps({
    city: Object,
    buildingContainerId: String
});

const selectedProducibleItem = ref(null);
const emits = defineEmits('cityChanged');

const itemStore = useItemStore();
const notificationStore = useNotificationStore();

const buildingContainer = computed(() => {
    var result = props.city.buildings.find(x => x.id == props.buildingContainerId);
    result.building = itemStore.find(result.buildingKey);
    result.product = itemStore.find(result.productKey);
    return result;
})

const producibleItems = computed(() => {
    return buildingContainer.value.building.producibleItems.map(x => itemStore.find(x))
})

const stopProduction = function () {
    axios.post(`city/${props.city.id}/stopProduction/${props.buildingContainerId}`)
        .then(x => {
            emits('cityChanged', x.data);
        })
        .catch(notificationStore.addResponse);
}

const startProduction = function () {
    if (selectedProducibleItem.value == null)
        notificationStore.addWarning('Select an item to produce.');

    axios.post(`city/${props.city.id}/startProduction/`, {
        buildingContainer: props.buildingContainerId,
        productKey: selectedProducibleItem.value.key
    })
        .then(x => {
            emits('cityChanged', x.data);
        })
        .catch(notificationStore.addResponse);
}

</script>
<template>
    <ProducibleItemList :producibleItems="producibleItems" v-model="selectedProducibleItem"></ProducibleItemList>
    <div v-if="buildingContainer.product">
        <div>Producing: {{ buildingContainer.product.name }}</div>
        <div>Duration Per Item: {{ $duration(buildingContainer.durationPerItem) }}</div>
        <button type="button" @click="stopProduction">Stop Production</button>
    </div>
    <div v-else>
        <button type="button" @click="startProduction">Start Production</button>
    </div>
</template>