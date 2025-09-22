<template>
    <div>
        <div v-if="isLoading">
            <loader />
        </div>

        <div class="config-block" v-else>
            <div class="p-4 border-0" v-if="this.data.question_type == 'Yes/No'">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="quis-block">
                            <div class="quis-q" v-if="hasInstruction">
                                {{ this.data.question_no }}. {{ firstPart }}
                                <div class="q-intro">{{ secondPart }}</div>
                            </div>
                            <div class="quis-q" v-else-if="hasNote">
                                {{ this.data.question_no }}. {{ firstPart }}
                                <div class="q-intro">{{ secondPart }}</div>
                            </div>
                            <div class="quis-q" v-else>
                                {{ this.data.question_no }}. {{ this.data.question_statement }}
                            </div>
                            <div class="quis-a">
                                <div class="row">
                                    <div class="col-md-11 offset-md-1" v-if="optionsLoaded">
                                        <ul class="list-unstyled prf-options">
                                            <li v-for="option in RadioOptions" :key="option.option_value">
                                                <div class="form-check">
                                                    <label class="form-check-label">
                                                        <input class="form-check-input" type="radio" v-model="selectedRadio"
                                                            :value="option.option_value" />
                                                        <span>{{ option.option_label.split(":")[0].trim() }}</span>
                                                        <span>{{ `: ${option.option_label.split(":")[1].trim()}` }}</span>
                                                    </label>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="text-danger col-md-12" v-if="isEmpty">
                                    Please fill the relevant values
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mt-5">
                    <div class="col-sm-12">
                        <div class="quis-nav pt-5 mt-4">
                            <div class="d-flex justify-content-center w-100">
                                <div><button @click="goBack" class="btn btn-outline-info">BACK</button></div>
                                <div class="position-relative"><button @click="continueProcessForOption"
                                        class="btn btn-info">CONTINUE</button>
                                </div>
                            </div>
                        </div>
                        <div class="quis-progress mt-4">
                            <div class="row">
                                <div class="col-12 text-center quis-count">
                                    {{ data.question_no }} of {{ totalQuestionCount }}
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12 text-center">
                                    <div class="progress-outer">
                                        <div class="progress">
                                            <div class="progress-bar" role="progressbar"
                                                :style="`width: ${completePercentage}%`" aria-valuenow="20"
                                                aria-valuemin="0" aria-valuemax="60"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="p-4 border-0" v-if="this.data.question_type == 'Text'">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="quis-block">
                            <div class="quis-q" v-if="hasInstruction">
                                {{ this.data.question_no }}. {{ firstPart }}
                                <div class="q-intro">{{ secondPart }}</div>
                            </div>
                            <div class="quis-q" v-else>
                                {{ this.data.question_no }}. {{ this.data.question_statement }}
                            </div>
                            <div class="quis-a">
                                <textarea type="text" class="form-control mx-auto" v-model="inputField"
                                    :placeholder="this.data.question_statement"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mt-5">
                    <div class="col-sm-12">
                        <div class="quis-nav pt-5 mt-4">
                            <div class="d-flex justify-content-center w-100">
                                <div><button @click="goBack" class="btn btn-outline-info">BACK</button></div>
                                <div class="position-relative"><button @click="continueProcessForTextAndNumber"
                                        class="btn btn-info">CONTINUE</button>
                                    <!-- <div class="callout-box active align-items-end">
                                    <i class="ri-checkbox-blank-circle-fill icon-blinking"></i>
                                    <span>
                                        Click on this to continue with the wizard
                                    </span>
                                </div> -->
                                </div>
                            </div>
                        </div>
                        <div class="quis-progress mt-4">
                            <div class="row">
                                <div class="col-12 text-center quis-count">
                                    {{ data.question_no }} of {{ totalQuestionCount }}
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12 text-center">
                                    <div class="progress-outer">
                                        <div class="progress">
                                            <div class="progress-bar" role="progressbar"
                                                :style="`width: ${completePercentage}%`" aria-valuenow="10"
                                                aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="p-4 border-0" v-if="this.data.question_type == 'Select Option'">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="quis-block">
                            <div class="quis-q" v-if="hasInstruction">
                                {{ this.data.question_no }}. {{ firstPart }}
                                <div class="q-intro">{{ secondPart }}</div>
                            </div>
                            <div class="quis-q" v-else>
                                {{ this.data.question_no }}. {{ this.data.question_statement }}
                            </div>
                            <div class="quis-a">
                                <div class="row">
                                    <div class="col-md-11 offset-md-1" v-if="optionsLoaded">
                                        <ul class="list-unstyled prf-options">
                                            <li v-for="option in RadioOptions" :key="option.option_value">
                                                <div class="form-check">
                                                    <label class="form-check-label">
                                                        <input class="form-check-input" type="radio" v-model="selectedRadio"
                                                            :value="option.option_value" />
                                                        <span>{{ option.option_label.split(":")[0].trim() }}</span>
                                                        <span>{{ `: ${option.option_label.split(":")[1].trim()}` }}</span>
                                                    </label>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="text-danger w-50 mx-auto" v-if="v$.inputField.$error">
                                        {{ requiredComment }}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mt-5">
                    <div class="col-sm-12">
                        <div class="quis-nav pt-5 mt-4">
                            <div class="d-flex justify-content-center w-100">
                                <div><button @click="goBack" class="btn btn-outline-info">BACK</button></div>
                                <div class="position-relative"><button @click="continueProcessForSelectOption"
                                        class="btn btn-info">CONTINUE</button>
                                    <!-- <div class="callout-box active align-items-end">
                                    <i class="ri-checkbox-blank-circle-fill icon-blinking"></i>
                                    <span>
                                        Click on this to continue with the wizard
                                    </span>
                                </div> -->
                                </div>
                            </div>
                        </div>
                        <div class="quis-progress mt-4">
                            <div class="row">
                                <div class="col-12 text-center quis-count">
                                    {{ data.question_no }} of {{ totalQuestionCount }}
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12 text-center">
                                    <div class="progress-outer">
                                        <div class="progress">
                                            <div class="progress-bar" role="progressbar"
                                                :style="`width: ${completePercentage}%`" aria-valuenow="10"
                                                aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="p-4 border-0" v-if="this.data.question_type == 'Image'">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="quis-block">
                            <div class="quis-q" v-if="hasInstruction">
                                {{ this.data.question_no }}. {{ firstPart }}
                                <div class="q-intro">{{ secondPart }}</div>
                            </div>
                            <div class="quis-q" v-else>
                                {{ this.data.question_no }}. {{ this.data.question_statement }}
                            </div>
                            <div class="quis-a">
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
                                                        <input type="file" accept=".jpg,.png,.jpeg" ref="fileInput"
                                                            @change="handleFileChange" @click="clearFileInput"
                                                            style="display: none;">
                                                    </div>
                                                    <div v-if="selectedFile" class="file-info">
                                                        <p><b>Selected File Name : </b>{{ selectedFile.name }}</p>
                                                    </div>
                                                    <div v-else-if="this.selectedFileName != ''" class="file-info">
                                                        <p><b>Selected File Name : </b>{{ selectedFileName }}</p>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 text-center mt-4">Importing requires .JPG or .PNG format
                                        </div>
                                    </div>
                                    <!-- <div class="row">
                                        <div class="col-12 text-end">
                                            <div class="button-group">
                                                <button class="btn btn-cancel" @click="cancelFile"
                                                    :disabled="isDisabled">Delete</button>
                                                <button class="btn btn-primary" @click="uploadFile"
                                                    style="margin-left: 5px;" :disabled="isDisabled">Upload</button>
                                            </div>
                                        </div>
                                    </div> -->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mt-2">
                    <div class="col-sm-12">
                        <div class="quis-nav pt-5 mt-4">
                            <div class="d-flex justify-content-center w-100">
                                <div><button @click="goBack" class="btn btn-outline-info">BACK</button></div>
                                <div class="position-relative"><button @click="continueProcessForImage"
                                        class="btn btn-info">CONTINUE</button>
                                    <!-- <div class="callout-box active align-items-end">
                                    <i class="ri-checkbox-blank-circle-fill icon-blinking"></i>
                                    <span>
                                        Click on this to continue with the wizard
                                    </span>
                                </div> -->
                                </div>
                            </div>
                        </div>
                        <div class="quis-progress mt-4">
                            <div class="row">
                                <div class="col-12 text-center quis-count">
                                    {{ data.question_no }} of {{ totalQuestionCount }}
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12 text-center">
                                    <div class="progress-outer">
                                        <div class="progress">
                                            <div class="progress-bar" role="progressbar"
                                                :style="`width: ${completePercentage}%`" aria-valuenow="10"
                                                aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import useValidate from '@vuelidate/core'
import { required, maxLength } from '@vuelidate/validators'
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { Base64 } from "js-base64";
import { getCommonConfigRowID } from '@/localStorageUtils'
import { extendCookieTimeout } from '@/cookieUtils'
export default ({
    data() {
        return {
            v$: useValidate(),
            inputField: '',
            width: 10,
            optionSelected: null,
            isLoading: false,
            leaveTypeOptions: [],
            completedPercentage: 0,
            optionsLoaded: false,
            RadioOptions: [],
            isDisabled: false,
            selectedRadio: null,
            selectedFile: null,
            fileSizeExceeded: false,
            imageBase64String: '',
            selectedFileName: null,
            hasInstruction: false,
            firstPart: '',
            secondPart: '',
            logoImageName: '',
            mobileLogoImageName: '',
            hasNote: false
        }
    },

    props: {
        data: {
            type: Object,
            required: true,
        },
        totalQuestionCount: {
            type: Number,
            required: true
        }
    },


    components: {
        "loader": PageLoader,
    },

    validations() {
        return {
            inputField: { required, maxLength: maxLength(300) },
        }
    },

    computed: {
        requiredComment() {
            return this.v$.inputField.$error ? 'Please fill the relevant feilds' : '';
        },
        completePercentage() {
            return ((this.data.question_no - 1) / this.totalQuestionCount) * 100;
        }
    },

    methods: {
        selectOption(option) {
            this.optionSelected = option;
        },


        async continueProcessForTextAndNumber() {
            await this.updateRecord(this.inputField);
        },

        async continueProcessForOption() {
            if (this.selectedRadio == null) {
                this.$emit('next');
                return;
            }
            if (this.selectedRadio == 'Yes') {
                await this.updateRecord(true);
            }
            else if (this.selectedRadio == 'No') {
                await this.updateRecord(false);
            }
        },

        async continueProcessForImage() {
            if (this.selectedFile == null) {
                this.$emit('next');
                return;
            }
            await this.uploadFile();
        },

        async continueProcessForSelectOption() {
            if (this.selectedRadio == null) {
                return;
            }
            if (this.data.question_no == 1) {
                await this.updateRecord(this.selectedRadio);
            }
        },


        async updateRecord(response) {
            this.isLoading = true;
            let recordId = getCommonConfigRowID();
            if (this.data.question_no == 2) {
                this.logoImageName = this.selectedFile.name;
            }
            if (this.data.question_no == 4) {
                this.mobileLogoImageName = this.selectedFile.name;
            }
            await this.$http
                .post(`/updateRecordForCommonConfig`, {
                    QuestionNo: this.data.question_no,
                    RecordId: recordId,
                    ResponseText: response,
                    QuestionType: this.data.question_type,
                    LogoImageName: this.logoImageName,
                    MobileLogoImageName: this.mobileLogoImageName
                }, {
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
                .then(() => {
                    extendCookieTimeout();
                    this.isLoading = false;
                    this.$emit('next');
                })
                .catch((error) => {
                    console.log(error.message);
                    this.showErrorAlert();
                    if (error.response.status == 401) {
                        this.emitter.emit('loggedOut', true);
                    }
                    else if (error.response.status == 403) {
                        this.emitter.emit('accessDenied', true);
                    }
                });
        },

        async loadRadioOptions(questionNumber) {
            this.isLoading = true;
            //var params = {questionNo: questionNumber };
            this.$http.get(`/getRadioOptions?questionNo=${encodeURIComponent(questionNumber)}`, {
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
                .then(response => {
                    this.RadioOptions = response.data;
                    this.optionsLoaded = true;
                    this.isLoading = false;
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
        },

        goBack() {
            this.isLoading = true;
            this.$emit('back');
        },

        showErrorAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'There was a problem. Please try again',
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.isLoading = false;
                }
            })
        },

        handleFileChange() {
            const file = this.$refs.fileInput.files[0];
            this.selectedFile = file;
            const fileSize = file.size;

            const maxSize = 1000000000; // 1GB = 1000000000 bytes
            if (fileSize > maxSize) {
                this.fileSizeExceeded = true;
            } else {
                this.selectedFile = file;
                this.fileSizeExceeded = false;
            }
        },

        clearFileInput(event) {
            event.target.value = null; // This clears the selected file from the input field
        },

        async uploadFile() {
            if (!this.selectedFile.name.endsWith('.jpg') && !this.selectedFile.name.endsWith('.png') && !this.selectedFile.name.endsWith('.jpeg') && !this.selectedFile.name.endsWith('.JPG') && !this.selectedFile.name.endsWith('.JPEG') && !this.selectedFile.name.endsWith('.PNG')) {
                this.selectedFile = null; // Clear the file input element
                this.showNotImageAlert();
                return;
            }
            else if (this.fileSizeExceeded) {
                this.selectedFile = null; // Clear the file input element
                this.showFileSizeExceededAlert();
                return;
            }
            const reader = new FileReader();

            // When the file reading is completed, the result will be available
            reader.onload = () => {
                // Convert the file content to a Base64 string
                const base64String = reader.result.split(',')[1];
                this.imageBase64String = base64String;
                this.updateRecord(this.imageBase64String);
            };

            reader.readAsDataURL(this.selectedFile);

            // if (this.imageBase64String != null) {
            //     this.updateRecord(this.imageBase64String);
            // }
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
        },

        cancelFile() {
            this.selectedFile = null;
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

        showNotImageAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'Please select a .jpg or .png file.',
                timer: 2000,
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            });
        },

    },
    mounted() {
        this.isLoading = false;

        if (this.data.question_type == 'Select Option') {
            this.loadRadioOptions(this.data.question_no);
            this.selectedRadio = this.data.response;
        }

        else if (this.data.question_type == 'Text' || this.data.question_type == 'Numeric') {
            this.inputField = this.data.response;
        }

        else if (this.data.question_type == 'Yes/No') {
            this.loadRadioOptions(this.data.question_no);
            if (this.data.response == 1) {
                this.selectedRadio = 'Yes';
            }
            else if (this.data.response == 0) {
                this.selectedRadio = 'No';
            }
        }

        else if (this.data.question_type == 'Image') {
            if (this.data.question_no == 2) {
                this.selectedFileName = this.data.response;
            }
            else if (this.data.question_no == 4) {
                this.selectedFileName = this.data.response;
            }
        }

        this.hasInstruction = this.data.question_statement.toLowerCase().includes("instruction");
        if (this.hasInstruction) {
            this.firstPart = this.data.question_statement.split("Instruction:")[0].trim();
            this.secondPart = `Instruction: ${this.data.question_statement.split("Instruction:")[1].trim()}`;
        }

        this.hasNote = this.data.question_statement.toLowerCase().includes("note");
        if (this.hasNote) {
            this.firstPart = this.data.question_statement.split("note:")[0].trim();
            this.secondPart = `Note: ${this.data.question_statement.split("Note:")[1].trim()}`;
        }
    }
})
</script>

<style scoped>
.disable {
    pointer-events: none;
    opacity: 0.6;
    /* Optionally reduce the opacity for disabled appearance */
}
</style>
