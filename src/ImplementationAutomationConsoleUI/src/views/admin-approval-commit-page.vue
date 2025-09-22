<template>
  <div>
    <div v-if="isLoading">
      <loader />
    </div>
    <div v-else>
      <div>
        <sideBar :remainingCount="remainingApprovalCount" />
        <div class="col page-content px-0">
          <topBar />
          <div class="page-body">
            <div class="page-title col-md-12 mt-3 mb-3">
              <h2> Approve Data For {{ subsectionName }}</h2>
            </div>
            <section class="mt-3">
              <div class="section-body">
                <div>
                  <adminCommit :record_ID="record_Id" :subsectionName="subsectionName"></adminCommit>
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
//import SideBar from "../components/navigation/SideBar.vue";
import SideBar from "../components/navigation/SideBarForAdmin.vue";
import TopBar from "../components/navigation/TopBar.vue";
import AdminCommit from "../components/pageElements/AdminApprovalCommit.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      selectedFile: null,
      isLoading: false,
      subsectionName: null,
      record_Id: null,
      isLeaving: false,
      remainingApprovalCount: 0,
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "adminCommit": AdminCommit,
    "loader": PageLoader
  },

  beforeRouteEnter(to, from, next) {
    const fromPage = from.name || 'unknown'
    if (fromPage !== 'admin-approval') {
      next({ name: 'admin-approval' })
    } else {
      next()
    }
  },

  beforeRouteUpdate(to, from, next) {
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
      next();
    }, 300);
  },

  beforeRouteLeave(to, from, next) {
    if (to.name === 'admin-approval-summary' && from.name === 'admin-commit') {
      this.isLeaving = true;
    }

    if (this.isLeaving) {
      next();
    }
    else {
      this.isLeaving = true;
      this.$swal({
        title: 'Are you sure?',
        text: 'You have unsaved changes. Are you sure you want to leave?',
        icon: 'warning',
        width: 300,
        height: 200,
        showCancelButton: true,
        confirmButtonText: 'Stay',
        cancelButtonText: 'Leave',
        allowOutsideClick: false,
      }).then(result => {
        if (!result.isConfirmed) {
          next();
        } else {
          this.isLeaving = false;
          next(false);
        }
      })
    }
  },

  async mounted() {
    this.record_Id = this.$route.params.record_Id;
    this.subsectionName = this.$route.params.subsection;

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
  }
}
</script>

<style scoped>
.card-content {
  margin: 5px 20px;
}

.ri-checkbox-circle-fill {
  font-size: 20px;
}
</style>
