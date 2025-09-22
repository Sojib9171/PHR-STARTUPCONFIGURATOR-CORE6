// import axios from "axios";
// const currentURL = window.location.href;
// const baseURL = currentURL.match(/^(https?:\/\/[^/]+)/)[1];
// axios.defaults.baseURL = `${baseURL}/service/api`;
// export default axios;

import axios from "axios";
axios.defaults.baseURL = `https://localhost:7171/api`;
export default axios;