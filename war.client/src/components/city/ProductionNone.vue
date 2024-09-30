<script setup lang="js">

const emits = defineEmits('cityChanged')

const props = defineProps({
    city: Object,
    buildingContainerId: String
});

</script>
<template>
    <div>
        <h3>None</h3>
        {{ props.building }}
    </div>
</template>