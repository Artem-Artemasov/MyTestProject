<template>
  <div class="py-4">
    <url-input-form :onTestAdded="concatTest"></url-input-form>

    <!--Render tests list-->
    <div class="tests text-center">
      <h2 class="caption py-2">Test results</h2>
      <div class="container rounded m-auto bg-light">

        <div class="row py-3 border">
          <div class="col-6 url border-right">Website</div>
          <div class="col-3 date border-right">Date</div>
        </div>

        <tests-list :dataTests="tests"></tests-list>

      </div>
    </div>

  </div>
</template>

<script>
  import TestsList from '../components/TestsList/testsList.vue'
  import UrlInputForm from '../components/UrlInputForm/urlInputForm.vue'

  export default {
    name: 'Tests',

    components: {
      'tests-list': TestsList,
      'url-input-form': UrlInputForm,
    },

    data() {
      return {
        tests: [],
      }
    },

    created() {
      this.downloadTests();
    },

    methods: {
      concatTest(test) {
        this.tests.unshift(test);
      },

      downloadTests(){
        this.resource = this.$resource('test');
        
        this.tests = this.resource.get()
          .then(response => response.json())
          .then(tests => this.tests = tests.content)
          .catch(badResponse => { });
      },

    },

  }
</script>
