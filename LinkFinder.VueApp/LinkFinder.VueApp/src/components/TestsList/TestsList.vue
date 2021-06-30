<template>
  <div class="tests-list">

      <div v-for="test of getTests" class="row py-3 border">
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
        tests:[],
        currentPage: 1,
        countOnPage: 10,
        startIndexOfData: 0,
        endIndexOfData: 0,
      }
    },

    computed: {
      totalItems: function (){
        return this.dataTests.length;
      },

      getTests: function (){
        this.startIndexOfData = (this.currentPage - 1) * this.countOnPage;
        this.endIndexOfData = this.currentPage * this.countOnPage;
        return this.tests = Array.from(this.dataTests).slice(this.startIndexOfData, this.endIndexOfData);
      },

    },
   
  }
</script>
