<template>
  <div class="inputForm w-75 mx-auto my-4">
    <form id="urlForm" class="d-flex justify-content-around align-middle flex-wrap" method="post" asp-controller="Test" asp-action="PostUrl">
      <p class="px-3 my-2" style="font-size:1.2rem">Enter a website</p>
      <input type="text" v-model="msg" required class="flex-fill mx-4 my-2 border rounded">
      <button type="button" @click="onSubmit" id="testButtonSubmit" class="form-control w-auto px-5 my-2 btn-outline-success">Test</button>
    </form>
  </div>
</template>

<script>
  export default {
    name: 'WebsiteUrlForm',
    data() {
      return {
        msg: "Tests is working"
      }
    },

    methods: {
      onSubmit: function () {
        this.msg = "work";
      }

    }
  }
</script>
