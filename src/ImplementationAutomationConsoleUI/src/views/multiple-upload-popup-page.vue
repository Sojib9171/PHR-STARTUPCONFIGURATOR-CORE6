<template>
    <div>
        <button type="button" id="historyBtn" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#MlpUplModal"
            hidden>
        </button>

        <div class="modal fade" id="MlpUplModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="100"
            aria-labelledby="MlpUplModalLabel" aria-hidden="true" :key="componentKey">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="MlpUplModalLabel">Multiple upload</h5>
                        <button type="button" @click='GoToBack()' class="btn-close" data-bs-dismiss="modal"
                            aria-label="Close"></button>
                    </div>
                    <div class="modal-body my-5">
                        <ul class="ms-5 list-unstyled list-bullets">
                            <li>
                                <p>Do you wish to upload a new set of data to the system in addition to the previous
                                    set?
                                </p>
                            </li>
                            <li>
                                <p>If you want to proceed, please click the <span class="fw-semibold">"YES"</span> button,
                                    otherwise, click the <span class="fw-semibold">"No"</span> button.
                                </p>
                            </li>
                        </ul>
                    </div>
                    <div class="modal-footer">
                        <button type="button" @click='GoToBack()' class="btn btn-secondary"
                            data-bs-dismiss="modal">No</button>
                        <button @click='GoToMultipleUpload()' type="button" class="btn btn-primary"
                            data-bs-dismiss="modal">Yes</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

import $ from 'jquery';
import { getSubsecNameForMultipleUpload, removeAbsenceSubsectionName, removeAbsenceRowID } from '@/localStorageUtils';

export default {
    data() {
        return {
            subsectionName: null
        }
    },

    mounted() {
        this.subsectionName = getSubsecNameForMultipleUpload();
        if (this.subsectionName) {
            $('#historyBtn').click();
        }
    },
    props: {
        'subsection': String
    },

    methods: {
        modalClose() {
            this.subsectionName = '';
        },
        getValue() {
            return getSubsecNameForMultipleUpload();
        },
        GoToMultipleUpload() {
            removeAbsenceRowID();
            removeAbsenceSubsectionName();
            if (this.subsectionName == 'Short Leave' || this.subsectionName == 'Statutory Leave') {
                this.$router.push({ name: 'wizard-popup', params: { subsection: this.subsectionName } });
            }
            else {
                this.$router.push({ name: 'upload', params: { subsection: getSubsecNameForMultipleUpload() } });
            }
        },

        GoToBack() {
            this.$router.go(-1);
        }
    }
};
</script>