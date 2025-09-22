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
                <li class="breadcrumb-item"><router-link :to="{ name: 'basic-configuration-1' }"></router-link>
                  Basic Configuration</li>
                <li class="breadcrumb-item">
                  Common Configuration
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
              <commonConfigEdit :key="componentKey" :totalQuestionCount="totalQuestionCount" :data="currentObject"
                @next="next" @back="back"></commonConfigEdit>
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
import CommonConfigEdit from '../components/pageElements/CommonConfigEdit.vue'
import PageLoader from '@/components/pageElements/PageLoader.vue';
import '../../css/styles.css';
import { extendCookieTimeout } from '@/cookieUtils';
import { Base64 } from "js-base64";
import { getCommonConfigRowID } from '@/localStorageUtils';

export default {
  data() {
    return {
      isLoading: false,
      pageName: "Basic Configuration",
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      objects: [
      ],
      currentIndex: 0,
      componentKey: 0,
      questionsLoaded: false,
      totalQuestionCount: 0,
      disableSideBar: false,
      rowId: 0,
      message: '',
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

    // currentObject() {
    //   for (var question of this.objects) {
    //     if (question.response == null || question.response == '' || question.response == -1) {
    //       this.currentIndex = question.question_no - 1;
    //       break;
    //     }
    //   }
    //   return this.objects[this.currentIndex];
    // },

    next() {
      if (this.currentIndex < this.objects.length - 1) {
        this.currentIndex++;
        this.componentKey++;
      }
      else if (this.currentIndex >= this.objects.length - 1) {
        this.$router.push({ name: 'common-config-summary' });
      }

      this.isLoading = true;
      setTimeout(() => {
        this.isLoading = false;
      }, 300);
    },

    back() {
      if (this.currentIndex == 0) {
        this.$router.push({ name: 'basic-configuration-1' });
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
    "loader": PageLoader,
    "commonConfigEdit": CommonConfigEdit
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
    this.rowId = getCommonConfigRowID();
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

    await this.$http
      .get(
        `/getCommonConfigSummary?rowId=${encodeURIComponent(this.rowId)}`,
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
        for (var i = 0; i < this.objects.length; i++) {
          var item = this.objects[i];
          if (item.response === null || item.response === '' || item.response === -1) {
            this.currentIndex = item.question_no - 1;
            break;
          }
          else {
            if (i === this.objects.length - 1) {
              this.$router.push({ name: 'common-config-summary' });
              return;
            }
          }
        }

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
  },

  computed: {
    currentObject() {
      return this.objects[this.currentIndex];
    },
  },
}
</script>
