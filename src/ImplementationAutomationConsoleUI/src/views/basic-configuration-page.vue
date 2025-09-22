<template>
  <div>
    <div v-if="isLoading">
      <loader />
    </div>
    <div v-else>
      <div>
        <sideBar :isDisabled="false" :allObjects="allObjects" :emiObjects="eimObjects" :absenceObjects="absenceObjects"
          :attendanceObjects="attendanceObjects" />
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
                    <li class="breadcrumb-item"><router-link :to="{ name: 'home' }"><i
                          class="ri-home-4-line"></i></router-link>
                    </li>
                    <li class="breadcrumb-item"><router-link :to="{ name: 'basic-configuration-1' }"></router-link>
                      Basic Configuration</li>
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
                        <h3 class="section-title">Common Configuration</h3>
                        <div class="sub-heading">Setup initial common configurations related to
                          overall product</div>
                      </div>
                      <div class="col-sm-4 text-end">
                        <div class="position-relative" v-if="!isApproved">
                          <button @click="startBasicConfig" class="btn btn-primary btn-rounded btn-lg">Get
                            Started</button>
                          <!-- <div class="callout-box active">
                            <i class="ri-checkbox-blank-circle-fill icon-blinking"></i>
                            <span>
                              Click on Get Started to initiate the General Configurations,
                              where you can set up basic settings for PeoplesHR
                            </span>

                          </div> -->
                        </div>

                        <div class="row" v-else>
                          <div class="col-md-12">
                            <div class="button-group d-flex justify-content-end">
                              <button class="btn btn-success btn-rounded" disabled>Completed</button>
                              <button @click="showCommonConfigHistory" class="btn btn-success btn-rounded"
                                style="margin-left: 5px;">Approved</button>
                            </div>
                          </div>
                        </div>
                        <div v-if="showHistory">
                          <commonConfigHistoryTable :userId="userId" @id-selected="handleIdSelected" :key="componentKey">
                          </commonConfigHistoryTable>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="config-block mb-4 disabled" hidden>
                  <div class="p-4 border-0">
                    <div class="row align-items-center">
                      <div class="col-sm-8">
                        <h3 class="section-title">Module Configuration</h3>
                        <div class="sub-heading">Setup module level initial configurations</div>
                      </div>
                      <div class="col-sm-4 text-end">
                        <button class="btn btn-primary btn-rounded btn-lg" @click="showSecondModal()">Get
                          Started</button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <button type="button" id="moda13Btn" class="btn btn-outline-primary" data-bs-toggle="modal"
                data-bs-target=".myclass3" hidden>
                <span class="ms-1">Summary</span>
              </button>

              <div class="modal fade myclass3" id="configIntro3" tabindex="-1" data-bs-keyboard="false"
                data-bs-backdrop="static" aria-labelledby="configIntroLabel" aria-hidden="true">
                <div class="modal-dialog modal-xl modal-dialog-centered">
                  <div class="modal-content">
                    <div class="modal-header">
                      <button @click="showHistoryAgain" type="button" class="btn-close" data-bs-dismiss="modal"
                        aria-label="Close"></button>
                    </div>
                    <div class="modal-body p-0 m-0">
                      <div class="block-form-top bg-gray p-4">
                        <div class="row">
                          <div class="col-sm-12">
                            <div class="quis-block">
                              <ol class="quis-summary ps-2">
                                <li v-for="option in selectedOptions" :key="option.question_no">
                                  <span v-if="option.question_no == 2"><span class="quis-q">
                                      {{ option.question_statement
                                      }}:</span> <span class="quis-a">{{
  option.response
}}</span></span>

                                  <span v-else-if="option.question_no == 3"><span class="quis-q">
                                      {{ option.question_statement
                                      }}:</span>
                                    <span v-if="option.response == 1" class="quis-a">Yes</span>
                                    <span v-else-if="option.response == 0" class="quis-a">No</span>
                                    <span v-else class="quis-a"></span>
                                  </span>

                                  <span v-else-if="option.question_no == 4"><span class="quis-q">
                                      {{ option.question_statement
                                      }}:</span> <span class="quis-a">{{
  option.response
}}</span></span>

                                  <span v-else-if="option.question_no == 5"><span class="quis-q">
                                      {{ option.question_statement
                                      }}:</span> <span class="quis-a">
                                      <span v-if="option.response == 1" class="quis-a">Yes</span>
                                      <span v-else-if="option.response == 0" class="quis-a">No</span>
                                      <span v-else class="quis-a"></span></span></span>

                                  <span v-else><span class="quis-q">
                                      {{ option.question_statement
                                      }}:</span> <span class="quis-a">{{
  option.response
}}</span></span>
                                </li>
                              </ol>
                            </div>
                          </div>
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
import SideBar from "../components/navigation/SideBar.vue";
import TopBar from "../components/navigation/TopBar.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import CommonConfigHistoryTable from '../components/pageElements/CommonConfigHistoryTable.vue'
import $ from 'jquery';
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';
import { getCommonConfigRowID, getUserID, removeCommonConfigRowID, saveCommonConfigRowID } from '@/localStorageUtils';

export default {

  data() {
    return {
      componentKey: 0,
      historySubsection: 'Bank Details',
      isLoading: true,
      showModal: false,
      pageName: "Basic Configuration Setup",
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      userId: '',
      isDraft: false,
      rowId: 0,
      message: '',
      draftRowId: null,
      isApproved: false,
      showHistory: false,
      redirect: false,
      selectedOptions: [],
      attendanceObjects: [],
    }
  },

  methods: {
    showErrorAlert() {
      this.$swal.fire({
        icon: 'error',
        width: 300,
        height: 200,
        text: 'There was a problem. Please try again',
        confirmButtonText: 'Ok',
        allowOutsideClick: false,
      }).then((result) => {
        if (result.isConfirmed) {
          this.isLoading = false;
        }
      })
    },

    showHistoryAgain() {
      this.showHistory = true;
    },

    handleIdSelected(id) {
      this.showHistory = false;
      this.$http.get(`/getCommonConfigSummary?rowId=${encodeURIComponent(id)}`, {
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
        .then(response => {
          this.selectedOptions = response.data;
          $('#moda13Btn').click();
          extendCookieTimeout();
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

    redirectPage() {
      this.redirect = true;
    },

    showCommonConfigHistory() {
      this.showHistory = true;
      this.componentKey += 1;
    },

    startBasicConfig() {
      if (!this.isDraft) {
        this.$router.push({ name: 'common-config' })
      }
      else {
        this.showDraftRemainingAlert();
      }
    },

    showDraftRemainingAlert() {
      this.$swal({
        title: 'You have a draft saved',
        text: 'Do you want to continue the draft?',
        icon: 'info',
        width: 500,
        height: 200,
        showDenyButton: true,
        confirmButtonText: 'Continue',
        denyButtonText: 'Discard',
        allowOutsideClick: false,
      }).then(result => {
        if (!result.isConfirmed) {
          this.deleteDrafSavedOptions();
          removeCommonConfigRowID();
        } else {
          saveCommonConfigRowID(this.draftRowId);
          this.$router.push({ name: 'common-config-draft' });
        }
      })
    },

    async deleteDrafSavedOptions() {
      await this.$http
        .post(`/deleteDataByRowId`, { tableRowId: this.draftRowId }, {
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
        .then(() => {
          extendCookieTimeout();
          this.$router.push({ name: 'common-config' })
        })
        .catch((error) => {
          console.log(error.message);
          this.showErrorAlert();
          if (error.response.status == 401) {
            this.emitter.emit('loggedOut', true);
          }
          else if (error.response.status == 403) {
            this.emitter.emit('accessDenied', true);
          }
        });
    },
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "loader": PageLoader,
    "commonConfigHistoryTable": CommonConfigHistoryTable
  },

  beforeRouteUpdate(to, from, next) {
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
      next();
    }, 1000);
  },

  beforeRouteLeave(to, from, next) {
    this.showHistory = false;
    this.isLoading = true;

    setTimeout(() => {
      this.isLoading = false;
      next();
    }, 1000);
  },

  async mounted() {
    this.isCookieNull = false;
    this.redirect = false;
    this.showHistory = false;
    this.userId = getUserID();
    this.rowId = getCommonConfigRowID();

    await this.$http
      .get(
        "/GetSidebarComponents",
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
        this.allObjects = resp.data;
        this.eimObjects = this.allObjects.filter(item => item.module_Id === 2);
        this.absenceObjects = this.allObjects.filter(item => item.module_Id === 14);
        this.attendanceObjects = this.allObjects.filter(item => item.module_Id === 65);
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

    await this.$http
      .get(
        `/getIsDraftStatus?userId=${encodeURIComponent(this.userId)}`,
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
        this.isDraft = resp.data.isDraft;
        this.draftRowId = resp.data.tableRowId;
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
    this.isApproved = false;
    await this.$http
      .get(
        `/getCommonConfigApprovalStatus?userId=${encodeURIComponent(this.userId)}`,
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
        this.isApproved = resp.data;
        this.isLoading = false;
      })
      .catch((error) => {
        console.log(error.message);
        if (error.response.status == 401) {
          this.emitter.emit('loggedOut', true);
        }
        else if (error.response.status == 403) {
          this.emitter.emit('accessDenied', true);
        }
        else if(error.response.status == 400) {
          this.$router.push({ name: 'home' })
        }
        this.isLoading = false;
      });

    // this.isLoading = true;
    // setTimeout(() => {
    //   this.isLoading = false;
    // }, 300);
  }
}
</script>
