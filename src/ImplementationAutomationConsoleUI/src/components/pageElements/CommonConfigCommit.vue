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
                                        <!-- <button class="btn btn-outline-primary" @click="downloadExcel">
                                            <span class="ms-1">Summary</span>
                                        </button> -->
                                        <button type="button" id="modal1Btn" class="btn btn-outline-primary"
                                            data-bs-toggle="modal" data-bs-target=".myclass1">
                                            <span class="ms-1">Summary</span>
                                        </button>

                                        <div class="modal fade myclass1" id="configIntro" tabindex="-1"
                                            data-bs-keyboard="false" data-bs-backdrop="static"
                                            aria-labelledby="configIntroLabel" aria-hidden="true">
                                            <div class="modal-dialog modal-xl modal-dialog-centered">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <button type="button" class="btn-close" data-bs-dismiss="modal"
                                                            aria-label="Close"></button>
                                                    </div>
                                                    <div class="modal-body p-0 m-0">
                                                        <div class="block-form-top bg-gray p-4">
                                                            <div class="row">
                                                                <div class="col-sm-12">
                                                                    <div class="quis-block">
                                                                        <ol class="quis-summary ps-2">
                                                                            <li v-for="option in selectedOptions"
                                                                                :key="option.question_no">
                                                                                <span v-if="option.question_no == 2"><span
                                                                                        class="quis-q">
                                                                                        {{ option.question_statement
                                                                                        }}:</span> <span class="quis-a">{{
    this.logoName
}}</span></span>

                                                                                <span
                                                                                    v-else-if="option.question_no == 3"><span
                                                                                        class="quis-q">
                                                                                        {{ option.question_statement
                                                                                        }}:</span>
                                                                                    <span v-if="option.response == 1"
                                                                                        class="quis-a">Yes</span>
                                                                                    <span v-else-if="option.response == 0"
                                                                                        class="quis-a">No</span>
                                                                                    <span v-else class="quis-a"></span>
                                                                                </span>

                                                                                <span
                                                                                    v-else-if="option.question_no == 4"><span
                                                                                        class="quis-q">
                                                                                        {{ option.question_statement
                                                                                        }}:</span> <span class="quis-a">{{
    this.mobileLogoName
}}</span></span>

                                                                                <span
                                                                                    v-else-if="option.question_no == 5"><span
                                                                                        class="quis-q">
                                                                                        {{ option.question_statement
                                                                                        }}:</span> <span class="quis-a">
                                                                                        <span v-if="option.response == 1"
                                                                                            class="quis-a">Yes</span>
                                                                                        <span
                                                                                            v-else-if="option.response == 0"
                                                                                            class="quis-a">No</span>
                                                                                        <span v-else
                                                                                            class="quis-a"></span></span></span>

                                                                                <span v-else><span class="quis-q">
                                                                                        {{ option.question_statement
                                                                                        }}:</span> <span class="quis-a">{{
    option.response
}}</span></span>
                                                                            </li>
                                                                        </ol>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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
import { getCommonConfigLogoImageName, getCommonConfigMobileLogoImageName, getCommonConfigRowID, removeCommonConfigLogoImageName, removeCommonConfigMobileLogoImageName, removeCommonConfigRowID } from '@/localStorageUtils';

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
            isLoading: false,
            selectedOptions: null,
            logoName: '',
            mobileLogoName: '',
            rowId: 0
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
                await this.updateDraftStatusForCommonConfig(this.rowId);
            } else {
                return;
            }
        },

        async updateDraftStatusForCommonConfig(rowId) {
            await this.$http
                .post(`/updateDraftStatusForCommonConfig`, {
                    tableRowId: rowId,
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
                    this.generateSignOffDocument();
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
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'Your have rejected the confirmation.',
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
                await this.updateDraftStatusForCommonConfig(this.rowId);
            }
            else {
                return;
            }
        },

        async uploadCommonConfigData() {
            await this.$http
                .post(`/uploadCommonConfigData`, {
                    UserName: this.$cookies.get("userName"),
                    Name: this.$cookies.get("name"),
                    Comment: this.comment,
                    ApprovalStatus: this.approvalStatus,
                    Approvaldate: this.currentDate,
                    SignOffPdfData: '',
                    TableRowId: this.rowId
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
                    removeCommonConfigRowID();
                    removeCommonConfigLogoImageName();
                    removeCommonConfigMobileLogoImageName();
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
                .post(`/generateSignOffDocumentForCommonConfig`, {
                    UserName: this.$cookies.get("userName"),
                    Name: this.$cookies.get("name"),
                    Comment: this.comment,
                    ApprovalStatus: this.approvalStatus,
                    Approvaldate: this.currentDate,
                    SignOffPdfData: '',
                    TableRowId: this.rowId
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

                    this.uploadCommonConfigData();
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
            return this.v$.comment.$error ? 'Please fill the relevant fields' : '';
        },

        requiredName() {
            return this.v$.name.$error ? 'Please fill the relevant fields' : '';
        },
    },

    mounted() {
        this.logoName = getCommonConfigLogoImageName();
        this.mobileLogoName = getCommonConfigMobileLogoImageName();
        this.rowId = getCommonConfigRowID();
        this.$http.get(`/getCommonConfigSummary?rowId=${encodeURIComponent(this.rowId)}`, {
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
            .then(response => {
                this.selectedOptions = response.data;
                extendCookieTimeout();
            })
            .catch(error => {
                if (error.response.status == 401) {
                    this.emitter.emit('loggedOut', true);
                }
                else if (error.response.status == 403) {
                    this.emitter.emit('accessDenied', true);
                }
            });
    }
}
</script>

<style scoped>
:deep(.modal-header) {
    display: flex;
    justify-content: right;
}
</style>