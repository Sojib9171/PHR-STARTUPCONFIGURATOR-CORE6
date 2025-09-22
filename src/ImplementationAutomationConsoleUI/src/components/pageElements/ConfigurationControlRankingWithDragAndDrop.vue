<template>
    <div class="section-body">
        <div class="inner-wrapper p-3 mb-5">
            <div class="block-data">
                <div class="p-3">
                    <h3 class="block-title mb-3">Drag and drop the respective sections, sorting them in the order you wish to configure the data</h3>
                    <div class="row justify-content-center align-items-center pt-3">
                        <div class="col-12">
                            <ul class="list-group">
                                <draggable :list="modules" item-key="moduleId" class="list-group" ghost-class="ghost"
                                    :move="checkMoveForModule" @start="dragging = true" @end="dragging = false">
                                    <template #item="{ element, index }">
                                        <li class="list-group-item  mb-1">
                                            <span :class="{'disable':element.moduleId==2 || element.order!=0}">
                                                <span class="custom-ranking-number">{{ index + 1
                                                }}</span>
                                                <label class="col-sm-10 col-form-label">
                                                    <h3>{{ element.moduleName }}</h3>
                                                </label>
                                                <i class="ri-drag-move-2-line fl-r" v-if="element.moduleId != 2 && element.order==0"></i>
                                            </span>
                                            <ul class="list-group m-l-10">
                                                <div class="row">
                                                    <div class="col-6" v-if="element.moduleId == 2">
                                                        <draggable :list="eimSubsections" item-key="subsectionId"
                                                            class="list-group" ghost-class="ghost" :move="checkMoveForEIM"
                                                            @start="dragging = true" @end="dragging = false">
                                                            <template #item="{ element, index }">
                                                                <li class="list-group-item mb-1"
                                                                    :class="{ 'disable': element.order != 0 }">
                                                                    <span class="custom-ranking-number"
                                                                        :class="{ 'disable not-draggable': element.subsectionId == '1' }">{{
                                                                            index + 1
                                                                        }}</span>
                                                                    {{ element.subsectionName }}
                                                                    <i class="ri-drag-move-2-line fl-r" v-if="element.subsectionId !== '1'"></i>
                                                                </li>
                                                            </template>
                                                        </draggable>
                                                    </div>
                                                    <div class="col-6" v-else-if="element.moduleId == 14">
                                                        <draggable :list="absenceSubsections" item-key="subsectionId"
                                                            class="list-group" ghost-class="ghost" @start="dragging = true"
                                                            @end="dragging = false">
                                                            <template #item="{ element, index }">
                                                                <li class="list-group-item mb-1"
                                                                    :class="{ 'disable': element.order != 0 }">
                                                                    <span class="custom-ranking-number">{{ index + 1
                                                                    }}</span>
                                                                    {{ element.subsectionName }}
                                                                    <i class="ri-drag-move-2-line fl-r"></i>
                                                                </li>
                                                            </template>
                                                        </draggable>
                                                    </div>
                                                    <div class="col-6" v-else-if="element.moduleId == 65">
                                                        <draggable :list="attendanceSubsections" item-key="subsectionId"
                                                            class="list-group" ghost-class="ghost" @start="dragging = true"
                                                            @end="dragging = false">
                                                            <template #item="{ element, index }">
                                                                <li class="list-group-item mb-1"
                                                                    :class="{ 'disable': element.order != 0 }">
                                                                    <span class="custom-ranking-number">{{ index + 1
                                                                    }}</span>
                                                                    {{ element.subsectionName }}
                                                                    <i class="ri-drag-move-2-line fl-r"></i>
                                                                </li>
                                                            </template>
                                                        </draggable>
                                                    </div>

                                                    <rawDisplayer class="col-3" :value="eimSubsections"
                                                        title="eimSubsections" />
                                                </div>
                                            </ul>
                                        </li>
                                    </template>
                                </draggable>
                            </ul>
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col-12 text-end">
                            <div class="button-group">
                                <button class="btn btn-primary" @click="updateEnabledModules()"
                                    style="margin-left: 5px;">Continue</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
  
<script>
import { addAbsenceActiveSubsectionToArray, addActiveModulesToArray, addAttendanceActiveSubsectionToArray, addEimActiveSubsectionToArray, getAbsenceActiveSubsectionArray, getActiveModulesArray, getAttendanceActiveSubsectionArray, getEimActiveSubsectionArray } from '@/localStorageUtils'
//import { Base64 } from "js-base64";
//import { extendCookieTimeout } from '@/cookieUtils'
import draggable from "vuedraggable";
export default {
    data() {
        return {
            modules: [
            ],
            eimSubsections: [
            ],
            absenceSubsections: [
            ],
            attendanceSubsections: [
            ],
        };
    },

    components: {
        draggable
    },

    methods: {
        checkMoveForEIM: function (e) {
            if (e.draggedContext.element.subsectionId == '1' || e.draggedContext.futureIndex == 0) {
                return false;
            }
        },

        checkMoveForModule: function (e) {
            const fIndex=e.draggedContext.futureIndex;
            if (e.draggedContext.element.moduleId == 2 ||e.draggedContext.element.order != 0 ||fIndex == 0) {
                return false;
            }
            if(this.modules[fIndex].order!==0)
            {
                return false;
            }
        },

        async updateEnabledModules() {
            addActiveModulesToArray(this.modules);
            addEimActiveSubsectionToArray(this.eimSubsections);
            addAbsenceActiveSubsectionToArray(this.absenceSubsections);
            addAttendanceActiveSubsectionToArray(this.attendanceSubsections);
            this.$router.push({ name: 'configuration-control-commit' })
        },
    },

    mounted() {
        this.modules = getActiveModulesArray();
        const eimHasNonZeroOrder = getEimActiveSubsectionArray().some(item => item.order !== 0);
        if (eimHasNonZeroOrder) {
            const nonZeroOrder = getEimActiveSubsectionArray().filter(item => item.order !== 0).sort((a, b) => a.order - b.order);
            const zeroOrder = getEimActiveSubsectionArray().filter(item => item.order === 0);
            const concatenatedOrdersArray = nonZeroOrder.concat(zeroOrder);
            this.eimSubsections=concatenatedOrdersArray;
        }
        else {
            this.eimSubsections = getEimActiveSubsectionArray().sort((a, b) => a.subsectionId.localeCompare(b.subsectionId));
        }

        const absenceHasNonZeroOrder = getAbsenceActiveSubsectionArray().some(item => item.order !== 0);
        if (absenceHasNonZeroOrder) {
            const nonZeroOrder = getAbsenceActiveSubsectionArray().filter(item => item.order !== 0).sort((a, b) => a.order - b.order);
            const zeroOrder = getAbsenceActiveSubsectionArray().filter(item => item.order === 0);
            const concatenatedOrdersArray = nonZeroOrder.concat(zeroOrder);
            this.absenceSubsections=concatenatedOrdersArray;
        }
        else {
            this.absenceSubsections = getAbsenceActiveSubsectionArray().sort((a, b) => a.subsectionId.localeCompare(b.subsectionId));
        }

        const attendanceHasNonZeroOrder = getAttendanceActiveSubsectionArray().some(item => item.order !== 0);
        if (attendanceHasNonZeroOrder) {
            const nonZeroOrder = getAttendanceActiveSubsectionArray().filter(item => item.order !== 0).sort((a, b) => a.order - b.order);
            const zeroOrder = getAttendanceActiveSubsectionArray().filter(item => item.order === 0);
            const concatenatedOrdersArray = nonZeroOrder.concat(zeroOrder);
            this.attendanceSubsections=concatenatedOrdersArray;
        }
        else {
            this.attendanceSubsections = getAttendanceActiveSubsectionArray().sort((a, b) => a.subsectionId.localeCompare(b.subsectionId));
        }
    },
    computed: {
        absenceEmpty() {
            return this.absenceSubsections.length === 0;
        },
        attendanceEmpty() {
            return this.attendanceSubsections.length === 0;
        },
    },

};
</script>

<style scoped>
.borderless {
    border: none;
}

.disable {
    pointer-events: none;
    opacity: 0.5;
    /* Optionally reduce the opacity for disabled appearance */
}

.buttons {
    margin-top: 35px;
}

.ghost {
    opacity: 0.5;
    background: #c8ebfb;
}

.not-draggable {
    cursor: no-drop;
}

.custom-ranking-number {
    background-color: #3498db;
    color: #fff;
    padding: 5px 10px;
    border-radius: 5px;
    margin-right: 10px;
}

.m-l-10 {
    margin-left: 10px;
}

.fl-r {
    float: right;
}
</style>
  