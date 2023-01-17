import { boot } from 'quasar/wrappers'
import axios from 'axios'

const axiosInstance = axios.create()

export default boot(({ app }) => {
  // for use inside Vue files (Options API) through this.$axios and this.$api

  app.config.globalProperties.$axios = axiosInstance
  // ^ ^ ^ this will allow you to use this.$axios (for Vue Options API form)
  //       so you won't necessarily have to import axios in each vue file
})

export { axiosInstance }
