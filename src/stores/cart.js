/* eslint-disable no-extra-boolean-cast */
import { defineStore } from 'pinia'
import { ref } from 'vue'
import useLocalStorage from 'src/compositionFunctions/useLocalStorage'
const { updateCart, retrieveCartData } = useLocalStorage()

export const useCartStore = defineStore('cart', () => {
  const products = ref(retrieveCartData())

  function addProduct(product, quantity) {
    const index = products.value.findIndex(productInCart => productInCart.id === product.id)
    if (index !== -1) {
      products.value[index].quantity += quantity
    } else {
      products.value.push({ ...product, quantity })
    }
    updateCart(products.value)
  }

  function removeProduct(product) {
    const index = products.value.findIndex(productInCart => productInCart.id === product.id)
    if (index !== -1) {
      products.value.splice(index, 1)
      updateCart(products.value)
    }
  }

  function getNumberOfProducts() {
    return products.value.reduce((accumulator, value) => {
      return accumulator + value.quantity
    }, 0)
  }

  return { products, getNumberOfProducts, addProduct, removeProduct }
})
