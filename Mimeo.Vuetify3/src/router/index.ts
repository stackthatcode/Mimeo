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
        path: 'config',
        name: 'stock-config',
        title: 'Stocks Config',
        component: () => import( '@/components/Config/Stocks.vue' ),
      },
      {
        path: 'home',
        name: 'home-view',
        title: 'Home View',
        component: () => import( '@/components/Home/HelloWorld.vue' ),
      },
    ],
  },
]

const router = createRouter({
  history: createWebHashHistory(process.env.BASE_URL),
  routes,
})

export default router

