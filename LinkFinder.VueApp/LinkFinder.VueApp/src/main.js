import Vue from 'vue'
import App from './App.vue'
import BoostrapVue from 'bootstrap-vue/dist/bootstrap-vue.esm'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import 'bootstrap/dist/css/bootstrap.css'
import router from './routes.js'
import VueRouter from 'vue-router'
import VueResource from 'vue-resource'


Vue.use(BoostrapVue);
Vue.use(VueRouter);
Vue.use(VueResource);
Vue.use(require('vue-moment'));

Vue.http.options.root = "http://dev.siteCrawler/api/";

new Vue({
  el: '#app',
  render: h => h(App),
  router
})
