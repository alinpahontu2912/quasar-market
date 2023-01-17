<template>
  <RouterView />
</template>

<script setup>
import useLocalStorage from 'src/compositionFunctions/useLocalStorage'
import { useQuasar } from 'quasar'
import { ref, onMounted } from 'vue'
const { retrieveDarkMode } = useLocalStorage()
const darkMode = ref(retrieveDarkMode())
const $q = useQuasar()
onMounted(() => {
  if (darkMode.value) $q.dark.set(true)
})
</script>
