<template>
  <div class="performance w-50 my-4 p-4 border mx-auto">
    <div class="row border-bottom pb-3">
      <div class="col-10 px-5 border-right">Url</div>
      <div v-if="!this.inSitemap && !this.inHtml" class="col-2  text-center">Time Response</div>
    </div>

      <crawled-link v-for="result of testDetail.results"
                    :url="result.url"
                    :enableTime="enableTime"
                    :timeResponse="result.timeResponse"
                    :key="result.id">
      </crawled-link>

    <div v-if="this.inSitemap && this.inHtml"
         class="pages w-50 justify-content-center align-items-center px-auto d-flex my-2 mx-auto">

      <b-pagination v-model="currentPage"
                    :total-rows="countResults"
                    :per-page="perPage"
                    first-number
                    last-number
                    @change="changePage"></b-pagination>
    </div>

  </div>
</template>

<script>
  import Link from '../Link/Link.vue';

  export default {
    name: 'linksBlock',

    props:
    {
      testId: "",
      enableTime: {
        type: Boolean,
        default: false,
      },
      inSitemap: {
        type: Boolean,
        default: false,
      },
      inHtml: {
        type: Boolean,
        default: false,
      },
    },

    components:{
      'crawled-link' : Link
    },

    data() {
      return {
        testDetail: [],
        currentPage: 1,
        perPage: 10,
        countResults: 0,
      }
    },

    methods: {
      changePage(newPage) {
        this.downloadPerfomanceList(newPage);
      },

      downloadPerfomanceList(page) {
        this.resource = this.$resource('test/' + this.testId
                                               + "?Page=" + page
                                               + "&CountResultsOnPage=" + this.perPage
                                               + "&InHtml=" + this.inHtml
                                               + "&InSitemap=" + this.inSitemap
                                      );

        this.resource.get()
          .then(response => response.json())
          .then(links => this.testDetail = links.content)
          .catch(badResponse => { });
      },
    },

    created() {
      this.downloadPerfomanceList(1);

      this.resource = this.$resource('test/' + this.testId + '/count');

      this.resource.get()
        .then(response => response.json())
        .then(count => this.countResults = count.content.countResults)
        .catch(badResponse => { });
    },

  }
</script>

