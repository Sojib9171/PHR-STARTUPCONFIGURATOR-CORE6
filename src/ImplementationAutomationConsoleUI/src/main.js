import { createApp } from 'vue'
import App from './App.vue'
import router from "./router";
import 'bootstrap/dist/css/bootstrap.min.css';
import '../node_modules/remixicon/fonts/remixicon.css';
import '../css/styles.css';
import axios from './service';
import VueCookies from 'vue-cookies';
import mitt from 'mitt';
import '../node_modules/bootstrap/dist/js/bootstrap.bundle.min.js';
import '../js/sidebars.js';
import '../js/custom.js'
import Donut from 'vue-css-donut-chart';
import 'vue-css-donut-chart/dist/vcdonut.css';
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';
import { createPinia } from 'pinia';

const emitter = mitt();
const app = createApp(App);
const pinia = createPinia();
app.use(Donut);
app.use(pinia);
app.use(VueSweetalert2);
app.use(router, axios);
app.use(VueCookies, { expires: '1800s' });
app.config.globalProperties.$http = axios;
app.config.globalProperties.$cookies = VueCookies;
app.config.globalProperties.emitter = emitter;
app.config.globalProperties.leaveWizardOptions = [];
app.config.globalProperties.leaveTypes = [];
app.provide('appInstance', app);
app.mount('#app');




