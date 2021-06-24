import VueRouter from "vue-router";
import Tests from './pages/Tests.vue';
import Result from './pages/Result.vue';

export default new VueRouter({
  routes: [
    {
      path: '',
      component: Tests
    },
    {
      path: '/results/:id',
      component: Result
    }
  ],
  mode: 'history',
  
})
