import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useItemStore = defineStore('item', () => {
  const items = ref({})

  const find = function (key) {
    return items.value[key]
  }

  const setItems = function (val) {
    items.value = val
  }

  return { items, find, setItems }
})
