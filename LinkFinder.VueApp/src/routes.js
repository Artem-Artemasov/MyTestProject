import VueRouter from "vue-router";
import TestsPage from './pages/TestsPage.vue';
import ResultPage from './pages/ResultPage.vue';

export default new VueRouter({
  routes: [
    {
      path: '',
      component: TestsPage
    },
    {
      path: '/results/:id',
      component: ResultPage
    }
  ],
  mode: 'history',
  
})
