<template>
  <q-expansion-item :header-inset-level="1" expand-separator :icon=icon :label="$t(page)" default-closed>
    <PageRoute v-for="item in items" :key="item.id" :pageName="item.pageName" :pageRoute="item.pageRoute" />
  </q-expansion-item>
</template>
<script setup>
import PageRoute from './PageRoute.vue'

const props = defineProps({
  icon: String,
  page: String,
  items: Array
})

</script>
