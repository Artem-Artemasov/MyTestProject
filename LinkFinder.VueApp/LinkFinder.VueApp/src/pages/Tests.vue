<template>
  <div class="py-4">
    <website-url-form></website-url-form>

    <div class="tests text-center">
      <h2 class="caption py-2">Test results</h2>
      <div class="container rounded m-auto bg-light">

        <div class="row py-3 border">
          <div class="col-6 url border-right">Website</div>
          <div class="col-3 date border-right">Date</div>
        </div>

        <tests-list></tests-list>

      </div>
    </div>
  </div>
</template>

<script>
  import WebsiteUrlForm from "../components/WebsiteUrlForm/WebsiteUrlForm.vue"
  import TestsList from '../components/TestsList/TestsList.vue'

  export default {
    name: 'Tests',
    components: {
      'website-url-form': WebsiteUrlForm,
      'tests-list': TestsList,
    }
  }
</script>
