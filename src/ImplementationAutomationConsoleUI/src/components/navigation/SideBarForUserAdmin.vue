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
                <li @click="this.$router.push({ name: 'configuration-control' })">
                    <button class="w-100 btn btn-toggle d-inline-flex align-items-center rounded border-0 collapsed"
                        data-bs-toggle="collapse" data-bs-target="#dashboard-collapse" aria-expanded="false">
                        <i class="ri-home-5-line icon-line"></i><i class="ri-home-5-fill icon-fill"></i><span>Configuration
                            Control</span>
                    </button>
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
            copyrightText: '',
        }
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