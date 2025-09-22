<template>
    <div class="config-block p-0">
        <div class="block-form-top bg-gray p-4">
            <div class="row">
                <div class="col-sm-12">
                    <div class="quis-block">
                        <ol class="quis-summary ps-2">
                            <li v-for="option in selectedOptions" :key="option.question_no">
                                <!-- <span v-if="option.question_no == 2"><span class="quis-q">
                                        {{ option.question_statement }}:</span> <span class="quis-a">{{
                                            this.logoName
                                        }}</span></span> -->

                                <span v-if="option.question_no == 3"><span class="quis-q">
                                        {{ option.question_statement }}:</span>
                                    <span v-if="option.response == 1" class="quis-a">Yes</span>
                                    <span v-else-if="option.response == 0" class="quis-a">No</span>
                                    <span v-else class="quis-a"></span>
                                </span>

                                <!-- <span v-else-if="option.question_no == 4"><span class="quis-q">
                                        {{ option.question_statement }}:</span> <span class="quis-a">{{
                                            this.mobileLogoName
                                        }}</span></span> -->

                                <span v-else-if="option.question_no == 5"><span class="quis-q">
                                        {{ option.question_statement }}:</span> <span class="quis-a"> <span
                                            v-if="option.response == 1" class="quis-a">Yes</span>
                                        <span v-else-if="option.response == 0" class="quis-a">No</span>
                                        <span v-else class="quis-a"></span></span></span>

                                <span v-else><span class="quis-q">
                                        {{ option.question_statement }}:</span> <span class="quis-a">{{ option.response
                                        }}</span></span>
                            </li>
                        </ol>
                    </div>
                </div>
            </div>
        </div>
        <div class="approve-form-block bg-white p-4">
            <div class="row mt-4">
                <div class="col-12 text-center">
                    <div class="button-group">
                        <button class="btn btn-success" @click="saveOptions">Save</button>
                        <button class="btn btn-outline-info" style="margin-left: 5px;" @click="editOptions">Edit</button>
                        <button class="btn btn-danger" style="margin-left: 5px;" @click="deleteOptions">Delete</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { getCommonConfigRowID, getCommonConfigLogoImageName, getCommonConfigMobileLogoImageName, removeCommonConfigLogoImageName, removeCommonConfigMobileLogoImageName, removeCommonConfigRowID } from '@/localStorageUtils';
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils'
export default ({
    data() {
        return {
            isLoading: false,
            selectedOptions: null,
            logoName: '',
            mobileLogoName: '',
            rowId: 0
        }
    },

    props: {
        subsection: {
            type: String,
            required: true,
        },
        id: {
            type: Number,
            required: true
        }
    },
    methods: {
        async saveOptions() {
            this.$router.push({ name: 'common-config-saved' });
        },

        editOptions() {
            this.$router.push({ name: 'common-config-edit', params: { id: this.rowId } });
        },

        deleteOptions() {
            this.showDeleteConfimAlert();
        },

        async removeCommonConfigWithRowId() {
            await this.$http
                .post(`/deleteDataByRowId`, { tableRowId: this.rowId }, {
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
                    this.showErrorAlert();
                    if (error.response.status == 401) {
                        this.emitter.emit('loggedOut', true);
                    }
                    else if (error.response.status == 403) {
                        this.emitter.emit('accessDenied', true);
                    }
                });
        },

        showDeleteConfimAlert() {
            this.$swal({
                title: 'Are you sure you want to delete?',
                showDenyButton: true,
                confirmButtonText: 'No',
                denyButtonText: `Yes`,
            }).then((result) => {
                if (result.isConfirmed) {
                    return;
                } else if (result.isDenied) {
                    this.removeCommonConfigWithRowId();
                    removeCommonConfigLogoImageName();
                    removeCommonConfigMobileLogoImageName();
                    removeCommonConfigRowID();
                    this.$router.push({ name: 'basic-configuration-1' });
                }
            })
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
})
</script>

