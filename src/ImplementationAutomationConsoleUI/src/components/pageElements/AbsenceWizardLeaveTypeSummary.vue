<template>
    <div>
        <div v-if="isLoading == true">
            <loader />
        </div>
        <div v-else>
            <div class="row mb-3">
                <div class="search-container">
                    <div class="input-group">
                        <span class="input-group-text"><i class="ri-search-line"></i></span>
                        <input type="text" class="form-control form-control-sm" v-model="searchItem" @input="onInputChange"
                            placeholder="Search This Table">
                    </div>
                </div>
                <vue-good-table v-on:selected-rows-change="selectionChanged" mode="remote" v-on:page-change="onPageChange"
                    v-on:sort-change="onSortChange($event[0], $event[1])" v-on:per-page-change="onPerPageChange"
                    :totalRows="totalRecords" v-model="isLoading"
                    :select-options="{ enabled: true, selectOnCheckboxOnly: true, clearSelectionText: '', }"
                    :pagination-options="{
                        enabled: true,
                    }" :loading=true :rows="tableData" :columns="tableColumns">
                    <template #table-row="props">
                        <span v-if="props.column.field == 'edit_leave'">
                            <button class="btn" @click="editLeaveType(props.row.record_id)"><i
                                    class="ri-edit-2-fill"></i></button>
                        </span>

                        <span v-if="props.column.field == 'delete_leave'">
                            <button class="btn" @click="showDeleteConfimAlert(props.row.record_id)"><i
                                    class="ri-delete-bin-fill"></i></button>
                        </span>
                    </template>
                    <template #selected-row-actions>
                        <button class="btn" @click="deleteSelectedRows"><i class="ri-delete-bin-fill"></i></button>
                    </template>
                </vue-good-table>
            </div>
            <div class="root" v-if="showApproveSection">
                <div class="inner-wrapper p-3 mb-5">
                    <div class="block-data">
                        <div class="block-form">
                            <div>
                                <div class="row mb-3">
                                    <label class="col-md-12 mb-1">
                                        Comment - (Please enter a comment as the acceptance
                                        before confirming)
                                    </label>
                                    <div class="col-md-12">
                                        <textarea class="form-control" v-model="comment"></textarea>
                                        <span class="text-danger" v-if="v$.comment.$error">
                                            {{ requiredComment }}
                                        </span>
                                    </div>
                                </div>

                                <div class="row mb-3">
                                    <label class="col-md-12 mb-1">
                                        Confirm By - (Confirming Person's Name)
                                    </label>
                                    <div class="col-md-12">
                                        <input class="form-control" v-model="name">
                                        <span class="text-danger" v-if="v$.name.$error">
                                            {{ requiredName }}
                                        </span>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="button-group d-flex justify-content-end">
                                            <!-- <button class="btn btn-outline-primary" @click="downloadExcel">
                                        <i class="ri-file-excel-2-fill"></i> <span class="ms-1">Download</span>
                                    </button> -->
                                            <button class="btn btn-primary" @click="submitData" style="margin-left: 5px;">
                                                Confirm
                                            </button>
                                            <button class="btn btn-danger" @click="rejectData" style="margin-left: 5px;">
                                                Delete
                                            </button>
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
import 'vue-good-table-next/dist/vue-good-table-next.css'
import { VueGoodTable } from 'vue-good-table-next';
import { removeAbsencePendingRowIdToArray, addAbsencePendingRowIdToArray, removeAbsenceIdFromLocalStorageArray } from '@/localStorageUtils';
import { Base64 } from "js-base64";
import useValidate from '@vuelidate/core'
import { required, maxLength } from '@vuelidate/validators'
import PageLoader from '@/components/pageElements/PageLoader.vue';
import { extendCookieTimeout } from '@/cookieUtils'
export default {
    components: {
        VueGoodTable,
        "loader": PageLoader
    },

    data() {
        return {
            v$: useValidate(),
            showApproveSection: false,
            comment: '',
            name: '',
            approvalStatus: '',
            currentDate: null,
            moduleName: '',
            formattedDate: '',
            showPdfSection: false,
            searchItem: '',
            data: [],
            isLoading: false,
            tableData: [
            ],
            sortColumnName: '',
            selectedRows: [],
            pendingItemRows: [],
            tableColumns: [
                {
                    label: 'Leave Name',
                    field: 'leaveTypeCode',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Edit',
                    field: 'edit_leave',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Delete',
                    field: 'delete_leave',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
            ],
            totalRecords: 10,
            serverParams: {
                sortField: 'ID',
                sortType: 'asc',
                page: 1,
                perPage: 10,
                searchText: '',
                subsection: this.subsectionName,
                rowIds: []
            }
        };
    },

    validations() {
        return {
            comment: { required, maxLength: maxLength(300) },
            name: { required },
        }
    },

    props: {
        subsectionName: String
    },

    methods: {
        selectionChanged(selectedRows) {
            this.selectedRows = selectedRows;
        },

        deleteSelectedRows() {
            this.showMultipleDeleteConfimAlert();
        },

        async submitData() {
            this.v$.$validate();
            if (!this.v$.$error) {
                if (this.name != this.$cookies.get("userName")) {
                    this.showNameUnmatchAlert();
                    return;
                }
                this.approvalStatus = 'Approved';
                await this.updateDependentColumns();
                // await this.uploadWizardData();
            } else {
                return;
            }
        },

        async rejectData() {
            this.v$.$validate()
            if (!this.v$.$error) {
                if (this.name != this.$cookies.get("userName")) {
                    this.showNameUnmatchAlert();
                    return;
                }
                this.approvalStatus = 'Rejected';
                await this.generateSignOffDocument();
            }
            else {
                return;
            }
        },

        async deleteDataFromTables() {
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
                this.showErrorAlert();
                if (error.response.status == 401) {
                    this.emitter.emit('loggedOut', true);
                }
                else if (error.response.status == 403) {
                    this.emitter.emit('accessDenied', true);
                }
            });
        },

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
                this.showErrorAlert();
                if (error.response.status == 401) {
                    this.emitter.emit('loggedOut', true);
                }
                else if (error.response.status == 403) {
                    this.emitter.emit('accessDenied', true);
                }
            });
        },

        async updateDependentColumns() {
            this.isLoading = true;
            await this.$http
                .get(`/updateDependentColumnsBySubsection?subsectionName=${encodeURIComponent(this.subsectionName)}`, {
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
                    this.generateSignOffDocument();
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

        async generateSignOffDocument() {
            this.isLoading = true;
            let date = new Date();
            this.currentDate = date;
            await this.$http
                .post(`/generateSignOffDocument`, {
                    SubsectionName: this.subsectionName,
                    UserName: this.$cookies.get("userName"),
                    Name: this.$cookies.get("name"),
                    Comment: this.comment,
                    ModuleId: 0,
                    SubsectionID: '',
                    ApprovalStatus: this.approvalStatus,
                    ApprovalSignature: '',
                    Approvaldate: this.currentDate,
                    ApprovalData: '',
                    SignOffPdfData: ''
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
                .then((resp) => {
                    const binaryData = atob(resp.data);
                    const uint8Array = new Uint8Array(binaryData.length);

                    for (let i = 0; i < binaryData.length; i++) {
                        uint8Array[i] = binaryData.charCodeAt(i);
                    }

                    const blob = new Blob([uint8Array], { type: 'application/pdf' });
                    const url = window.URL.createObjectURL(blob);
                    window.open(url, '_blank');
                    this.uploadDashboardInfo();
                    extendCookieTimeout();
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

        async uploadDashboardInfo() {
            let date = new Date();
            this.currentDate = date;
            await this.$http
                .post(`/uploadDashboardDataForWizard`, {
                    SubsectionName: this.subsectionName,
                    UserName: this.$cookies.get("userName"),
                    Name: this.$cookies.get("name"),
                    Comment: this.comment,
                    ModuleId: 0,
                    SubsectionID: '',
                    ApprovalStatus: this.approvalStatus,
                    ApprovalSignature: '',
                    Approvaldate: this.currentDate,
                    ApprovalData: '',
                    SignOffPdfData: '',
                    RowIds: this.pendingItemRows
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
                    this.isLoading = false;
                    if (this.approvalStatus === 'Approved')
                        this.showSubmitAlert();
                    else if (this.approvalStatus === 'Rejected') {
                        this.showRejectAlert();
                        this.deleteDataFromTables();
                    }
                    extendCookieTimeout();
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

        showDeleteConfimAlert(recordId) {
            this.$swal({
                title: 'Are you sure you want to delete?',
                showDenyButton: true,
                confirmButtonText: 'No',
                denyButtonText: `Yes`,
            }).then((result) => {
                if (result.isConfirmed) {
                    return;
                } else if (result.isDenied) {
                    this.deleteLeaveType(recordId);
                }
            })
        },

        showMultipleDeleteConfimAlert() {
            this.$swal({
                title: 'Are you sure you want to delete?',
                showDenyButton: true,
                confirmButtonText: 'No',
                denyButtonText: `Yes`,
            }).then((result) => {
                if (result.isConfirmed) {
                    return;
                } else if (result.isDenied) {
                    this.deleteMultipleLeaveType(this.selectedRows);
                }
            })
        },

        showSubmitAlert() {
            this.$swal.fire({
                icon: 'success',
                width: 300,
                height: 200,
                text: 'Your confirmation has been successfully recorded.',
                // timer: 3000,
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$router.push({ name: 'home' });
                }
            })
        },

        showRejectAlert() {
            this.deleteDataFromDependantTables();
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'Your have rejected the confirmation.',
                // timer: 2000
                confirmButtonText: 'Ok',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$router.push({ name: 'home' });
                }
            });
        },

        showErrorAlert() {
            this.deleteDataFromDependantTables();
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'There was a problem. Please try again.',
                //timer: 2000,
                allowOutsideClick: false,
                confirmButtonText: 'Ok',
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$router.push({ name: 'home' });
                }
            });
        },

        showNameUnmatchAlert() {
            this.$swal.fire({
                icon: 'error',
                width: 300,
                height: 200,
                text: 'User name does not match',
                timer: 2000
            });
        },

        async deleteLeaveType(recordId) {
            await this.$http
                .post(`/deleteAbsenceDataByRowId`, { tableRowId: recordId, subsectionName: this.subsectionName }, {
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
                    removeAbsenceIdFromLocalStorageArray(recordId);
                    extendCookieTimeout();
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
            this.$emit('reloadComp');
        },

        async deleteMultipleLeaveType(selectedRows) {
            const plainSelectedRows = JSON.parse(JSON.stringify(selectedRows));
            await this.$http
                .post(`/deleteMultipleAbsenceRecords`, {
                    selectedRows: plainSelectedRows.selectedRows,
                    subsectionName: this.subsectionName
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
            this.$emit('reloadComp');
        },

        editLeaveType(recordId) {
            this.$router.push({ name: 'absence-wizard-edit-leave-type', params: { subsection: this.subsectionName, id: recordId } });
        },

        onInputChange(event) {
            this.updateParams({ searchText: event.target.value });
            this.loadItems();
        },

        updateParams(newProps) {
            this.serverParams = Object.assign({}, this.serverParams, newProps);
        },

        onPageChange(params) {
            this.updateParams({ page: params.currentPage });
            this.loadItems();
        },

        onPerPageChange(params) {
            this.updateParams({ perPage: params.currentPerPage });
            this.loadItems();
        },

        onSortChange(params) {
            const columnIndex = this.columns.findIndex(column => column.field === params.field);
            this.updateParams({ sortField: this.columns[columnIndex].field });
            this.updateParams({ sortType: params.type });
            this.loadItems();
        },

        loadItems() {
            //this.showApproveSection=false;
            //this.isLoading = true;
            this.$http.get(`/getPendingLeaveTypes?serverParams=${encodeURIComponent(JSON.stringify(this.serverParams))}`, {
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
                    this.tableData = response.data.data;
                    this.totalRecords = response.data.recordsTotal;
                    if (this.tableData.length !== 0) {
                        this.showApproveSection = true;
                    }
                    //this.isLoading = false;
                })
                .catch(error => {
                    this.showErrorAlert();
                    if (error.response.status == 401) {
                        this.emitter.emit('loggedOut', true);
                    }
                    else if (error.response.status == 403) {
                        this.emitter.emit('accessDenied', true);
                    }
                });
        }
    },

    async mounted() {
        removeAbsencePendingRowIdToArray();
        if (this.subsectionName === 'Short Leave') {
            this.sortColumnName = 'SHORT_LEAVE_TYPE_CODE'
        }
        else if (this.subsectionName === 'Statutory Leave') {
            this.sortColumnName = 'LEAVE_TYPE'
        }

        this.$http.get(`/getAbsenceWizardPendingOptionsRowNumber?subsectionName=${encodeURIComponent(this.subsectionName)}`, {
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
                addAbsencePendingRowIdToArray(response.data);
                this.pendingItemRows = response.data;
                this.serverParams = {
                    sortField: 'ID',
                    sortType: 'asc',
                    page: 1,
                    perPage: 10,
                    searchText: '',
                    subsection: this.subsectionName,
                    rowIds: this.pendingItemRows
                }
                this.loadItems();
                extendCookieTimeout();
            })
            .catch(error => {
                if (error.response.status == 401) {
                    this.emitter.emit('loggedOut', true);
                }
                else if (error.response.status == 403) {
                    this.emitter.emit('accessDenied', true);
                }
            });
    },

    computed: {
        requiredComment() {
            return this.v$.comment.$error ? 'Please fill the relevant fields' : '';
        },

        requiredName() {
            return this.v$.name.$error ? 'Please fill the relevant fields' : '';
        },
    },
};
</script>
  
  
<style scoped>
.search-container {
    display: flex;
    justify-content: flex-end;
    margin-bottom: 2px;
}

.input-group {
    width: 250px;
}

:deep(.popper) {
    background: #9DB2BF;
    padding: 10px;
    border-radius: 10px;
    color: #fff;
}

:deep(.vgt-selection-info-row) {
    display: flex;
    align-items: center;
    /* Vertically center items */
    justify-content: right;
}


/* :deep(.vgt-selection-info-row__actions) {
    float: right;
} */

:deep(.popper #arrow::before) {
    background: #9DB2BF;
}

:deep(.popper:hover),
:deep(.popper:hover > #arrow::before) {
    background: #9DB2BF;
}

:deep(.footer__row-count__select) {
    cursor: pointer;
    margin-left: -5px;
}
</style>
  
  