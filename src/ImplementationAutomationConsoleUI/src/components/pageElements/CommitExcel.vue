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
                                    before confirming)
                                </label>
                                <div class="col-md-12">
                                    <textarea class="form-control" v-model="comment"></textarea>
                                    <span class="text-danger" v-if="v$.comment.$error">
                                        {{ requiredComment }}
                                    </span>
                                </div>
                            </div>

                            <div class="row mb-3">
                                <label class="col-md-12 mb-1">
                                    Confirm By - (Confirming Person's Name)
                                </label>
                                <div class="col-md-12">
                                    <input class="form-control" v-model="name">
                                    <span class="text-danger" v-if="v$.name.$error">
                                        {{ requiredName }}
                                    </span>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="button-group d-flex justify-content-end">
                                        <button class="btn btn-outline-primary" @click="downloadExcel">
                                            <i class="ri-file-excel-2-fill"></i> <span class="ms-1">Download</span>
                                        </button>
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
import PageLoader from '@/components/pageElements/PageLoader.vue';
//const requiredWithMessage = common.withMessage(required, 'This field is required and must be filled in')
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils'
export default {
    data() {
        return {
            v$: useValidate(),
            comment: '',
            name: '',
            approvalStatus: '',
            currentDate: null,
            moduleName: '',
            formattedDate: '',
            isLoading: false
        }
    },

    components: {
        "loader": PageLoader
    },

    validations() {
        return {
            comment: { required, maxLength: maxLength(300) },
            name: { required },
        }
    },

    methods: {
        async submitForm() {
            this.v$.$validate();
            if (!this.v$.$error) {
                if (this.name != this.$cookies.get("userName")) {
                    this.showNameUnmatchAlert();
                    return;
                }
                this.approvalStatus = 'Approved';
                await this.generateSignOffDocument();
            } else {
                return;
            }
        },

        showSubmitAlert() {
            this.$swal.fire({
                icon: 'success',
                width: 300,
                height: 200,
                text: 'Your confirmation has been successfully recorded.',
                // timer: 3000,
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$router.push({ name: 'home' });
                }
            })
        },

        showRejectAlert() {
            this.deleteDataFromDependantTables();
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'You have rejected the confirmation.',
                // timer: 2000
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$router.push({ name: 'home' });
                }
            });
        },

        showErrorAlert() {
            this.deleteDataFromDependantTables();
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'There was a problem. Please try again',
                timer: 2000,
                allowOutsideClick: false,
                confirmButtonText: 'Ok',
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$router.push({ name: 'home' });
                }
            });
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

        async rejectForm() {
            this.v$.$validate()
            if (!this.v$.$error) {
                if (this.name != this.$cookies.get("userName")) {
                    this.showNameUnmatchAlert();
                    return;
                }
                this.approvalStatus = 'Rejected';
                await this.generateSignOffDocument();

            }
            else {
                return;
            }
        },

        async uploadDashboardInfo() {
            await this.$http
                .post(`/uploadDashboardData`, {
                    SubsectionName: this.subsectionName,
                    UserName: this.$cookies.get("userName"),
                    Name: this.$cookies.get("name"),
                    Comment: this.comment,
                    ModuleId: 0,
                    SubsectionID: '',
                    ApprovalStatus: this.approvalStatus,
                    ApprovalSignature: '',
                    Approvaldate: this.currentDate,
                    ApprovalData: '',
                    SignOffPdfData: ''
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

        async generateSignOffDocument() {
            this.isLoading = true;
            let date = new Date();
            this.currentDate = date;
            await this.$http
                .post(`/generateSignOffDocument`, {
                    SubsectionName: this.subsectionName,
                    UserName: this.$cookies.get("userName"),
                    Name: this.$cookies.get("name"),
                    Comment: this.comment,
                    ModuleId: 0,
                    SubsectionID: '',
                    ApprovalStatus: this.approvalStatus,
                    ApprovalSignature: '',
                    Approvaldate: this.currentDate,
                    ApprovalData: '',
                    SignOffPdfData: ''
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
                    window.open(url, '_blank');
                    this.uploadDashboardInfo();
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

        async downloadExcel() {
            await this.$http
                .get(`/downloadExcelWithData?subsectionName=${encodeURIComponent(this.subsectionName)}`, {
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
    computed: {
        requiredComment() {
            return this.v$.comment.$error ? 'Please fill the relevant fields' : '';
        },

        requiredName() {
            return this.v$.name.$error ? 'Please fill the relevant fields' : '';
        },
    },
    props: {
        'subsectionName': String
    },
}
</script>