<script setup lang="js">
import ItemList from '../tools/ItemList.vue';

const props = defineProps({
    city: Object
})
</script>
<template>
    <div>
        <h5>Resources in city</h5>
        <ItemList :items="props.city.resources"></ItemList>
    </div>
</template>