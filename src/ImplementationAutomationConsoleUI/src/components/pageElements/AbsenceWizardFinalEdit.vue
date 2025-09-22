<template>
    <div>
        <div v-if="isLoading == true">
            <loader />
        </div>

        <div class="config-block">
            <div class="p-4 border-0" v-if="this.data.question_type == 'Yes/No'">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="quis-block">
                            <div class="quis-q">
                                {{ this.data.question_no }}. {{ this.data.question_statement }}
                            </div>
                            <div class="quis-a" :class="{ 'disable': this.isDisabled == true }">
                                <div class="d-flex justify-content-center w-100">
                                    <div class="position-relative"><button class="btn btn-outline-secondary"
                                            :class="{ active: optionSelected === false }"
                                            @click="selectOption(false)">No</button>
                                    </div>
                                    <div class="position-relative"><button class="btn btn-outline-info"
                                            :class="{ active: optionSelected === true }"
                                            @click="selectOption(true)">Yes</button></div>
                                </div>
                            </div>
                            <div class="quis-a">
                                <div class="text-danger w-50 mx-auto" v-if="hasToSelectAnOption">
                                    Please select an option
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
                                    <!-- <div class="callout-box active ali">
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
                            <div class="quis-q">
                                {{ this.data.question_no }}. {{ this.data.question_statement }}
                            </div>
                            <div class="quis-a">
                                <input type="text" class="form-control w-50 mx-auto" v-model="inputField"
                                    :placeholder="this.data.question_statement">
                                <div class="text-danger w-50 mx-auto" v-if="v$.inputField.$error">
                                    {{ requiredComment }}
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="exceedFieldLimit">
                                    Maximum Allowed Length is {{ fieldMaxLength }} characters
                                </div>
                                <!-- <div class="text-danger w-50 mx-auto" v-if="hasDuplicateLeaveType">
                                    You have already create this leave type
                                </div> -->
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

            <div class="p-4 border-0" v-if="this.data.question_type == 'Drop down-Text'">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="quis-block">
                            <div class="quis-q">
                                {{ this.data.question_no }}. {{ this.data.question_statement }}
                            </div>
                            <div class="quis-a">
                                <div class="row">
                                    <div class="col-md-6 offset-md-3" v-if="optionsLoaded">
                                        <select class="form-select" v-model="inputField">
                                            <option value="" disabled selected hidden>{{ data.response }}</option>
                                            <option v-for="option in dropdownOptions" :key="option">{{ option }}</option>
                                        </select>
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
                                <div class="position-relative"><button @click="continueProcessForDropDown"
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

            <div class="p-4 border-0" v-if="this.data.question_type == 'Numeric'">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="quis-block">
                            <div class="quis-q">
                                {{ this.data.question_no }}. {{ this.data.question_statement }}
                            </div>
                            <div class="quis-a">
                                <input type="number" class="form-control w-50 mx-auto" v-model="inputField"
                                    :placeholder="this.data.question_statement" :disabled="isDisabled"
                                    @input="restrictToInteger">
                                <div class="text-danger w-50 mx-auto" v-if="v$.inputField.$error">
                                    {{ requiredComment }}
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="exceedFieldLimit">
                                    Maximum Allowed Length is {{ fieldMaxLength }} digits
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasZeroValue">
                                    Value can not be equal or less than 0
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasToBeBiggerValue">
                                    Value has to be bigger than minimum value
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasToBeRangeValue">
                                    Value has to be between 1 and 12
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasToBeNonNullValue">
                                    Value can not be null or 0
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasDuplicateApplySequence">
                                    Value can not be duplicated
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasDayRangeValue">
                                    Value has to be between 0 and 23
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasNegativeValue">
                                    Value has to be equal or greater than 0
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasValueGreaterThanMaximum">
                                    Value has to be less than Maximum Hours
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasValueLessThanMinimum">
                                    Value has to be greater than Minimum Hours
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasValueLessThanMaximum">
                                    Value has to be greater than the Maximum Hours
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasToBeValidDayValue">
                                    Value has to be between 1 and 365
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasToBeValidMonthValue">
                                    Value has to be between 1 and 12
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasToBeTwoDigitsValue">
                                    Value has to be between 0 and 99
                                </div>
                                <div class="text-danger w-50 mx-auto" v-if="hasToBeThreeDigitsValue">
                                    Value has to be between 0 and 999
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
        </div>
    </div>
</template>

<script>
import useValidate from '@vuelidate/core'
import { Base64 } from "js-base64";
import { required, maxLength } from '@vuelidate/validators'
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { getUserID } from '@/localStorageUtils';
import { extendCookieTimeout } from '@/cookieUtils';
export default ({
    data() {
        return {
            v$: useValidate(),
            inputField: '',
            width: 10,
            optionSelected: '',
            isLoading: false,
            dropDownSelectedOption: '',
            leaveTypeOptions: [],
            optionsLoaded: false,
            exceedFieldLimit: false,
            fieldMaxLength: 0,
            hasToBeNonNullValue: false,
            hasToBeRangeValue: false,
            hasToBeBiggerValue: false,
            hasZeroValue: false,
            hasDuplicateApplySequence: false,
            hasDayRangeValue: false,
            hasNegativeValue: false,
            hasValueGreaterThanMaximum: false,
            hasValueLessThanMinimum: false,
            isDisabled: false,
            previousColumnNotNull: false,
            prevColNotNullForStat: false,
            hasToSelectAnOption: false,
            hasValueLessThanMaximum: false,
            hasToBeValidDayValue: false,
            hasToBeValidMonthValue: false,
            hasToBeTwoDigitsValue: false,
            hasToBeThreeDigitsValue:false
        }
    },

    components: {
        "loader": PageLoader,
    },

    props: {
        data: {
            type: Object,
            required: true,
        },
        totalQuestionCount: {
            type: Number,
            required: true
        },
        subsectionName: {
            type: String,
            required: true
        },
        recordId: {
            type: String,
            required: true
        }
    },

    validations() {
        return {
            inputField: { required, maxLength: maxLength(300) },
        }
    },

    computed: {
        requiredComment() {
            return this.v$.inputField.$error ? 'Please fill the relevant fields' : '';
        },
        completePercentage() {
            return ((this.data.question_no - 1) / this.totalQuestionCount) * 100;
        }
    },

    methods: {
        restrictToInteger() {
            if (this.data.question_no == 29 || this.data.question_no == 37) {
                this.inputField = String(this.inputField).replace(/[^0-9]/g, "");
            }
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

        selectOption(option) {
            this.optionSelected = option;
        },

        async loadDropdownOptions(questionNumber, subsection) {
            var params = { subsectionName: subsection, questionNo: questionNumber };
            this.$http.get(`/getDropdownOptions?parameters=${encodeURIComponent(JSON.stringify(params))}`, {
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
                    this.dropdownOptions = response.data;
                    this.optionsLoaded = true;
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

        async continueProcessForTextAndNumber() {

            var fieldLength = 0;
            if (this.data.question_type == 'Text') {
                fieldLength = this.inputField.length;
            }
            if (this.data.question_type == 'Numeric') {
                fieldLength = this.inputField.toString().length;
            }
            var fieldLimit = 0;

            if (this.subsectionName == 'Short Leave') {
                if (this.data.question_no == 1) {
                    fieldLimit = 6;
                    this.fieldMaxLength = fieldLimit;
                }
                else if (this.data.question_no == 2) {
                    fieldLimit = 18
                    this.fieldMaxLength = fieldLimit;
                }
                else if (this.data.question_no == 15) {
                    fieldLimit = 200
                    this.fieldMaxLength = fieldLimit;
                }
                // else if (this.data.question_no == 3 || this.data.question_no == 4 || this.data.question_no == 6 || this.data.question_no == 8 || this.data.question_no == 9 || this.data.question_no == 10 || this.data.question_no == 11 || this.data.question_no == 12 || this.data.question_no == 13) {
                //     fieldLimit = 5
                //     this.fieldMaxLength = fieldLimit;
                // }
                // else if (this.data.question_no == 5) {
                //     fieldLimit = 2
                //     this.fieldMaxLength = fieldLimit;
                // }

                if (this.data.question_no == 1 || this.data.question_no == 2 || this.data.question_no == 15) {
                    if (fieldLength > fieldLimit) {
                        this.exceedFieldLimit = true;
                        return;
                    }
                }

                if (this.data.question_no == 3 || this.data.question_no == 4 || this.data.question_no == 6 ||  this.data.question_no == 9 || this.data.question_no == 10 || this.data.question_no == 11) {
                    this.hasDayRangeValue = this.inputField.length != 0 && (parseFloat(this.inputField) < 0 || parseFloat(this.inputField) > 23);

                    if (this.hasDayRangeValue) {
                        return;
                    }
                }

                if (this.data.question_no == 12 || this.data.question_no == 13) {
                    this.hasToBeThreeDigitsValue = this.inputField.length != 0 && (parseFloat(this.inputField) < 0 || parseFloat(this.inputField) > 999);

                    if (this.hasToBeThreeDigitsValue) {
                        return;
                    }
                }

                if (this.data.question_no == 4) {
                    if (this.inputField.length != 0)
                        await this.checkIfSmallerThanPreviousColumnValue(this.inputField, this.data.question_no);

                    if (this.hasToBeBiggerValue) {
                        return;
                    }
                }

                if (this.data.question_no == 1 || this.data.question_no == 2) {
                    this.v$.$validate();
                    if (this.v$.$error) {
                        return;
                    }
                }

                // if (this.data.question_no == 1) {
                //     await this.checkForDuplicateLeaveTypeShortLeave(this.inputField);
                //     if (this.hasDuplicateLeaveType) {
                //         return;
                //     }
                // }

                if (this.data.question_no == 5) {
                    this.hasNegativeValue = this.inputField.length != 0 && parseInt(this.inputField) < 0;

                    if (this.hasNegativeValue) {
                        return;
                    }
                }

                if (this.data.question_no == 5) {
                    this.hasToBeTwoDigitsValue = this.inputField.length != 0 && (parseFloat(this.inputField) < 0 || parseFloat(this.inputField)>99);

                    if (this.hasToBeTwoDigitsValue) {
                        return;
                    }
                }

                if (this.data.question_no == 6) {
                    // this.hasValueGreaterThanMaximum = getLeaveWizardOptionsFromLocalStorage().some((item) => {
                    //     return item.question_no === 4 && item.response.length != 0 && parseInt(this.inputField) > parseInt(item.response);
                    // });

                    if (this.inputField.length != 0) {
                        await this.checkIfLessThanMaximumForShortLeave(4, this.inputField);
                    }

                    if (this.hasValueLessThanMaximum) {
                        return;
                    }
                }

                if (this.data.question_no == 8) {
                    this.hasToBeTwoDigitsValue = this.inputField.length != 0 && (parseFloat(this.inputField) < 0 || parseFloat(this.inputField)>99);

                    if (this.hasToBeTwoDigitsValue) {
                        return;
                    }
                }

                if (this.data.question_no == 10) {
                    this.hasNegativeValue = this.inputField.length != 0 && parseInt(this.inputField) < 0;

                    if (this.hasNegativeValue) {
                        return;
                    }

                    if (this.inputField.length != 0) {
                        await this.checkIfLessThanMinimumForShortLeave(3, this.inputField);
                    }

                    if (this.hasValueLessThanMinimum) {
                        return;
                    }
                }

                if (this.data.question_no == 11) {
                    this.hasNegativeValue = this.inputField.length != 0 && parseInt(this.inputField) < 0;

                    if (this.hasNegativeValue) {
                        return;
                    }

                    if (this.inputField.length != 0) {
                        await this.checkIfGreaterThanMaximumForShortLeave(4, this.inputField);
                    }
                    if (this.hasValueGreaterThanMaximum) {
                        return;
                    }

                    if (this.inputField.length == 0) {
                        if (this.previousColumnNotNull) {
                            this.v$.$validate();
                            if (this.v$.$error) {
                                return;
                            }
                        }
                    }
                }
            }

            if (this.subsectionName == 'Statutory Leave') {
                if (this.data.question_no == 29) {
                    fieldLimit = 1
                    this.fieldMaxLength = fieldLimit;
                }

                // if (this.data.question_no == 29 || this.data.question_no == 37) {
                //     fieldLimit = 3
                //     this.fieldMaxLength = fieldLimit;
                // }

                else if (this.data.question_no == 1) {
                    fieldLimit = 6
                    this.fieldMaxLength = fieldLimit;
                }
                // else if (this.data.question_no == 31) {
                //     fieldLimit = 18
                //     this.fieldMaxLength = fieldLimit;
                // }
                else if (this.data.question_no == 2) {
                    fieldLimit = 100
                    this.fieldMaxLength = fieldLimit;
                }
                else if (this.data.question_no == 43) {
                    fieldLimit = 200
                    this.fieldMaxLength = fieldLimit;
                }

                if (this.data.question_no == 1 || this.data.question_no == 2 || this.data.question_no == 29 || this.data.question_no == 43) {
                    if (fieldLength > fieldLimit) {
                        this.exceedFieldLimit = true;
                        return;
                    }
                }

                //Check If Field is empty for Statutory Leave(Mandatory Fields)
                if (this.data.question_no == 1 || this.data.question_no == 2 || this.data.question_no == 42) {
                    this.v$.$validate();
                    if (this.v$.$error) {
                        return;
                    }
                }

                if (this.data.question_no == 29 || this.data.question_no == 32 || this.data.question_no == 34 || this.data.question_no == 41) {
                    this.hasZeroValue = (this.inputField.length != 0 && parseInt(this.inputField) <= 0) ? true : false;

                    if (this.hasZeroValue) {
                        return;
                    }
                }

                if (this.data.question_no == 29) {
                    if (this.inputField.length != 0)
                        await this.checkDuplicateApplySeqForStatLeave(this.inputField);
                    if (this.hasDuplicateApplySequence) {
                        return;
                    }
                }

                //Check for value between 1 and 365
                if (this.data.question_no == 30 || this.data.question_no == 31 || this.data.question_no == 32 || this.data.question_no == 33 || this.data.question_no == 34 || this.data.question_no == 39 || this.data.question_no == 40 || this.data.question_no == 41) {
                    if (this.inputField.length != 0)
                        this.hasToBeValidDayValue = (parseFloat(this.inputField) < 1 || parseFloat(this.inputField) > 365) ? true : false;
                    if (this.hasToBeValidDayValue) {
                        return;
                    }
                }

                if (this.data.question_no == 33) {
                    if (this.inputField.length != 0)
                        await this.checkIfSmallerThanPreviousColumnValue(this.inputField, this.data.question_no);

                    if (this.hasToBeBiggerValue) {
                        return;
                    }

                    if (this.inputField.length == 0) {
                        if (this.prevColNotNullForStat) {
                            this.v$.$validate();
                            if (this.v$.$error) {
                                return;
                            }
                        }
                    }
                }

                if (this.data.question_no == 37) {
                    await this.checkIfZeroOrNullValue(this.data.question_no);

                    if (this.previousColumnNotNull == true && (this.inputField.length == 0 || parseInt(this.inputField) == 0)) {
                        this.hasToBeNonNullValue = true;
                        return;
                    }

                    if (this.inputField.length != 0) {
                        this.hasToBeRangeValue = (parseInt(this.inputField) <= 0 || parseInt(this.inputField) > 12) ? true : false;
                    }

                    if (this.hasToBeRangeValue) {
                        return;
                    }

                    if (this.inputField.length == 0) {
                        if (this.prevColNotNullForStat) {
                            this.v$.$validate();
                            if (this.v$.$error) {
                                return;
                            }
                        }
                    }
                }

                //Check for valid value between 1 and 12
                if (this.data.question_no == 37) {
                    if (this.inputField.length != 0)
                        this.hasToBeValidMonthValue = (parseInt(this.inputField) < 1 || parseInt(this.inputField) > 12) ? true : false;
                    if (this.hasToBeValidMonthValue) {
                        return;
                    }
                }

                if (this.data.question_no == 39 || this.data.question_no == 40) {
                    await this.checkIfZeroOrNullValue(this.data.question_no);
                    if (this.previousColumnNotNull == true && (this.inputField.length == 0 || parseInt(this.inputField) == 0)) {
                        this.hasToBeNonNullValue = true;
                        return;
                    }
                }

                if (this.data.question_no == 1 || this.data.question_no == 2 || this.data.question_no == 42) {
                    this.v$.$validate();
                    if (this.v$.$error) {
                        return;
                    }
                }

                // //Check For Duplicate Leave Type
                // if (this.data.question_no == 1) {
                //     await this.checkForDuplicateLeaveTypeStatutoryLeave(this.inputField);

                //     if (this.hasDuplicateLeaveType) {
                //         return;
                //     }
                // }
            }
            await this.updateRecord(this.inputField);
        },

        async continueProcessForDropDown() {
            await this.updateRecord(this.inputField);
            this.$emit('next');
        },

        async continueProcessForOption() {
            if (this.subsectionName == 'Short Leave' && this.data.question_no == 7) {
                if (this.optionSelected == null || this.optionSelected == '') {
                    if (this.previousColumnNotNull) {
                        this.hasToSelectAnOption = true;
                        return
                    }
                }
            }

            if (this.optionSelected === null || this.optionSelected === '') {
                this.$emit('next');
                return;
            }
            await this.updateRecord(this.optionSelected);
        },

        goBack() {
            this.$emit('back');
        },

        async updateRecord(response) {
            await this.$http
                .post(`/updateRecordForAbsenceWizard`, {
                    QuestionNo: this.data.question_no,
                    RecordId: this.recordId,
                    ResponseText: response,
                    QuestionType: this.data.question_type,
                    SubsectionName: this.subsectionName
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

        async checkIfLessThanMaximumForShortLeave(questionNoForMaxValue, response) {
            var id = this.recordId;
            //var questionNoForMaxValue=4;
            this.isLoading = true;
            await this.$http.get(`/checkIfLessThanMaximumForShortLeave?responseText=${encodeURIComponent(response)}&questionNo=${encodeURIComponent(questionNoForMaxValue)}&rowId=${encodeURIComponent(id)}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                this.hasValueLessThanMaximum = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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


        async checkDuplicateApplySeqForStatLeave(response) {
            this.isLoading = true;
            await this.$http.get(`/checkDuplicateApplySeqForStatutoryLeave?responseText=${encodeURIComponent(response)} &userId=${encodeURIComponent(getUserID())}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                this.hasDuplicateApplySequence = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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

        async checkIfSmallerThanPreviousColumnValue(response, questionNo) {
            var id = this.recordId;

            this.isLoading = true;
            await this.$http.get(`/checkIfSmallerThanPreviousColumnValue?responseText=${encodeURIComponent(response)}&questionNo=${encodeURIComponent(questionNo)}&rowId=${encodeURIComponent(id)}&subsectionName=${encodeURIComponent(this.subsectionName)}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                this.hasToBeBiggerValue = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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

        async checkIfZeroOrNullValue(questionNo) {
            var id = this.recordId;
            this.isLoading = true;
            await this.$http.get(`/checkIfZeroOrNullValue?questionNo=${encodeURIComponent(questionNo)}&rowId=${encodeURIComponent(id)}&subsectionName=${encodeURIComponent(this.subsectionName)}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                this.previousColumnNotNull = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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
        async checkIfGreaterThanMaximumForShortLeave(questionNoForMaxValue, response) {
            var id = this.recordId;
            //var questionNoForMaxValue=4;
            this.isLoading = true;
            await this.$http.get(`/checkIfGreaterThanMaximumForShortLeave?responseText=${encodeURIComponent(response)}&questionNo=${encodeURIComponent(questionNoForMaxValue)}&rowId=${encodeURIComponent(id)}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                this.hasValueGreaterThanMaximum = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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

        async checkIfLessThanMinimumForShortLeave(questionNoForMaxValue, response) {
            var id = this.recordId;
            //var questionNoForMaxValue=4;
            this.isLoading = true;
            await this.$http.get(`/checkIfLessThanMinimumForShortLeave?responseText=${encodeURIComponent(response)}&questionNo=${encodeURIComponent(questionNoForMaxValue)}&rowId=${encodeURIComponent(id)}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                this.hasValueLessThanMinimum = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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

        async checkIfPreviousColumnIsNotNullForStatLeave(questionNo) {
            var id = this.recordId;
            this.isLoading = true;
            await this.$http.get(`/checkIfPreviousColumnIsNotNullForStatLeave?questionNo=${encodeURIComponent(questionNo)}&rowId=${encodeURIComponent(id)}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                this.prevColNotNullForStat = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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

        async checkIfPreviousColumnIsNotNullForShortLeave(questionNo) {
            var id = this.recordId;
            this.isLoading = true;
            await this.$http.get(`/checkIfPreviousColumnIsNotNullForShortLeave?questionNo=${encodeURIComponent(questionNo)}&rowId=${encodeURIComponent(id)}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                if (questionNo == '7' || questionNo == '11')
                    this.previousColumnNotNull = resp.data;
                else
                    this.isDisabled = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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

        async checkIfColumnBeforeTheLastIsNotNullForShortLeave(questionNo) {
            var id = this.recordId;
            this.isLoading = true;
            await this.$http.get(`/checkIfColumnBeforeTheLastIsNotNullForShortLeave?questionNo=${encodeURIComponent(questionNo)}&rowId=${encodeURIComponent(id)}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                this.isDisabled = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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

        async checkIfPreviousTwoColumnsAreNotNullForShortLeave(questionNo) {
            var id = this.recordId;
            this.isLoading = true;
            await this.$http.get(`/checkIfPreviousTwoColumnsAreNotNullForShortLeave?questionNo=${encodeURIComponent(questionNo)}&rowId=${encodeURIComponent(id)}`, {
                headers: {
                    accept: "*/*",
                    Authorization:
                        "Basic " +
                        Base64.toBase64(
                            this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                        ),
                },
                withCredentials: true,
            }).then((resp) => {
                this.isDisabled = resp.data;
                extendCookieTimeout();
                this.isLoading = false;
            }).catch((error) => {
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
    },

    async mounted() {
        if (this.data.question_type == 'Text' || this.data.question_type == 'Numeric') {
            this.inputField = this.data.response;
        }

        else if (this.data.question_type == 'Yes/No') {
            if (this.data.response == 'Yes') {
                this.optionSelected = true;
            }
            else if (this.data.response == 'No') {
                this.optionSelected = false;
            }
            else if (this.data.response == '') {
                this.optionSelected = ''
            }
        }
        else if (this.data.question_type == 'Drop down-Text') {
            this.loadDropdownOptions(this.data.question_no, this.subsectionName);
            this.inputField = this.data.response;
        }

        if (this.subsectionName == 'Short Leave' && this.data.question_no == 7) {
            await this.checkIfPreviousColumnIsNotNullForShortLeave(this.data.question_no);
        }
        if (this.subsectionName == 'Short Leave' && this.data.question_no == 10) {
            await this.checkIfPreviousColumnIsNotNullForShortLeave(this.data.question_no);
        }

        if (this.subsectionName == 'Short Leave' && this.data.question_no == 11) {
            await this.checkIfPreviousColumnIsNotNullForShortLeave(this.data.question_no);
        }

        if (this.subsectionName == 'Short Leave' && this.data.question_no == 11) {
            await this.checkIfColumnBeforeTheLastIsNotNullForShortLeave(this.data.question_no);
        }

        if (this.subsectionName == 'Short Leave' && this.data.question_no == 13) {
            await this.checkIfPreviousColumnIsNotNullForShortLeave(this.data.question_no);
        }

        if (this.subsectionName == 'Short Leave' && this.data.question_no == 14) {
            await this.checkIfPreviousTwoColumnsAreNotNullForShortLeave(this.data.question_no);
        }

        if (this.subsectionName == 'Statutory Leave' && this.data.question_no == 33) {
            await this.checkIfPreviousColumnIsNotNullForStatLeave(this.data.question_no);
        }

        if (this.subsectionName == 'Statutory Leave' && this.data.question_no == 37) {
            await this.checkIfPreviousColumnIsNotNullForStatLeave(this.data.question_no);
        }
    },

})
</script>

<style scoped>
.disable {
    pointer-events: none;
    opacity: 0.6;
    /* Optionally reduce the opacity for disabled appearance */
}
</style>