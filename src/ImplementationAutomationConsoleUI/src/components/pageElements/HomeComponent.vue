<template>
    <div class="config-block mb-4">
        <h3 class="block-title mb-3">General Configuration</h3>
        <div class="card mb-3 p-3">
            <div class="row align-items-center">
                <div class="col-sm-3">Basic Configuration</div>
                <div class="col-sm-6 config-progress">
                    <div class="progress">
                        <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                            aria-valuenow="100" aria-valuemin="0" aria-valuemax="100"
                            :style="`width: ${commonConfigPercentage}%`">
                            <span class="progress-value">{{ commonConfigPercentage }}%</span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3 config-status text-end" v-if="commonConfigComplete">
                    <span><i class="ri-checkbox-circle-fill"></i> Ready</span>
                </div>
            </div>
        </div>
    </div>
    <div class="config-block mb-4" v-if="advancedActive">
        <h3 class="block-title mb-3">Advanced Configurations</h3>
        <div class="card mb-3 p-3" v-if="eimActive">
            <div class="row align-items-center">
                <div class="col-sm-3">EIM</div>
                <div class="col-sm-6 config-progress">
                    <div class="progress">
                        <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                            aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" :style="`width: ${eimPercentage}%`">
                            <span class="progress-value">{{ eimPercentage }}%</span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3 config-status text-end" v-if="eimComplete">
                    <span><i class="ri-checkbox-circle-fill"></i> Ready</span>
                </div>
            </div>
        </div>

        <div v-if="absenceOrder == 2">
            <div class="card mb-3 p-3" v-if="absenceActive">
                <div class="row align-items-center">
                    <div class="col-sm-3">Leave</div>
                    <div class="col-sm-6 config-progress">
                        <div class="progress">
                            <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                                aria-valuenow="100" aria-valuemin="0" aria-valuemax="100"
                                :style="`width: ${absencePercentage}%`">
                                <span class="progress-value">{{ absencePercentage }}%</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3 config-status text-end" v-if="absenceComplete">
                        <span><i class="ri-checkbox-circle-fill"></i> Ready</span>
                    </div>
                </div>
            </div>

            <div class="card mb-3 p-3" v-if="attendanceActive">
                <div class="row align-items-center">
                    <div class="col-sm-3">Attendance</div>
                    <div class="col-sm-6 config-progress">
                        <div class="progress">
                            <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                                aria-valuenow="100" aria-valuemin="0" aria-valuemax="100"
                                :style="`width: ${attendancePercentage}%`">
                                <span class="progress-value">{{ attendancePercentage }}%</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3 config-status text-end" v-if="attendanceComplete">
                        <span><i class="ri-checkbox-circle-fill"></i> Ready</span>
                    </div>
                </div>
            </div>
        </div>

        <div v-if="attendanceOrder == 2">
            <div class="card mb-3 p-3" v-if="attendanceActive">
                <div class="row align-items-center">
                    <div class="col-sm-3">Attendance</div>
                    <div class="col-sm-6 config-progress">
                        <div class="progress">
                            <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                                aria-valuenow="100" aria-valuemin="0" aria-valuemax="100"
                                :style="`width: ${attendancePercentage}%`">
                                <span class="progress-value">{{ attendancePercentage }}%</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3 config-status text-end" v-if="attendanceComplete">
                        <span><i class="ri-checkbox-circle-fill"></i> Ready</span>
                    </div>
                </div>
            </div>

            <div class="card mb-3 p-3" v-if="absenceActive">
                <div class="row align-items-center">
                    <div class="col-sm-3">Leave</div>
                    <div class="col-sm-6 config-progress">
                        <div class="progress">
                            <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                                aria-valuenow="100" aria-valuemin="0" aria-valuemax="100"
                                :style="`width: ${absencePercentage}%`">
                                <span class="progress-value">{{ absencePercentage }}%</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3 config-status text-end" v-if="absenceComplete">
                        <span><i class="ri-checkbox-circle-fill"></i> Ready</span>
                    </div>
                </div>
            </div>


        </div>

    </div>


    <div class="row p-4 border-0" v-if="showRedirectSection">
        <div class="col-sm-8 col-12 offset-2">
            <div class="config-message success text-center p-0">
                <div class="message-image mb-4">

                    <img src="../../../images/good-job-picture-001.png" alt="">

                </div>
                <div class="message-content">
                    <div class="message-title">
                        Good Job!
                    </div>
                    <p class="sub-title">All good to get onboard to PeoplesHR</p>
                </div>
                <button @click="redirectToExternal" class="btn btn-info btn-lg">Let's
                    Get
                    Started</button>
            </div>
        </div>
    </div>
</template>

<script>
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';
import { getUserID } from '@/localStorageUtils';
export default (
    {
        data() {
            return {
                showRedirectSection: false,
                activeModuleList: [],
                eimActive: false,
                absenceActive: false,
                attendanceActive: false,
                commonConfigPercentage: 0,
                commonConfigComplete: false,
                subsecApprovalComplete: false,
                advancedActive: false,
                absenceOrder: 0,
                attendanceOrder: 0
            }
        },

        props: {
            eimPercentage: {
                type: Number,
                required: true,
            },

            absencePercentage: {
                type: Number,
                required: true,
            },
            attendancePercentage: {
                type: Number,
                required: true,
            },
            eimComplete: {
                type: Boolean,
                required: true,
            },
            absenceComplete: {
                type: Boolean,
                required: true,
            },
            attendanceComplete: {
                type: Boolean,
                required: true,
            },
        },

        async mounted() {
            await this.$http
                .get(
                    "/GetActiveModules",
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
                    this.activeModuleList = resp.data;
                    this.eimActive = this.activeModuleList.some(x => x.moduleId == 2 && x.order != 0);
                    this.absenceActive = this.activeModuleList.some(x => x.moduleId == 14 && x.order != 0);
                    this.attendanceActive = this.activeModuleList.some(x => x.moduleId == 65 && x.order != 0);
                    this.advancedActive = this.eimActive || this.absenceActive || this.attendanceActive;
                    extendCookieTimeout();
                })
                .catch((error) => {
                    if (error.response.status == 401) {
                        this.emitter.emit('loggedOut', true);
                    }
                    else if (error.response.status == 403) {
                        this.emitter.emit('accessDenied', true);
                    }
                });

            await this.$http
                .get(
                    `/getCommonConfigApprovalStatus?userId=${encodeURIComponent(getUserID())}`,
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
                    if (resp.data == true) {
                        this.commonConfigPercentage = 100;
                        this.commonConfigComplete = true;
                    }
                })
                .catch((error) => {
                    console.log(error.message);
                    if (error.response.status == 401) {
                        this.emitter.emit('loggedOut', true);
                    }
                    else if (error.response.status == 403) {
                        this.emitter.emit('accessDenied', true);
                    }
                    else
                    {
                        this.$router.push({name:'home'})
                    }
                });

            await this.$http
                .get(
                    `/CheckAllSubsectionApproved`,
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
                    if (resp.data == true) {
                        this.subsecApprovalComplete = true;
                    }
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

            await this.$http
                .get(
                    `/getModuleOrdersWithModuleId`,
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
                    this.absenceOrder = resp.data.find(x => x.module_Id == 14).view_Order;
                    this.attendanceOrder = resp.data.find(x => x.module_Id == 65).view_Order;
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

            if (this.commonConfigComplete == true && this.subsecApprovalComplete == true) {
                this.showRedirectSection = true;
            }
        },

        methods: {
            async redirectToExternal() {
                var currentURL = window.location.href;
                const baseURL = currentURL.match(/https?:\/\/(.*?)\./g)[0];
                const regex = /^https?:\/\/|\.+$/g;
                const subdomain = baseURL.replace(regex, '');

                await this.$http.get(`/getClientURL?subdomainName=${encodeURIComponent(subdomain)}`, {
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
                        window.open(response.data, '_blank');
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
    })
</script>