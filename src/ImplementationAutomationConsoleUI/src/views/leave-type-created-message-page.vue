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
            <pageTitle :pageName="pageName" />
            <section class="mt-3">
              <div class="section-body">
                <leaveTypeCreatedMessage :subsectionName="subsectionName"></leaveTypeCreatedMessage>
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
import PageTitle from "../components/navigation/PageTitle.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import LeaveTypeCreatedMessage from '@/components/pageElements/LeaveTypeCreatedMessage.vue'
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      pageName: "Advance Configuration",
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      isLoading: false,
      subsectionName: '',
      attendanceObjects: []
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "pageTitle": PageTitle,
    "leaveTypeCreatedMessage": LeaveTypeCreatedMessage,
    "loader": PageLoader
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

  beforeRouteUpdate(to, from, next) {
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
      next();
    }, 300);
  },

  beforeRouteLeave(to, from, next) {
    this.isLoading = true;
    if (to.name == 'absence-wizard' || to.name == 'leave-type-summary') {
      setTimeout(() => {
        this.isLoading = false;
        next();
      }, 300);
    }
    else {
      next(false);
      this.isLoading = false;
    }
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

    // this.isLoading = true;
    // setTimeout(() => {
    //   this.isLoading = false;
    // }, 300);
  },
}
</script>
