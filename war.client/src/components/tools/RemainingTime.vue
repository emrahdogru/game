<script setup lang="js">
import { ref, onMounted, computed, onUnmounted } from 'vue';

var props = defineProps({
    requestDate: Date,
    duration: Number,
    total: Number
})

var emits = defineEmits(["completed"]);

const duration = ref(parseInt(props.requestDate ? props.duration - ((new Date().getTime() - props.requestDate.getTime()) / 1000) : props.duration, 10));
const percentageCounter = ref(parseInt(props.requestDate ? props.duration - ((new Date().getTime() - props.requestDate.getTime()) / 1000) : props.duration, 10));

const percentage = computed(() => Math.floor(100 - (Math.max(percentageCounter.value, 0) / props.total * 100)));

const pieStyle = computed(() => {
    return {
        background: `conic-gradient(
          #4CAF50 ${(percentage.value) * 3.6}deg, 
          #FFC107 ${(percentage.value) * 3.6}deg)`
    };
})

let intervalId;
let intervalPercentage;

onMounted(() => {
    console.log('RT mounted');
    intervalId = setInterval(() => {
        duration.value = duration.value - 1;
        if (duration.value <= 1) {
            clearInterval(intervalId);
            duration.value = -1;
            emits('completed');
        }
    }, 1000);

    intervalPercentage = setInterval(() => {
        percentageCounter.value = percentageCounter.value - 0.1;
        if (percentageCounter.value < 0) {
            clearInterval(intervalPercentage);
        }
    }, 100);
});

onUnmounted(() => {
    clearInterval(intervalId);
    clearInterval(intervalPercentage)
})
</script>
<template>
    <div class="pie-chart" style="width: 120px; height: 120px; border-radius: 50;" :style="pieStyle">
        <div style="font-size:30pt; text-align: center; height: 90px; padding-top:20px;">%{{ percentage.toFixed(0) }}
        </div>
        <div style="text-align: center;">{{ $duration(Math.max(duration, 0)) }}</div>
    </div>
</template>