<script setup lang="js">
import axios from 'axios';
import { ref, onMounted } from 'vue';
import GameBoard from './GameBoard.vue';
import { useItemStore } from '@/stores/item'

const gameBoards = ref([]);
const gameBoard = ref(null);

const getGameBoards = async () => {
    axios.get('gameboard')
        .then(x => {
            gameBoards.value = x.data;
        })
        .catch(x => {
            alert("Get gameboard list failed!");
            console.error(x);
        })
}

const getItems = async () => {
    axios.get('item')
        .then(x => {
            const itemStore = useItemStore();
            itemStore.setItems(x.data);
        })
}

onMounted(async () => {
    await getGameBoards();
    await getItems();
});

function selectGame(game) {
    axios.defaults.headers['game'] = game.id;
    gameBoard.value = game;
}

</script>
<template>
    <div v-if="gameBoard == null" style="border:1px dotted red">
        <ul>
            <li v-for="game in gameBoards" :key="game.id">
                <button v-on:click="selectGame(game)">{{ game }}</button>
            </li>
        </ul>
    </div>
    <GameBoard v-else :id="gameBoard.id"></GameBoard>
</template>