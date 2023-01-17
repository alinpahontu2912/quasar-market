import { LocalStorage, SessionStorage } from 'quasar'
import { useStorage } from '@vueuse/core'

const LOCAL_STORAGE_KEY = {
  PROFILE_PICTURE: 'ProfilePicture',
  GRID_ITEMS: 'GridItems',
  CART_ITEMS: 'CartItems',
  TOTAL_PRODUCTS: 'numberOfProducts',
  DARK_MODE: 'darkMode',
  USER_TOKEN: 'authenticaton_token',
  USER_PERMISSION: 'userPermission'
}

export default function () {
  const theDefault = {
    items: retrieveCartData()
  }
  const cartState = useStorage(LOCAL_STORAGE_KEY.CART_ITEMS, [])

  function saveUserData(auth_token) {
    try {
      LocalStorage.set(LOCAL_STORAGE_KEY.USER_TOKEN, auth_token)
    } catch (e) {
      console.log('Could not save data on local storage')
    }
  }

  function retrieveUserData() {
    return LocalStorage.getItem(LOCAL_STORAGE_KEY.USER_TOKEN) || null
  }

  function setDarkMode(value) {
    try {
      LocalStorage.set(LOCAL_STORAGE_KEY.DARK_MODE, value)
    } catch (e) {
      console.log('Could not save data on local storage')
    }
  }

  function retrieveDarkMode() {
    return LocalStorage.getItem(LOCAL_STORAGE_KEY.DARK_MODE) || false
  }

  function updateCart(value) {
    try {
      LocalStorage.set(LOCAL_STORAGE_KEY.CART_ITEMS, value)
    } catch (e) {
      console.log('Could not save data on local storage')
    }
  }

  function retrieveCartData() {
    return LocalStorage.getItem(LOCAL_STORAGE_KEY.CART_ITEMS) || []
  }

  function updateTotalProducts(value) {
    try {
      LocalStorage.set(LOCAL_STORAGE_KEY.TOTAL_PRODUCTS, value)
    } catch (e) {
      console.log('Could not save data on local storage')
    }
  }

  function retrieveNumberOfProducts() {
    return LocalStorage.getItem(LOCAL_STORAGE_KEY.TOTAL_PRODUCTS) || 0
  }

  function updateGrid(value) {
    try {
      LocalStorage.set(LOCAL_STORAGE_KEY.GRID_ITEMS, value)
    } catch (e) {
      console.log('Could not save data on local storage')
    }
  }

  function updateProfilePicture(value) {
    try {
      LocalStorage.set(LOCAL_STORAGE_KEY.PROFILE_PICTURE, value)
    } catch (e) {
      console.log('Could not save data on local storage')
    }
  }

  function retrieveGridData() {
    return LocalStorage.getItem(LOCAL_STORAGE_KEY.GRID_ITEMS) || []
  }

  function retrieveProfilePicture() {
    return LocalStorage.getItem(LOCAL_STORAGE_KEY.PROFILE_PICTURE) || ''
  }

  function clearLocalStorage() {
    LocalStorage.clear()
  }

  return {
    updateGrid,
    updateProfilePicture,
    updateCart,
    updateTotalProducts,
    retrieveNumberOfProducts,
    retrieveGridData,
    retrieveProfilePicture,
    retrieveCartData,
    clearLocalStorage,
    setDarkMode,
    retrieveDarkMode,
    saveUserData,
    retrieveUserData,
    cartState,
    theDefault
  }
}
