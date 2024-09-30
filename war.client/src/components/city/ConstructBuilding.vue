<script setup lang="js">
import { onMounted, ref } from 'vue';
import { useItemStore } from '@/stores/item'
import ItemList from '../tools/ItemList.vue';
import axios from 'axios';
import { useNotificationStore } from '@/stores/notification'

const notificationStore = useNotificationStore();
const emits = defineEmits('cityChanged');

const itemStore = useItemStore();

const props = defineProps({
    city: Object
})

const constructibleBuildings = ref([]);

onMounted(() => {
    constructibleBuildings.value = props.city.constructibleBuildings.map(x => itemStore.find(x));
})

function construct(building) {
    axios
        .post(`city/${props.city.id}/constructBuilding`, {
            buildingKey: building.key
        })
        .then(x => {
            if (x.data != null)
                emits('cityChanged', x.data);
            emits('close');
        })
        .catch(notificationStore.addResponse);
}

</script>
<template>
    <div>
        Select a building to construct:
        <div v-for="b in constructibleBuildings" :key="b.key">
            <img style="width:120px; height: 120px; float: left;" />
            <div>
                <div style="font-weight: bold;">{{ b.name }}</div>
                Duration: {{ $duration(b.receipe.duration) }}
                <ItemList :items="b.receipe.items"></ItemList>
            </div>
            <button type="button" style="float:right" @click="construct(b)">Construct</button>
            <div style="clear: both;"></div>
        </div>
    </div>
</template>