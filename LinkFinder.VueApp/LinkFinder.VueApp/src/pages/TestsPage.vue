<template>
  <div class="py-4">

    <div class="inputForm w-75 mx-auto my-4">

      <form id="urlForm" class="d-flex justify-content-around align-middle flex-wrap">
        <!--Input url section-->
        <p class="px-3 my-2" style="font-size:1.2rem">Enter a website</p>

        <input type="text"
               id="siteUrl"
               v-model="inputSiteUrl"
               @blur="$v.inputSiteUrl.$touch()"
               class="d-flex flex-fill mx-4 my-2 border rounded form-control"
               :class="{'is-invalid' : $v.inputSiteUrl.$error}"
               style="width:unset;">

        <button type="button" @click="onSubmit" id="testButtonSubmit" class="form-control w-auto px-5 my-2 btn-outline-success">Test</button>
        <!--End input section-->
        <!--Error message section-->
        <div v-if="!$v.inputSiteUrl.required && $v.inputSiteUrl.$error" class="text-center w-100 text-danger" style="font-size:1rem">
          You must enter something
        </div>

        <div v-if="!$v.inputSiteUrl.url" class="text-center w-100 text-danger" style="font-size:1rem">
          It must be a link
        </div>

        <div v-if="this.errorMessage != ''" class="text-center w-100 text-danger" style="font-size:1rem">
          {{errorMessage}}
        </div>
        <!--End error section-->
      </form>

    </div>

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
    <!--Hiding window-->
    <hiding-window :isVisible="contentIsHidden"></hiding-window>
  </div>
</template>

<script>
  import TestsList from '../components/TestsList/testsList.vue'
  import HidingWindow from '../components/HidingWindow/hidingWindow.vue'
  import { required, url } from 'vuelidate/lib/validators'

  export default {
    name: 'Tests',

    components: {
      'tests-list': TestsList,
      'hiding-window': HidingWindow, 
    },

    data() {
      return {
        inputSiteUrl: "",
        errorMessage: "",
        tests: [],
        contentIsHidden: false,
      }
    },

    created() {
      this.downloadTests();
    },

    validations: {
      inputSiteUrl: {
        required,
        url 
      },
    },

    methods: {
      onSubmit() {
        this.$v.$touch()

        if (this.$v.$invalid === false)
        {
          this.contentIsHidden = true;

          this.postNewTest();
        }
      },

      downloadTests(){
        this.resource = this.$resource('tests');
        
        this.tests = this.resource.get()
          .then(response => response.json())
          .then(tests => this.tests = tests);
      },

      postNewTest() {
        this.resource = this.$resource('test/');

        const postedData = {
          "url": this.inputSiteUrl,
        };

        this.resource.save(postedData)
          .then(response => {
            if (response.status === 400) {
              this.errorMessage = badResponse.body.errorMessage[0];
              this.contentIsHidden = false;
              this.inputSiteUrl = "";
            }
            else
            {
              response.json();
              this.tests.unshift(response.body);
              this.contentIsHidden = false;
            }
          })

      },

    },

  }
</script>
