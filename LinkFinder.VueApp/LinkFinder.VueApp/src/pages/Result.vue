<template>
  <div>
    <perfomance-block :id="id"> </perfomance-block>
    resultId {{id}}
  </div>
</template>

<script>
  import PerfomanceBlock from '../components/PerformanceLinksBlock/PerformanceLinksBlock.vue'

  export default {
    name: 'Result',

    data() {
      return {
        id : this.$router.currentRoute.params['id'],
      }
    },

    components: {
      'perfomance-block': PerfomanceBlock,
    },
  }
</script>
