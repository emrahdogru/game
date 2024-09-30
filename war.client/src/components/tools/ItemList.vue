<script setup lang="js">
import { computed } from 'vue';
import { useItemStore } from '@/stores/item';

const itemStore = useItemStore();

const props = defineProps({
    items: Array
})

const itemList = computed(() => {
    let itemList = Object.entries(props.items ?? []).map(x => {
        return {
            item: itemStore.find(x[0]), amount: x[1]
        };
    });

    return itemList;
})
</script>
<template>
    <div>
        <ul>
            <li v-for="item in itemList" :key="item.item.key">{{ item.item.name }} {{ item.amount }} </li>
        </ul>
    </div>
</template>