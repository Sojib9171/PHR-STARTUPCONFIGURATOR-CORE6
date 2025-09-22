<template>
    <div class="section-body">
        <div class="inner-wrapper p-3 mb-5">
            <div class="block-data">
                <div class="p-3">
                    <h3 class="block-title mb-3">Enter the respective number for each section in which order you wish to
                        configure the data</h3>
                    <div class="row justify-content-center align-items-center pt-3">
                        <div class="col-12">
                            <ul class="list-group">
                                <li class="list-group-item">
                                    <div class="form-group row" :class="{ 'disable': true }">
                                        <div class="col-sm-2">
                                            <input type="number" class="form-control" :min="minCountModule"
                                                :max="moduleCount" step="1" v-model="eimRank" @input="validateEimRank">
                                        </div>
                                        <label class="col-sm-10 col-form-label">
                                            <h3>EIM</h3>
                                        </label>
                                    </div>
                                    <ul class="list-group">
                                        <li class="list-group-item borderless">
                                            <div class="form-group row disable">
                                                <div class="col-sm-2">
                                                    <input type="number" class="form-control" min="1"
                                                        :max="eimSubsectionCount" step="1" v-model="employeeDataOrder">
                                                </div>
                                                <label class="col-sm-10 col-form-label">Employee Data</label>
                                            </div>
                                        </li>
                                        <li class="list-group-item borderless" v-if="bankDetailsActive"
                                            :class="{ 'disable': BankDetailsPrevSelected }">
                                            <div class="form-group row">
                                                <div class="col-sm-2">
                                                    <input type="number" class="form-control" :min="minCountEim"
                                                        :max="eimSubsectionCount" step="1" v-model="bankDetailsOrder">
                                                </div>
                                                <label class="col-sm-10 col-form-label">Bank Details</label>
                                            </div>
                                        </li>
                                        <li class="list-group-item borderless" v-if="reportingHierarchyActive"
                                            :class="{ 'disable': RepHierarchyPrevSelected }">
                                            <div class="form-group row">
                                                <div class="col-sm-2">
                                                    <input type="number" class="form-control" :min="minCountEim"
                                                        :max="eimSubsectionCount" step="1"
                                                        v-model="reportingHierarchyOrder">
                                                </div>
                                                <label class="col-sm-10 col-form-label">Reporting Hierarchy</label>
                                            </div>
                                        </li>
                                        <li class="list-group-item borderless" v-if="dependentInfoActive"
                                            :class="{ 'disable': DepInfoPrevSelected }">
                                            <div class="form-group row">
                                                <div class="col-sm-2">
                                                    <input type="number" class="form-control" :min="minCountEim"
                                                        :max="eimSubsectionCount" step="1" v-model="dependentInfoOrder">
                                                </div>
                                                <label class="col-sm-10 col-form-label">Dependent Information</label>
                                            </div>
                                        </li>
                                        <li class="list-group-item borderless" v-if="emergencyContactActive"
                                            :class="{ 'disable': EmContactPrevSelected }">
                                            <div class="form-group row">
                                                <div class="col-sm-2">
                                                    <input type="number" class="form-control" :min="minCountEim"
                                                        :max="eimSubsectionCount" step="1" v-model="emergencyContactOrder">
                                                </div>
                                                <label class="col-sm-10 col-form-label">Emergency Contact</label>
                                            </div>
                                        </li>
                                    </ul>
                                </li>
                            </ul>

                            <ul class="list-group mt-2" v-if="!absenceEmpty">
                                <li class="list-group-item">
                                    <div class="form-group row" :class="{ 'disable': absencePrevSelected }">
                                        <div class="col-sm-2">
                                            <input type="number" class="form-control" :min="minCountModule"
                                                :max="moduleCount" step="1" v-model="absenceRank" @blur="inputBlurHandler">
                                        </div>
                                        <label class="col-sm-10 col-form-label">
                                            <h3>Leave</h3>
                                        </label>
                                    </div>
                                    <ul class="list-group">
                                        <li class="list-group-item borderless" v-if="statLeaveActive"
                                            :class="{ 'disable': StatLeavePrevSelected }">
                                            <div class="form-group row">
                                                <div class="col-sm-2">
                                                    <input type="number" class="form-control" :min="minCountLeave"
                                                        :max="absenceSubsectionCount" step="1" v-model="statLeaveOrder">
                                                </div>
                                                <label class="col-sm-10 col-form-label">Statutory Leave</label>
                                            </div>
                                        </li>
                                        <li class="list-group-item borderless" v-if="shortLeaveActive"
                                            :class="{ 'disable': ShortLeavePrevSelected }">
                                            <div class="form-group row">
                                                <div class="col-sm-2">
                                                    <input type="number" class="form-control" :min="minCountLeave"
                                                        :max="absenceSubsectionCount" step="1" v-model="shortLeaveOrder">
                                                </div>
                                                <label class="col-sm-10 col-form-label">Short Leave</label>
                                            </div>
                                        </li>
                                    </ul>
                                </li>
                            </ul>

                            <ul class="list-group mt-2" v-if="!attendanceEmpty">
                                <li class="list-group-item">
                                    <div class="form-group row" :class="{ 'disable': attendancePrevSelected }">
                                        <div class="col-sm-2">
                                            <input type="number" class="form-control" :min="minCountModule"
                                                :max="moduleCount" step="1" v-model="attendanceRank">
                                        </div>
                                        <label class="col-sm-10 col-form-label">
                                            <h3>Attendance</h3>
                                        </label>
                                    </div>
                                    <ul class="list-group">
                                        <li class="list-group-item borderless" v-if="shiftInfoActive"
                                            :class="{ 'disable': ShiftInfoPrevSelected }">
                                            <div class="form-group row">
                                                <div class="col-sm-2">
                                                    <input type="number" class="form-control" :min="minCountAttendance"
                                                        :max="attendanceSubsectionCount" step="1" v-model="shiftInfoOrder">
                                                </div>
                                                <label class="col-sm-10 col-form-label">Shift Information</label>
                                            </div>
                                        </li>
                                        <li class="list-group-item borderless" v-if="rosterInfoActive"
                                            :class="{ 'disable': RosterInfoPrevSelected }">
                                            <div class="form-group row">
                                                <div class="col-sm-2">
                                                    <input type="number" class="form-control" :min="minCountAttendance"
                                                        :max="attendanceSubsectionCount" step="1" v-model="rosterInfoOrder">
                                                </div>
                                                <label class="col-sm-10 col-form-label">Roster Information</label>
                                            </div>
                                        </li>
                                    </ul>
                                </li>
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
export default {
    data() {
        return {
            tempModuleRank: null,
            tempEimSubsecOrder: null,
            tempAbsenceSubsecOrder: null,
            tempAttendanceSubsecOrder: null,
            eimRank: null,
            absenceRank: null,
            attendanceRank: null,
            modules: [
            ],
            eimSubsections: [
            ],
            absenceSubsections: [
            ],
            attendanceSubsections: [
            ],

            moduleCount: 0,
            eimSubsectionCount: 0,
            absenceSubsectionCount: 0,
            attendanceSubsectionCount: 0,
            employeeDataOrder: null,
            bankDetailsOrder: null,
            reportingHierarchyOrder: null,
            dependentInfoOrder: null,
            emergencyContactOrder: null,
            statLeaveOrder: null,
            shortLeaveOrder: null,
            shiftInfoOrder: null,
            rosterInfoOrder: null,
            employeeDataActive: null,
            bankDetailsActive: null,
            reportingHierarchyActive: null,
            dependentInfoActive: null,
            emergencyContactActive: null,
            statLeaveActive: null,
            shortLeaveActive: null,
            shiftInfoActive: null,
            rosterInfoActive: null,

            eimPrevSelected: false,
            absencePrevSelected: false,
            attendancePrevSelected: false,
            EmployeeDataPrevSelected: false,
            BankDetailsPrevSelected: false,
            RepHierarchyPrevSelected: false,
            DepInfoPrevSelected: false,
            EmContactPrevSelected: false,
            StatLeavePrevSelected: false,
            ShortLeavePrevSelected: false,
            ShiftInfoPrevSelected: false,
            RosterInfoPrevSelected: false,
            minCountModule: 2,
            minCountEim: 2,
            minCountLeave: 1,
            minCountAttendance: 1,
            minCountFixed: 0
        };
    },

    methods: {
        validateEimRank() {
            if (this.eimRank == '') {
                this.eimRank = 1;
            }
            else if (this.eimRank < 1) {
                this.eimRank = 1;
            } else if (this.eimRank > this.moduleCount) {
                this.eimRank = this.moduleCount;
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
        this.eimSubsections = getEimActiveSubsectionArray();
        this.absenceSubsections = getAbsenceActiveSubsectionArray();
        this.attendanceSubsections = getAttendanceActiveSubsectionArray();

        this.moduleCount = getActiveModulesArray().length;
        const eimModule = getActiveModulesArray().find((module) => module.moduleId === 2);
        const absenceModule = getActiveModulesArray().find((module) => module.moduleId === 14);
        const attendanceModule = getActiveModulesArray().find((module) => module.moduleId === 65);

        this.tempModuleRank = this.modules.reduce((max, obj) => {
            return obj.order > max ? obj.order : max;
        }, this.modules[0].order) + 1;

        if (eimModule) {
            if (eimModule.order != 0) {
                this.eimRank = eimModule.order;
                this.eimPrevSelected = true;
                this.minCountModule = this.tempModuleRank;
            }
            else {
                this.eimRank = 1;
                this.tempModuleRank++;
            }
        }

        if (absenceModule) {
            if (absenceModule.order != 0) {
                this.absenceRank = absenceModule.order;
                this.absencePrevSelected = true;
                this.minCountModule = this.tempModuleRank;
            }
            else {
                this.absenceRank = this.tempModuleRank;
                this.tempModuleRank++;
            }
        }

        if (attendanceModule) {
            if (attendanceModule.order != 0) {
                this.attendanceRank = attendanceModule.order;
                this.attendancePrevSelected = true;
                this.minCountModule = this.tempModuleRank;
            }
            else {
                this.attendanceRank = this.tempModuleRank;
            }
        }

        this.eimSubsectionCount = this.eimSubsections.length;
        const empData = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Employee Data');
        const bankDetails = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Bank Details');
        const reportingHie = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Reporting Hierarchy');
        const depInfo = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Dependent Information');
        const emergnContact = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Emergency Contact');

        if (this.eimSubsections.length != 0) {
            this.tempEimSubsecOrder = this.eimSubsections.reduce((max, obj) => {
                return obj.order > max ? obj.order : max;
            }, this.eimSubsections[0].order) + 1;

            this.minCountEim = 2;

            const temp = getEimActiveSubsectionArray().filter(x => x.order != 0);
            this.minCountFixed = temp.length + 1;
        }

        if (empData) {
            if (empData.order != 0) {
                this.employeeDataOrder = empData.order;
                this.EmployeeDataPrevSelected = true;
                //this.minCountEim = this.tempEimSubsecOrder;
            }
            else {
                this.employeeDataOrder = this.tempEimSubsecOrder;
                this.tempEimSubsecOrder++;
            }
        }

        if (bankDetails) {
            this.bankDetailsActive = true;
            if (bankDetails.order != 0) {
                this.bankDetailsOrder = bankDetails.order;
                this.BankDetailsPrevSelected = true;
                this.minCountEim = this.minCountFixed;
            }
            else {
                this.bankDetailsOrder = this.tempEimSubsecOrder;
                this.tempEimSubsecOrder++;
            }
        }

        if (reportingHie) {
            this.reportingHierarchyActive = true;
            if (reportingHie.order != 0) {
                this.reportingHierarchyOrder = reportingHie.order;
                this.RepHierarchyPrevSelected = true;
                this.minCountEim = this.minCountFixed;
            }
            else {
                this.reportingHierarchyOrder = this.tempEimSubsecOrder;
                this.tempEimSubsecOrder++;
            }
        }

        if (depInfo) {
            this.dependentInfoActive = true;
            if (depInfo.order != 0) {
                this.dependentInfoOrder = depInfo.order;
                this.DepInfoPrevSelected = true;
                this.minCountEim = this.minCountFixed;
            }
            else {
                this.dependentInfoOrder = this.tempEimSubsecOrder;
                this.tempEimSubsecOrder++;
            }

        }

        if (emergnContact) {
            this.emergencyContactActive = true;
            if (emergnContact.order != 0) {
                this.emergencyContactOrder = emergnContact.order;
                this.EmContactPrevSelected = true;
                this.minCountEim = this.minCountFixed;
            }
            else {
                this.emergencyContactOrder = this.tempEimSubsecOrder;
                this.tempEimSubsecOrder++;
            }
        }

        this.absenceSubsectionCount = this.absenceSubsections.length;
        const statutryLeave = this.absenceSubsections.find((subsec) => subsec.subsectionName === 'Statutory Leave');
        const shortLeave = this.absenceSubsections.find((subsec) => subsec.subsectionName === 'Short Leave');

        if (this.absenceSubsections.length != 0) {
            this.tempAbsenceSubsecOrder = this.absenceSubsections.reduce((max, obj) => {
                return obj.order > max ? obj.order : max;
            }, this.absenceSubsections[0].order) + 1;
        }

        if (statutryLeave) {
            this.statLeaveActive = true;
            if (statutryLeave.order != 0) {
                this.statLeaveOrder = statutryLeave.order;
                this.StatLeavePrevSelected = true;
                this.minCountLeave = this.tempAbsenceSubsecOrder;
            }
            else {
                this.statLeaveOrder = this.tempAbsenceSubsecOrder;
                this.tempAbsenceSubsecOrder++;
            }
        }

        if (shortLeave) {
            this.shortLeaveActive = true;
            if (shortLeave.order != 0) {
                this.shortLeaveOrder = shortLeave.order;
                this.ShortLeavePrevSelected = true;
                this.minCountLeave = this.tempAbsenceSubsecOrder;
            }
            else {
                this.shortLeaveOrder = this.tempAbsenceSubsecOrder;
            }
        }

        this.attendanceSubsectionCount = this.attendanceSubsections.length;
        const shiftInfo = this.attendanceSubsections.find((subsec) => subsec.subsectionName === 'Shift Information');
        const rosterInfo = this.attendanceSubsections.find((subsec) => subsec.subsectionName === 'Roster Information');

        // this.tempAttendanceSubsecOrder = 1;
        if (this.attendanceSubsections.length != 0) {
            this.tempAttendanceSubsecOrder = this.attendanceSubsections.reduce((max, obj) => {
                return obj.order > max ? obj.order : max;
            }, this.attendanceSubsections[0].order) + 1;
        }
        if (shiftInfo) {
            this.shiftInfoActive = true;
            if (shiftInfo.order != 0) {
                this.shiftInfoOrder = shiftInfo.order;
                this.ShiftInfoPrevSelected = true;
                this.minCountAttendance = this.tempAttendanceSubsecOrder;
            }
            else {
                this.shiftInfoOrder = this.tempAttendanceSubsecOrder;
                this.tempAttendanceSubsecOrder++;
            }
        }

        if (rosterInfo) {
            this.rosterInfoActive = true;
            if (rosterInfo.order != 0) {
                this.rosterInfoOrder = rosterInfo.order;
                this.RosterInfoPrevSelected = true;
                this.minCountAttendance = this.tempAttendanceSubsecOrder;
            }
            else {
                this.rosterInfoOrder = this.tempAttendanceSubsecOrder;
            }
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

    watch: {
        eimRank(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.eimRank = 1;
                newRank = this.eimRank;
            }
            else if (newRank > this.moduleCount) {
                this.eimRank = this.moduleCount;
                newRank = this.eimRank;
            }

            if (newRank === this.absenceRank) {
                this.absenceRank = oldValue;
            }

            else if (newRank === this.attendanceRank) {
                this.attendanceRank = oldValue;
            }

            const eimModule = this.modules.find((module) => module.moduleName === 'EIM');
            if (eimModule) {
                eimModule.order = newRank;
            }
        },

        absenceRank(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.absenceRank = 2;
                newRank = this.absenceRank;
            }
            else if (newRank > this.moduleCount) {
                this.absenceRank = this.moduleCount;
                newRank = this.absenceRank;
            }

            if (newRank === this.attendanceRank) {
                this.attendanceRank = oldValue;
            }

            const absenceModule = this.modules.find((module) => module.moduleName === 'Leave');
            if (absenceModule) {
                absenceModule.order = newRank;
            }
        },

        attendanceRank(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.attendanceRank = 2;
                newRank = this.attendanceRank;
            }
            else if (newRank > this.moduleCount) {
                this.attendanceRank = this.moduleCount;
                newRank = this.attendanceRank;
            }

            if (newRank === this.absenceRank) {
                this.absenceRank = oldValue;
            }

            const attendanceModule = this.modules.find((module) => module.moduleName === 'Attendance');
            if (attendanceModule) {
                attendanceModule.order = newRank;
            }
        },

        employeeDataOrder(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.employeeDataOrder = 1;
                newRank = this.employeeDataOrder;
            }
            else if (newRank > this.eimSubsectionCount) {
                this.employeeDataOrder = this.eimSubsectionCount;
                newRank = this.employeeDataOrder;
            }

            if (newRank === this.bankDetailsOrder) {
                this.bankDetailsOrder = oldValue;
            }

            else if (newRank === this.reportingHierarchyOrder) {
                this.reportingHierarchyOrder = oldValue;
            }

            else if (newRank === this.dependentInfoOrder) {
                this.dependentInfoOrder = oldValue;
            }

            else if (newRank === this.emergencyContactOrder) {
                this.emergencyContactOrder = oldValue;
            }


            const empData = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Employee Data');
            if (empData) {
                empData.order = newRank;
            }
        },

        bankDetailsOrder(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.bankDetailsOrder = this.minCountEim;
                newRank = this.bankDetailsOrder;
            }
            else if (newRank > this.eimSubsectionCount) {
                this.bankDetailsOrder = this.eimSubsectionCount;
                newRank = this.bankDetailsOrder;
            }

            if (newRank === this.employeeDataOrder) {
                this.employeeDataOrder = oldValue;
            }

            else if (newRank === this.reportingHierarchyOrder) {
                this.reportingHierarchyOrder = oldValue;
            }

            else if (newRank === this.dependentInfoOrder) {
                this.dependentInfoOrder = oldValue;
            }

            else if (newRank === this.emergencyContactOrder) {
                this.emergencyContactOrder = oldValue;
            }


            const bankDetails = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Bank Details');
            if (bankDetails) {
                bankDetails.order = newRank;
            }
        },

        reportingHierarchyOrder(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.reportingHierarchyOrder = this.minCountEim;
                newRank = this.reportingHierarchyOrder;
            }
            else if (newRank > this.eimSubsectionCount) {
                this.reportingHierarchyOrder = this.eimSubsectionCount;
                newRank = this.reportingHierarchyOrder;
            }

            if (newRank === this.employeeDataOrder) {
                this.employeeDataOrder = oldValue;
            }

            else if (newRank === this.bankDetailsOrder) {
                this.bankDetailsOrder = oldValue;
            }

            else if (newRank === this.dependentInfoOrder) {
                this.dependentInfoOrder = oldValue;
            }

            else if (newRank === this.emergencyContactOrder) {
                this.emergencyContactOrder = oldValue;
            }

            const repHie = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Reporting Hierarchy');
            if (repHie) {
                repHie.order = newRank;
            }
        },

        dependentInfoOrder(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.dependentInfoOrder = this.minCountEim;
                newRank = this.dependentInfoOrder;
            }
            else if (newRank > this.eimSubsectionCount) {
                this.dependentInfoOrder = this.eimSubsectionCount;
                newRank = this.dependentInfoOrder;
            }

            if (newRank === this.employeeDataOrder) {
                this.employeeDataOrder = oldValue;
            }

            else if (newRank === this.bankDetailsOrder) {
                this.bankDetailsOrder = oldValue;
            }

            else if (newRank === this.reportingHierarchyOrder) {
                this.reportingHierarchyOrder = oldValue;
            }

            else if (newRank === this.emergencyContactOrder) {
                this.emergencyContactOrder = oldValue;
            }

            const depInfo = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Dependent Information');
            if (depInfo) {
                depInfo.order = newRank;
            }
        },

        emergencyContactOrder(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.emergencyContactOrder = this.minCountEim;
                newRank = this.emergencyContactOrder;
            }
            else if (newRank > this.eimSubsectionCount) {
                this.emergencyContactOrder = this.eimSubsectionCount;
                newRank = this.emergencyContactOrder;
            }

            if (newRank === this.employeeDataOrder) {
                this.employeeDataOrder = oldValue;
            }

            else if (newRank === this.bankDetailsOrder) {
                this.bankDetailsOrder = oldValue;
            }

            else if (newRank === this.reportingHierarchyOrder) {
                this.reportingHierarchyOrder = oldValue;
            }

            else if (newRank === this.dependentInfoOrder) {
                this.dependentInfoOrder = oldValue;
            }

            const emContact = this.eimSubsections.find((subsec) => subsec.subsectionName === 'Emergency Contact');
            if (emContact) {
                emContact.order = newRank;
            }
        },

        statLeaveOrder(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.statLeaveOrder = this.minCountLeave;
                newRank = this.statLeaveOrder;
            }
            else if (newRank > this.absenceSubsectionCount) {
                this.statLeaveOrder = this.absenceSubsectionCount;
                newRank = this.statLeaveOrder;
            }

            if (newRank === this.shortLeaveOrder) {
                this.shortLeaveOrder = oldValue;
            }

            const statLeave = this.absenceSubsections.find((subsec) => subsec.subsectionName === 'Statutory Leave');
            if (statLeave) {
                statLeave.order = newRank;
            }
        },

        shortLeaveOrder(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.shortLeaveOrder = this.minCountLeave;
                newRank = this.shortLeaveOrder;
            }
            else if (newRank > this.absenceSubsectionCount) {
                this.shortLeaveOrder = this.absenceSubsectionCount;
                newRank = this.shortLeaveOrder;
            }

            if (newRank === this.statLeaveOrder) {
                this.statLeaveOrder = oldValue;
            }

            const shortLeave = this.absenceSubsections.find((subsec) => subsec.subsectionName === 'Short Leave');
            if (shortLeave) {
                shortLeave.order = newRank;
            }
        },

        shiftInfoOrder(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.shiftInfoOrder = this.minCountAttendance;
                newRank = this.shiftInfoOrder;
            }
            else if (newRank > this.attendanceSubsectionCount) {
                this.shiftInfoOrder = this.attendanceSubsectionCount;
                newRank = this.shiftInfoOrder;
            }

            if (newRank === this.rosterInfoOrder) {
                this.rosterInfoOrder = oldValue;
            }

            const shiftInfo = this.attendanceSubsections.find((subsec) => subsec.subsectionName === 'Shift Information');
            if (shiftInfo) {
                shiftInfo.order = newRank;
            }
        },

        rosterInfoOrder(newRank, oldValue) {
            if (newRank < 1 || newRank == '') {
                this.rosterInfoOrder = this.minCountAttendance;
                newRank = this.rosterInfoOrder;
            }
            else if (newRank > this.moduleCount) {
                this.rosterInfoOrder = this.moduleCount;
                newRank = this.rosterInfoOrder;
            }

            if (newRank === this.shiftInfoOrder) {
                this.shiftInfoOrder = oldValue;
            }

            const rosterInfo = this.attendanceSubsections.find((subsec) => subsec.subsectionName === 'Roster Information');
            if (rosterInfo) {
                rosterInfo.order = newRank;
            }
        },
    }
};
</script>

<style scoped>
.borderless {
    border: none;
}

.disable {
    pointer-events: none;
    opacity: 0.6;
    /* Optionally reduce the opacity for disabled appearance */
}
</style>
  