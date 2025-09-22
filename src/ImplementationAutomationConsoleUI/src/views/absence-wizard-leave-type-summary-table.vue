<template>
  <div v-if="isLoading">
    <loader />
  </div>
  <div v-else>
    <sideBar :isDisabled="false" :allObjects="allObjects" :emiObjects="eimObjects" :absenceObjects="absenceObjects"
      :attendanceObjects="attendanceObjects" />
    <div class="col page-content px-0">
      <topBar />
      <div class="page-body">
        <div>
          <div class="page-title">
            <!--Page title-->
            <h2>{{ page_name }}</h2>

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
        <section class="mt-3" v-if="isDataLoaded">
          <div class="section-body">
            <div class="config-block">
              <absenceWizardLeaveTypeSummary :subsectionName="subsectionName" :key="componentKey"
                @reloadComp="reloadComp" />
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
import AbsenceWizardLeaveTypeSummary from "../components/pageElements/AbsenceWizardLeaveTypeSummary.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
import '../../css/styles.css';
import { extendCookieTimeout } from '@/cookieUtils'

export default {
  data: function () {
    return {
      page_name: "Leave Type Summary",
      isLoading: false,
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      subsectionName: '',
      componentKey: 0,
      isDataLoaded: false,
      attendanceObjects: []
    };
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "absenceWizardLeaveTypeSummary": AbsenceWizardLeaveTypeSummary,
    "loader": PageLoader
  },

  methods: {
    reloadComp() {
      this.componentKey++;
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
        this.isDataLoaded = true;
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
};
</script>
  