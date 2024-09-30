<script setup lang="js">
defineProps({
    point: Object
})
</script>
<template>
    <span>[{{ point.x }}:{{ point.y }}]</span>
</template>