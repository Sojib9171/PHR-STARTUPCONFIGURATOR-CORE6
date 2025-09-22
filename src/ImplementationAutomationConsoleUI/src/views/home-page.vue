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
                    <li class="breadcrumb-item"><router-link :to="{ name: 'home' }"></router-link>
                      Configuration Summary</li>
                  </ol>
                </nav>
              </div>
            </div>

            <section class="mt-3">
              <div class="section-body">
                <homeComponent :eimPercentage="eimApprovalPercentage" :absencePercentage="absenceApprovalPercentage" :attendancePercentage="attendanceApprovalPercentage"
                  :eimComplete="eimComplete"
                  :absenceComplete="absenceComplete" :attendanceComplete="attendanceComplete"></homeComponent>
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
import HomeComponent from '@/components/pageElements/HomeComponent.vue'
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

//import logoutComp from "@/components/auth/logout-comp.vue";


export default {
  data() {
    return {
      isLoading: false,
      pageName: "Home",
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      eimApprovalPercentage: 0,
      absenceApprovalPercentage: 0,
      attendanceApprovalPercentage:0,
      eimComplete: false,
      absenceComplete: false,
      attendanceComplete: false,
      commonConfigPercentage: 0,
      commonConfigComplete: false,
      subsecApprovalComplete:false,
      attendanceObjects: []
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "homeComponent": HomeComponent,
    "loader": PageLoader,
    //"logout-comp": logoutComp,
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
    this.isLoading = true;
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

        if(this.eimObjects.length!=0)
        {
          this.eimComplete = (this.eimObjects).every(item => item.vendor_ApprovalStatus === 'Approved');
        }

        if(this.absenceObjects.length!=0)
        {
          this.absenceComplete = (this.absenceObjects).every(item => item.vendor_ApprovalStatus === 'Approved');
        }

        if(this.attendanceObjects.length!=0)
        {
          this.attendanceComplete = (this.attendanceObjects).every(item => item.vendor_ApprovalStatus === 'Approved');
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

    await this.$http
      .get(
        "/getApprovalPercentage",
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
        this.eimApprovalPercentage = resp.data[0];
        this.absenceApprovalPercentage = resp.data[1];
        this.attendanceApprovalPercentage = resp.data[2];
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
      this.isLoading = false;
  },
}
</script>
