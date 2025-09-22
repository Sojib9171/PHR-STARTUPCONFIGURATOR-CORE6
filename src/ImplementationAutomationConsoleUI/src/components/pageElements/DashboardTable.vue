<template>
    <div>
        <div class="search-container">
            <div class="input-group">
                <span class="input-group-text"><i class="ri-search-line"></i></span>
                <input type="text" class="form-control form-control-sm" v-model="searchItem" @input="onInputChange"
                    placeholder="Search This Table">
            </div>
        </div>
        <vue-good-table mode="remote" v-on:page-change="onPageChange" v-on:sort-change="onSortChange($event[0], $event[1])"
            v-on:per-page-change="onPerPageChange" :totalRows="totalRecords" v-model="isLoading" :pagination-options="{
                enabled: true,
            }" :loading=true :rows="rows" :columns="columns">
            <template #table-row="props">
                <span v-if="props.column.field == 'user_approval_history'">
                    <button class="btn btn-success" v-if="props.row.approval_status === 'Approved'"
                        @click="viewUserApprovalHistory(props.row.subsection_name)">Confirmed</button>
                    <button class="btn btn-danger" v-if="props.row.approval_status === 'Rejected'"
                        @click="viewUserApprovalHistory(props.row.subsection_name)">{{ props.row.approval_status }}</button>
                    <button class="btn btn-outline-secondary" v-if="props.row.approval_status === 'Not Initiated'"
                        disabled>{{ props.row.approval_status }}</button>
                </span>

                <span v-if="props.column.field == 'vendor_approval_history'">
                    <button class="btn btn-success" v-if="props.row.vendor_approval_status === 'Approved'"
                        @click="viewVendorApprovalHistory(props.row.subsection_name)">{{ props.row.vendor_approval_status
                        }}</button>
                    <button class="btn btn-danger" v-if="props.row.vendor_approval_status === 'Rejected'"
                        @click="viewVendorApprovalHistory(props.row.subsection_name)">{{ props.row.vendor_approval_status
                        }}</button>
                    <button class="btn btn-outline-secondary" v-if="props.row.vendor_approval_status === 'Not Initiated'"
                        disabled>{{ props.row.vendor_approval_status }}</button>
                </span>

                <!-- <span v-else-if="props.column.field == 'approval_status'">
                    <Popper arrow>
                        <i class="ri-task-line"></i>
                        <template #content>
                            <vue-good-table mode="remote" :loading=true  class="custom-table"
                                :rows="[{ approval_status: props.row.approval_status, approval_by: props.row.approval_by, vendor_approval_status: props.row.vendor_approval_status, vendor_approval_by: props.row.vendor_approval_by }]"
                                :columns="statusColumns">
                                <template  #table-row="params">
                                    <span v-if="params.column.field == 'approval_status'">
                                        <span class="badge bg-success" v-if="props.row.approval_status === 'Approved'">{{
                                            props.row.approval_status }}</span>
                                        <span class="badge bg-danger" v-if="props.row.approval_status === 'Rejected'">{{
                                            props.row.approval_status
                                        }}</span>
                                    </span>

                                    <span v-else-if="params.column.field == 'vendor_approval_status'">
                                        <span class="badge bg-success"
                                            v-if="props.row.vendor_approval_status === 'Approved'">{{
                                                props.row.vendor_approval_status }}</span>
                                        <span class="badge bg-danger"
                                            v-if="props.row.vendor_approval_status === 'Rejected'">{{
                                                props.row.vendor_approval_status
                                            }}</span>
                                        <span class="badge bg-warning"
                                            v-if="props.row.vendor_approval_status === 'Pending'">{{
                                                props.row.vendor_approval_status
                                            }}</span>
                                    </span>

                                    <span v-else>
                                        {{ params.formattedRow[params.column.field] }}
                                    </span>
                                </template>
                            </vue-good-table>
                        </template>
                    </Popper>
                </span> -->

                <span v-else>
                    {{ props.formattedRow[props.column.field] }}
                </span>
            </template>
        </vue-good-table>
        <div v-if="showHistory">
            <dashboard-history :subsectionName="historySubsection" :key="componentKey"></dashboard-history>
        </div>
        <div v-if="showVendorHistory">
            <vendor-dashboard-history :subsectionName="vendorHistorySubsection"
                :key="vendorComponentKey"></vendor-dashboard-history>
        </div>
    </div>
</template>
  
<script>
import 'vue-good-table-next/dist/vue-good-table-next.css'
import { VueGoodTable } from 'vue-good-table-next';
import DashboardHistoryTable from "./DashboardHistoryTable.vue";
import VendorDashboardHistoryTable from "./DashboardVendorHistoryTable.vue";
import { Base64 } from "js-base64";
// import Popper from "vue3-popper";
import '../../../css/styles.css';
import 'bootstrap'

export default {
    components: {
        VueGoodTable,
        // Popper,
        "dashboard-history": DashboardHistoryTable,
        "vendor-dashboard-history": VendorDashboardHistoryTable
    },
    data() {
        return {
            componentKey: 0,
            vendorComponentKey: 0,
            historySubsection: '',
            vendorHistorySubsection: '',
            searchItem: '',
            isLoading: true,
            showHistory: false,
            showVendorHistory: false,
            columns: [
                {
                    label: 'Main Section',
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
                //     label: 'Approval Status',
                //     field: 'approval_status',
                //     sortable: false,
                //     tdClass: 'text-center align-middle',
                //     thClass: 'text-center align-middle',
                // },
                {
                    label: 'User Confirmation Status / History',
                    field: 'user_approval_history',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Admin Approval / History',
                    field: 'vendor_approval_history',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
            ],

            // statusColumns: [
            //     {
            //         label: 'Approval Status',
            //         field: 'approval_status',
            //         sortable: false,
            //         tdClass: 'text-center align-middle',
            //         thClass: 'text-center align-middle',
            //     },
            //     {
            //         label: 'Approved/Rejected By',
            //         field: 'approval_by',
            //         sortable: false,
            //         tdClass: 'text-center align-middle',
            //         thClass: 'text-center align-middle',
            //     },
            //     {
            //         label: 'Admin Approval Status',
            //         field: 'vendor_approval_status',
            //         sortable: false,
            //         tdClass: 'text-center align-middle',
            //         thClass: 'text-center align-middle',
            //     },
            //     {
            //         label: 'Approved/Rejected By (Admin)',
            //         field: 'vendor_approval_by',
            //         sortable: false,
            //         tdClass: 'text-center align-middle',
            //         thClass: 'text-center align-middle',
            //     },
            // ],

            rows: [
            ],
            totalRecords: 10,
            serverParams: {
                sortField: 'module_name',
                sortType: 'asc',
                page: 1,
                perPage: 10,
                searchText: '',
                moduleName: this.moduleName
            }
        };
    },

    props: {
        moduleName: {
            type: String,
            required: true
        }
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

        // load items is what brings back the rows from server
        loadItems() {
            this.$http.get(`/getDashboardData?serverParams=${encodeURIComponent(JSON.stringify(this.serverParams))}`, {
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
        viewUserApprovalHistory(subsectionName) {
            this.showVendorHistory = false;
            this.historySubsection = subsectionName;
            this.showHistory = true;
            this.componentKey += 1;
        },
        viewVendorApprovalHistory(subsectionName) {
            this.showHistory = false;
            this.vendorHistorySubsection = subsectionName;
            this.showVendorHistory = true;
            this.vendorComponentKey += 1;
        }
    },

    mounted() {
        this.loadItems();
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

:deep(.popper #arrow::before) {
    background: #9DB2BF;
}

:deep(.popper:hover),
:deep(.popper:hover > #arrow::before) {
    background: #9DB2BF;
}

.ri-search-eye-line {
    font-size: 20px;
}

.ri-task-line {
    font-size: 20px;
}

:deep(.footer__row-count__select) {
    cursor: pointer;
}
</style>

