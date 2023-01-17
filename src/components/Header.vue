<template>
  <q-header elevated class="bg-black row no-wrap items-center" style="height: 10vh;">
    <q-btn flat @click="toggleDrawer" round dense icon="menu"></q-btn>
    <ShoppingCart />
    <q-space />
    <q-btn-dropdown label="View">
      <q-list>
        <q-item>
          <q-select @update:model-value="changePageSize" class="q-pa-md" color="grey" outlined label-color="white"
            v-model="pageSize" :options="pageOptions">
            <template v-slot:before>
              <q-icon name="event" />
            </template>
          </q-select>
        </q-item>

        <q-item>
          <q-select class="q-pa-md" color="grey" outlined label-color="white" v-model="orderBy"
            @update:model-value="changeOrdering" :options="orderOptions" style="width: 150px;">
            <template v-slot:before>
              <q-icon :name="arrow" clickable @click="changeOrderType" />
            </template>
          </q-select>
        </q-item>
      </q-list>
    </q-btn-dropdown>
    <q-btn-dropdown label="Settings">
      <div class="row no-wrap q-pa-md">
        <q-checkbox v-model="darkMode" label="Dark Mode" @update:model-value="toggleDarkMode" />
      </div>
    </q-btn-dropdown>
    <ProfilePicture />
  </q-header>
</template>

<script setup>
import { ref, inject } from 'vue'
import ProfilePicture from './ProfilePicture.vue'
import { EVENT_KEYS } from '../utils/eventKeys.js'
import ShoppingCart from './ShoppingCart.vue'
import { useQuasar } from 'quasar'
import useLocalStorage from 'src/compositionFunctions/useLocalStorage'
const { setDarkMode, retrieveDarkMode } = useLocalStorage()
const pageOptions = ref([10, 15, 20])
const pageSize = ref(10)
const orderBy = ref('id')
const darkMode = ref(retrieveDarkMode())
const orderOptions = ref(['id', 'price', 'name'])
const arrow = ref('keyboard_arrow_up')

const $q = useQuasar()
const bus = inject('bus')

function toggleDrawer() {
  bus.emit(EVENT_KEYS.TOGGLE_DRAWER, '')
}

function changePageSize(value) {
  pageSize.value = value
  bus.emit(EVENT_KEYS.CHANGE_PAGE_SIZE, pageSize.value)
}

function changeOrderType() {
  if (arrow.value === 'keyboard_arrow_up') {
    arrow.value = 'keyboard_arrow_down'
  } else {
    arrow.value = 'keyboard_arrow_up'
  }
  bus.emit(EVENT_KEYS.ORDER_TYPE, arrow.value === 'keyboard_arrow_up' ? 'ASC' : 'DSC')
}

function changeOrdering(value) {
  orderBy.value = value
  bus.emit(EVENT_KEYS.ORDER_CRITERIA, orderBy.value)
}

function toggleDarkMode() {
  $q.dark.toggle()
  setDarkMode($q.dark.isActive)
}

</script>
