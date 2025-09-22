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
          <topBar />
          <div class="page-body">
            <div>
              <div class="page-title">
                <!--Page title-->
                <h2>Commit {{ subsectionName }}</h2>

                <!--Breadcrumb-->
                <nav style="--bs-breadcrumb-divider: '|';" aria-label="breadcrumb">
                  <ol class="breadcrumb">
                    <li class="breadcrumb-item"><router-link :to="{ name: 'home' }"><i
                          class="ri-home-4-line"></i></router-link>
                    </li>
                    <li class="breadcrumb-item"><router-link :to="{ name: 'advance-configuration-1' }"></router-link>
                      Advanced Configuration</li>
                    <li class="breadcrumb-item">{{ subsectionName }}
                    </li>
                  </ol>
                </nav>
              </div>
            </div>

            <section class="mt-3">
              <div class="section-heading">
                <h3 class="section-title">Ready to Commit</h3>
              </div>
              <div class="section-body">
                <div class="block-form-top bg-gray p-4" v-if="this.subsectionName == 'Employee Data'">
                  <div class="block-data">
                    <div class="block-commits">
                      <div class="row">
                        <div class="col-md-12">
                          <ul class="list list-unstyled">
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Personal Details</span>
                              </div>
                            </li>
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Employment Details</span>
                              </div>
                            </li>
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Workstation Details</span>
                              </div>
                            </li>
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Tax Details</span>
                              </div>
                            </li>
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Permanent, Temporary & Official Contact
                                  Details</span>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="block-form-top bg-gray p-4" v-if="this.subsectionName == 'Bank Details'">
                  <div class="block-data">
                    <div class="block-commits">
                      <div class="row">
                        <div class="col-md-12">
                          <ul class="list list-unstyled">
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Bank Details</span>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="block-form-top bg-gray p-4" v-if="this.subsectionName == 'Reporting Hierarchy'">
                  <div class="block-data">
                    <div class="block-commits">
                      <div class="row">
                        <div class="col-md-12">
                          <ul class="list list-unstyled">
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Reporting Hierarchy Details</span>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="block-form-top bg-gray p-4" v-if="this.subsectionName == 'Dependent Information'">
                  <div class="block-data">
                    <div class="block-commits">
                      <div class="row">
                        <div class="col-md-12">
                          <ul class="list list-unstyled">
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Dependent Details</span>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="block-form-top bg-gray p-4" v-if="this.subsectionName == 'Emergency Contact'">
                  <div class="block-data">
                    <div class="block-commits">
                      <div class="row">
                        <div class="col-md-12">
                          <ul class="list list-unstyled">
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Emergency Contact Details</span>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>


                <div class="block-form-top bg-gray p-4" v-if="this.subsectionName == 'Statutory Leave'">
                  <div class="block-data">
                    <div class="block-commits">
                      <div class="row">
                        <div class="col-md-12">
                          <ul class="list list-unstyled">
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Statutory Leave</span>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="block-form-top bg-gray p-4" v-if="this.subsectionName == 'Short Leave'">
                  <div class="block-data">
                    <div class="block-commits">
                      <div class="row">
                        <div class="col-md-12">
                          <ul class="list list-unstyled">
                            <li>
                              <div class="d-flex">
                                <i class="ri-checkbox-fill me-2"></i>
                                <span>Short Leave</span>
                              </div>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <commitExcel :subsectionName="subsectionName" />
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
import CommitExcel from "../components/pageElements/CommitExcel.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      selectedFile: null,
      isLoading: false,
      subsectionName: null,
      isLeaving: false,
      isLoggingOut: false,
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      attendanceObjects: []
    }
  },

  props: {
    subsection: {
      type: String,
      required: true
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    // "pageTitle": PageTitle,
    "commitExcel": CommitExcel,
    "loader": PageLoader
  },

  beforeRouteEnter(to, from, next) {
    const fromPage = from.name || 'unknown'
    if (fromPage !== 'data-overview') {
      next({ name: 'home' })
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
    if (to.name === 'home' && from.name === 'data-commit') {
      this.isLeaving = true;
    }

    if (this.isLeaving) {
      next();
    }
    else if (this.isLoggingOut) {
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
          this.deleteDataFromDependantTables();
          this.$http.post(`/clearTable`, { subsectionName: this.subsectionName }, {
          }, {
            headers: {
              'Content-Type': 'multipart/form-data',
              accept: "*/*",
              Authorization:
                "Basic " +
                Base64.toBase64(
                  this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                ),
            },
            withCredentials: true,
          });
          next();
        } else {
          this.isLeaving = false;
          next(false);
        }
      })
    }
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

    this.subsectionName = this.$route.params.subsection;
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
    }, 300);

    this.emitter.on("loggingOut", (istrue) => {
      if (istrue == true) {
        this.isLoggingOut = true;
        this.emitter.emit('loggedOut', true);
        this.$http
          .post("/initiate", {}, { withCredentials: true })
          .then((resp) => {
            this.$cookies.set("cookie", Base64.btoa(resp.data));
          })
          .catch((error) => {
            console.log(error.message);
          });
      }
    });
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

    async deleteDataFromDependantTables() {
      this.isLoading = true;
      await this.$http.post(`/deleteDataFromDependentTable`, { subsectionName: this.subsectionName }, {
        headers: {
          accept: "*/*",
          Authorization:
            "Basic " +
            Base64.toBase64(
              this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
            ),
        },
        withCredentials: true,
      }).then(() => {
        extendCookieTimeout();
        this.isLoading = false;
      }).catch(error => {
        this.showErrorAlert();
        if (error.response.status == 401) {
          this.emitter.emit('loggedOut', true);
        }
        else if (error.response.status == 403) {
          this.emitter.emit('accessDenied', true);
        }
      });
    },

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
  },
}
</script>
