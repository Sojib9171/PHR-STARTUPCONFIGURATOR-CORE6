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
            <ul class="main-nav list-unstyled ps-0">
                <li>
                    <button class="w-100 btn btn-toggle d-inline-flex align-items-center border-0 collapsed"
                        @click="toggleApprovalCollapse" :aria-expanded="isCollapsedApproval ? 'false' : 'true'">
                        <i class="ri-check-double-fill"></i>
                        <span>Approval</span>
                        <i v-if="isCollapsedApproval" class="ri-arrow-right-s-line icon-right"></i>
                        <i v-if="!isCollapsedApproval" class="ri-arrow-up-s-line icon-right"></i>
                    </button>
                    <div :class="{ 'collapse': isCollapsedApproval, 'show': !isCollapsedApproval }"
                        id="admin-summary-collapse">
                        <ul class="btn-toggle-nav list-unstyled fw-normal pb-1">
                            <li @click="this.$router.push({ name: 'admin-approval-summary' })">
                                <button class="w-100 btn btn-toggle d-inline-flex align-items-center rounded border-0"
                                    aria-expanded="false">
                                    <span>Approval Summary</span>
                                </button>
                            </li>

                            <li @click="this.$router.push({ name: 'admin-approval' })">
                                <button class="w-100 btn btn-toggle d-inline-flex align-items-center rounded border-0"
                                    aria-expanded="false">
                                    <span>For Your Approval</span>
                                    <span class="ap-count">{{ remainingCount }}</span>
                                </button>
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
import 'bootstrap/dist/js/bootstrap.min'
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {
    data: function () {
        return {
            isCollapsedApproval: false,
            copyrightText: '',
        };
    },

    props: {
        remainingCount: {
            type: Number,
            required: true
        }
    },


    methods: {
        toggleApprovalCollapse() {
            this.isCollapsedApproval = !this.isCollapsedApproval;
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
    }
}
</script>
<!-- <style scoped>
.push-right {
    margin-left: 10px;
}
</style> -->