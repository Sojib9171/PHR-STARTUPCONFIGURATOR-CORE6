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

                <span v-if="props.column.field == 'vendor_approval_history'">
                    <button class="btn icon-button" @click="viewVendorApprovalHistory(props.row.subsection_name)"><i
                            class="ri-search-eye-line"></i></button>
                </span>

                <span v-else-if="props.column.field == 'vendor_approval_status'">
                    <span class="badge bg-success" v-if="props.row.vendor_approval_status === 'Approved'">{{
                        props.row.vendor_approval_status }}</span>
                    <span class="badge bg-danger" v-else-if="props.row.vendor_approval_status === 'Rejected'">{{
                        props.row.vendor_approval_status }}</span>
                    <span class="badge bg-info" v-else>Not Approved</span>
                </span>

                <span v-else>
                    {{ props.formattedRow[props.column.field] }}
                </span>
            </template>
        </vue-good-table>
        <div v-if="showHistory">
            <admin-approval-history :subsectionName="historySubsection" :key="componentKey"></admin-approval-history>
        </div>
    </div>
</template>
  
<script>
import 'vue-good-table-next/dist/vue-good-table-next.css'
import { VueGoodTable } from 'vue-good-table-next';
import AdminApprovalHistoryTable from "./AdminApprovalSummaryHistoryTable.vue";
import { Base64 } from "js-base64";

export default {
    components: {
        VueGoodTable,
        "admin-approval-history": AdminApprovalHistoryTable,
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
                {
                    label: 'Approval Status',
                    field: 'vendor_approval_status',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Approved/Rejected By',
                    field: 'vendor_approval_by',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Admin Approval History',
                    field: 'vendor_approval_history',
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

        // load items is what brings back the rows from server
        loadItems() {
            this.$http.get(`/getApprovalSummaryListForVendor?serverParams=${encodeURIComponent(JSON.stringify(this.serverParams))}`, {
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
        viewVendorApprovalHistory(subsectionName) {
            this.historySubsection = subsectionName;
            this.showHistory = true;
            this.componentKey += 1;
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

:deep(.footer__row-count__select) {
    cursor: pointer;
}
</style>

