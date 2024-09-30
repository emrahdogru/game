import axios from 'axios'
import 'bootstrap/dist/css/bootstrap.min.css'
import './assets/main.css'
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'

axios.defaults.baseURL = 'https://localhost:7200/api'

const app = createApp(App)

app.use(createPinia())

app.config.globalProperties.$duration = function (seconds) {
  seconds = parseInt(seconds, 10)
  const days = Math.floor(seconds / (24 * 3600)) // 1 gün = 24 saat * 3600 saniye
  seconds %= 24 * 3600
  const hours = Math.floor(seconds / 3600) // 1 saat = 3600 saniye
  seconds %= 3600
  const minutes = Math.floor(seconds / 60) // 1 dakika = 60 saniye
  seconds %= 60

  let result = ''
  if (days > 0) result += `${days}d `
  if (hours > 0) result += `${hours}h `
  if (minutes > 0) result += `${minutes}m `
  if (seconds > 0) result += `${seconds}s `

  return result.trimEnd()
}

app.mount('#app')
