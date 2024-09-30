import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useNotificationStore = defineStore('notification', () => {
  const notifications = ref([])

  const add = function (item) {
    item.id = new Date().getTime().toString() + (Math.random() * 100000).toFixed(0)
    notifications.value.push(item)
    setTimeout(() => {
      remove(item.id)
    }, 4000)
    return item.id
  }

  const addResponse = function (val) {
    var message = val.response.data.message
    return addError(message)
  }

  const addWarning = function (message) {
    var item = { message, type: 'warning' }
    return add(item)
  }

  const addError = function (message) {
    var item = { message, type: 'danger' }
    return add(item)
  }

  const addInfo = function (message) {
    var item = { message, type: 'info' }
    return add(item)
  }

  const remove = function (id) {
    notifications.value = notifications.value.filter((x) => x.id != id)
  }

  return { notifications, add, addResponse, addWarning, addError, addInfo, remove }
})
