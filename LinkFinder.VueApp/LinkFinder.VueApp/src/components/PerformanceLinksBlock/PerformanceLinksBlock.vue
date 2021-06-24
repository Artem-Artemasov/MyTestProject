<template>
  <div>
    perfomance
  
  </div>
</template>

<script>
  import Link from '../Link/Link.vue';

  export default {
    name: 'performanceLinksBlock',

    props: {
      id: {
        required:true
      },
    },

    components:{
      'crawled-link' : Link
    },

    data() {
      return {
        testDetail:[]
      }
    },

    methods: {
      requestData: function () {
        console.log("request");

        this.resource = this.$resource('test/${id}');

        this.resource.get()
          .then(response => response.json())
          .then(links => this.links = links);
      },
    },

    mounted() {
        requestData();
    },

  }
</script>
