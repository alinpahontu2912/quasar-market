import { defineStore } from 'pinia'
import { ref } from 'vue'
import useLocalStorage from 'src/compositionFunctions/useLocalStorage'
import useHttpQuery from 'src/compositionFunctions/useHttpQuery'
const { saveUserData, retrieveUserData } = useLocalStorage()
const { addNewUser, verifyUserCredentials } = useHttpQuery()

export const userStore = defineStore('user', () => {
  const userToken = ref(retrieveUserData())

  function checkUserRemainSignedIn(JWTtoken) {
    if (userToken.value) {
      this.router.replace({ path: '/store/products' })
    }
  }

  async function signUpRequest(email, fullName, phoneNumber, password, address, remainSignedIn) {
    const response = await addNewUser(email, fullName, phoneNumber, password, address)
    if (response) {
      userToken.value = response
      if (remainSignedIn) { saveUserData(response) }
      this.router.replace({
        path: '/store/products'
      })
    }
  }

  async function loginRequest(email, password, remainSignedIn) {
    const response = await verifyUserCredentials(email, password)
    if (response) {
      userToken.value = response
      if (remainSignedIn) { saveUserData(response) }
      this.router.replace({
        path: '/store/products'
      })
      return true
    }
    return false
  }

  function logout() {
    userToken.value = null
    saveUserData(userToken.value)
    this.router.replace({
      path: '/auth'
    })
  }

  function getUserRole() {
    return userToken.value.role === 1
  }

  return { userToken, signUpRequest, loginRequest, logout, checkUserRemainSignedIn, getUserRole }
})
