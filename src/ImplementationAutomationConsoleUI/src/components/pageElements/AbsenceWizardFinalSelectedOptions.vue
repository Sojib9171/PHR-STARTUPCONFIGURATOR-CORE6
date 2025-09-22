<template>
    <div>
        <div v-if="isLoading == true">
            <loader />
        </div>
        <div v-else class="config-block p-0">
            <div class="block-form-top bg-gray p-4">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="quis-block">
                            <ol class="quis-summary ps-2">
                                <li v-for="option in selectedOptions" :key="option.number">
                                    <span class="quis-q">
                                        {{ option.question_statement }}:</span> <span class="quis-a">{{ option.response
                                        }}</span>
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
                            <button class="btn btn-outline-info" style="margin-left: 5px;"
                                @click="editOptions">Edit</button>
                            <button class="btn btn-danger" style="margin-left: 5px;"
                                @click="showDeleteConfimAlert">Delete</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { extendCookieTimeout } from '@/cookieUtils';
import { Base64 } from 'js-base64';
import { numeric } from '@vuelidate/validators';
import PageLoader from '@/components/pageElements/PageLoader.vue';
export default ({
    data() {
        return {
            isLoading: false,
            selectedOptions: null,
        }
    },

    components: {
        "loader": PageLoader,
    },

    props: {
        subsection: String,
        rowId: numeric
    },

    methods: {
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
                    this.deleteOptions();
                }
            })
        },

        async saveOptions() {
            this.$router.push({ name: 'leave-type-summary', params: { subsection: this.subsection } });
        },

        editOptions() {
            this.$router.push({ name: 'absence-wizard-edit-leave-type', params: { subsection: this.subsection, id: this.rowId } });
        },

        deleteOptions() {
            this.removeAbsenceWizardWithRowId();

        },

        async updateDraftStatusForAbsence() {
            await this.$http
                .post(`/updateDraftStatusForAbsence`, {
                    tableRowId: this.rowId,
                    subsectionName: this.subsection
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

        async removeAbsenceWizardWithRowId() {
            await this.$http
                .post(`/deleteAbsenceDataByRowId`, { tableRowId: this.rowId, subsectionName: this.subsection }, {
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
                    this.$router.push({ name: 'leave-type-summary', params: { subsection: this.subsection } });
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
        }
    },

    mounted() {
        this.isLoading = true;
        this.$http.get(`/getAbsenceWizardSelectedOptionsSummary?rowId=${encodeURIComponent(this.rowId)}&subsectionName=${encodeURIComponent(this.subsection)}`, {
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
                this.isLoading = false;
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