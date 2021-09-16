<template>
  <div class="tests-list">

      <div v-for="test of getTests" :key="test" class="row py-3 border">
        <div class="col-6 url border-right">{{test.url}}</div>
        <div class="col-3 date border-right">{{test.timeCreated | moment("HH:mm:ss  L")}}</div>

        <div class="col-3 linkToDetail">
          <router-link class="link-primary" :to="'/Results/' + test.id">see details</router-link>
        </div>
      </div>

      <div class="pages w-100 d-flex justify-content-center my-4">
        <b-pagination v-model="currentPage"
                      :total-rows="totalItems"
                      :per-page="countOnPage"
                      first-number
                      last-number></b-pagination>
      </div>
  </div>
</template>

<script>
  export default {
    name: 'TestsList',
    props: ['dataTests'],

    data() {
      return {
        currentPage: 1,
        countOnPage: 10,
      }
    },

    computed: {
      totalItems: function (){
        return this.dataTests.length;
      },

      getTests: function (){
        const startIndexOfData = (this.currentPage - 1) * this.countOnPage;
        const endIndexOfData = this.currentPage * this.countOnPage;
        return Array.from(this.dataTests).slice(startIndexOfData, endIndexOfData);
      },

    },
   
  }
</script>
