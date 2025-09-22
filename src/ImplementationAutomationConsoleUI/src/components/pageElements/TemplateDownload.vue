<template>
    <div class="row">
        <div class="col-4">
            <div class="label"> Download Data Sheet Template<br>
                <span class="text-gray">Microsoft Excel.xlsm format</span>
            </div>
        </div>
        <div class="col-8">
            <button class="btn btn-outline-primary" @click="downloadFile"><i class="ri-download-cloud-2-fill"></i>
                Download Template</button>
        </div>
    </div>
</template>
<script>
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils'
export default {
    props: {
        'subsectionName': String
    },
    methods: {
        async downloadFile() {
            await this.$http
                .get(`/DownloadTemplate?subsectionName=${encodeURIComponent(this.subsectionName)}`, {
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
                .then((resp) => {
                    var templateData = resp.data;

                    const link = document.createElement("a");
                    link.href =
                        "data:application/octet-stream;charset=utf-8;base64," + templateData[0];
                    link.target = "_blank";
                    link.download = templateData[1];
                    link.click();
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
    },
}
</script>
