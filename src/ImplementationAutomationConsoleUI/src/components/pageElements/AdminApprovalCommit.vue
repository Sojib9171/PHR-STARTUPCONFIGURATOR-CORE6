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
                                    <textarea class="form-control" v-model="vendorComment"
                                        placeholder="Add a Comment"></textarea>
                                    <span class="text-danger" v-if="v$.vendorComment.$error">
                                        {{ requiredComment }}
                                    </span>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">
                                        Approval Person
                                    </label>
                                    <input type="text" class="form-control" placeholder="Approval person's name"
                                        v-model="vendorName">
                                    <span class="text-danger" v-if="v$.vendorName.$error">
                                        {{ requiredName }}
                                    </span>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="button-group d-flex justify-content-end">
                                        <button class="btn btn-outline-info" @click="downloadSignOff">Sign-off</button>
                                        <button class="btn btn-primary" @click="submitForm" style="margin-left: 5px;">
                                            Approve
                                        </button>
                                        <button class="btn btn-danger" @click="rejectForm" style="margin-left: 5px;">
                                            Reject
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

export default {
    data() {
        return {
            v$: useValidate(),
            vendorComment: '',
            vendorApprovalStatus: '',
            vendorName: '',
            isLoading: false
        }
    },

    components: {
        "loader": PageLoader
    },

    validations() {
        return {
            vendorComment: { required, maxLength: maxLength(300) },
            vendorName: { required },
        }
    },

    methods: {
        async submitForm() {
            this.v$.$validate();
            if (!this.v$.$error) {
                if (this.vendorName != this.$cookies.get("userName")) {
                    this.showNameUnmatchAlert();
                    return;
                }
                this.vendorApprovalStatus = 'Approved';
                await this.uploadVendorInfo();
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
                    this.$router.push({ name: 'admin-approval-summary' });
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
                    this.$router.push({ name: 'admin-approval-summary' });
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


        async rejectForm() {
            this.v$.$validate()
            if (!this.v$.$error) {
                if (this.vendorName != this.$cookies.get("userName")) {
                    this.showNameUnmatchAlert();
                    return;
                }
                this.vendorApprovalStatus = 'Rejected';
                await this.uploadVendorInfo();
            } else {
                return;
            }
        },

        async uploadVendorInfo() {
            this.isLoading = true;
            let currentDate = new Date();
            await this.$http
                .post("/uploadVendorApprovalData", {
                    VendorName: this.$cookies.get("userName"),
                    VendorApprovalStatus: this.vendorApprovalStatus,
                    VendorApprovalComment: this.vendorComment,
                    VendorApprovaldate: currentDate,
                    UserRecordId: parseInt(this.record_ID),
                    SubsectionName: this.subsectionName
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
                    if (this.vendorApprovalStatus === 'Approved') {
                        if (this.subsectionName == 'Short Leave' || this.subsectionName == 'Statutory Leave') {
                            this.deleteDataFromTablesForWizard();
                        }
                        this.showSubmitAlert();
                    }
                    if (this.vendorApprovalStatus === 'Rejected') {
                        if (this.subsectionName == 'Short Leave' || this.subsectionName == 'Statutory Leave') {
                            this.deleteDataFromTablesForWizard();
                        }
                        this.showRejectAlert();
                    }

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

        async deleteDataFromTablesForWizard() {
            await this.$http.post(`/deleteDataFromDraftAndMainTable`, { subsectionName: this.subsectionName }, {
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

        async insertDataToMainTable() {
            await this.$http
                .post(`/insertDataToMainTable`, { subsectionName: this.subsectionName }, {
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
                })
                .catch((error) => {
                    console.log(error.message);
                    //this.showErrorAlert();
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
                .get(`/downloadExcelWithRecordID?recordId=${encodeURIComponent(this.record_ID)}`, {
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
                    const link = document.createElement("a");
                    link.href =
                        "data:application/octet-stream;charset=utf-8;base64," + resp.data;
                    link.target = "_blank";
                    link.download = this.subsectionName + '.xlsm';
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
        async downloadSignOff() {
            await this.$http
                .get(`/downloadSignOffPdfWithRecordID?recordId=${encodeURIComponent(this.record_ID)}`, {
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
                    const link = document.createElement('a');
                    link.href = url;
                    link.download = 'PeopleHR Config Assist-Approval Report.pdf';
                    link.target = '_blank';
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
    },
    computed: {
        requiredComment() {
            return this.v$.vendorComment.$error ? 'Please fill the relevant fields' : '';
        },

        requiredName() {
            return this.v$.vendorName.$error ? 'Please fill the relevant fields' : '';
        },
    },
    props: {
        'record_ID': String,
        'subsectionName': String
    }
}
</script>