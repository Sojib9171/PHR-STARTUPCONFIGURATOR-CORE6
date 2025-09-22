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
                      Common Configuration</li>
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
                        <h3 class="section-title">Basic Configuration</h3>
                        <div class="sub-heading">Setup initial common configurations related to
                          overall product</div>
                      </div>
                      <div class="col-sm-4 text-end">
                        <div class="position-relative">
                          <button @click="this.$router.push({ name: 'common-config' })"
                            class="btn btn-primary btn-rounded btn-lg">Get Started</button>
                          <div class="callout-box active">
                            <i class="ri-checkbox-blank-circle-fill icon-blinking"></i>
                            <span>
                              Click on Get Started to initiate the General Configurations,
                              where you can set up basic settings for PeoplesHR
                            </span>

                          </div>
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

                <button type="button" id="modal1Btn" class="btn btn-primary" data-bs-toggle="modal"
                  data-bs-target=".myclass1" hidden>
                </button>

                <div class="modal fade myclass1" id="configIntro" tabindex="-1" data-bs-backdrop="static"
                  data-bs-keyboard="false" aria-labelledby="configIntroLabel" aria-hidden="true">
                  <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                      <div class="modal-body p-0 m-0">
                        <div class="step1">
                          <div class="intro-picture">
                            <img src="../../images/intro-image-a001.jpg" alt="Intro">
                          </div>
                          <div class="intro-content text-center p-5">
                            <h2>Welcome to PeoplesHR Config Assist!</h2>
                            <div class="p-4">We are here to help you with the initial setting-up of PeoplesHR for you to
                              comfortably
                              onboard.</div>
                            <div class="text-center">
                              <button class="btn btn-primary btn-xl" data-bs-dismiss="modal"
                                @click="showSecondModal()">Let's Begin</button>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <button type="button" id="modal2Btn" class="btn btn-primary" data-bs-toggle="modal"
                  data-bs-target=".myclass" hidden>
                </button>

                <div class="modal fade myclass" id="configIntro2" tabindex="-1" data-bs-backdrop="static"
                  data-bs-keyboard="false" aria-labelledby="configIntroLabel" aria-hidden="true">
                  <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                      <div class="modal-body p-0 m-0">
                        <div class="step2">
                          <div class="intro-content p-5">
                            <h2 class="text-center">Let's get started with the following configurations</h2>
                            <div class="p-4">
                              <ul class="list-unstyled step-list">
                                <li class="mt-4 item1">
                                  <i class="ri-checkbox-circle-fill check-fill"></i>
                                  <h4>Set-up Organization</h4>
                                  <ul>
                                    <li>Configure your organization settings
                                    </li>
                                  </ul>
                                </li>
                                <li class="mt-4 item1">
                                  <i class="ri-checkbox-circle-fill check-fill"></i>
                                  <h4>Configure Employee Information</h4>
                                  <ul>
                                    <li>Upload employee masters
                                    </li>
                                  </ul>
                                </li>
                                <li class="mt-4 item1">
                                  <i class="ri-checkbox-circle-fill check-fill"></i>
                                  <h4>Configure Absence Data</h4>
                                  <ul>
                                    <li>Create new leave types that comply with organizational policies
                                    </li>
                                  </ul>
                                </li>
                                <!-- <li class="mt-4 item2">
                                  <i class="ri-checkbox-circle-fill check-fill"></i>
                                  <h4>Post Configurations</h4>
                                  <ul>
                                    <li>Assist you in starting basic functions smoothly on initial onboarding
                                    </li>
                                    <li>Accommodates in maintaining a content library</li>
                                    <li>Links with PeoplesHR Learning Academy to enhance product knowledge</li>
                                  </ul>
                                </li> -->
                              </ul>
                            </div>

                            <div class="text-center"><button data-bs-dismiss="modal"
                                class="btn btn-primary btn-xl">Continue</button>
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
import $ from 'jquery';
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {

  data() {
    return {
      componentKey: 0,
      isLoading: false,
      showModal: false,
      pageName: "Basic Configuration Setup",
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      attendanceObjects: []
    }
  },

  methods: {
    showSecondModal() {
      $('#modal2Btn').click();
    },

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

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "loader": PageLoader
  },

  // beforeRouteUpdate(to, from, next) {
  //   this.isLoading = true;
  //   setTimeout(() => {
  //     this.isLoading = false;
  //     next();
  //   }, 300);
  // },

  beforeRouteLeave(to, from, next) {
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
      next();
    }, 300);
  },

  async mounted() {
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

    this.isLoading = true;
    $('#modal1Btn').click();
    setTimeout(() => {
      this.isLoading = false;
    }, 300);
  },
}
</script>
