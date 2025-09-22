<!-- AccessDenied.vue -->

<template>
  <div class="access-denied-container">
    <div class="access-denied-card">
      <h1>Access Denied</h1>
      <p>You do not have permission to access this page.</p>
      <button @click="LogOut" class="home-button">Go to Login</button>
    </div>
  </div>
</template>

<script>
import { removeAbsencePendingRowIdToArray, emptyLeaveWizardOptionsInLocalStorage, emptyLeaveTypesInLocalStorage, removeCommonConfigLogoImageName, removeCommonConfigMobileLogoImageName, removeCommonConfigRowID, removeAbsenceRowID, removeAbsenceSubsectionName, emptyActiveModulesArray, emptyEimActiveSubsectionArray, emptyAbsenceActiveSubsectionArray, emptyAttendanceActiveSubsectionArray } from '@/localStorageUtils';
import { Base64 } from "js-base64";

export default ({
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
  }
})
</script>

<style scoped>
.access-denied-container {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100vh;
}

.access-denied-card {
  background-color: #fff;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  padding: 40px;
  border-radius: 8px;
  text-align: center;
  max-width: 400px;
  width: 100%;
  transition: transform 0.3s;
}

.access-denied-card:hover {
  transform: scale(1.02);
}

h1 {
  color: #ff6347;
  /* Tomato color for the header */
  margin-bottom: 20px;
}

p {
  color: #555;
  /* Dark gray color for the paragraph */
  margin-bottom: 30px;
}

.home-button {
  background-color: #4caf50;
  /* Green color for the button */
  color: #fff;
  /* White text color */
  padding: 12px 24px;
  font-size: 16px;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  transition: background-color 0.3s;
}

.home-button:hover {
  background-color: #45a049;
  /* Darker green color on hover */
}</style>