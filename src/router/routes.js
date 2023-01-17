
const routes = [
  {
    path: '/',
    component: () => import('layouts/BlankLayout.vue'),
    children: [
      { path: '', component: () => import('pages/AuthForm.vue'), meta: { requiresPermission: false } }
    ]
  },
  {
    path: '/store/',
    component: () => import('layouts/Layout.vue'),
    children: [
      { path: 'home', component: () => import('pages/HomePage.vue'), meta: { requiresPermission: false } },
      { path: 'hello', component: () => import('pages/HelloWorld.vue'), meta: { requiresPermission: false } },
      { path: 'products', component: () => import('pages/ProductsPage.vue'), meta: { requiresPermission: false } }, // auth Required : true, roles
      { path: 'control', component: () => import('pages/AdminProdPanel.vue'), meta: { requiresPermission: true } }
    ]
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue')
  }
]

export default routes
