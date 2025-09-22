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
              <div class="section-body">
                <div class="config-block mb-4 active">
                  <div class="p-4 border-0">
                    <div class="row align-items-center">
                      <div class="col-sm-8">
                        <h3 class="section-title">Configuration Control</h3>
                        <div class="sub-heading">Setup initial configuration control related to
                          overall product</div>
                      </div>
                      <div class="col-sm-4 text-end">
                        <div class="position-relative" v-if="!isApproved">
                          <button @click="startConfigControl" class="btn btn-primary btn-rounded btn-lg">Get
                            Started</button>
                        </div>
                        <div class="row" v-else>
                          <div class="col-xl-12">
                            <div v-if="approvalStatus == 'Approved'">
                              <button @click="showConfigControlHistory" class="btn btn-success btn-rounded btn-m"
                                style="margin-bottom: 5px;">Approved</button>
                            </div>
                            <div v-else-if="approvalStatus == 'Rejected'">
                              <button @click="showConfigControlHistory" class="btn btn-danger btn-rounded btn-m"
                                style="margin-bottom: 5px;">Rejected</button>
                            </div>

                            <div>
                              <button @click="startConfigControl" class="btn btn-primary btn-rounded btn-m"
                                v-if="!isAllSubsecEnabled">Configure Again</button>

                                <button @click="startConfigControl" class="btn btn-success btn-rounded btn-m"
                                v-else disabled>Completed</button>
                            </div>
                          </div>
                        </div>
                        <div v-if="showHistory">
                          <configControlHistoryTable :key="componentKey">
                          </configControlHistoryTable>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
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
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';
import ConfigControlHistoryTable from '../components/pageElements/ConfigControlHistoryTable.vue'

export default {
  data() {
    return {
      isLoading: false,
      pageName: "Configuration Control Setup",
      showHistory: false,
      componentKey: 0,
      isApproved: false,
      approvalStatus: '',
      isAllSubsecEnabled: true
    }
  },

  methods: {
    startConfigControl() {
      this.$router.push({ name: 'configuration-control-selection' });
    },

    showConfigControlHistory() {
      this.showHistory = true;
      this.componentKey += 1;
    },
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "loader": PageLoader,
    "configControlHistoryTable": ConfigControlHistoryTable
  },

  // beforeRouteEnter() {
  //   if (this.$cookies.get("userTypeCode") != '3')
  //     this.$router.go({
  //       name: 'home',
  //     });
  // },

  beforeRouteUpdate(to, from, next) {
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
      next();
    }, 1000);
  },

  async mounted() {
    this.isCookieNull = false;
    this.redirect = false;

    this.isLoading = true;
    await this.$http
      .get(
        `/getConfigControlApprovalStatus`,
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
        this.isApproved = resp.data.isApproved;
        this.approvalStatus = resp.data.approvalStatus;
        this.isLoading = false;
      })
      .catch((error) => {
        this.isLoading = false;
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

    await this.$http
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
        this.isAllSubsecEnabled = resp.data;
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
  }
}
</script>
