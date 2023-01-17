<template>
  <div class="q-pa-md">
    <q-infinite-scroll @load="onLoad" :offset="250">
      <div class="row fit ">
        <ProductTile v-for="(item, index) in items" :key="index" :product="item" />
      </div>
      <template v-slot:loading>
        <div class="row justify-center q-my-md">
          <q-spinner-dots color="primary" size="40px" />
        </div>
      </template>
    </q-infinite-scroll>
  </div>

</template>
<script setup>
import { ref, inject } from 'vue'
import useHttpQuery from 'src/compositionFunctions/useHttpQuery'
import { EVENT_KEYS } from '../utils/eventKeys.js'
import ProductTile from 'src/components/ProductTile.vue'

const items = ref([])
const pageSize = ref(10)
const orderCriteria = ref('id')
const orderType = ref('ASC')

const bus = inject('bus')
const { loadInfScrollPage } = useHttpQuery()

bus.on(EVENT_KEYS.CHANGE_PAGE_SIZE, (newSize) => {
  pageSize.value = newSize
})

bus.on(EVENT_KEYS.ORDER_CRITERIA, (newCriteria) => {
  orderCriteria.value = newCriteria
})

bus.on(EVENT_KEYS.ORDER_TYPE, (newOrderType) => {
  orderType.value = newOrderType
})

async function onLoad(index, done) {
  await loadInfScrollPage(index, pageSize.value, orderCriteria.value, orderType.value, done, items.value)
}

</script>
