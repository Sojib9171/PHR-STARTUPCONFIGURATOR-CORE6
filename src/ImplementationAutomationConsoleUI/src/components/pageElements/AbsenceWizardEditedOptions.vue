<template>
    <div class="config-block p-0">
        <div class="block-form-top bg-gray p-4">
            <div class="row">
                <div class="col-sm-12">
                    <div class="quis-block">
                        <ol class="quis-summary ps-2">
                            <li v-for="option in selectedOptions" :key="option.number">
                                <span class="quis-q">
                                    {{ option.question_statement }}:</span> <span class="quis-a">{{ option.response
                                    }}</span>
                            </li>
                        </ol>
                    </div>
                </div>
            </div>
        </div>
        <div class="approve-form-block bg-white p-4">
            <div class="row mt-4">
                <div class="col-12 text-center">
                    <div class="button-group">
                        <button class="btn btn-success" @click="saveOptions">Save</button>
                        <button class="btn btn-outline-info" style="margin-left: 5px;" @click="editOptions">Edit</button>
                        <button class="btn btn-danger" style="margin-left: 5px;" @click="deleteOptions">Delete</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { emptyLeaveWizardOptionsInLocalStorage, removeLeaveTypeAtIndexFromLocalStorage, addLeaveTypeAtIndexToLocalStorage, getLeaveWizardOptionsFromLocalStorage } from '@/localStorageUtils';

export default ({
    data() {
        return {
            isLoading: false,
            selectedOptions: null
        }
    },

    props: {
        subsection: {
            type: String,
            required: true,
        },
        id: {
            type: Number,
            required: true
        }
    },
    methods: {
        saveOptions() {
            removeLeaveTypeAtIndexFromLocalStorage(this.id - 1);
            addLeaveTypeAtIndexToLocalStorage(this.id - 1, this.selectedOptions);
            emptyLeaveWizardOptionsInLocalStorage();
            this.$router.push({ name: 'leave-type-created-message', params: { subsection: this.subsection } });
        },

        editOptions() {
            this.$router.push({ name: 'absence-wizard-edit', params: { subsection: this.subsection, id: this.id } });
        },

        deleteOptions() {
            this.showDeleteConfimAlert();
        },

        showDeleteConfimAlert() {
            this.$swal({
                title: 'Are you sure you want to delete?',
                showDenyButton: true,
                confirmButtonText: 'No',
                denyButtonText: `Yes`,
            }).then((result) => {
                if (result.isConfirmed) {
                    return;
                } else if (result.isDenied) {
                    removeLeaveTypeAtIndexFromLocalStorage(this.id - 1);
                    this.$router.push({ name: 'leave-type-summary', params: { subsection: this.subsection } });
                }
            })
        }
    },

    mounted() {
        this.selectedOptions = getLeaveWizardOptionsFromLocalStorage();
    }
})
</script>

