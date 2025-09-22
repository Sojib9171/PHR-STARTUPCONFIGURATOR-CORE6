<template>
    <div class="inner-wrapper p-3 mb-5">
        <div class="block-data">
            <h4 class="mb-5">Upload template via excel</h4>
            <div class="card p-3">
                <div class="row justify-content-center align-items-center pt-3">
                    <div class="col-12 text-center">
                        <div style="margin-bottom: 10px;">
                            <div style="margin-bottom: 5px;">
                                <label for="template_id">Template ID : </label>
                                <input type="text" id="template_id" v-model="templateID" style="margin-left: 4px;" />
                            </div>
                            <div style="margin-bottom: 5px;">
                                <label for="template_id">Module ID : </label>
                                <input type="number" id="module_id" v-model="moduleID" style="margin-left: 4px;" />
                            </div>
                            <div style="margin-bottom: 5px;">
                                <label for="template_id">Subsection ID : </label>
                                <input type="text" id="subsection_id" v-model="subsectionID" style="margin-left: 4px;" />
                            </div>
                        </div>

                        <div class="drag-drop">
                            <div class="drop-area" v-on:dragover.prevent v-on:drop="handleFileDrop">
                                <p>Drag and drop your file here</p>
                            </div>
                        </div>
                        <div class="mb-3">or</div>
                        <div class="mb-3"><button class="btn btn-outline-info" @click="this.$refs.fileInput.click();">Select
                                File</button>
                            <input type="file" accept=".xlsx,.xlsm" ref="fileInput" @change="handleFileChange"
                                style="display: none;">
                        </div>
                        <div v-if="selectedFile" class="file-info">
                            <p><b>Selected File Name : </b>{{ selectedFile.name }}</p>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 text-center mt-4">Importing requires Microsoft
                        Excel.xlsx format
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 text-end">
                        <div class="button-group">
                            <button class="btn btn-cancel" @click="cancelFile">Cancel</button>
                            <button class="btn btn-primary" @click="uploadFile" style="margin-left: 5px;">Import</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
//import axios from 'axios';
import { Base64 } from "js-base64";
export default {
    data() {
        return {
            selectedFile: null,
            templateID: '',
            moduleID: null,
            subsectionID: '',
        }
    },

    methods: {
        handleFileChange() {
            this.selectedFile = this.$refs.fileInput.files[0];
            const file = this.$refs.fileInput.files[0];

            const reader = new FileReader();
            reader.onload = (e) => {
                const data = new Uint8Array(e.target.result);
                const workbook = read(data, { type: 'array' });
                const sheetNames = workbook.SheetNames;
                const lastSheetName = sheetNames[sheetNames.length - 1];
                const worksheet = workbook.Sheets[lastSheetName];
                const range = utils.decode_range(worksheet['!ref']);

                //check if all the cells from 5th and 6th row are empty
                for (let row = 3; row < 4; row++) {
                    for (let col = range.s.c; col <= range.e.c; col++) {
                        const cellAddress = utils.encode_cell({ r: row, c: col });
                        const cellValue = worksheet[cellAddress] ? worksheet[cellAddress].v : '';
                    }
                }
            };
            reader.readAsArrayBuffer(file);
        },

        uploadFile() {
            if (this.selectedFile == null || (!this.selectedFile.name.endsWith('.xlsx') && !this.selectedFile.name.endsWith('.xlsm'))) {
                this.$refs.fileInput.value = ''; // Clear the file input element
                return alert('Please select an Excel file');
            }
            const formData = new FormData();
            formData.append('file', this.selectedFile);
            formData.append('template_id', this.templateID);
            formData.append('module_id', this.moduleID);
            formData.append('subsection_id', this.subsectionID);

            this.$http.post('/UploadTemplate', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then(() => {
                this.cancelFile();
            }).catch(error => {
                if (error.response.status == 401) {
                    this.emitter.emit('loggedOut', true);
                }
                else if (error.response.status == 403) {
                    this.emitter.emit('accessDenied', true);
                }
            });
        },

        handleFileDrop(event) {
            event.preventDefault();
            const file = event.dataTransfer.files[0];
            this.selectedFile = file;
        },

        cancelFile() {
            this.selectedFile = null;
            this.templateID = '';
            this.moduleID = null;
            this.subsectionID = ''
        }
    }
}
</script>