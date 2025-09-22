<template>
  <button class="btn btn-logout" @click="LogOut">
    <i class="ri-logout-circle-r-line"></i>
  </button>

  <button class="btn btn-logout" id="logOutSession" @click="this.$router.push({ name: 'login' });" hidden>
    <i class="ri-logout-circle-r-line"></i>
  </button>
</template>

<script>
import { removeAbsencePendingRowIdToArray, emptyLeaveWizardOptionsInLocalStorage, emptyLeaveTypesInLocalStorage, removeCommonConfigLogoImageName, removeCommonConfigMobileLogoImageName, removeCommonConfigRowID, removeAbsenceRowID, removeAbsenceSubsectionName, emptyActiveModulesArray, emptyEimActiveSubsectionArray, emptyAbsenceActiveSubsectionArray, emptyAttendanceActiveSubsectionArray } from '@/localStorageUtils';
import $ from 'jquery';
import { Base64 } from "js-base64";
export default {
  async mounted() {
    await this.emitter.on("loggedOut", (istrue) => {
      if (istrue == true) {
        $('#logOutSession').click();
      }
    });
  },

  beforeUnmount() {
    this.emitter.all.clear()
  },

  methods: {
    async LogOut() {
      await this.$http
        .post(
          "/LogOut",
          {},
          {
            headers: {
              accept: "*/*",
              Authorization:
                "Basic " +
                Base64.toBase64(
                  this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                ),
            },
            withCredentials: true,
          }
        )
        .then(() => {
          if (this.$cookies.get('cookie') == null || this.$cookies.get('enc') == null || this.$cookies.get('encVal') == null) {
            this.$router.push({
              path: "/",
            });
          }
          else {
            this.$cookies.remove("enc");
            this.$cookies.remove("encVal");
            this.$cookies.remove("cookie");
            this.$cookies.remove("publicKey");
            emptyLeaveWizardOptionsInLocalStorage();
            emptyLeaveTypesInLocalStorage();
            removeCommonConfigLogoImageName();
            removeCommonConfigMobileLogoImageName();
            removeCommonConfigRowID();
            removeAbsenceRowID();
            removeAbsenceSubsectionName();
            removeAbsencePendingRowIdToArray();
            emptyActiveModulesArray();
            emptyEimActiveSubsectionArray();
            emptyAbsenceActiveSubsectionArray();
            emptyAttendanceActiveSubsectionArray();
            this.$router.push({
              name: "login",
            });
          }
        })
        .catch((error) => {
          console.log(error.message);
        });
    },
  },
};
</script>
