<template>
    <div class="col sidebar-left">
        <div class="sidebar-close mobile-only">
            <span class="menu-btn d-inline-block">
                <i class="ri-arrow-left-s-line icon-right"></i>
            </span>
        </div>
        <div class="site-branding d-flex align-items-center justify-content-between">
            <div class="logo"><img class="logo" src="../../../images/logo.png" alt="Logo"></div>
        </div>
        <h4>MAIN MENU</h4>
        <div class="nav-block">
            <ul class="main-nav list-unstyled ps-0" :class="{ 'disable': isDisabled }">
                <li @click="this.$router.push({ name: 'home' })">
                    <button class="w-100 btn btn-toggle d-inline-flex align-items-center rounded border-0 collapsed"
                        data-bs-toggle="collapse" data-bs-target="#dashboard-collapse" aria-expanded="false">
                        <i class="ri-home-5-line icon-line"></i><i class="ri-home-5-fill icon-fill"></i><span>Home</span>
                    </button>
                </li>

                <li @click="this.$router.push({ name: 'basic-configuration-1' })">
                    <button class="w-100 btn btn-toggle d-inline-flex align-items-center rounded border-0 collapsed"
                        data-bs-toggle="collapse" data-bs-target="" aria-expanded="false">
                        <i class="ri-settings-line icon-line"></i><i class="ri-settings-fill icon-fill"></i><span>Basic
                            Configuration</span>
                    </button>
                </li>

                <li v-if="isConfigApproved">
                    <button class="w-100 btn btn-toggle d-inline-flex align-items-center rounded border-0 collapsed"
                        @click="toggleAdvanceCollapse" :aria-expanded="isCollapsedAdvance ? 'false' : 'true'">
                        <i class="ri-settings-5-line icon-line"></i><i
                            class="ri-settings-5-fill icon-fill"></i><span>Advanced Configuration</span>
                        <i v-if="isCollapsedAdvance" class="ri-arrow-right-s-line icon-right"></i>
                        <i v-if="!isCollapsedAdvance" class="ri-arrow-up-s-line icon-right"></i>
                    </button>
                    <div :class="{ 'collapse-transition': true, 'collapse': isCollapsedAdvance, 'show': !isCollapsedAdvance }"
                        id="advance-collapse">
                        <ul class="btn-toggle-nav list-unstyled fw-normal pb-1">
                            <li v-for="item in modules" :key="item.moduleId" :class="{ 'disable': item.is_enable != true }">
                                <button class="w-100 btn btn-toggle d-inline-flex align-items-center border-0 collapsed"
                                    @click="toggleCollapse(item.moduleName)"
                                    :aria-expanded="isSectionExpanded(item.moduleName) ? 'false' : 'true'">
                                    <i class="ri-user-3-line icon-line"></i><i class="ri-user-3-fill icon-fill"></i>
                                    <span
                                        @click="this.$router.push({ name: 'dashboard', params: { moduleName: item.moduleName } })">{{
                                            item.moduleName }}</span>
                                    <i v-if="isSectionExpanded(item.moduleName)"
                                        class="ri-arrow-right-s-line icon-right"></i>
                                    <i v-if="!isSectionExpanded(item.moduleName)" class="ri-arrow-up-s-line icon-right"></i>
                                </button>
                                <div
                                    :class="{ 'collapse': isSectionExpanded(item.moduleName), 'show': !isSectionExpanded(item.moduleName) }">
                                    <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                                        <li v-for="subItem in filteredSubItems(item.moduleId)" :key="subItem.subsection_Id"
                                            :class="{ 'disable': subItem.enabled != true }"><a
                                                @click="goToSubsec(subItem.subsection_Name)"
                                                class="link-dark d-inline-flex text-decoration-none clickable-link">{{
                                                    subItem.subsection_Name
                                                }}</a>
                                        </li>
                                    </ul>
                                </div>
                            </li>
                        </ul>
                    </div>
                </li>
            </ul>
        </div>

        <div class="nav-footer-links">
            <ul class="nav-copy list-unstyled ps-3">
                <li>
                    <div>&copy; {{ copyrightText }}</div>
                </li>
            </ul>
        </div>

    </div>
</template>
<script>
import '../../../node_modules/bootstrap/dist/css/bootstrap.min.css'
import '../../../node_modules/bootstrap/dist/js/bootstrap.min.js'
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';
import { saveSubsecNameForMultipleUpload, saveSubsecNameForWizardPopup, getSubsecNameForWizardPopup } from '@/localStorageUtils';

export default {
    data: function () {
        return {
            itemDisabled: true,
            remainingApprovalCount: 0,
            disableAbsence: false,
            isCollapsedAdvance: true,
            isCollapsedEIM: true,
            isCollapsedAbsence: true,
            eimComplete: null,
            activeModelIdList: [],
            eimActive: false,
            absenceActive: false,
            copyrightText: '',
            disableAttendance: false,
            isCollapsedAttendance: true,
            attendanceActive: false,
            modules: [],
            collapsedSections: {},
            isConfigApproved: false,
            isSubsecApproved: false,
            showUpload: false,
            componentKey: 0,
            showModal: false,
            tempSubsec: ''
        };
    },

    props: {
        isDisabled: {
            type: Boolean,
            required: true
        },
        allObjects: {
            type: Array,
            required: true
        },
        emiObjects: {
            type: Array,
            required: true
        },
        absenceObjects: {
            type: Array,
            required: true
        },
        attendanceObjects: {
            type: Array,
            required: true
        },
        remainingCount: {
            type: Number,
            required: true
        }
    },

    methods: {
        toggleCollapse(sectionName) {
            this.collapsedSections[sectionName] = !this.collapsedSections[sectionName];
        },

        isSectionExpanded(sectionName) {
            return this.collapsedSections[sectionName] ? false : true;
        },

        filteredSubItems(moduleId) {
            return this.allObjects.filter(subItem => subItem.module_Id === moduleId);
        },

        toggleAdvanceCollapse() {
            this.isCollapsedAdvance = !this.isCollapsedAdvance;
        },
        toggleEimCollapse() {
            this.isCollapsedEIM = !this.isCollapsedEIM;
        },
        toggleAbsenceCollapse() {
            this.isCollapsedAbsence = !this.isCollapsedAbsence;
        },
        toggleAttendanceCollapse() {
            this.isCollapsedAttendance = !this.isCollapsedAttendance;
        },

        async setSubsecName(subsecName) {
            this.tempSubsec = subsecName;
        },

        async goToSubsec(subsectionName) {
            saveSubsecNameForWizardPopup(subsectionName);
            this.showModal = false;
            await this.setSubsecName(subsectionName);
            await this.$http
                .get(
                    `/checkIfSubsectionApproved?subsectionName=${encodeURIComponent(subsectionName)}`,
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
                    if (resp.data != true) {
                        if (subsectionName == 'Statutory Leave' || subsectionName == 'Short Leave') {
                            this.$router.push({ name: 'wizard-popup', params: { subsection: getSubsecNameForWizardPopup() } });
                        }
                        else
                            this.$router.push({ name: 'upload', params: { subsection: subsectionName } });
                    }
                    else {
                        saveSubsecNameForMultipleUpload(this.tempSubsec);
                        this.$router.push({ name: 'multiple-upload-popup', params: { subsection: subsectionName } });
                    }
                    this.componentKey++;
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

    async mounted() {
        await this.$http
            .get(
                "/GetCopyRightText",
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
                this.copyrightText = resp.data;
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

        await this.$http
            .get(
                "/GetActiveModulesFromDashboard",
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
                this.modules = resp.data.filter(obj => obj.order != 0);
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

        await this.$http
            .get(
                `/getAdvanceConfigActiveStatusForSidebar`,
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
                this.isConfigApproved = resp.data;
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
    }
}
</script>
<style scoped>
.disable {
    pointer-events: none;
    opacity: 0.6;
    /* Optionally reduce the opacity for disabled appearance */
}

.clickable-link {
    cursor: pointer;
    /* Set the cursor style to a hand icon */
}

.modal-content {
    margin: 2px auto;
    z-index: 1100 !important;
}
</style>