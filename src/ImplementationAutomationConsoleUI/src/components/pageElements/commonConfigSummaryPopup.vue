<template>
    <div>
        <button type="button" id="modal1Btn" class="btn btn-outline-primary" data-bs-toggle="modal"
            data-bs-target=".myclass1">
            <span class="ms-1">Summary</span>
        </button>

        <div class="modal fade myclass1" id="configIntro" tabindex="-1" data-bs-keyboard="false"
            aria-labelledby="configIntroLabel" aria-hidden="true">
            <div class="modal-dialog modal-xl modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-body p-0 m-0">
                        <!-- <div class="block-form-top bg-gray p-4">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="quis-block">
                                        <ol class="quis-summary ps-2">
                                            <li v-for="option in selectedOptions" :key="option.question_no">
                                                <span v-if="option.question_no == 2"><span class="quis-q">
                                                        {{ option.question_statement
                                                        }}:</span> <span class="quis-a">{{
    this.logoName
}}</span></span>

                                                <span v-else-if="option.question_no == 3"><span class="quis-q">
                                                        {{ option.question_statement
                                                        }}:</span>
                                                    <span v-if="option.response == 1" class="quis-a">Yes</span>
                                                    <span v-else-if="option.response == 0" class="quis-a">No</span>
                                                    <span v-else class="quis-a"></span>
                                                </span>

                                                <span v-else-if="option.question_no == 4"><span class="quis-q">
                                                        {{ option.question_statement
                                                        }}:</span> <span class="quis-a">{{
    this.mobileLogoName
}}</span></span>

                                                <span v-else-if="option.question_no == 5"><span class="quis-q">
                                                        {{ option.question_statement
                                                        }}:</span> <span class="quis-a">
                                                        <span v-if="option.response == 1" class="quis-a">Yes</span>
                                                        <span v-else-if="option.response == 0" class="quis-a">No</span>
                                                        <span v-else class="quis-a"></span></span></span>

                                                <span v-else><span class="quis-q">
                                                        {{ option.question_statement
                                                        }}:</span> <span class="quis-a">{{
    option.response
}}</span></span>
                                            </li>
                                        </ol>
                                    </div>
                                </div>
                            </div>
                        </div> -->
                        <p>Hi</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import 'vue-good-table-next/dist/vue-good-table-next.css'
import $ from 'jquery';
//import { Base64 } from "js-base64";

export default {
    props: {
        'tableRecordId': String
    },

    data() {
        return {
        };
    },

    methods: {

    },

    mounted() {
        $('#modal1Btn').click();
    }
};
</script>

<style scoped>
</style>