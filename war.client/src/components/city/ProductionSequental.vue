<script setup lang="js">
import { computed, ref } from 'vue';
import { useItemStore } from '@/stores/item';
import { useNotificationStore } from '@/stores/notification'
import RemainingTime from '../tools/RemainingTime.vue';
import axios from 'axios';
import ProducibleItemList from './ProducibleItemList.vue';

const notificationStore = useNotificationStore();

const props = defineProps({
    city: Object,
    buildingContainerId: String
});

const emits = defineEmits('cityChanged');

const itemStore = useItemStore();

const buildingContainer = computed(() => {
    var result = props.city.buildings.find(x => x.id == props.buildingContainerId);
    result.building = itemStore.find(result.buildingKey);
    return result;
})

const producibleItems = computed(() => {
    return buildingContainer.value.building.producibleItems.map(x => itemStore.find(x))
})

const queue = computed(() => {
    return buildingContainer.value.productionQueue.map(x => {
        x.item = itemStore.find(x.itemKey);
        return x;
    })
})

const receipe = computed(() => {
    if (selectedProducibleItem.value == null)
        return [];
    else
        return Object.entries(selectedProducibleItem.value.receipe.items).map(x => { return { item: itemStore.find(x[0]), amount: x[1] } })
})

const selectedProducibleItem = ref(null);
const amount = ref(1);


function addToProductionQueue() {
    axios.post(`city/${props.city.id}/addToProductionQueue`, {
        buildingContainer: buildingContainer.value.id,
        itemKey: selectedProducibleItem.value.key,
        amount: amount.value
    })
        .then(x => {
            emits('cityChanged', x.data);
            selectedProducibleItem.value = null;
            amount.value = 1;
        })
        .catch(notificationStore.addResponse);
}

function cancelProductionInQueue(production) {
    axios.post(`city/${props.city.id}/cancelProductionInQueue`, {
        buildingContainer: buildingContainer.value.id,
        instructionId: production.id
    })
        .then(x => {
            emits('cityChanged', x.data);
            selectedProducibleItem.value = null;
        })
        .catch(notificationStore.addResponse);
}

</script>
<template>
    <div>
        <!-- <div class="row">
            <div v-for="item in producibleItems" :key="item.key" class="col-md-3"
                @click="selectedProducibleItem = item">
                <div class="producible-item"
                    :class="{ selected: selectedProducibleItem != null && item.key == selectedProducibleItem.key }">
                    <img style="width:60px; height: 60px; display:inline-block" />
                    <div style=" display:inline-block">
                        <span><b>{{ item.name }}</b></span><br />
                        <span>{{ $duration(item.receipe.duration) }}</span>
                    </div>
                </div>
            </div>
        </div> -->
        <ProducibleItemList :producibleItems="producibleItems" v-model="selectedProducibleItem">
        </ProducibleItemList>
        <div class="row" v-if="selectedProducibleItem">
            <div class="col-6">
                <h4>{{ selectedProducibleItem.name }}</h4>
                <span>Duration: {{ $duration(selectedProducibleItem.receipe.duration) }}</span>
                <div>
                    <input type="number" v-model="amount" min="1" max="100" /> <button type="button"
                        @click="addToProductionQueue">Produce</button>
                </div>
            </div>
            <div class="col-6">
                <h6>Required Items For Production</h6>
                <div v-for="r in receipe" :key="r.item.key">
                    {{ r.amount }} {{ r.item.name }}
                </div>
            </div>
        </div>
        <div class="row">
            <h6>Production Queue</h6>
            <div class="col-12">
                <div v-for="p in queue" :key="p.id">
                    <div style="float:left">
                        <RemainingTime v-if="p.isStarted" :requestDate="props.city.requestDate"
                            :key="p.id + props.city.requestDate" :total="p.durationPerItem"
                            :duration="p.remainingDurationForCurrentItem" @completed="$emit('cityChanged')" />
                        <div v-else style="width:120px; height: 120px;">

                        </div>
                    </div>
                    <div style="float: left;">
                        <div style="font-weight:bold">{{ p.item.name }}</div>
                        <div>Remaining Amount: {{ p.remainingAmount }}</div>
                        <button type="button" @click="cancelProductionInQueue(p)">Cancel</button>
                    </div>
                    <div style="clear:both"></div>
                </div>
            </div>
        </div>
    </div>
</template>
