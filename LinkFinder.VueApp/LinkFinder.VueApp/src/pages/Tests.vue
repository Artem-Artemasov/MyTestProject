<template>
  <div class="py-4">

    <div class="inputForm w-75 mx-auto my-4">

      <form id="urlForm" class="d-flex justify-content-around align-middle flex-wrap">
        <p class="px-3 my-2" style="font-size:1.2rem">Enter a website</p>

        <input type="text"
               id="siteUrl"
               v-model="inputSiteUrl"
               @blur="$v.inputSiteUrl.$touch()"
               class="d-flex flex-fill mx-4 my-2 border rounded form-control"
               :class="{'is-invalid' : $v.inputSiteUrl.$error}"
               style="width:unset;">
        <button type="button" @click="onSubmit" id="testButtonSubmit" class="form-control w-auto px-5 my-2 btn-outline-success">Test</button>

        <div v-if="!$v.inputSiteUrl.required && $v.inputSiteUrl.$error" class="text-center w-100 text-danger" style="font-size:1rem">
          You must enter something
        </div>

        <div v-if="!$v.inputSiteUrl.url" class="text-center w-100 text-danger" style="font-size:1rem">
          It must be a link
        </div>

        <div v-if="this.errorMessage != ''" class="text-center w-100 text-danger" style="font-size:1rem">
          {{errorMessage}}
        </div>

      </form>

    </div>

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
  import TestsList from '../components/TestsList/TestsList.vue'
  import { required, url } from 'vuelidate/lib/validators'

  export default {
    name: 'Tests',

    components: {
      'tests-list': TestsList,
    },

    data() {
      return {
        inputSiteUrl: "",
        errorMessage:"",
      }
    },

    created() {
      this.resource = this.$resource('test/');
    },

    validations: {
      inputSiteUrl: {
        required,

      },
    },

    methods: {
      onSubmit() {
        this.$v.$touch()
        if (!this.$v.$invalid)
        {
          const siteUrl = {
            "url": this.inputSiteUrl,
          };

          this.resource.save(siteUrl)
            .then(response => { }, badResponse => { alert(badResponse); })
        }
      }

    },

  }
</script>
