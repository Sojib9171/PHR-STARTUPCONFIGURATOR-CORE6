<template>
  <div v-if="isLoading">
    <loader />
  </div>
  <div v-else>
    <sideBar :remainingCount="remainingApprovalCount" />
    <div class="col page-content px-0">
      <topBar />
      <div class="page-body">
        <div>
          <div class="page-title">
            <!--Page title-->
            <h2>{{ pageName }}</h2>

            <!--Breadcrumb-->
            <nav style="--bs-breadcrumb-divider: '|';" aria-label="breadcrumb">
              <ol class="breadcrumb">
                <li class="breadcrumb-item"><router-link :to="{ name: 'admin-approval-summary' }"><i
                      class="ri-home-4-line"></i></router-link>
                </li>
                <li class="breadcrumb-item"><router-link :to="{ name: 'admin-approval' }"></router-link>
                  Admin Approval</li>
              </ol>
            </nav>
          </div>
        </div>
        <section class="mt-3">
          <div class="section-body">
            <div class="config-block">
              <AdminApprovalTable />
            </div>
          </div>
        </section>
      </div>
    </div>
  </div>
</template>
  
<script>
//import SideBar from "../components/navigation/SideBar.vue";
import SideBar from "../components/navigation/SideBarForAdmin.vue";
import TopBar from "../components/navigation/TopBar.vue";
import AdminApprovalTable from "../components/pageElements/AdminPendingApprovalTable.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
import '../../css/styles.css';
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data: function () {
    return {
      pageName: "For Admin Approval",
      isLoading: false,
      remainingApprovalCount: 0,
    };
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "AdminApprovalTable": AdminApprovalTable,
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
    await this.$http
      .get(
        "/getRemainingApprovalCount",
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
        this.remainingApprovalCount = resp.data;
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
  