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
              <ConfigurationControlRanking></ConfigurationControlRanking>
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
import ConfigurationControlRanking from "@/components/pageElements/ConfigurationControlRankingWithDragAndDrop.vue"
import { Base64 } from "js-base64";
import { emptyActiveModulesArray, emptyEimActiveSubsectionArray, emptyAbsenceActiveSubsectionArray, emptyAttendanceActiveSubsectionArray } from '@/localStorageUtils';
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      isLoading: false,
      pageName: "Configuration Control Setup",
      isLeaving: false,
      isLoggingOut: false,
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "loader": PageLoader,
    "ConfigurationControlRanking": ConfigurationControlRanking
  },

  beforeRouteUpdate(to, from, next) {
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
      next();
    }, 1000);
  },

  beforeRouteLeave(to, from, next) {
    if (to.name === 'configuration-control-commit') {
      this.isLeaving = true;
    }

    if (this.isLeaving) {
      next();
    }
    else if (this.isLoggingOut) {
      emptyActiveModulesArray();
      emptyEimActiveSubsectionArray();
      emptyAbsenceActiveSubsectionArray();
      emptyAttendanceActiveSubsectionArray();
      next();
    }
    else {
      this.isLeaving = true;
      this.$swal({
        title: 'Are you sure?',
        text: 'You have unsaved changes. Are you sure you want to leave?',
        icon: 'warning',
        width: 300,
        height: 200,
        showCancelButton: true,
        confirmButtonText: 'Stay',
        cancelButtonText: 'Leave',
        allowOutsideClick: false,
      }).then(result => {
        if (!result.isConfirmed) {
          emptyActiveModulesArray();
          emptyEimActiveSubsectionArray();
          emptyAbsenceActiveSubsectionArray();
          emptyAttendanceActiveSubsectionArray();
          next();
        } else {
          this.isLeaving = false;
          next(false);
        }
      }).catch((error) => {
        console.log(error.message);
        next(false);
      });
    }
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

    this.isCookieNull = false;
    this.redirect = false;
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
    }, 300);

    this.emitter.on("loggingOut", (istrue) => {
      if (istrue == true) {
        this.isLoggingOut = true;
        this.emitter.emit('loggedOut', true);
        this.$http
          .post("/initiate", {}, { withCredentials: true })
          .then((resp) => {
            this.$cookies.set("cookie", Base64.btoa(resp.data));
          })
          .catch(() => {
          });
      }
    });
  }
}
</script>
