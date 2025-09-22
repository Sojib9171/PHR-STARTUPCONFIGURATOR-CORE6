<template>
  <div>
    <div v-if="isLoading">
      <loader />
    </div>
    <div v-else>
      <div>
        <sideBar />
        <div class="col page-content px-0">
          <topBar />
          <div class="page-body">
            <div>
              <div class="page-title">
                <!--Page title-->
                <h2>Commit Configuration Control</h2>
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
              <div class="section-heading">
                <h3 class="section-title">Ready to Commit</h3>
              </div>
              <div class="section-body">
                <div class="block-form-top bg-gray p-4">
                  <div class="block-data">
                    <div class="block-commits">
                      <div class="row">
                        <div class="col-md-12">
                          <ul class="list list-unstyled">
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Configuration Control</span>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <configurationControlCommit />
              </div>
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
import ConfigurationControlCommit from "../components/pageElements/ConfigurationControlCommit.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
import { emptyActiveModulesArray, emptyEimActiveSubsectionArray, emptyAbsenceActiveSubsectionArray, emptyAttendanceActiveSubsectionArray } from '@/localStorageUtils';
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      isLoading: false,
      subsectionName: null,
      isLeaving: false,
      isLoggingOut: false,
    }
  },

  props: {
    subsection: {
      type: String,
      required: true
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "configurationControlCommit": ConfigurationControlCommit,
    "loader": PageLoader
  },

  beforeRouteUpdate(to, from, next) {
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
      next();
    }, 300);
  },

  beforeRouteLeave(to, from, next) {
    if (to.name === 'configuration-control' && from.name === 'configuration-control-commit') {
      this.isLeaving = true;
    }

    if (this.isLeaving) {
      emptyActiveModulesArray();
      emptyEimActiveSubsectionArray();
      emptyAbsenceActiveSubsectionArray();
      emptyAttendanceActiveSubsectionArray();
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
          next({ name: 'configuration-control' })
        } else {
          this.isLeaving = false;
          next(false);
        }
      })
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

    this.subsectionName = this.$route.params.subsection;
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
  },

  methods: {
    showLogOutAlert() {
      this.$swal.fire({
        position: 'top',
        icon: 'error',
        width: 400,
        height: 100,
        text: 'Your Session Has Expired',
        timer: 5000
      });
    },
  },
}
</script>
