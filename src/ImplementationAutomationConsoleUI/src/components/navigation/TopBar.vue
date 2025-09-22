<template>
    <header class="page-header">
        <div class="row g-0">
            <div class="col-auto col-md-6">
                <div class="d-flex justify-content-start align-items-center">
                    <button class="menu-toggle">
                        <i class="ri-menu-line"></i>
                        <i class="ri-menu-5-line"></i>
                    </button>
                    <span class="welcome-note ps-4">Welcome to PeoplesHR Config Assist</span>
                </div>
            </div>
            <div class="col col-md-6 text-end">
                <div class="top-nav h-100">
                    <ul class="list-unstyled d-flex justify-content-end align-items-center h-100 m-0 px-0">
                        <li>
                            <i class="ri-notification-3-line"></i>
                        </li>
                        <li>
                            <a class="d-flex user-profile">
                                <span><i class="ri-user-3-line"></i></span> <span class="ms-1">{{ userName }}</span>
                            </a>
                        </li>
                        <li>
                            <logout-comp />
                        </li>
                        <li hidden>
                            <access-denied-comp />
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </header>
</template>

<script>
import logoutComp from "../auth/logout-comp.vue";
import accessDeniedComp from "../auth/access-denied-comp.vue";
import $ from 'jquery';
//import store from '../../store'
//const userName = store.getters.getUsername;

export default {
    data() {
        return {
            userName: this.$cookies.get("userName")
        };
    },

    components: {
        "logout-comp": logoutComp,
        "access-denied-comp":accessDeniedComp
    },

    mounted() {
        $(document).ready(function () {
            // minimize menu
            $('.menu-toggle, .sidebar-close').on('click', function (e) {
                $(this).toggleClass("sidebar-hidden");
                $(this.parentNode).toggleClass("sidebar-hidden");
                $('body').toggleClass("sidebar-hidden");
                e.stopPropagation()
            });
        });
    },
};
</script>
