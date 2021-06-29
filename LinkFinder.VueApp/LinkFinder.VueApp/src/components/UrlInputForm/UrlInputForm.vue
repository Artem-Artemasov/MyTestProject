<template>

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

      <div v-if="this.errorMessage != '' " class="text-center w-100 text-danger" style="font-size:1rem">
        {{errorMessage}}
      </div>
      <!--End error section-->
    </form>

   <!--Window that hide all content when site is crawled-->
    <hiding-window :isVisible="contentIsHidden"></hiding-window>
  </div>

</template>

<script>
  import { eventEmitter } from '../../main'
  import { required, url } from 'vuelidate/lib/validators'
  import HidingWindow from '../../components/HidingWindow/hidingWindow.vue'

  export default {
    name : 'urlInputForm',
    components: {
      'hiding-window': HidingWindow,
    },

    data() {
      return {
        inputSiteUrl: "",
        errorMessage: "",
        contentIsHidden: false,
      }
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

        if (this.$v.$invalid === false) {
          this.contentIsHidden = true;

          this.postNewTest();

          this.inputSiteUrl = "";

          this.$v.$reset();
        }
      },

      postNewTest() {
        this.resource = this.$resource('test/');

        const postedData = {
          "url": this.inputSiteUrl,
        };

        this.resource.save(postedData)
          .then(response => {
            if (response.status === 400) {
              this.errorMessage = badResponse.body.content.errorMessage;
              this.contentIsHidden = false;
            }
            else {
              response.json();
              eventEmitter.$emit('testIsAdded', response.body.content);
              this.contentIsHidden = false;
            }
          })

      },

    }

  }
</script>
