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
                  </ol>
                </nav>
              </div>
            </div>

            <section class="mt-3">
              <div class="section-heading">
                <uploadSectionHeader :subsectionName="pageName" />
              </div>
              <div class="section-body">

                <button type="button" id="advanceModal1Btn" class="btn btn-primary" data-bs-toggle="modal"
                  data-bs-target=".myclass2" hidden>
                </button>

                <div class="modal fade myclass2" id="configIntro" tabindex="-1" data-bs-backdrop="static"
                  data-bs-keyboard="false" aria-labelledby="configIntroLabel" aria-hidden="true">
                  <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                      <div class="modal-body p-0 m-0">
                        <div class="step1">
                          <div class="intro-picture">
                            <img src="../../images/intro-image-a002-advance.jpg" alt="Intro">
                          </div>
                          <div class="intro-content text-center p-5">
                            <h2>Preferred Option selection for Advanced Configurations</h2>
                            <div class="p-4">Please click "Excel" button, If you wish to continue the configurations via
                              an excel upload. To continue via a wizard, please click "Wizard" button.</div>
                            <div class="text-center">
                              <button
                                @click="this.$router.push({ name: 'upload', params: { subsection: 'Employee Data' } })"
                                class="btn" data-bs-dismiss="modal"><i class="ri-file-excel-2-fill me-1"></i>
                                Excel</button>
                              <button @click="this.$router.push({ name: 'statutory-leave-wizard' })"
                                class="btn btn-primary" style="margin-left: 5px;" data-bs-dismiss="modal"><i
                                  class="ri-bubble-chart-fill me-1"></i> Wizard</button>
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
//import PageTitle from "../components/navigation/PageTitle.vue";
import UploadSectionHeader from "../components/pageElements/UploadSectionHeader.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
import $ from 'jquery';
import '../../css/styles.css';
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      isLoading: false,
      pageName: "Advance Configuration",
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      attendanceObjects: []
    }
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

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    //"pageTitle": PageTitle,
    "uploadSectionHeader": UploadSectionHeader,
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

    // this.isLoading = true;
    $('#advanceModal1Btn').click();
    // setTimeout(() => {
    //   this.isLoading = false;
    // }, 300);
  },
}
</script>
