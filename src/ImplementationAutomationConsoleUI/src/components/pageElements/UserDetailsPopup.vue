<template>
    <Popper>
        <i class="ri-file-text-fill"></i>
        <template #content>
            <span>
                <vue-good-table
                    :rows="tableRows"
                    :columns="userDetailsColumns">
                    <template #table-row="params">
                        <span v-if="params.column.field == 'comment'">
                            <Popper arrow>
                                <i class="ri-file-text-fill"></i>
                                <template #content>
                                    <span>{{ params.row.approval_comment }}</span>
                                </template>
                            </Popper>
                        </span>

                        <span v-else-if="params.column.field == 'approval_status'">
                            <span class="badge bg-success" v-if="params.row.approval_status === 'Approved'">{{
                                params.row.approval_status }}</span>
                            <span class="badge bg-danger" v-else-if="params.row.approval_status === 'Rejected'">{{
                                params.row.approval_status }}</span>
                            <span class="badge bg-info" v-else>Not Approved</span>
                        </span>

                        <span v-else>
                            {{ params.formattedRow[params.column.field] }}
                        </span>
                    </template>
                </vue-good-table>
            </span>
        </template>
    </Popper>
</template>
  
<script>
import 'vue-good-table-next/dist/vue-good-table-next.css'
import { VueGoodTable } from 'vue-good-table-next';
import Popper from "vue3-popper";

export default {
    components: {
        VueGoodTable,
        Popper,
    },
    props: {
        tableRows: {
            type: Array,
            required: true,
        },
    },
};
</script>

  