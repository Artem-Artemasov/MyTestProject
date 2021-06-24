<template>
  <div class="tests-list">
    <div v-for="test of tests" class="row py-3 border">
      <div class="col-6 url border-right">{{test.url}}</div>
      <div class="col-3 date border-right">{{test.timeCreated | moment("HH:mm:ss  L")}}</div>

      <div class="col-3 linkToDetail">
        <router-link class="link-primary" :to="'/Results/' + test.id">see details</router-link>
      </div>

    </div>
  </div>
</template>

<script>
  export default {
    name: 'TestsList',
    data() {
      return {
        tests: []
      }
    },

    created() {
      this.resource = this.$resource('tests');

      this.tests = this.resource.get()
                        .then(response => response.json())
                        .then(tests => this.tests = tests);
    }

  }
</script>
