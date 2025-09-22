<template>
  <div class="col-md-6 align-self-center">
    <div class="login-block ms-auto">
      <div class="title">Sign in</div>
      <div class="alert alert-danger d-flex align-items-center" v-if="isFailedLogin">
        <i class="ri-error-warning-line mr-2"> {{ responseMessage }}</i>
      </div>

      <div class="form-floating input-field mb-3">
        <input type="text" id="floatingUID" class="form-control" placeholder="User ID" v-model="UserId"
          @keyup.enter="login" />
        <label for="floatingUID">User ID</label>
        <span class="text-danger" v-if="v$.UserId.$error">
          {{ requiredUserId }}
        </span>
      </div>
      <div class="form-floating input-field">
        <input type="password" id="floatingPW" class="form-control" placeholder="Your password" v-model="Password"
          @keyup.enter="login" />
        <label for="floatingPW">Your password</label>
        <span class="text-danger mt-2" v-if="v$.Password.$error">
          {{ requiredPassword }}
        </span>
      </div>

      <div class="row input-field">
        <div class="col-md-12">
          <button @click="login" class="btn btn-primary btn-xlarge">
            Sign in Now
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { Base64 } from "js-base64";
import { JSEncrypt } from "jsencrypt";
import useValidate from '@vuelidate/core';
import { required } from '@vuelidate/validators';
import '../../../css/styles.css'
import { saveUserID } from '@/localStorageUtils'

export default {
  data: function () {
    return {
      v$: useValidate(),
      UserId: "",
      Password: "",
      encrypt: new JSEncrypt(),
      isFailedLogin: false,
      responseMessage: ''
    };
  },

  validations() {
    return {
      UserId: {
        required
      },
      Password: {
        required
      }
    }
  },

  computed: {
    requiredUserId() {
      return this.v$.UserId.$error ? 'Please provide a User Id' : '';
    },
    requiredPassword() {
      return this.v$.Password.$error ? 'Please provide a Password' : '';
    },
  },

  async mounted() {
    await this.getPublicKey();
  },

  methods: {
    async getPublicKey() {
      await this.$http
        .post("/initiate", {}, { withCredentials: true })
        .then((resp) => {
          this.$cookies.set("cookie", Base64.btoa(resp.data));
        })
        .catch((error) => {
          console.log(error.message);
        });
    },

    async login() {
      this.v$.$validate();
      if (!this.v$.$error) {
        if (this.$cookies.isKey("cookie")) {
          await this.getPublicKey();
        }

        this.encrypt.setPublicKey(Base64.atob(this.$cookies.get("cookie")));

        var encVal = this.encrypt.encrypt(this.Password);
        var enc = this.encrypt.encrypt(this.UserId);

        this.$cookies.set("enc", enc);
        this.$cookies.set("encVal", encVal);

        let LoginDto = {
          UserID: enc,
          Password: encVal,
        };

        await this.$http
          .post("/login", LoginDto, {
            withCredentials: true,
          })
          .then((response) => {
            saveUserID(response.data.userId);
            this.$cookies.set("userName", response.data.username);
            this.$cookies.set("userTypeCode", response.data.userTypeCode);
            this.$cookies.set("name", response.data.name);

            if (this.$cookies.get("userTypeCode") == '1') {
              if (response.data.hasUserLoggedInBefore == true) {
                this.$router.push({
                  name: 'home',
                });
              }
              else {
                this.UpdateBeforeLoggedInStatus(response.data.userId);
                this.$router.push({
                  name: 'basic-config-popup',
                });
              }
            }

            if (this.$cookies.get("userTypeCode") == '2')
              this.$router.push({
                name: 'admin-approval-summary',
              });

            if (this.$cookies.get("userTypeCode") == '3')
              this.$router.push({
                name: 'configuration-control',
              });
          })
          .catch((error) => {
            console.log(error);
            this.isFailedLogin = true;
            this.responseMessage = error.response.data;
          });
      } else {
        return;
      }
    },

    async UpdateBeforeLoggedInStatus(userId) {
      this.$http.get(`/updateUserLoggedInBeforeStatus?userId=${encodeURIComponent(userId)}`, {
        headers: {
          accept: "*/*",
          Authorization:
            "Basic " +
            Base64.toBase64(
              this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
            ),
        },
        withCredentials: true,
      })
        .catch(error => {
          if (error.response.status == 401) {
            this.emitter.emit('loggedOut', true);
          }
          else if (error.response.status == 403) {
            this.emitter.emit('accessDenied', true);
          }
        });
    },

    clearForm() {
      this.$http;
      this.UserId = "";
      this.Password = "";
    },

    async testAPI() {
      await this.$http
        .get("/TestAuth")
        .then(() => {

        })
        .catch((error) => {
          console.log(error.message);
        });
    },
  },
};
</script>
