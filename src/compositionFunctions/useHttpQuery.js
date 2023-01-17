
import useLocalStorage from './useLocalStorage'
import { Product } from 'src/models/Product'
import { User } from 'src/models/User'
import { axiosInstance } from '../boot/axios'
import axios from 'axios'
const storeEndpoint = 'http://localhost:7023/api/products'
const userEndpoint = 'http://localhost:7023/api/users'
const { retrieveUserData, saveUserData } = useLocalStorage()

axiosInstance.interceptors.request.use(function (config) {
  const userToken = retrieveUserData()
  if (userToken) {
    config.headers.Authorization = 'Bearer ' + userToken
  }
  return config
})

// not tested yet
axiosInstance.interceptors.response.use((response) => {
  return response
}, async function (error) {
  const originalRequest = error.config
  if (error.response.status !== 200) {
    const response = await axios.get('http://localhost:7023/api/refresh', originalRequest)
    saveUserData(response.data)
  }
  return Promise.reject(error)
})

function createProductCountQuery(filter) {
  const target = new URL(storeEndpoint + '/count')
  const params = new URLSearchParams()
  params.set('filter', filter)
  target.search = params.toString()
  return target.href
}

function createPageNumberQuery(pageNumber, pageSize, orderCriteria, orderType, filter) {
  const target = new URL(storeEndpoint)
  const params = new URLSearchParams()
  params.set('pgsize', pageSize)
  params.set('page', pageNumber)
  params.set('orderBy', orderCriteria)
  params.set('orderType', orderType)
  params.set('filter', filter)
  target.search = params.toString()
  return target.href
}

function createUserCredentialsQuery(user, password) {
  const target = new URL(userEndpoint)
  const params = new URLSearchParams()
  params.set('user', user)
  params.set('pswd', password)
  target.search = params.toString()
  return target.href
}

function createTokenRefreshQuery(user, token) {
  const target = new URL(userEndpoint + '/refreshToken')
  const params = new URLSearchParams()
  params.set('user', user)
  params.set('refreshToken', token)
  target.search = params.toString()
  return target.href
}

async function refreshToken(mail, token) {
  try {
    const response = await axiosInstance.get(createTokenRefreshQuery(mail, token))
    if (response.status === 200) {
      return response.data
    }
  } catch (error) {
    return null
  }
}

async function getProductCount(filter) {
  try {
    const response = await axiosInstance.get(createProductCountQuery(filter))
    if (response.status === 200) {
      return response.data
    }
  } catch (error) {
    return null
  }
}

function createDeleteIdQuery(productId) {
  const target = new URL(storeEndpoint + '/delete/')
  return target.href + productId
}

function createPriceUpdateQuery(productId, newPrice) {
  const target = new URL(storeEndpoint)
  return target.href + '/' + productId + '/' + newPrice
}

export default function () {
  async function verifyUserCredentials(mail, password) {
    try {
      const response = await axiosInstance.get(createUserCredentialsQuery(mail, password))
      if (response.status === 200) {
        return response.data
      }
    } catch (error) {
      return null
    }
  }

  async function addNewUser(email, fullName, phoneNumber, password, address) {
    const newUser = new User(email, fullName, phoneNumber, password, address)
    try {
      const response = await axiosInstance.post(userEndpoint + '/add/', newUser)
      return response.data
    } catch (error) {
      return null
    }
  }

  async function getNoPaginationProducts() {
    const response = await axiosInstance.get(storeEndpoint)
    const data = response.data
    const products = []
    for (let i = 0; i < data.length; i++) {
      products.push(new Product(...Object.values(data[i])))
    }
    return products
  }

  async function loadInfScrollPage(index, pageSize, orderCriteria, orderType, done, container) {
    const response = await axiosInstance.get(createPageNumberQuery(index, pageSize, orderCriteria, orderType, ''))
    const data = response.data
    for (let i = 0; i < data.length; i++) {
      container.push(new Product(...Object.values(data[i])))
    }
    done()
  }

  async function getProductPage(pageNumber, pageSize, orderCriteria, orderType, filter) { // pageNumber, pageSize, orderCriteria, orderType, filter
    try {
      const response = await axiosInstance.get(createPageNumberQuery(pageNumber, pageSize, orderCriteria, orderType, filter))
      const newProducts = []
      response.data.forEach(element => {
        newProducts.push(new Product(...Object.values(element)))
      })
      return newProducts
    } catch (error) {
      return null
    }
  }

  async function deleteProduct(productId) {
    const response = await axiosInstance.delete(createDeleteIdQuery(productId))
    return response.status === 200
  }

  async function updateProductPrice(productId, newPrice) {
    const response = await axiosInstance.put(createPriceUpdateQuery(productId, newPrice))
    return response.status === 200
  }

  async function addProduct(productId, name, price, description, image) {
    const newProd = new Product(productId, name, price, description, image)
    const response = await axiosInstance.post(storeEndpoint + '/add/', newProd)
    return response.status === 200
  }

  return { getNoPaginationProducts, deleteProduct, loadInfScrollPage, getProductPage, updateProductPrice, addProduct, addNewUser, verifyUserCredentials, getProductCount, refreshToken }
}
