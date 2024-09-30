<script setup lang="js">
import RemainingTime from '../tools/RemainingTime.vue';
import { useItemStore } from '@/stores/item';
import { defineEmits, defineProps, computed } from 'vue';
import axios from 'axios';
import { useNotificationStore } from '@/stores/notification'

const notificationStore = useNotificationStore();
var itemStore = useItemStore();
const emits = defineEmits('cityChanged', 'close', 'buildingSelect');

var props = defineProps({
    city: Object
})

const buildings = computed(() => {
    var result = props.city.buildings.map(x => {
        var item = itemStore.find(x.buildingKey);

        if (item == null)
            throw 'Building colud not found: ' + x.buildingKey;

        x.building = item;
        return x;
    }) ?? [];
    console.log('Computed buildings: ', buildings)
    return result;
})


function cancelConstruction(building, event) {
    event.stopPropagation();
    axios.post(`city/${props.city.id}/cancelConstruction/${building.id}`)
        .then(x => {
            emits('cityChanged', x.data);
        })
        .catch(notificationStore.addResponse);
}

</script>
<template>
    <div>
        <div v-if="buildings && buildings.length > 0">

            <div v-for="b in buildings" :key="b.id" @click="$emit('buildingSelect', b)" class="building">
                <RemainingTime v-if="!b.isConstructionCompleted" :duration="b.remainingTimeForConstruction"
                    @completed="$emit('cityChanged')" :total="b.building.receipe.duration"
                    :requestDate="city.requestDate" style="float:left">
                </RemainingTime>
                <img v-else style="width:120px; height:120px; float:left;" />
                <div style="float: left;">
                    <div style="font-weight: bold;">{{ b.building.name }} #{{ b.index }}</div>
                    <div v-if="b.isConstructionCompleted">
                        İnşaat tamamlandı.
                    </div>
                    <div v-else>
                        İnşaat devam ediyor...
                        <button type="button" @click.capture="cancelConstruction(b, $event)">Cancel</button>
                    </div>
                </div>
                <div style="clear:both"></div>
            </div>

        </div>
        <div v-else>
            There is no building in this city
        </div>
    </div>
</template>
<style lang="css" scoped>
.building {
    border: 1px solid #999;
    border-radius: 8px;
    margin-bottom: 8px;
    overflow: hidden;
    background-color: #FFF;
}

.building:hover {
    background-color: antiquewhite;
}
</style>