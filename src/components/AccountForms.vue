<template>
  <div class="q-pa-md">
    <q-card class="my-card">
      <q-card-section>
        <div class="text-h6 row items-center justify-center">Welcome to the store!</div>
      </q-card-section>
      <q-tabs v-model="tab" class="text-teal" dense align="justify" :breakpoint="0">
        <q-tab label="Create account" name="one" />
        <q-tab label="Have an account?" name="two" />
      </q-tabs>
      <q-separator />
      <q-tab-panels v-model="tab" animated>
        <q-tab-panel name="one">
          <q-form class="q-gutter-md row items-center justify-center">
            <q-input square filled clearable v-model="signUpData.email" type="email" label="Email"
              class="q-pa-sm col-12" />
            <q-input square filled clearable v-model="signUpData.fullName" type="text" label="Full Name"
              class="q-pa-sm col-12" />
            <q-input square filled clearable mask="(###) ### - ####" unmasked-value v-model="signUpData.phoneNumber"
              label="Phone number" class="q-pa-sm col-12" />
            <q-input square filled clearable v-model="signUpData.password" type="password" label="Password"
              class="q-pa-sm col-12" />
            <q-input square filled clearable v-model="signUpData.address" type="address" label="Address"
              class="q-pa-sm col-12" />
            <q-btn unelevated color="light-green-7" size="md" class="q-pa-sm col-12" label="Sign Up"
              @click="signUpRequest(...Object.values(signUpData), remainSignedIn)" />
            <q-checkbox left-label v-model="remainSignedIn" label="Stay logged in" checked-icon="task_alt"
              unchecked-icon="highlight_off" />
          </q-form>
        </q-tab-panel>
        <q-tab-panel name="two">
          <q-form class="q-gutter-md row items-center justify-center">
            <q-input square filled clearable v-model="logInData.email" type="email" label="email"
              class="q-pa-md col-12" />
            <q-input square filled clearable v-model="logInData.password" type="password" label="password"
              class="q-pa-md col-12" />
            <q-btn unelevated color="light-green-7" size="md" class="q-pa-md col-12" label="Login"
              @click="loginRequest(...Object.values(logInData), remainSignedIn)" />
            <q-checkbox left-label v-model="remainSignedIn" label="Stay logged in" checked-icon="task_alt"
              unchecked-icon="highlight_off" />
          </q-form>
        </q-tab-panel>
      </q-tab-panels>
    </q-card>
  </div>
</template>
<script setup>
import { ref, onMounted } from 'vue'
import { userStore } from '../stores/user'
const { checkUserRemainSignedIn, loginRequest, signUpRequest } = userStore()
const tab = ref('one')
const signUpData = ref({ email: '', fullName: '', phoneNumber: '', password: '', address: '' })
const logInData = ref({ email: '', password: '' })
const remainSignedIn = ref(true)
onMounted(async () => {
  checkUserRemainSignedIn()
})
</script>
<style lang="sass" scoped>
.my-card
  width: 100%
  max-width: 40vw
  height: 70%
</style>
