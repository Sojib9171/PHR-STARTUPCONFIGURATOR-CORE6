<template>
    <div>
        <button @click="showModal = true" type="button" id="historyBtn" class="btn btn-primary" data-bs-toggle="modal"
            data-bs-target="#dashboradHistryModal" hidden>
        </button>
        <!-- Modal -->
        <div class="modal fade" id="dashboradHistryModal" tabindex="-1" aria-labelledby="dashboradHistryModalLabel"
            aria-hidden="true">
            <div class="modal-dialog modal-xl modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="dashboradHistryModalLabel">Approval History - {{ subsectionName }}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div>
                            <div class="search-container">
                                <div class="input-group">
                                    <span class="input-group-text"><i class="ri-search-line"></i></span>
                                    <input type="text" class="form-control form-control-sm" v-model="searchItem"
                                        @input="onInputChange" placeholder="Search This Table">
                                </div>
                            </div>
                            <vue-good-table mode="remote" v-on:page-change="onPageChange"
                                v-on:sort-change="onSortChange($event[0], $event[1])" v-on:per-page-change="onPerPageChange"
                                :totalRows="totalRecords" v-model="isLoading" :pagination-options="{
                                    enabled: true,
                                }" :loading=true :rows="rows" :columns="columns">
                                <template #table-row="props">
                                    <span v-if="props.column.field == 'comment'">
                                        <i class="ri-message-line" data-bs-toggle="tooltip" data-bs-placement="bottom"
                                            :title="props.row.comment" ref="info"></i>
                                    </span>

                                    <span v-else-if="props.column.field == 'approval_status'">
                                        <span class="badge bg-success"
                                            v-if="props.row.approval_status === 'Approved'">Confirmed</span>
                                        <span class="badge bg-danger" v-if="props.row.approval_status === 'Rejected'">{{
                                            props.row.approval_status }}</span>
                                    </span>

                                    <span v-else-if="props.column.field == 'approval_data'">
                                        <button class="btn icon-button" @click="downloadExcel(props.row.approval_data)"><i
                                                class="ri-file-excel-2-fill"></i></button>
                                    </span>

                                    <span v-else-if="props.column.field == 'signoff_data'">
                                        <button class="btn icon-button" @click="downloadSignOff(props.row.signoff_data)"><i
                                                class="ri-file-download-line"></i></button>
                                    </span>

                                    <span v-else>
                                        {{ props.formattedRow[props.column.field] }}
                                    </span>
                                </template>
                            </vue-good-table>
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
import $ from 'jquery';
import { Tooltip } from 'bootstrap';
import { Base64 } from "js-base64";

export default {
    components: {
        VueGoodTable,
    },

    props: {
        'subsectionName': String
    },

    data() {
        return {
            historySubsection: '',
            showModal: false,
            searchItem: '',
            isLoading: true,
            columns: [
                {
                    label: 'Date',
                    field: 'approval_date',
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Confirmation Status',
                    field: 'approval_status',
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Confirmed/Rejected By',
                    field: 'approval_by',
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
                {
                    label: 'Comment',
                    field: 'comment',
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
                {
                    label: 'Sign-Off Document',
                    field: 'signoff_data',
                    sortable: false,
                    tdClass: 'text-center align-middle',
                    thClass: 'text-center align-middle',
                },
            ],
            rows: [
            ],
            totalRecords: 10,
            serverParams: {
                subsection: this.subsectionName,
                sortField: 'approval_date',
                sortType: 'desc',
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

        showComment(comment) {
            alert(comment); // Example: Show the comment in an alert
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
            this.$http.get(`/getDashboardHistoryData?serverParams=${encodeURIComponent(JSON.stringify(this.serverParams))}`, {
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

        async downloadExcel(excelData) {
            const link = document.createElement("a");
            link.href =
                "data:application/octet-stream;charset=utf-8;base64," + excelData;
            link.target = "_blank";
            link.download = this.subsectionName + '.xlsm';
            link.click();
        },

        async downloadSignOff(pdfData) {
            const binaryData = atob(pdfData);
            const uint8Array = new Uint8Array(binaryData.length);

            for (let i = 0; i < binaryData.length; i++) {
                uint8Array[i] = binaryData.charCodeAt(i);
            }

            const blob = new Blob([uint8Array], { type: 'application/pdf' });
            const url = window.URL.createObjectURL(blob);
            //window.open(url, '_blank');
            const link = document.createElement('a');
            link.href = url;
            link.download = 'PeopleHR Config Assist-Approval Report.pdf';
            link.target = '_blank';
            link.click();
        }
    },

    mounted() {
        this.loadItems();
        $('#historyBtn').click();

        new Tooltip(document.body, {
            selector: "[data-bs-toggle='tooltip']",
        });
    }
};
</script>

<style scoped>
.search-container {
    display: flex;
    justify-content: flex-end;
    margin-bottom: 2px;
}

.input-group {
    width: 200px;
}

.ri-message-line {
    font-size: 20px;
}
</style>