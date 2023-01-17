<template>
  <q-table ref="tableRef" style="height: 80vh;" v-model:pagination="pagination" row-key="id" title="Products"
    :rows="rows" :columns="columns" selection="multiple" :loading="loading" :filter="filter" binary-state-sort
    @request="onRequest">
    <template v-slot:loading>
      <q-inner-loading showing color="primary" />
    </template>
    <template v-slot:top-right>
      <q-input borderless dense debounce="300" v-model="filter" placeholder="Search" class="q-pa-sm">
        <template v-slot:append>
          <q-icon name="search" />
        </template>
      </q-input>
      <q-btn class="q-pa-sm" icon="add" @click="openNewProductForm" />
    </template>
    <template v-slot:header-selection>
      <q-btn flat icon="refresh" label="Refresh" @click="refresh(props)" />
    </template>
    <template v-slot:body-selection="scope">
      <q-btn flat icon="delete" @click="openDeletePopup(scope.row.id)">
        <q-tooltip>
          Delete Product
        </q-tooltip>
      </q-btn>
      <q-btn flat icon="edit" @click="openPricePopup(scope.row.id, scope.row.price)">
        <q-tooltip>
          Update Price
        </q-tooltip>
      </q-btn>
    </template>
    <template v-slot:body-cell-price="props">
      <q-td>
        {{ props.row.price }}
        <q-popup-edit v-model="props.row.price" auto-save v-slot="scope">
          <q-input type="number" v-model.number="scope.value" dense autofocus />
        </q-popup-edit>
      </q-td>
    </template>
  </q-table>
  <q-dialog v-model="firstDeletePopUp">
    <q-card>
      <q-card-section class="row items-center q-pb-none">
        <div class="text-h6">Are you sure you want to delete the product?</div>
        <q-space></q-space>
        <q-btn icon="close" flat round dense v-close-popup></q-btn>
      </q-card-section>
      <q-card-section class="row justify-around">
        <q-btn color="primary" label="YES" v-close-popup @click="deleteFromDB(selectedProdId)" />
        <q-btn color="primary" label="NO" v-close-popup />
      </q-card-section>
    </q-card>
  </q-dialog>
  <q-dialog v-model="secondDialog" transition-show="scale" transition-hide="scale">
    <q-card class="bg-teal text-white" style="width: 300px; height: 60px;">
      <q-card-section v-close-popup class="row items-center justify-center">
        {{ deleteResult }}
      </q-card-section>
    </q-card>
  </q-dialog>
  <q-dialog v-model="pricePopUp" transition-show="scale" transition-hide="scale">
    <q-card class="bg-teal text-white" style="width: 300px; height: 60px;">
      <q-card-section v-close-popup class="row items-center justify-center">
        {{ updateResult }}
      </q-card-section>
    </q-card>
  </q-dialog>
  <NewProductFormVue />
</template>

<script setup>
import { ref, inject, onMounted } from 'vue'
import NewProductFormVue from './NewProductForm.vue'
import { EVENT_KEYS } from '../utils/eventKeys.js'
import useHttpQuery from 'src/compositionFunctions/useHttpQuery'
const { deleteProduct, updateProductPrice, getProductCount, getProductPage } = useHttpQuery()
const pagination = ref({
  sortBy: 'id',
  descending: false,
  page: 1,
  rowsPerPage: 5,
  rowsNumber: -1
})
const tableRef = ref()
const selectedProdId = ref(-1)
const firstDeletePopUp = ref(false)
const deleteResult = ref('')
const updateResult = ref('')
const filter = ref('')
const secondDialog = ref(false)
const pricePopUp = ref(false)
const loading = ref(false)
const bus = inject('bus')
const columns = [{
  name: 'id',
  required: true,
  label: 'Id Produs',
  align: 'center',
  field: row => row.id,
  format: val => `${val}`,
  sortable: true
},
{
  name: 'name',
  label: 'Nume Produs',
  align: 'center',
  field: row => row.name,
  format: val => `${val}`,
  sortable: true
},
{
  name: 'price',
  label: 'Pret Produs',
  align: 'center',
  field: row => row.price,
  format: val => `${val}`,
  sortable: true
},
{
  name: 'description',
  label: 'Descriere Produs',
  align: 'center',
  field: row => row.description.slice(0, 50),
  format: val => `${val}`
},
{
  name: 'image',
  label: 'Imagine Produs',
  align: 'center',
  field: row => row.image,
  format: val => `${val}`
}
]

const rows = ref([])

async function refresh() {
  await tableRef.value.requestServerInteraction()
}

function openDeletePopup(productId) {
  firstDeletePopUp.value = !firstDeletePopUp.value
  selectedProdId.value = productId
}

async function openPricePopup(productId, newPrice) {
  loading.value = true
  const result = await updateProductPrice(productId, newPrice)
  updateResult.value = result ? 'Price updated' : 'Error'
  pricePopUp.value = !pricePopUp.value
  loading.value = false
}

async function deleteFromDB(productId) {
  loading.value = true
  const result = await deleteProduct(productId)
  secondDialog.value = !secondDialog.value
  deleteResult.value = result ? 'Product removed' : 'Removal issue'
  loading.value = false
}

function openNewProductForm() {
  bus.emit(EVENT_KEYS.NEW_PRODUCT, rows.value[rows.value.length - 1].id)
}

async function onRequest(props) {
  const { page, rowsPerPage, sortBy, descending } = props.pagination
  loading.value = true

  pagination.value.rowsNumber = await getProductCount(filter.value)
  const fetchCount = rowsPerPage === 0 ? pagination.value.rowsNumber : rowsPerPage
  const startRow = page
  const orderType = descending ? 'DESC' : 'ASC'
  const returnedData = await getProductPage(startRow, fetchCount, sortBy, orderType, filter.value)

  rows.value.splice(0, rows.value.length, ...returnedData)

  pagination.value.page = page
  pagination.value.rowsPerPage = rowsPerPage
  pagination.value.sortBy = sortBy
  pagination.value.descending = descending

  loading.value = false
}

onMounted(async () => {
  await tableRef.value.requestServerInteraction()
})
</script>
