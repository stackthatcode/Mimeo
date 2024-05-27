//
// Composables
//
import { createRouter, createWebHistory, createWebHashHistory } from 'vue-router'

const routes = [

  // Here are some sample routers worth of your consideration
  //
  // path: "/company/:companyId",
  // path: "/job/:number?",
  //

  {
    path: '',
    component: () => import('@/layouts/default/DefaultLayout.vue'),

    children: [
      {
        alias: '',
        path: 'main',
        name: 'main-dashboard',
        title: 'Main Dashboard',
        component: () => import( '@/components/Main/MainDashboard.vue' ),
      },
      {
        path: 'test',
        name: 'home-view',
        title: 'Test View',
        component: () => import( '@/components/Test/TestForm.vue' ),
      },
    ],
  },
]

const router = createRouter({
  history: createWebHashHistory(process.env.BASE_URL),
  routes,
})

export default router

