<template>
  <div>
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
                <absenceWizardEdit :key="componentKey" :subsectionName="subsectionName"
                  :totalQuestionCount="totalQuestionCount" :data="currentObject" @next="next" @back="back">
                </absenceWizardEdit>
              </div>
            </div>
          </section>
        </div>
      </div>
    </div>
  </div>
</template>
  
<script>
import SideBar from "../components/navigation/SideBar.vue";
import TopBar from "../components/navigation/TopBar.vue";
import AbsenceWizardEdit from '../components/pageElements/AbsenceWizardEdit.vue'
//import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
import '../../css/styles.css';
import { getAbsenceRowID, getAbsenceSubsectionName } from '@/localStorageUtils';
import { extendCookieTimeout } from '@/cookieUtils'

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
      rowId: 0,
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
        this.$router.push({ name: 'leave-type-summary', params: { subsection: this.subsectionName } });
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
    //"uploadSectionHeader": UploadSectionHeader,
    //"loader": PageLoader,
    "absenceWizardEdit": AbsenceWizardEdit
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
    this.subsectionName = this.$route.params.subsection;
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

    this.rowId = getAbsenceRowID();
    var subsecName = getAbsenceSubsectionName();
    this.$http.get(`/getAbsenceWizardSelectedOptionsSummary?rowId=${encodeURIComponent(this.rowId)}&subsectionName=${encodeURIComponent(subsecName)}`, {
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
        this.objects = response.data;
        extendCookieTimeout();
        this.totalQuestionCount = this.objects.length;
        this.questionsLoaded = true;
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

  computed: {
    currentObject() {
      return this.objects[this.currentIndex];
    },
  },
}
</script>
