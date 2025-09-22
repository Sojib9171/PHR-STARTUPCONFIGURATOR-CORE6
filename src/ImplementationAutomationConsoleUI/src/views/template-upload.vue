<template>
  <div>
    <sideBar :isDisabled="false" :allObjects="allObjects" :emiObjects="eimObjects" :absenceObjects="absenceObjects"
      :attendanceObjects="attendanceObjects" />
    <div class="col page-content px-0">
      <topBar />
      <div class="page-body">
        <div class="page-title">
          <!--Page title-->
          <h2>Template Upload</h2>
          <!--Breadcrumb-->
          <nav style="--bs-breadcrumb-divider: '|';" aria-label="breadcrumb">
            <ol class="breadcrumb">
              <li class="breadcrumb-item"><a href="#"><i class="ri-home-4-line"></i></a></li>
              <li class="breadcrumb-item active" aria-current="page">Overview</li>
            </ol>
          </nav>
        </div>
        <section class="mt-3">
          <!-- <input type="file" accept=".xlsx,.xlsm" ref="fileInput" @change="handleFileChange">
          <button @click="uploadFile">Upload</button>
          <button @click="downloadFile">Download</button> -->
          <div class="section-body">
            <templateUpload />
          </div>
        </section>
      </div>
    </div>
  </div>
</template>
  
<script>
import SideBar from "../components/navigation/SideBar.vue";
import TopBar from "../components/navigation/TopBar.vue";
import TemplateUpload from "../components/pageElements/TemplateUpload.vue";
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {
  data() {
    return {
      selectedFile: null,
      s: null,
    }
  },

  components: {
    "sideBar": SideBar,
    "topBar": TopBar,
    "templateUpload": TemplateUpload
  },

  methods: {
    handleFileChange() {
      this.selectedFile = this.$refs.fileInput.files[0];
    },

    async downloadFile() {
      await this.$http
        .get(`/DownloadTemplate`, {
          responseType: "base64String",
        }, {
          headers: {
            accept: "*/*",
            Authorization:
              "Basic " +
              Base64.toBase64(
                this.$cookies.get("enc") + ":" + this.$cookies.get("encVal")
              ),
          },
          withCredentials: true,
        })
        .then((resp) => {
          const link = document.createElement("a");
          link.href =
            "data:application/octet-stream;charset=utf-8;base64," + resp.data;
          link.target = "_blank";
          link.download = "test.xlsm";
          link.click();
          extendCookieTimeout();
        })
        .catch((error) => {
          console.log(error.message);
        });
    }
  }
}
</script>