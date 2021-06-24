<template>
  <div class="row border-bottom py-3">
    <div class="col-10 px-5 border-right">{{url}}</div>
    <div v-if="(timeResponse != '')" class="col-2  text-center">{{timeResponse}} ms</div>
  </div>
</template>

<script>
  export default {
    name: 'crawled-link',
    props:['url','timeResponse'],
  }
</script>
