<template>
    <div>
        <div class="page">
            <div class="search-container">
                <div class="input-group">
                    <span class="input-group-text"><i class="ri-search-line"></i></span>
                    <input type="text" class="form-control form-control-sm" v-model="searchItem" @input="onInputChange"
                        placeholder="Search This Table">
                </div>
            </div>
            <vue-good-table mode="remote" v-on:page-change="onPageChange"
                v-on:sort-change="onSortChange($event[0], $event[1])" v-on:per-page-change="onPerPageChange"
                :totalRows="totalRecords" v-model="isLoading" :pagination-options="{
                    enabled: true,
                }" :loading=true :rows="rows" :columns="columns">
                <template #table-row="props">
                    <span v-if="props.column.field == 'user_approval_detail'">
                        <Popper placement="right">
                            <i class="ri-file-text-fill"></i>
                            <template #content>
                                <span class="table-container" style="margin-bottom: 0;padding-bottom: 0;">
                                    <!-- <vue-good-table
                                    :rows="[{ approval_status: props.row.approval_status, approval_by: props.row.approval_by, approval_date: props.row.approval_date, approval_comment: props.row.comment }]"
                                    :columns="userDetailsColumns">
                                    <template #table-row="params">
                                        <span v-if="params.column.field == 'comment'">
                                            <Popper arrow>
                                                <i class="ri-file-text-fill"></i>
                                                <template #content>
                                                    <span>{{ props.row.comment }}</span>
                                                </template>
                                            </Popper>
                                        </span>

                                        <span v-else-if="params.column.field == 'approval_status'">
                                            <span class="badge bg-success"
                                                v-if="params.row.approval_status === 'Approved'">{{
                                                    params.row.approval_status }}</span>
                                            <span class="badge bg-danger"
                                                v-else-if="params.row.approval_status === 'Rejected'">{{
                                                    params.row.approval_status }}</span>
                                            <span class="badge bg-info" v-else>Not Approved</span>
                                        </span>

                                        <span v-else>
                                            {{ params.formattedRow[params.column.field] }}
                                        </span>
                                    </template>
                                </vue-good-table> -->
                                    <table class="table table-secondary" style="font-size: 11px;">
                                        <thead>
                                            <tr>
                                                <th scope="col">Approval Date</th>
                                                <th scope="col">Approved/Rejected By</th>
                                                <th scope="col">Approval Status</th>
                                                <th scope="col">Approval Comment</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td><span>{{ props.row.approval_date }}</span></td>
                                                <td><span>{{ props.row.approval_by }}</span></td>
                                                <td>
                                                    <span class="badge bg-success"
                                                        v-if="props.row.approval_status === 'Approved'">{{
                                                            props.row.approval_status }}</span>
                                                    <span class="badge bg-danger"
                                                        v-else-if="props.row.approval_status === 'Rejected'">{{
                                                            props.row.approval_status }}</span>
                                                    <span class="badge bg-info" v-else>Not Approved</span>
                                                </td>
                                                <td><span> <span>
                                                            <!-- <Popper class="comment-popper">
                                                                <i class="ri-file-text-fill"></i>
                                                                <template #content>
                                                                    <span>{{ props.row.comment }}</span>
                                                                </template>
                                                            </Popper> -->
                                                            <i class="ri-message-line" data-bs-toggle="tooltip"
                                                                data-bs-placement="bottom" :title="props.row.comment"
                                                                ref="info"></i>
                                                        </span></span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </span>
                            </template>
                        </Popper>
                    </span>

                    <span v-else-if="props.column.field == 'approval_status'">
                        <span class="badge bg-success" v-if="props.row.approval_status === 'Approved'">{{
                            props.row.approval_status }}</span>
                        <span class="badge bg-danger" v-else-if="props.row.approval_status === 'Rejected'">{{
                            props.row.approval_status }}</span>
                        <span class="badge bg-info" v-else>Not Approved</span>
                    </span>

                    <span v-else-if="props.column.field == 'view_for_approval'">
                        <button class="btn icon-button"
                            @click="this.$router.push({ name: 'admin-commit', params: { record_Id: props.row.record_id, subsection: props.row.subsection_name } })"><i
                                class="ri-file-search-line"></i></button>
                    </span>

                    <span v-else-if="props.column.field == 'approval_data'">
                        <button class="btn icon-button" @click="
                            downloadExcel(props.row.approval_data, props.row.subsection_name)"><i
                                class="ri-file-excel-2-fill"></i></button>
                    </span>

                    <span v-else-if="props.column.field == 'signoff_details'">
                        <button class="btn icon-button"
                            @click="downloadExcel(props.row.approval_data, props.row.subsection_name)"><i
                                class="ri-article-line"></i></button>
                    </span>

                    <span v-else>
                        {{ props.formattedRow[props.column.field] }}
                    </span>
                </template>
            </vue-good-table>
        </div>
    </div>
</template>
  
<script>
import 'vue-good-table-next/dist/vue-good-table-next.css'
import { VueGoodTable } from 'vue-good-table-next';
import { Base64 } from "js-base64";
import Popper from "vue3-popper";
import { Tooltip } from 'bootstrap';
export default {
    components: {
        VueGoodTable,
        Popper,
    },
    data() {
        return {
            componentKey: 0,
            historySubsection: '',
            searchItem: '',
            isLoading: true,
            showHistory: false,
            columns: [
                {
                    label: 'Module',
                    field: 'module_name',
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Sub Section',
                    field: 'subsection_name',
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                // {
                //     label: 'Sign Off Details',
                //     field: 'signoff_details',
                //     sortable: false,
                //     tdClass: 'text-center align-middle',
                //     thClass: 'text-center align-middle',
                // },
                {
                    label: 'Configuration - User',
                    field: 'user_approval_detail',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Download Excel',
                    field: 'approval_data',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                // {
                //     label: 'Status',
                //     field: 'approval_status',
                //     tdClass: 'text-center align-middle',
                //     thClass: 'text-center align-middle',
                // },
                {
                    label: 'View for Approval',
                    field: 'view_for_approval',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
            ],

            userDetailsColumns: [
                {
                    label: 'Approval Date',
                    field: 'approval_date',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Approved/Rejected By',
                    field: 'approval_by',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Approval Status',
                    field: 'approval_status',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Approval Comment',
                    field: 'comment',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
            ],

            rows: [
            ],
            totalRecords: 10,
            serverParams: {
                sortField: 'module_name',
                sortType: 'asc',
                page: 1,
                perPage: 10,
                searchText: ''
            }
        };
    },
    methods: {
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
            this.$http.get(`/getPendingApprovalListforVendor?serverParams=${encodeURIComponent(JSON.stringify(this.serverParams))}`, {
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
                    this.rows = response.data.data;
                    this.totalRecords = response.data.recordsTotal;
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

        async downloadExcel(excelData, subsectionName) {
            const link = document.createElement("a");
            link.href =
                "data:application/octet-stream;charset=utf-8;base64," + excelData;
            link.target = "_blank";
            link.download = subsectionName + '.xlsm';
            link.click();
        },

        goToApproval(recordID, subsectionName) {
            this.$router.push({ name: 'admin-commit', params: { record_Id: recordID, subsection: subsectionName } })
        }
    },

    mounted() {
        this.loadItems();

        new Tooltip(document.body, {
            selector: "[data-bs-toggle='tooltip']",
        });
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
    color: black;
    height: auto;
    margin-bottom: 0px;
    padding-bottom: 0px;
}


:deep(.footer__row-count__select) {
    cursor: pointer;
}

:deep(.vgt-inner-wrap) {
    position: relative;
    z-index: 2;
}

:deep(.page) {
    position: relative;
    z-index: 1;
}

:deep(.ri-file-text-fill) {
    cursor: pointer;
}
</style>

