<template>
    <div class="inner-wrapper p-3 mb-5">
        <div class="block-data">
            <h4 class="mb-5">Import data via excel for {{ subsectionName }}</h4>
            <div class="card p-3">
                <div class="row justify-content-center align-items-center pt-3">
                    <div class="col-12 text-center">
                        <div class="drag-drop">
                            <div class="drop-area" v-on:dragover.prevent @drop="handleFileDrop">
                                <p>Drag and drop your file here</p>
                                <div class="mb-3">or</div>
                                <div class="mb-3"><button class="btn btn-outline-info"
                                        @click="this.$refs.fileInput.click();">Select
                                        File</button>
                                    <input type="file" accept=".xlsx,.xlsm" ref="fileInput" @change="handleFileChange"
                                        @click="clearFileInput" style="display: none;">
                                </div>
                                <div v-if="selectedFile" class="file-info">
                                    <p><b>Selected File Name : </b>{{ selectedFile.name }}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 text-center mt-4">Importing requires Microsoft
                        Excel.xlsm format
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 text-end">
                        <div class="button-group">
                            <button class="btn btn-cancel" @click="cancelFile" :disabled="isDisabled">Delete</button>
                            <button class="btn btn-primary" @click="uploadFile" style="margin-left: 5px;"
                                :disabled="isDisabled">Upload</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
//import axios from 'axios';
import { read, utils } from 'xlsx';
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {
    data() {
        return {
            selectedFile: null,
            fileSizeExceeded: false,
            myHiddenValue: null,
            isEmptyRows: false
        }
    },

    props: {
        'subsectionName': String
    },

    methods: {
        async deleteDataFromDependantTables() {
            await this.$http.post(`/deleteDataFromDependentTable`, { subsectionName: this.subsectionName }, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then(() => {
                extendCookieTimeout();
            }).catch(error => {
                this.showErrorAlert(error.response.data);
                if (error.response.status == 401) {
                    this.emitter.emit('loggedOut', true);
                }
                else if (error.response.status == 403) {
                    this.emitter.emit('accessDenied', true);
                }
            });
        },

        handleFileChange() {
            const file = this.$refs.fileInput.files[0];
            const fileSize = file.size;

            const maxSize = 1000000000; // 1GB = 1000000000 bytes
            if (fileSize > maxSize) {
                this.fileSizeExceeded = true;
            } else {
                this.selectedFile = file;
                this.fileSizeExceeded = false;
            }

            const reader = new FileReader();
            reader.onload = (e) => {
                const data = new Uint8Array(e.target.result);
                const workbook = read(data, { type: 'array' });
                const sheetNames = workbook.SheetNames;
                const lastSheetName = sheetNames[sheetNames.length - 1];
                const worksheet = workbook.Sheets[lastSheetName];
                const range = utils.decode_range(worksheet['!ref']);

                let isEmpty = true;

                //check if all the cells from 5th and 6th row are empty
                for (let row = 5; row < 7; row++) {
                    for (let col = range.s.c; col <= range.e.c; col++) {
                        const cellAddress = utils.encode_cell({ r: row, c: col });
                        const cellValue = worksheet[cellAddress] ? worksheet[cellAddress].v : '';

                        if (cellValue !== '') {
                            isEmpty = false;
                            break;
                        }
                    }
                    if (!isEmpty) {
                        break;
                    }
                }
                this.isEmptyRows = isEmpty;
            };
            reader.readAsArrayBuffer(file);
        },

        clearFileInput(event) {
            event.target.value = null; // This clears the selected file from the input field
        },

        showFileSizeExceededAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'Please select a file that is less than 1GB.',
                timer: 2000
            });
        },

        showNotExcelAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'Please select an Excel file.',
                timer: 2000,
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            });
        },

        showUploadErrorAlert(errorMessage) {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: errorMessage,
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$router.go({ name: 'home' });
                }
            })
        },

        showEmptyExcelAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'The excel can not be empty!',
                timer: 2000,
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            })
        },

        async uploadFile() {
            if (this.selectedFile == null || (!this.selectedFile.name.endsWith('.xlsx') && !this.selectedFile.name.endsWith('.xlsm'))) {
                this.selectedFile = null; // Clear the file input element
                this.showNotExcelAlert();
                return;
            }
            else if (this.fileSizeExceeded) {
                this.selectedFile = null; // Clear the file input element
                this.showFileSizeExceededAlert();
                return;
            }
            else if (this.isEmptyRows) {
                this.showEmptyExcelAlert();
                this.selectedFile = null;
                setTimeout(() => {
                    this.$router.go();
                }, 2000);
                return;
            }

            this.$emit('api-response', true);

            const formData = new FormData();
            formData.append('subsectionName', this.subsectionName);
            formData.append('file', this.selectedFile);

            if (this.subsectionName == 'Short Leave' || this.subsectionName == 'Statutory Leave') {
                this.deleteDataFromDependantTables();
                await this.$http.post(`/deleteDataFromDraftAndMainTable`, { subsectionName: this.subsectionName }, {
                    headers: {
                        accept: "*/*",
                        Authorization:
                            "Basic " +
                            Base64.toBase64(
                                this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                            ),
                    },
                    withCredentials: true,
                }).then(() => {
                    extendCookieTimeout();
                }).catch(error => {
                    this.showErrorAlert(error.response.data);
                    if (error.response.status == 401) {
                        this.emitter.emit('loggedOut', true);
                    }
                    else if (error.response.status == 403) {
                        this.emitter.emit('accessDenied', true);
                    }
                });
            }

            await this.$http.post('/uploadData', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                }
            }).then(() => {
                this.$router.push({ name: 'data-overview', params: { subsection: this.subsectionName } });
                extendCookieTimeout();
            }).catch(error => {
                console.log(error);
                this.showUploadErrorAlert(error.response.data);
                if (error.response.status == 401) {
                    this.emitter.emit('loggedOut', true);
                }
                else if (error.response.status == 403) {
                    this.emitter.emit('accessDenied', true);
                }
            });
            this.sub = this.subsectionName;
        },

        handleFileDrop(event) {
            event.preventDefault();
            const file = event.dataTransfer.files[0];
            this.selectedFile = file;
            const fileSize = file.size;

            const maxSize = 1000000000; // 1GB = 1000000000 bytes
            if (fileSize > maxSize) {
                this.fileSizeExceeded = true;
            } else {
                this.selectedFile = file;
                this.fileSizeExceeded = false;
            }

            const reader = new FileReader();
            reader.onload = (e) => {
                const data = new Uint8Array(e.target.result);
                const workbook = read(data, { type: 'array' });
                const sheetNames = workbook.SheetNames;
                const lastSheetName = sheetNames[sheetNames.length - 1];
                const worksheet = workbook.Sheets[lastSheetName];
                const range = utils.decode_range(worksheet['!ref']);

                let isEmpty = true;

                //check if all the cells from 5th and 6th row are empty
                for (let row = 5; row < 7; row++) {
                    for (let col = range.s.c; col <= range.e.c; col++) {
                        const cellAddress = utils.encode_cell({ r: row, c: col });
                        const cellValue = worksheet[cellAddress] ? worksheet[cellAddress].v : '';

                        if (cellValue !== '') {
                            isEmpty = false;
                            break;
                        }
                    }
                    if (!isEmpty) {
                        break;
                    }
                }
                this.isEmptyRows = isEmpty;
            };
            reader.readAsArrayBuffer(file);
        },

        cancelFile() {
            this.selectedFile = null;
        },

        showErrorAlert(errorMessage) {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: errorMessage,
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.isLoading = false;
                }
            })
        },
    },

    computed: {
        isDisabled() {
            return this.selectedFile === null;
        }
    }
}
</script>
