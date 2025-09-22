<template>
    <div class="section-body">
        <div class="inner-wrapper p-3 mb-5">
            <div class="block-data">
                <div class="card p-3">
                    <h3 class="block-title mb-3">Select the check box that you wish to configure the data</h3>
                    <div class="row justify-content-center align-items-center pt-3">
                        <div class="col-12">
                            <div class="form-check" :class="{ 'disable': true }">
                                <input class="form-check-input" type="checkbox" id="eimCheckbox" v-model="eimSelected"
                                    @change="toggleEimCheckboxes" />
                                <label class="form-check-label" for="eimCheckbox">
                                    EIM
                                </label>
                            </div>

                            <!-- Nested checkboxes for EIM-->
                            <div :class="{ 'disable': !eimSelected }" class="ml20">
                                <div class="form-check disable">
                                    <input class="form-check-input" type="checkbox" id="EmployeeData" v-model="EmployeeData"
                                        :value="1" @change="updateEimSelectedArray('1')" />
                                    <label class="form-check-label" for="EmployeeData">
                                        Employee Data
                                    </label>
                                </div>
                                <div class="form-check" :class="{ 'disable': BankDetailsPrevSelected }">
                                    <input class="form-check-input" type="checkbox" id="BankDetails" v-model="BankDetails"
                                        :value="2"
                                        @change="updateEimSelectedArray({ subsectionName: 'Bank Details', subsectionId: '2', order: 0 })" />
                                    <label class="form-check-label" for="BankDetails">
                                        Bank Details
                                    </label>
                                </div>
                                <div class="form-check" :class="{ 'disable': RepHierarchyPrevSelected }">
                                    <input class="form-check-input" type="checkbox" id="RepHierarchy" v-model="RepHierarchy"
                                        :value="3"
                                        @change="updateEimSelectedArray({ subsectionName: 'Reporting Hierarchy', subsectionId: '3', order: 0 })" />
                                    <label class="form-check-label" for="RepHierarchy">
                                        Reporting Hierarchy
                                    </label>
                                </div>
                                <div class="form-check" :class="{ 'disable': DepInfoPrevSelected }">
                                    <input class="form-check-input" type="checkbox" id="DepInfo" v-model="DepInfo"
                                        :value="4"
                                        @change="updateEimSelectedArray({ subsectionName: 'Dependent Information', subsectionId: '4', order: 0 })" />
                                    <label class="form-check-label" for="DepInfo">
                                        Dependent Information
                                    </label>
                                </div>
                                <div class="form-check" :class="{ 'disable': EmContactPrevSelected }">
                                    <input class="form-check-input" type="checkbox" id="EmContact" v-model="EmContact"
                                        :value="5"
                                        @change="updateEimSelectedArray({ subsectionName: 'Emergency Contact', subsectionId: '5', order: 0 })" />
                                    <label class="form-check-label" for="EmContact">
                                        Emergency Contact
                                    </label>
                                </div>
                            </div>

                            <!-- Absence -->
                            <div class="form-check mt-3" v-if="absenceActive" :class="{ 'disable': absencePrevSelected }">
                                <input class="form-check-input" type="checkbox" id="absenceCheckbox"
                                    v-model="absenceSelected" @change="toggleAbsenceCheckboxes" />
                                <label class="form-check-label" for="absenceCheckbox">
                                    Leave
                                </label>
                            </div>

                            <!-- Nested checkboxes for Absence-->
                            <div :class="{ 'disable': !absenceSelected }" class="ml-10" style="margin-left: 20px;"
                                v-if="absenceActive">
                                <div class="form-check" :class="{ 'disable': StatLeavePrevSelected }">
                                    <input class="form-check-input" type="checkbox" id="statLeave" v-model="StatLeave"
                                        @change="updateAbsenceSelectedArray({ subsectionName: 'Statutory Leave', subsectionId: '6', order: 0 })" />
                                    <label class="form-check-label" for="statLeave">
                                        Statutory Leave
                                    </label>
                                </div>
                                <div class="form-check" :class="{ 'disable': ShortLeavePrevSelected }">
                                    <input class="form-check-input" type="checkbox" id="shortLeave" v-model="ShortLeave"
                                        @change="updateAbsenceSelectedArray({ subsectionName: 'Short Leave', subsectionId: '7', order: 0 })" />
                                    <label class="form-check-label" for="shortLeave">
                                        Short Leave
                                    </label>
                                </div>
                            </div>

                            <!-- Attendance -->
                            <div class="form-check mt-3" v-if="attendanceActive"
                                :class="{ 'disable': attendancePrevSelected }">
                                <input class="form-check-input" type="checkbox" id="attendanceCheckbox"
                                    v-model="attendanceSelected" @change="toggleAttendanceCheckboxes" />
                                <label class="form-check-label" for="attendanceCheckbox">
                                    Attendance
                                </label>
                            </div>

                            <!-- Nested checkboxes for Attendance-->
                            <div :class="{ 'disable': !attendanceSelected }" class="ml-10" style="margin-left: 20px;"
                                v-if="attendanceActive">
                                <div class="form-check" :class="{ 'disable': ShiftInfoPrevSelected }">
                                    <input class="form-check-input" type="checkbox" id="shiftInfo" v-model="ShiftInfo"
                                        @change="updateAttendanceSelectedArray({ subsectionName: 'Shift Information', subsectionId: '8', order: 0 })" />
                                    <label class="form-check-label" for="shiftInfo">
                                        Shift Information
                                    </label>
                                </div>
                                <div class="form-check" :class="{ 'disable': RosterInfoPrevSelected }">
                                    <input class="form-check-input" type="checkbox" id="rosterInfo" v-model="RosterInfo"
                                        @change="updateAttendanceSelectedArray({ subsectionName: 'Roster Information', subsectionId: '9', order: 0 })" />
                                    <label class="form-check-label" for="rosterInfo">
                                        Roster Information
                                    </label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
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
    </div>
</template>
  
<script>
import { addActiveModulesToArray, addEimActiveSubsectionToArray, addAbsenceActiveSubsectionToArray, addAttendanceActiveSubsectionToArray, emptyAbsenceActiveSubsectionArray, emptyAttendanceActiveSubsectionArray } from '@/localStorageUtils'
//import { getAbsenceActiveSubsectionArray, getActiveModulesArray, getAttendanceActiveSubsectionArray, getEimActiveSubsectionArray } from '@/localStorageUtils'
import { Base64 } from "js-base64";
import { extendCookieTimeout } from '@/cookieUtils';

export default {
    data() {
        return {
            eimSelected: true,
            EmployeeData: true,
            BankDetails: false,
            RepHierarchy: false,
            DepInfo: false,
            EmContact: false,
            EimSubsections: [],
            absenceSelected: false,
            attendanceSelected: false,
            StatLeave: false,
            ShortLeave: false,
            AbsenceSubsections: [],
            ActiveModules: [],
            AttendanceSubsections: [],
            ShiftInfo: false,
            RosterInfo: false,
            eimActive: false,
            absenceActive: false,
            attendanceActive: false,

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
        };
    },

    async mounted() {
        await this.$http
            .get(
                "/GetActiveModules",
                {
                    headers: {
                        accept: "*/*",
                        Authorization:
                            "Basic " +
                            Base64.toBase64(
                                this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                            ),
                    }
                }
            )
            .then((resp) => {
                if (resp.data.some(obj => obj.moduleId === 2 && obj.order === 0)) {
                    this.eimActive = true;
                }
                else if (resp.data.some(obj => obj.moduleId === 2 && obj.order !== 0)) {
                    this.eimActive = true;
                    this.eimSelected = true;
                    this.eimPrevSelected = true;
                    this.ActiveModules.push(resp.data.find(obj => obj.moduleId === 2));
                }

                if (resp.data.some(obj => obj.moduleId === 14 && obj.order === 0)) {
                    this.absenceActive = true;
                }
                else if (resp.data.some(obj => obj.moduleId === 14 && obj.order !== 0)) {
                    this.absenceActive = true;
                    this.absenceSelected = true;
                    this.absencePrevSelected = true;
                    this.ActiveModules.push(resp.data.find(obj => obj.moduleId === 14));
                }


                if (resp.data.some(obj => obj.moduleId === 65 && obj.order === 0)) {
                    this.attendanceActive = true;
                }
                else if (resp.data.some(obj => obj.moduleId === 65 && obj.order !== 0)) {
                    this.attendanceActive = true;
                    this.attendanceSelected = true;
                    this.attendancePrevSelected = true;
                    this.ActiveModules.push(resp.data.find(obj => obj.moduleId === 65));
                }
                extendCookieTimeout();
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

        await this.$http
            .get(
                "/GetActiveSubsectionsEim",
                {
                    headers: {
                        accept: "*/*",
                        Authorization:
                            "Basic " +
                            Base64.toBase64(
                                this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                            ),
                    }
                }
            )
            .then((resp) => {
                if (resp.data.some(obj => obj.subsectionId == '1')) {
                    this.EmployeeData = true;
                    this.EmployeeDataPrevSelected = true;
                    this.EimSubsections.push(resp.data.find(obj => obj.subsectionId === '1'));
                }

                if (resp.data.some(obj => obj.subsectionId == '2')) {
                    this.BankDetails = true;
                    this.BankDetailsPrevSelected = true;
                    this.EimSubsections.push(resp.data.find(obj => obj.subsectionId === '2'));
                }

                if (resp.data.some(obj => obj.subsectionId == '3')) {
                    this.RepHierarchy = true;
                    this.RepHierarchyPrevSelected = true;
                    this.EimSubsections.push(resp.data.find(obj => obj.subsectionId === '3'));
                }

                if (resp.data.some(obj => obj.subsectionId == '4')) {
                    this.DepInfo = true;
                    this.DepInfoPrevSelected = true;
                    this.EimSubsections.push(resp.data.find(obj => obj.subsectionId === '4'));
                }

                if (resp.data.some(obj => obj.subsectionId == '5')) {
                    this.EmContact = true;
                    this.EmContactPrevSelected = true;
                    this.EimSubsections.push(resp.data.find(obj => obj.subsectionId === '5'));
                }

                extendCookieTimeout();
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

        await this.$http
            .get(
                "/GetActiveSubsectionsLeave",
                {
                    headers: {
                        accept: "*/*",
                        Authorization:
                            "Basic " +
                            Base64.toBase64(
                                this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                            ),
                    }
                }
            )
            .then((resp) => {
                if (resp.data.some(obj => obj.subsectionId == '6')) {
                    this.StatLeave = true;
                    this.StatLeavePrevSelected = true;
                    this.AbsenceSubsections.push(resp.data.find(obj => obj.subsectionId === '6'));
                }

                if (resp.data.some(obj => obj.subsectionId == '7')) {
                    this.ShortLeave = true;
                    this.ShortLeavePrevSelected = true;
                    this.AbsenceSubsections.push(resp.data.find(obj => obj.subsectionId === '7'));
                }
                extendCookieTimeout();
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

        await this.$http
            .get(
                "/GetActiveSubsectionsAttendance",
                {
                    headers: {
                        accept: "*/*",
                        Authorization:
                            "Basic " +
                            Base64.toBase64(
                                this.$cookies.get("enc") + ":" + this.$cookies.get("encVal") + ":" + this.$cookies.get("userTypeCode") + ":" + this.$cookies.get("userName") + ":" + this.$cookies.get("name")
                            ),
                    }
                }
            )
            .then((resp) => {
                if (resp.data.some(obj => obj.subsectionId == '8')) {
                    this.ShiftInfo = true;
                    this.ShiftInfoPrevSelected = true;
                    this.AttendanceSubsections.push(resp.data.find(obj => obj.subsectionId === '8'));
                }

                if (resp.data.some(obj => obj.subsectionId == '9')) {
                    this.RosterInfo = true;
                    this.RosterInfoPrevSelected = true;
                    this.AttendanceSubsections.push(resp.data.find(obj => obj.subsectionId === '9'));
                }
                extendCookieTimeout();
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

    methods: {
        toggleEimCheckboxes() {
            if (this.eimSelected) {
                this.EmployeeData = true;
                this.BankDetails = true;
                this.RepHierarchy = true;
                this.DepInfo = true;
                this.EmContact = true;
                this.EimSubsections = [{ subsectionName: 'Employee Data', subsectionId: '1', order: 0 }, { subsectionName: 'Bank Details', subsectionId: '2', order: 0 }, { subsectionName: 'Reporting Hierarchy', subsectionId: '3', order: 0 }, { subsectionName: 'Dependent Information', subsectionId: '4', order: 0 }, { subsectionName: 'Emergency Contact', subsectionId: '5', order: 0 }];
                this.ActiveModules.push({ moduleName: 'EIM', moduleId: 2, order: 0 });
            }
            else {
                this.EmployeeData = false;
                this.BankDetails = false;
                this.RepHierarchy = false;
                this.DepInfo = false;
                this.EmContact = false;
                const index = this.ActiveModules.findIndex(module => module.moduleName === 'EIM' && module.moduleId === 2);
                if (index !== -1) {
                    this.ActiveModules.splice(index, 1);
                }
                this.EimSubsections = []
            }
        },

        toggleAbsenceCheckboxes() {
            if (this.absenceSelected) {
                this.ShortLeave = true;
                this.StatLeave = true;
                this.AbsenceSubsections = [{ subsectionName: 'Statutory Leave', subsectionId: '6', order: 0 }, { subsectionName: 'Short Leave', subsectionId: '7', order: 0 }];
                this.ActiveModules.push({ moduleName: 'Leave', moduleId: 14, order: 0 });
            }
            else {
                this.ShortLeave = false;
                this.StatLeave = false;
                const index = this.ActiveModules.findIndex(module => module.moduleName === 'Leave' && module.moduleId === 14);
                if (index !== -1) {
                    this.ActiveModules.splice(index, 1);
                }
                this.AbsenceSubsections = []
            }
        },

        toggleAttendanceCheckboxes() {
            if (this.attendanceSelected) {
                this.ShiftInfo = true;
                this.RosterInfo = true;
                this.AttendanceSubsections = [{ subsectionName: 'Shift Information', subsectionId: '8', order: 0 }, { subsectionName: 'Roster Information', subsectionId: '9', order: 0 }];
                this.ActiveModules.push({ moduleName: 'Attendance', moduleId: 65, order: 0 });
            }
            else {
                this.ShiftInfo = false;
                this.RosterInfo = false;
                const index = this.ActiveModules.findIndex(module => module.moduleName === 'Attendance' && module.moduleId === 65);
                if (index !== -1) {
                    this.ActiveModules.splice(index, 1);
                }
                this.AttendanceSubsections = []
            }
        },

        updateEimSelectedArray(subsection) {
            if (!this.EimSubsections.find(obj => obj.subsectionId === subsection.subsectionId)) {
                this.EimSubsections.push(subsection);
            }
            else {
                const index = this.EimSubsections.findIndex(obj => obj.subsectionId === subsection.subsectionId);
                if (index !== -1) {
                    this.EimSubsections.splice(index, 1);
                }
            }
        },

        updateAbsenceSelectedArray(subsection) {
            if (!this.AbsenceSubsections.find(obj => obj.subsectionId === subsection.subsectionId)) {
                this.AbsenceSubsections.push(subsection);
            }
            else {
                const index = this.AbsenceSubsections.findIndex(obj => obj.subsectionId === subsection.subsectionId);
                if (index !== -1) {
                    this.AbsenceSubsections.splice(index, 1);
                }
            }

            if (this.StatLeave == false && this.ShortLeave == false) {
                this.absenceSelected = false;
                this.AbsenceSubsections = [];
                const index = this.ActiveModules.findIndex(module => module.moduleName === 'Leave' && module.moduleId === 14);
                if (index !== -1) {
                    this.ActiveModules.splice(index, 1);
                }
            }
        },

        updateAttendanceSelectedArray(subsection) {
            if (!this.AttendanceSubsections.find(obj => obj.subsectionId === subsection.subsectionId)) {
                this.AttendanceSubsections.push(subsection);
            }
            else {
                const index = this.AttendanceSubsections.findIndex(obj => obj.subsectionId === subsection.subsectionId);
                if (index !== -1) {
                    this.AttendanceSubsections.splice(index, 1);
                }
            }

            if (this.ShiftInfo == false && this.RosterInfo == false) {
                this.attendanceSelected = false;
                this.AttendanceSubsections = [];
                const index = this.ActiveModules.findIndex(module => module.moduleName === 'Attendance' && module.moduleId === 65);
                if (index !== -1) {
                    this.ActiveModules.splice(index, 1);
                }
            }
        },

        async updateEnabledModules() {
            const hasEmpData = this.EimSubsections.some(x => x.subsectionId == '1');
            if (!hasEmpData) {
                this.EimSubsections.push({ subsectionName: 'Employee Data', subsectionId: '1', order: 0 });
            }

            const hasEim = this.ActiveModules.some(x => x.moduleId == 2);
            if (!hasEim) {
                this.ActiveModules.push({ moduleName: 'EIM', moduleId: 2, order: 0 });
            }
            addActiveModulesToArray(this.ActiveModules);
            addEimActiveSubsectionToArray(this.EimSubsections);
            if (this.AbsenceSubsections.length !== 0) {
                addAbsenceActiveSubsectionToArray(this.AbsenceSubsections);
            }
            else {
                emptyAbsenceActiveSubsectionArray();
            }

            if (this.AttendanceSubsections.length !== 0) {
                addAttendanceActiveSubsectionToArray(this.AttendanceSubsections);
            }
            else {
                emptyAttendanceActiveSubsectionArray();
            }
            this.$router.push({ name: 'configuration-control-ranking' });
        }
    },
};
</script>
  
<style scoped>
.disable {
    pointer-events: none;
    opacity: 0.6;
    /* Optionally reduce the opacity for disabled appearance */
}

.ml20 {
    margin-left: 20px;
}
</style>
  