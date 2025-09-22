<template>
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
                <li class="breadcrumb-item"><router-link :to="{ name: 'advance-configuration-1' }"></router-link>
                  Advanced Configuration</li>
                <li class="breadcrumb-item">{{ this.subsectionName }}
                </li>
              </ol>
            </nav>
          </div>
        </div>

        <section class="mt-3">
          <!-- <div class="section-heading">
                <uploadSectionHeader :subsectionName="pageName" />
              </div> -->
          <div class="section-body">
            <div v-if="questionsLoaded">
              <absenceWizard :key="componentKey" :subsectionName="subsectionName" :totalQuestionCount="totalQuestionCount"
                :data="currentObject" @next="next" @back="back"></absenceWizard>
            </div>
            <div v-else>
              <loader />
            </div>
          </div>
        </section>
      </div>
    </div>
  </div>
</template>
  
<script>
import SideBar from "../components/navigation/SideBar.vue";
import TopBar from "../components/navigation/TopBar.vue";
import AbsenceWizard from '../components/pageElements/AbsenceWizard.vue'
//import UploadSectionHeader from "../components/pageElements/UploadSectionHeader.vue";
//import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
//import $ from 'jquery';
import '../../css/styles.css';
import { saveAbsenceDate, saveAbsenceSubsectionName, removeAbsenceRowID, saveAbsenceRowID, getUserID } from '../localStorageUtils';
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      isLoading: false,
      pageName: "Advanced Configuration",
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      objects: [
      ],
      currentIndex: 0,
      componentKey: 0,
      subsectionName: null,
      questionsLoaded: false,
      totalQuestionCount: 0,
      disableSideBar: false,
      draftRowId: null,
      isDraft: false,
      userID: '',
      attendanceObjects: []
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

    startAbsenceWizard() {
      if (this.isDraft)
        this.showDraftRemainingAlert();
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
          removeAbsenceRowID();
        } else {
          saveAbsenceRowID(this.draftRowId);
          this.$router.push({ name: 'absence-wizard-draft-edit', params: { subsection: this.subsectionName } });
        }
      })
    },

    async deleteDrafSavedOptions() {
      await this.$http
        .post(`/deleteAbsenceDataByRowId`, { tableRowId: this.draftRowId, subsectionName: this.subsectionName }, {
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

    next() {
      if (this.currentIndex < this.objects.length - 1) {
        this.currentIndex++;
        this.componentKey++;
      }
      else if (this.currentIndex >= this.objects.length - 1) {
        this.$router.push({ name: 'absence-wizard-selected-options', params: { subsection: this.subsectionName } });
      }

      this.isLoading = true;
      setTimeout(() => {
        this.isLoading = false;
      }, 300);
    },

    back() {
      if (this.currentIndex == 0) {
        this.$router.push({ name: 'wizard-popup', params: { subsection: this.subsectionName } });
      }

      else if (this.currentIndex < this.objects.length) {
        this.currentIndex--;
        this.componentKey++;
      }

      this.isLoading = true;
      setTimeout(() => {
        this.isLoading = false;
      }, 300);
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    //"loader": PageLoader,
    "absenceWizard": AbsenceWizard
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
    saveAbsenceDate(new Date());
    this.subsectionName = this.$route.params.subsection;
    this.userID = getUserID();
    saveAbsenceSubsectionName(this.subsectionName);
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
        `/getIsDraftStatusForAbsence?userId=${encodeURIComponent(this.userID)}&subsectionName=${encodeURIComponent(this.subsectionName)}`,
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

    await this.$http
      .get(
        `/getWizardQuestionDetails?subsectionName=${encodeURIComponent(this.subsectionName)}`,
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
        this.objects = resp.data;
        this.totalQuestionCount = this.objects.length;
        this.questionsLoaded = true;
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

    this.startAbsenceWizard();
  },

  computed: {
    currentObject() {
      return this.objects[this.currentIndex];
    },
  },

}
</script>
