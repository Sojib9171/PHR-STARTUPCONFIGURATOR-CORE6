<template>
  <div v-if="validationCounts == null || isLoading == true">
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
            <h2>Overview for {{ subsectionName }} Upload</h2>

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
          <div class="section-body">
            <div class="inner-wrapper p-3 mb-5">
              <div class="block-data">
                <div class="row">
                  <div class="col-md-6">
                    <div class="block-overview-charts p-4">
                      <!-- Charts goes here -->
                      <div class="row">
                        <div class="col-md-6 mx-auto text-center">
                          <div>
                            <successChart :successCount="validationCounts[1]" :total="validationCounts[0]" />
                          </div>
                          <div>
                            <h3><span class="text-success">{{ validationCounts[1] }}</span> / {{ validationCounts[0] }}
                            </h3>
                          </div>
                        </div>

                        <div class="col-md-6 mx-auto text-center">
                          <div>
                            <errorChart :errorCount="validationCounts[2]" :total="validationCounts[0]" />
                          </div>
                          <div>
                            <h3><span class="text-danger">{{ validationCounts[2] }}</span> / {{ validationCounts[0] }}
                            </h3>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>

                  <div class="col-md-6">
                    <div class="block-overview-alerts p-4 pb-0">
                      <div class="alert alert-primary d-flex align-items-center mb-3" role="alert">
                        <i class="ri-information-fill"></i>
                        <div class="ms-2">
                          <span class="fw-500">Note:</span> Error count should be zero
                          in-order
                          to proceed with the confirmation
                        </div>
                      </div>
                      <div class="alert alert-warning d-flex align-items-baseline mb-3" role="alert"
                        v-if="validationCounts[2] != 0">
                        <i class="ri-information-fill"></i>
                        <div class="ms-2">
                          Please fix the errors identified, by downloading the excel
                          sheet and re-upload after resolving the errors
                        </div>
                      </div>
                    </div>
                    <div class="button-group p-4 pt-0">
                      <button @click="this.downloadValidatedExcel" class="btn btn-outline-primary"
                        style="margin-right: 5px;">
                        <i class="ri-file-excel-2-fill"></i> <span class="ms-1">Download</span>
                      </button>
                      <button v-if="this.hasError" class="btn btn-primary" @click="reuploadData"
                        style="margin-right: 5px;">
                        Re-upload
                      </button>
                      <button v-else class="btn btn-primary" @click="confirmOverview" style="margin-right: 5px;">
                        Confirm
                      </button>
                      <!-- <button class="btn btn-primary" @click="onConfirm">
                        Demo Success
                      </button> -->
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
</template>
  
<script>
import SideBar from "../components/navigation/SideBar.vue";
import TopBar from "../components/navigation/TopBar.vue";
//import PageTitle from "../components/navigation/PageTitle.vue";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import SuccessChart from '@/components/pageElements/SuccessCountChart.vue';
import ErrorChart from '@/components/pageElements/ErrorCountChart.vue';
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      subsectionName: null,
      validationCounts: null,
      isLoading: false,
      isLeaving: false,
      isLoggingOut: false,
      skipBeforeRouteLeave: false,
      allObjects: [],
      eimObjects: [],
      absenceObjects: [],
      hasError: false,
      attendanceObjects: []
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    //"pageTitle": PageTitle,
    "loader": PageLoader,
    "successChart": SuccessChart,
    "errorChart": ErrorChart,
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
    await this.$http
      .get(`/validation?subsectionName=${encodeURIComponent(this.subsectionName)}`, {
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
      .then((resp) => {
        this.validationCounts = resp.data;
        if(this.validationCounts[0]==0 && this.validationCounts[1]==0 && this.validationCounts[2]==0)
        {
          this.skipBeforeRouteLeave=true;
          this.$router.push({ name: 'home'});
        }
        this.hasError = this.validationCounts[2] != 0;
        this.isLoading = false;
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

  methods:
  {
    async updateDependentColumns() {
      this.isLoading = true;
      await this.$http
        .get(`/updateDependentColumnsBySubsection?subsectionName=${encodeURIComponent(this.subsectionName)}`, {
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
          this.$router.push({ name: 'data-commit', params: { subsection: this.subsectionName } });
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

    async downloadValidatedExcel() {
      this.isLoading = true;
      await this.$http
        .get(`/validatedExcelDownload?subsectionName=${encodeURIComponent(this.subsectionName)}`, {
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
        .then((resp) => {
          var excelData = resp.data;
          const link = document.createElement("a");
          link.href =
            "data:application/octet-stream;charset=utf-8;base64," + excelData[0];
          link.target = "_blank";
          link.download = excelData[1];
          link.click();
          this.isLoading = false;
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

    showErrorRemainingAlert() {
      this.$swal.fire({
        icon: 'error',
        width: 300,
        height: 200,
        text: 'Please fix the errors identified, by downloading the excel sheet and re-upload after resolving the errors',
        timer: 2000
      });
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

    showSuccessfulValidationAlert() {
      this.$swal.fire({
        icon: 'success',
        width: 300,
        height: 200,
        text: 'Successfully validated',
        timer: 2000
      });
    },

    confirmOverview() {
      if (this.validationCounts[2] !== 0) {
        this.showErrorRemainingAlert();
      }
      else {
        //router.push to next page
        this.showSuccessfulValidationAlert();
        this.skipBeforeRouteLeave = true;
        this.updateDependentColumns();
      }
    },

    reuploadData() {
      this.$router.push({ name: 'upload', params: { subsection: this.subsectionName } });
    },

    onConfirm() {
      this.showSuccessfulValidationAlert();
      this.skipBeforeRouteLeave = true;
      this.$router.push({ name: 'data-commit', params: { subsection: this.subsectionName } });
    },

    async deleteDataFromDependantTables() {
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
  },

  beforeRouteEnter(to, from, next) {
    const fromPage = from.name || 'unknown'
    if (fromPage === 'upload') {
      next()
    }
    else if (fromPage === 'data-commit') {
      next({ name: 'upload', params: { subsection: from.params.subsection } })
    }
    else {
      next({ name: 'home' })
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
    if (this.skipBeforeRouteLeave) {
      next();
    }
    else if (this.isLeaving) {
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
          //this.deleteDataFromDependantTables();

          this.$http.post(`/deleteDataFromDependentTable`, { subsectionName: this.subsectionName }, {
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
            this.$http.post(`/clearTable`, { subsectionName: this.subsectionName }, {
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
              next(-1);
            });
          }).catch(error => {
            if (this.$cookies.get('enc') != null && this.$cookies.get('encVal') != null && this.$cookies.get('userTypeCode') != null && this.$cookies.get('userName') != null && this.$cookies.get('name') != null) {
              this.showErrorAlert();
            }
            if (error.response.status == 401) {
              this.emitter.emit('loggedOut', true);
            }
            else if (error.response.status == 403) {
              this.emitter.emit('accessDenied', true);
            }
          });
        } else {
          this.isLeaving = false;
          next(false);
        }
      }).catch((error) => {
        console.log(error.message);
        next(false);
      });
    }
  }
}
</script>
