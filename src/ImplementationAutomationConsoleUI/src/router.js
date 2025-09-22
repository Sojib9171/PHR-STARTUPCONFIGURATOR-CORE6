import { createApp } from 'vue'
import App from './App.vue'
import VueCookies from 'vue-cookies';
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {
        path: '/',
        name: 'login',
        component: () => import(/* webpackChunkName: "login" */ './views/login-page.vue')
    },
    {
        path: '/dashboard/:moduleName',
        name: 'dashboard',
        component: () => import(/* webpackChunkName: "dashboard" */ './views/dashboard-page.vue'),
        key: (to) => String(to.params.moduleName),
    },
    {
        path: '/home',
        name: 'home',
        component: () => import(/* webpackChunkName: "home" */ './views/home-page.vue'),
    },
    {
        path: '/admin-approval-summary',
        name: 'admin-approval-summary',
        component: () => import(/* webpackChunkName: "admin-approval-summary" */ './views/admin-approval-summary-page.vue')
    },
    {
        path: '/admin-approval',
        name: 'admin-approval',
        component: () => import(/* webpackChunkName: "admin-pending-approval" */ './views/admin-pending-approval-page.vue')
    },
    {
        path: '/template-upload',
        name: 'template-upload-summary',
        component: () => import(/* webpackChunkName: "template-configuration" */ './views/template-configuration-page.vue')
    },
    {
        path: '/upload/:subsection',
        name: 'upload',
        component: () => import(/* webpackChunkName: "excel-upload" */ './views/excel-upload-page.vue'),
        props: true
    },
    {
        path: '/template-upload/:subsection',
        name: 'template-upload',
        component: () => import(/* webpackChunkName: "template-upload" */ './views/template-upload-page.vue'),
        props: true
    },
    {
        path: '/data-overview/:subsection',
        name: 'data-overview',
        component: () => import(/* webpackChunkName: "data-overview" */ './views/data-overview-page.vue')
    },
    {
        path: '/data-commit/:subsection',
        name: 'data-commit',
        component: () => import(/* webpackChunkName: "data-commit" */ './views/data-commit-page.vue')
    },
    {
        path: '/admin-commit/:subsection/:record_Id',
        name: 'admin-commit',
        component: () => import(/* webpackChunkName: "admin-approval-commit" */ './views/admin-approval-commit-page.vue')
    },
    {
        path: '/basic-configuration',
        name: 'basic-configuration-1',
        component: () => import(/* webpackChunkName: "basic-configuration" */ './views/basic-configuration-page.vue')
    },
    {
        path: '/configuration-control',
        name: 'configuration-control',
        component: () => import(/* webpackChunkName: "configuration-control" */ './views/configuration-control-page.vue')
    },
    {
        path: '/configuration-control-selection',
        name: 'configuration-control-selection',
        component: () => import(/* webpackChunkName: "configuration-control-selection" */ './views/configuration-control-selection-page.vue')
    },
    {
        path: '/configuration-control-ranking',
        name: 'configuration-control-ranking',
        component: () => import(/* webpackChunkName: "configuration-control-ranking" */ './views/configuration-control-ranking-page.vue')
    },
    {
        path: '/advance-configuration',
        name: 'advance-configuration-1',
        component: () => import(/* webpackChunkName: "advance-configuration" */ './views/advance-configuration-page.vue')
    },
    {
        path: '/template-upload-column-mapping/:subsection',
        name: 'template-upload-column-mapping',
        component: () => import(/* webpackChunkName: "template-upload-column-mapping" */ './views/template-upload-column-map.vue')
    },
    {
        path: '/absence-wizard/:subsection',
        name: 'absence-wizard',
        component: () => import(/* webpackChunkName: "absence-wizard" */ './views/absence-wizard-page.vue')
    },
    {
        path: '/absence-wizard-selected-options/:subsection',
        name: 'absence-wizard-selected-options',
        component: () => import(/* webpackChunkName: "absence-wizard-selected-options" */ './views/absence-wizard-Selected-Options-page.vue')
    },
    {
        path: '/absence-wizard-options/:subsection/:id',
        name: 'absence-wizard-options',
        component: () => import(/* webpackChunkName: "absence-wizard-Final-Selected-Options" */ './views/absence-wizard-Final-Selected-Options-page.vue')
    },
    {
        path: '/leave-type-created-message/:subsection',
        name: 'leave-type-created-message',
        component: () => import(/* webpackChunkName: "leave-type-created-message" */ './views/leave-type-created-message-page.vue')
    },
    {
        path: '/leave-type-summary/:subsection',
        name: 'leave-type-summary',
        component: () => import(/* webpackChunkName: "absence-wizard-leave-type-summary" */ './views/absence-wizard-leave-type-summary-table.vue')
    },
    {
        path: '/absence-wizard-edit/:subsection',
        name: 'absence-wizard-edit',
        component: () => import(/* webpackChunkName: "absence-wizard-edit" */ './views/absence-wizard-edit-page.vue')
    },
    {
        path: '/absence-wizard-edit-leave-type/:subsection/:id',
        name: 'absence-wizard-edit-leave-type',
        component: () => import(/* webpackChunkName: "absence-wizard-edit-leave" */ './views/absence-wizard-edit-leave-page.vue')
    },
    {
        path: '/absence-wizard-draft-edit/:subsection',
        name: 'absence-wizard-draft-edit',
        component: () => import(/* webpackChunkName: "absence-wizard-draft-edit" */ './views/absence-wizard-draft-edit-page.vue')
    },
    {
        path: '/absence-wizard-Edited-options/:subsection',
        name: 'absence-wizard-edited-options',
        component: () => import(/* webpackChunkName: "absence-wizard-edited-options" */ './views/absence-wizard-Edited-Options-page.vue')
    },
    {
        path: '/common-configuration-summary',
        name: 'common-config-summary',
        component: () => import(/* webpackChunkName: "common-config-summary" */ './views/common-config-summary-page.vue')
    },
    {
        path: '/common-configuration-edit',
        name: 'common-config-edit',
        component: () => import(/* webpackChunkName: "common-config-edit" */ './views/common-config-edit-page.vue')
    },
    {
        path: '/wizard-popup/:subsection',
        name: 'wizard-popup',
        component: () => import(/* webpackChunkName: "wizard-popup" */ './views/wizard-popup-page.vue')
    },
    {
        path: '/common-configuration',
        name: 'common-config',
        component: () => import(/* webpackChunkName: "common-config" */ './views/common-config-wizard-page.vue')
    },
    {
        path: '/common-configuration-saved',
        name: 'common-config-saved',
        component: () => import(/* webpackChunkName: "common-config-saved" */ './views/common-config-saved-message-page.vue')
    },
    {
        path: '/common-configuration-commit',
        name: 'common-config-commit',
        component: () => import(/* webpackChunkName: "common-config-commit" */ './views/common-config-commit-page.vue')
    },
    {
        path: '/configuration-control-commit',
        name: 'configuration-control-commit',
        component: () => import(/* webpackChunkName: "configuration-control-commit" */ './views/configuration-control-commit-page.vue')
    },
    {
        path: '/common-configuration-draft',
        name: 'common-config-draft',
        component: () => import(/* webpackChunkName: "common-config-draft-edit" */ './views/common-config-draft-edit-page.vue')
    },
    // {
    //     path: '/test-view',
    //     name: 'test-view',
    //     component: () => import(/* webpackChunkName: "test-view" */ './views/test-view-page.vue')
    // },
    {
        path: '/basic-config-popup',
        name: 'basic-config-popup',
        component: () => import(/* webpackChunkName: "basic-config-popup" */ './views/basic-config-popup-page.vue')
    },
    {
        path: '/multiple-upload-popup',
        name: 'multiple-upload-popup',
        component: () => import(/* webpackChunkName: "multiple-upload-popup" */ './views/multiple-upload-popup-page.vue')
    },
    {
        path: '/access-denied',
        name: 'access-denied',
        component: () => import(/* webpackChunkName: "access-denied" */ './views/access-denied-page.vue')
    },
    {   path: '/:pathMatch(.*)*', 
        name: 'page-not-found', 
        component: () => import(/* webpackChunkName: "-denied" */ './views/not-found-page.vue')
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

const app = createApp(App);
app.use(VueSweetalert2);


router.beforeEach((to, from, next) => {
    if (to.name !== 'login' && (VueCookies.get('cookie') == null || VueCookies.get('enc') == null || VueCookies.get('encVal') == null)) {
        app.config.globalProperties.$swal.fire({
            position: 'top',
            icon: 'error',
            width: 400,
            height: 100,
            text: 'Your Session Has Expired',
            confirmButtonText: 'Ok',
            allowOutsideClick: false,
        }).then((result) => {
            if (result.isConfirmed) {
                next({ name: 'login' });
            }
        })

    } else {
        next();
    }
});

app.use(router).mount('#app')

export default router