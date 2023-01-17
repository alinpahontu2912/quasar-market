/* eslint-disable no-extra-boolean-cast */
import { defineStore } from 'pinia'
import useLocalStorage from 'src/compositionFunctions/useLocalStorage'
const { cartState } = useLocalStorage()

export const useCartStore = defineStore('cart', () => {
  function addProduct(product, quantity) {
    const index = cartState.value.findIndex(productInCart => productInCart.id === product.id)
    if (index !== -1) {
      cartState.value[index].quantity += quantity
    } else {
      cartState.value.push({ ...product, quantity })
    }
  }

  function removeProduct(product) {
    const index = cartState.value.findIndex(productInCart => productInCart.id === product.id)
    if (index !== -1) {
      cartState.value.splice(index, 1)
    }
  }

  function getNumberOfProducts() {
    return cartState.value.reduce((accumulator, value) => {
      return accumulator + value.quantity
    }, 0)
  }

  return { cartState, getNumberOfProducts, addProduct, removeProduct }
})
