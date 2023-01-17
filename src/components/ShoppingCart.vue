<template>

  <q-btn class="btn btn-small btn-info btn-popup q-pa-md" icon="local_grocery_store" label="Cart">
    <q-tooltip v-if="cartState.length > 0" anchor="bottom middle" self="top middle">
      Number of products:{{ cartState.length }}
    </q-tooltip>
    <q-tooltip v-else anchor="bottom middle" self="top middle">
      No products :/
    </q-tooltip>
    <q-menu max-height="150px" wid>
      <q-list style="min-width: 230px">
        <q-item clickable v-for="prod in cartState" :key="prod.id">
          <div class="row">
            <div class="col-5">
              <q-img style="width: 70px; height: 70px;" :src="prod.image" no-native-menu>
                <div class="absolute-bottom-right text-subtitle2">
                  {{ prod.name }}
                </div>
              </q-img>
            </div>
            <div class="col-4 q-mt-md q-ml-md">
              <q-item-section>Cantitate: </q-item-section>
              <q-item-section>{{ prod.quantity }}</q-item-section>
            </div>
            <div class="col q-ml-md">
              <q-btn dense flat icon="close" @click="() => removeProduct(prod)"></q-btn>
            </div>
          </div>
          <q-separator></q-separator>
        </q-item>
        <q-item v-if="cartState.length > 0">
          <q-btn dense flat icon class="btn btn-small btn-info btn-popup q-pa-md">Checkout</q-btn>
        </q-item>
      </q-list>
    </q-menu>
  </q-btn>
</template>
<script setup>
import { useCartStore } from '../stores/cart'
import useLocalStorage from '../compositionFunctions/useLocalStorage'
const { cartState } = useLocalStorage()
const { removeProduct } = useCartStore()

</script>
