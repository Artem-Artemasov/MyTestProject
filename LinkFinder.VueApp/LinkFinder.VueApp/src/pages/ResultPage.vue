<template>
  <div>
    <!--Backward button-->
    <div class="d-flex px-4 my-3 w-100 mx-auto">
      <router-link  :to="'/'" class="btn btn-dark" role="button">
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-arrow-left" viewBox="0 0 16 16">
          <path fill-rule="evenodd" d="M15 8a.5.5 0 0 0-.5-.5H2.707l3.147-3.146a.5.5 0 1 0-.708-.708l-4 4a.5.5 0 0 0 0 .708l4 4a.5.5 0 0 0 .708-.708L2.707 8.5H14.5A.5.5 0 0 0 15 8z" />
        </svg>
        Back
      </router-link>
    </div>

    <!--Perfomance table-->
    <div class="perfomance-table">
      <h2 class="px-4 mt-5 w-50 mx-auto">Performance</h2>
      <links-block :testId="testId" :enableTime="true"> </links-block>
    </div>

    <!--Not found at website-->
    <div class="links-block">
      <h3 class="px-4 mt-5 w-50 mx-auto">URLs NOT FOUND AT WEBSITE</h3>
      <links-block :testId="testId" :inSitemap="true"> </links-block>
    </div>

    <!--Not found at sitemap-->
    <div class="links-block">
      <h3 class="px-4 mt-5 w-50 mx-auto">URLs NOT FOUND AT SITEMAP</h3>
      <links-block :testId="testId" :inHtml="true"> </links-block>
    </div>

  </div>
</template>

<script>
  import LinksBlock from '../components/LinksBlock/linksBlock.vue'

  export default {
    name: 'Result',

    data() {
      return {
        testId : this.$router.currentRoute.params['id'],
      }
    },

    components: {
      'links-block': LinksBlock,
    },
  }
</script>
