<template>
    <div>
        <div v-if="isLoading == true">
            <loader />
        </div>
        <div v-else>
            <div class="root">
                <div class="inner-wrapper p-3 mb-5">
                    <div class="block-data">
                        <div class="block-form">
                            <div class="row mb-3">
                                <label class="col-md-12 mb-1">
                                    Comment - (Please enter a comment as the acceptance
                                    before approving)
                                </label>
                                <div class="col-md-12 mb-3">
                                    <textarea class="form-control" v-model="approvalComment"
                                        placeholder="Add a Comment"></textarea>
                                    <span class="text-danger" v-if="v$.approvalComment.$error">
                                        {{ requiredComment }}
                                    </span>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">
                                        Approval Person
                                    </label>
                                    <input type="text" class="form-control" placeholder="Approval person's name"
                                        v-model="approverName">
                                    <span class="text-danger" v-if="v$.approverName.$error">
                                        {{ requiredName }}
                                    </span>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="button-group d-flex justify-content-end">
                                        <!-- <button class="btn btn-outline-info" @click="downloadSignOff">Sign-off</button> -->
                                        <button class="btn btn-primary" @click="submitForm" style="margin-left: 5px;">
                                            Confirm
                                        </button>
                                        <button class="btn btn-danger" @click="rejectForm" style="margin-left: 5px;">
                                            Delete
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import useValidate from '@vuelidate/core'
import { required, maxLength } from '@vuelidate/validators'
import { Base64 } from "js-base64";
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { extendCookieTimeout } from '@/cookieUtils'
import { getAbsenceActiveSubsectionArray, getActiveModulesArray, getAttendanceActiveSubsectionArray, getEimActiveSubsectionArray } from '@/localStorageUtils'
import { emptyActiveModulesArray, emptyEimActiveSubsectionArray, emptyAbsenceActiveSubsectionArray, emptyAttendanceActiveSubsectionArray } from '@/localStorageUtils';

export default {
    data() {
        return {
            v$: useValidate(),
            approvalComment: '',
            approvalStatus: '',
            approverName: '',
            isLoading: false,
            currentDate: null,
        }
    },

    components: {
        "loader": PageLoader
    },

    validations() {
        return {
            approvalComment: { required, maxLength: maxLength(300) },
            approverName: { required },
        }
    },

    methods: {
        async submitForm() {
            this.v$.$validate();
            if (!this.v$.$error) {
                if (this.approverName != this.$cookies.get("userName")) {
                    this.showNameUnmatchAlert();
                    return;
                }
                this.approvalStatus = 'Approved';
                await this.generateSignOffDocument();
            } else {
                return;
            }
        },

        showNameUnmatchAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'User name does not match',
                timer: 2000,
                allowOutsideClick: false,
            });
        },

        showSubmitAlert() {
            this.$swal.fire({
                icon: 'success',
                width: 300,
                height: 200,
                text: 'Your Approval has been successfully recorded.',
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    emptyActiveModulesArray();
                    emptyEimActiveSubsectionArray();
                    emptyAbsenceActiveSubsectionArray();
                    emptyAttendanceActiveSubsectionArray();
                    this.$router.push({ name: 'configuration-control' });
                }
            })
        },

        showRejectAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'Your have rejected the approval.',
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    emptyActiveModulesArray();
                    emptyEimActiveSubsectionArray();
                    emptyAbsenceActiveSubsectionArray();
                    emptyAttendanceActiveSubsectionArray();
                    this.$router.push({ name: 'configuration-control' });
                }
            });
        },

        showErrorAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'There was a problem. Please try again',
                allowOutsideClick: false,
                confirmButtonText: 'Ok',
            }).then((result) => {
                if (result.isConfirmed) {
                    emptyActiveModulesArray();
                    emptyEimActiveSubsectionArray();
                    emptyAbsenceActiveSubsectionArray();
                    emptyAttendanceActiveSubsectionArray();
                    this.$router.push({ name: 'configuration-control' });
                }
            });
        },

        async rejectForm() {
            this.v$.$validate()
            if (!this.v$.$error) {
                if (this.approverName != this.$cookies.get("userName")) {
                    this.showNameUnmatchAlert();
                    return;
                }
                this.approvalStatus = 'Rejected';
                this.generateSignOffDocument();
            } else {
                return;
            }
        },

        async updateEnabledModules() {
            this.isLoading = true;
            await this.$http.post(`/updateOrdersForModule`, {
                Modules: getActiveModulesArray()
            }, {
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
                this.updateEnabledSubsections();
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

        async updateEnabledSubsections() {
            await this.$http.post(`/updateOrdersForSubsection`, {
                EimObjects: getEimActiveSubsectionArray(),
                AbsenceObjects: getAbsenceActiveSubsectionArray(),
                AttendanceObjects: getAttendanceActiveSubsectionArray()
            }, {
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

        async uploadConfigControlData() {
            await this.$http
                .post(`/uploadConfigControlData`, {
                    UserName: this.$cookies.get("userName"),
                    Name: this.$cookies.get("name"),
                    Comment: this.approvalComment,
                    ApprovalStatus: this.approvalStatus,
                    Approvaldate: this.currentDate,
                    SignOffPdfData: '',
                }, {
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
                    this.isLoading = false;
                    if (this.approvalStatus === 'Approved')
                        this.showSubmitAlert();
                    else if (this.approvalStatus === 'Rejected')
                        this.showRejectAlert();
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

        async generateSignOffDocument() {
            this.isLoading = true;
            let date = new Date();
            this.currentDate = date;
            await this.$http
                .post(`/generateSignOffDocumentForConfigControl`, {
                    UserName: this.$cookies.get("userName"),
                    Name: this.$cookies.get("name"),
                    Comment: this.approvalComment,
                    ApprovalStatus: this.approvalStatus,
                    Approvaldate: this.currentDate,
                    SignOffPdfData: '',
                }, {
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
                    const binaryData = atob(resp.data);
                    const uint8Array = new Uint8Array(binaryData.length);

                    for (let i = 0; i < binaryData.length; i++) {
                        uint8Array[i] = binaryData.charCodeAt(i);
                    }

                    const blob = new Blob([uint8Array], { type: 'application/pdf' });
                    const url = window.URL.createObjectURL(blob);
                    // downloadLink.href = url;
                    // downloadLink.download = 'document.pdf';
                    window.open(url, '_blank');

                    if (this.approvalStatus === 'Approved')
                        this.updateEnabledModules();

                    this.uploadConfigControlData();
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
    },
    computed: {
        requiredComment() {
            return this.v$.approvalComment.$error ? 'Please fill the relevant fields' : '';
        },

        requiredName() {
            return this.v$.approverName.$error ? 'Please fill the relevant fields' : '';
        },
    },
}
</script>