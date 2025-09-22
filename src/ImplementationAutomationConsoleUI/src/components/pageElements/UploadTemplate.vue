<template>
    <div class="inner-wrapper p-3 mb-5">
        <div class="block-data">
            <h4 class="mb-5">Upload template via excel</h4>
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
                                        style="display: none;">
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
import { read, utils } from 'xlsx';
import { Base64 } from "js-base64";
// import { useMyStore } from '../../store';
var columnNames = [];
export default {
    data() {
        return {
            selectedFile: null,
            templateID: '',
            moduleID: null,
            subsectionID: '',
            fileSizeExceeded: false,
        }
    },

    props: {
        'subsectionName': String
    },

    methods: {
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
                timer: 2000
            });
        },

        showUploadErrorAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'There was a problem uploading the excel. Please check if the template is appropriate',
                timer: 5000
            });
        },

        showUploadSuccessAlert() {
            this.$swal.fire({
                icon: 'success',
                width: 300,
                height: 200,
                text: 'Template Sucessfully Uploaded',
                timer: 5000
            });
        },

        handleFileChange() {
            const file = this.$refs.fileInput.files[0];
            const fileSize = file.size;
            const maxSize = 1000000000; // 1GB = 1000000000 bytes
            if (fileSize > maxSize) {
                this.showFileSizeExceededAlert();
                return;
            } else {
                this.selectedFile = file;
                this.fileSizeExceeded = false;
                const reader = new FileReader();
                reader.onload = (e) => {
                    const data = new Uint8Array(e.target.result);
                    const workbook = read(data, { type: 'array' });
                    const sheetNames = workbook.SheetNames;
                    const lastSheetName = sheetNames[sheetNames.length - 1];
                    const worksheet = workbook.Sheets[lastSheetName];
                    const range = utils.decode_range(worksheet['!ref']);
                    columnNames.splice(0, columnNames.length)
                    for (let row = 2; row < 3; row++) {
                        for (let col = range.s.c; col <= range.e.c; col++) {

                            const cellAddress = utils.encode_cell({ r: row, c: col });
                            const cellValue = worksheet[cellAddress] ? worksheet[cellAddress].v : '';
                            if (cellValue != '') {
                                columnNames.push(cellValue);
                            }
                        }
                    }
                };
                reader.readAsArrayBuffer(file);
            }
            // const store = useMyStore();
            // store.setData(columnNames);
            // this.$router.push({name:'template-upload-column-mapping', params:{subsection:this.subsectionName}});
        },

        uploadFile() {
            if (this.selectedFile == null || !this.selectedFile.name.endsWith('.xlsm')) {
                this.cancelFile();// Clear the file input element
                this.showNotExcelAlert();
                return;
            }

            if (this.fileSizeExceeded) {
                this.cancelFile(); // Clear the file input element
                this.showFileSizeExceededAlert();
                return;
            }

            if (this.subsectionName == 'Employee Data') {
                this.templateID = 'temp1';
                this.moduleID = 2;
                this.subsectionID = '1';
            }

            if (this.subsectionName == 'Bank Details') {
                this.templateID = 'temp2';
                this.moduleID = 2;
                this.subsectionID = '2';
            }

            if (this.subsectionName == 'Reporting Hierarchy') {
                this.templateID = 'temp3';
                this.moduleID = 2;
                this.subsectionID = '3';
            }

            if (this.subsectionName == 'Dependent Information') {
                this.templateID = 'temp4';
                this.moduleID = 2;
                this.subsectionID = '4';
            }

            if (this.subsectionName == 'Emergency Contact') {
                this.templateID = 'temp5';
                this.moduleID = 2;
                this.subsectionID = '5';
            }

            if (this.subsectionName == 'Statutory Leave') {
                this.templateID = 'temp6';
                this.moduleID = 14;
                this.subsectionID = '6';
            }


            if (this.subsectionName == 'Short Leave') {
                this.templateID = 'temp7';
                this.moduleID = 14;
                this.subsectionID = '7';
            }

            if (this.subsectionName == 'Shift Information') {
                this.templateID = 'temp8';
                this.moduleID = 65;
                this.subsectionID = '8';
            }

            if (this.subsectionName == 'Roster Information') {
                this.templateID = 'temp9';
                this.moduleID = 65;
                this.subsectionID = '9';
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
                this.showUploadSuccessAlert();
                this.cancelFile();
            }).catch(error => {
                if (error.response.status == 401) {
                    this.emitter.emit('loggedOut', true);
                }
                else if (error.response.status == 403) {
                    this.emitter.emit('accessDenied', true);
                }
                this.cancelFile();
                this.showUploadErrorAlert();
            });
        },

        handleFileDrop(event) {
            event.preventDefault();
            const file = this.$refs.fileInput.files[0];
            const fileSize = file.size;

            const maxSize = 1000000000; // 1GB = 1000000000 bytes
            if (fileSize < maxSize) {
                this.showFileSizeExceededAlert();
                return;
            } else {
                this.selectedFile = file;
                this.fileSizeExceeded = false;
            }
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