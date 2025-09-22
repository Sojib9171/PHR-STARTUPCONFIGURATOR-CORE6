<template>
  <div>
    <div v-if="isLoading">
      <loader />
    </div>
    <div v-else>
      <div>
        <sideBar />
        <div class="col page-content px-0">
          <topBar :isDisabled=false />
          <div class="page-body">
            <div>
              <div class="page-title">
                <!--Page title-->
                <h2>{{ pageName }}</h2>

                <!--Breadcrumb-->
                <nav style="--bs-breadcrumb-divider: '|';" aria-label="breadcrumb">
                  <ol class="breadcrumb">
                    <li class="breadcrumb-item"><router-link :to="{ name: 'configuration-control' }"><i
                          class="ri-home-4-line"></i></router-link>
                    </li>
                    <li class="breadcrumb-item"><router-link :to="{ name: 'configuration-control' }"></router-link>
                      Configuration Control</li>
                  </ol>
                </nav>
              </div>
            </div>
            <section class="mt-3">
              <ConfigurationControlSelection></ConfigurationControlSelection>
            </section>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
  
<script>
import SideBar from "../components/navigation/SideBarForUserAdmin.vue";
import TopBar from "../components/navigation/TopBar.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import ConfigurationControlSelection from '@/components/pageElements/ConfigurationControlSelection.vue';
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      isLoading: false,
      pageName: "Configuration Control Setup",
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "loader": PageLoader,
    "ConfigurationControlSelection": ConfigurationControlSelection
  },

  beforeRouteUpdate(to, from, next) {
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
      next();
    }, 1000);
  },

  beforeRouteLeave(to, from, next) {
    this.isLoading = true;
    setTimeout(() => {
      next();
    }, 1000);
  },

  async mounted() {
    this.$http
      .get(
        "/checkIfAllSubsectionIsEnabled",
        {
          headers: {
            accept: "*/*",
            Authorization:
              "Basic " +
              Base64.toBase64(
                this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
              ),
          }
        }
      )
      .then((resp) => {
        if (resp.data) {
          this.$router.push({ name: 'access-denied' });
        }
        extendCookieTimeout();
      })
      .catch((error) => {
        console.log(error.message);
        if (error.response.status == 401) {
          this.emitter.emit('loggedOut', true);
        }
        else if (error.response.status == 403) {
          this.emitter.emit('accessDenied', true);
        }
      });

    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
    }, 300);
  }
}
</script>
