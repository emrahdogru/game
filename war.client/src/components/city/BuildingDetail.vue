<script setup lang="js">
import { useItemStore } from '@/stores/item';
import { computed, ref } from 'vue';
import ProductionContinious from './ProductionContinious.vue';
import ProductionSequental from './ProductionSequental.vue';
import ProductionNone from './ProductionNone.vue';
import ProductionBreeding from './ProductionBreeding.vue';
import WorkerPanel from './WorkerPanel.vue';


const props = defineProps({
    city: Object,
    buildingContainerId: String
});

//const emits = defineEmits(['cityChanged']);

const itemStore = useItemStore();

const buildingContainer = computed(() => {
    var result = props.city.buildings.find(x => x.id == props.buildingContainerId);
    result.building = itemStore.find(result.buildingKey);
    return result;
})
</script>

<template>
    <div>
        <h3>{{ buildingContainer.building.name }} #{{ buildingContainer.index }}</h3>
        <WorkerPanel v-if="buildingContainer.building.workablePeople.length" :city="props.city"
            @cityChanged="$emit('cityChanged')" :buildingContainerId="props.buildingContainerId"
            :key="props.buildingContainerId"></WorkerPanel>
    </div>
    <ProductionContinious :city="city" @cityChanged="$emit('cityChanged')" :key="props.buildingContainerId"
        :building-container-id="props.buildingContainerId"
        v-if="buildingContainer.building.productionType == 'Continious'">
    </ProductionContinious>
    <ProductionSequental :city="city" @cityChanged="$emit('cityChanged')" :key="props.buildingContainerId"
        :building-container-id="props.buildingContainerId"
        v-if="buildingContainer.building.productionType == 'Sequental'">
    </ProductionSequental>
    <ProductionNone :city="city" @cityChanged="$emit('cityChanged')" :key="props.buildingContainerId"
        :building-container-id="props.buildingContainerId" v-if="buildingContainer.building.productionType == 'None'">
    </ProductionNone>
    <ProductionBreeding :city="city" @cityChanged="$emit('cityChanged')" :key="props.buildingContainerId"
        :building-container-id="props.buildingContainerId"
        v-if="buildingContainer.building.productionType == 'Breeding'">
    </ProductionBreeding>
</template>