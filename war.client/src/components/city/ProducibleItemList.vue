<script setup lang="js">
var props = defineProps({
    producibleItems: Array
})

const model = defineModel();

var emits = defineEmits(["itemSelect"])

const onItemSelect = function (item) {
    model.value = item;
    emits.$emit('itemSelect', item);
}
</script>
<template>
    <div class="row">
        <div v-for="item in props.producibleItems" :key="item.key" class="col-md-3" @click="onItemSelect(item)">
            <div class="producible-item" :class="{ selected: model != null && item.key == model.key }">
                <img style="width:60px; height: 60px; display:inline-block" />
                <div style=" display:inline-block">
                    <span><b>{{ item.name }}</b></span><br />
                    <span>{{ $duration(item.receipe.duration) }}</span>
                </div>
            </div>
        </div>
    </div>
</template>
<style lang="css" scoped>
.producible-item {
    border: 1px solid #DDD;
    border-radius: 6px;
    background-color: #FFF;
    cursor: pointer;
}

.producible-item:hover {
    background-color: antiquewhite;
}

.producible-item.selected {
    border-color: red;
}
</style>