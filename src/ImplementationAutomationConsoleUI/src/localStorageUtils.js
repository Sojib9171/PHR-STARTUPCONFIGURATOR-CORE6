// localStorageUtils.js

export function saveDate(dateValue) {
  localStorage.setItem('storedDate', dateValue);
}

export function getDate() {
  return localStorage.getItem('storedDate');
}

export function saveUserID(userId) {
  localStorage.setItem('userId', userId);
}

export function getUserID() {
  return localStorage.getItem('userId');
}

export function saveCommonConfigRowID(CommonConfigRowID) {
  localStorage.setItem('CommonConfigRowID', CommonConfigRowID);
}

export function getCommonConfigRowID() {
  return localStorage.getItem('CommonConfigRowID');
}

export function removeCommonConfigRowID() {
  localStorage.removeItem('CommonConfigRowID');
}

export function saveAbsenceDate(dateValue) {
  localStorage.setItem('storedAbsenceDate', dateValue);
}

export function getAbsenceDate() {
  return localStorage.getItem('storedAbsenceDate');
}

export function saveAbsenceRowID(AbsenceRowID) {
  localStorage.setItem('AbsenceRowID', AbsenceRowID);
}

export function getAbsenceRowID() {
  return localStorage.getItem('AbsenceRowID');
}

export function removeAbsenceRowID() {
  localStorage.removeItem('AbsenceRowID');
}

export function saveAbsenceSubsectionName(SubsectionName) {
  localStorage.setItem('SubsectionName', SubsectionName);
}

export function getAbsenceSubsectionName() {
  return localStorage.getItem('SubsectionName');
}

export function removeAbsenceSubsectionName() {
  localStorage.removeItem('SubsectionName');
}

export function saveCommonConfigLogoImageName(imageName) {
  localStorage.setItem('ImageName', imageName);
}

export function getCommonConfigLogoImageName() {
  return localStorage.getItem('ImageName');
}

export function removeCommonConfigLogoImageName() {
  localStorage.removeItem('ImageName');
}

export function saveCommonConfigMobileLogoImageName(imageName) {
  localStorage.setItem('MobileImageName', imageName);
}

export function getCommonConfigMobileLogoImageName() {
  return localStorage.getItem('MobileImageName');
}

export function removeCommonConfigMobileLogoImageName() {
  localStorage.removeItem('MobileImageName');
}

export function addAbsenceRowIdToArray(newItem) {
  const storedArray = localStorage.getItem('AbsenceIdRowArray');
  let updatedArray = storedArray ? JSON.parse(storedArray) : [];

  // Add the new item to the updated array

  if (!updatedArray.includes(newItem)) {
    updatedArray.push(newItem);
  }

  // Save the updated array to localStorage
  localStorage.setItem('AbsenceIdRowArray', JSON.stringify(updatedArray));
}

export function getAbsenceIdArray() {
  const storedArray = localStorage.getItem('AbsenceIdRowArray');
  return storedArray ? JSON.parse(storedArray) : [];
}

export function addAbsencePendingRowIdToArray(idArray) {
  const storedArray = localStorage.getItem('PendingAbsenceIdRowArray');
  let updatedArray = storedArray ? JSON.parse(storedArray) : [];

  // Add the new item to the updated array

  idArray.forEach(element => {
    updatedArray.push(element);
  });

  // Save the updated array to localStorage
  localStorage.setItem('PendingAbsenceIdRowArray', JSON.stringify(updatedArray));
}

export function getAbsencePendingRowIdToArray() {
  const storedArray = localStorage.getItem('PendingAbsenceIdRowArray');
  return storedArray ? JSON.parse(storedArray) : [];
}

export function removeAbsencePendingRowIdToArray() {
  localStorage.removeItem('PendingAbsenceIdRowArray');
}

export function removeAbsenceIdFromLocalStorageArray(itemToRemove) {
  const storedArray = localStorage.getItem('AbsenceIdRowArray');
  let updatedArray = storedArray ? JSON.parse(storedArray) : [];

  const arrayIndex = updatedArray.indexOf(itemToRemove.toString());
  if (arrayIndex !== -1) {
    updatedArray.splice(arrayIndex, 1);
  }
  localStorage.setItem('AbsenceIdRowArray', JSON.stringify(updatedArray));
}

export function addLeaveWizardOptionsToLocalStorage(newItem) {

  const storedArray = localStorage.getItem('leaveWizardOptions');
  let updatedArray = storedArray ? JSON.parse(storedArray) : [];
  updatedArray.push(newItem);

  localStorage.setItem('leaveWizardOptions', JSON.stringify(updatedArray));
}

export function saveLeaveWizardOptionsToLocalStorage(array) {
  localStorage.setItem('leaveWizardOptions', JSON.stringify(array));
}

export function getLeaveWizardOptionsFromLocalStorage() {
  const storedArray = localStorage.getItem('leaveWizardOptions');
  return storedArray ? JSON.parse(storedArray) : [];
}

export function removeLeaveWizardOptionsAtIndexFromLocalStorage(arrayIndex) {
  const storedArray = localStorage.getItem('leaveWizardOptions');
  let updatedArray = storedArray ? JSON.parse(storedArray) : [];

  updatedArray.splice(arrayIndex, 1);
  localStorage.setItem('leaveWizardOptions', JSON.stringify(updatedArray));
}

export function emptyLeaveWizardOptionsInLocalStorage() {
  localStorage.removeItem('leaveWizardOptions');
}

export function addLeaveTypesToLocalStorage(newItem) {
  const storedArray = localStorage.getItem('leaveTypes');
  let updatedArray = storedArray ? JSON.parse(storedArray) : [];

  updatedArray.push(newItem);

  localStorage.setItem('leaveTypes', JSON.stringify(updatedArray));
}

export function removeLeaveTypeAtIndexFromLocalStorage(arrayIndex) {
  const storedArray = localStorage.getItem('leaveTypes');
  let updatedArray = storedArray ? JSON.parse(storedArray) : [];

  updatedArray.splice(arrayIndex, 1);
  localStorage.setItem('leaveTypes', JSON.stringify(updatedArray));
}

export function addLeaveTypeAtIndexToLocalStorage(arrayIndex, newArray) {
  const storedArray = localStorage.getItem('leaveTypes');
  let updatedArray = storedArray ? JSON.parse(storedArray) : [];
  updatedArray.splice(arrayIndex, 0, newArray);
  localStorage.setItem('leaveTypes', JSON.stringify(updatedArray));
}

export function saveLeaveTypesToLocalStorage(array) {
  localStorage.setItem('leaveTypes', JSON.stringify(array));
}

export function getLeaveTypesFromLocalStorage() {
  const storedArray = localStorage.getItem('leaveTypes');
  return storedArray ? JSON.parse(storedArray) : [];
}

export function emptyLeaveTypesInLocalStorage() {
  localStorage.removeItem('leaveTypes');
}

export function addActiveModulesToArray(newItem) {
  localStorage.setItem('ActiveModules', JSON.stringify(newItem));
}

export function getActiveModulesArray() {
  const storedArray = localStorage.getItem('ActiveModules');
  return storedArray ? JSON.parse(storedArray) : [];
}

export function emptyActiveModulesArray() {
  localStorage.removeItem('ActiveModules');
}

export function addEimActiveSubsectionToArray(newItem) {
  localStorage.setItem('EimActiveSubsections', JSON.stringify(newItem));
}

export function getEimActiveSubsectionArray() {
  const storedArray = localStorage.getItem('EimActiveSubsections');
  return storedArray ? JSON.parse(storedArray) : [];
}

export function emptyEimActiveSubsectionArray() {
  localStorage.removeItem('EimActiveSubsections');
}

export function addAbsenceActiveSubsectionToArray(newItem) {
  localStorage.setItem('AbsenceActiveSubsections', JSON.stringify(newItem));
}

export function getAbsenceActiveSubsectionArray() {
  const storedArray = localStorage.getItem('AbsenceActiveSubsections');
  return storedArray ? JSON.parse(storedArray) : [];
}

export function emptyAbsenceActiveSubsectionArray() {
  localStorage.removeItem('AbsenceActiveSubsections');
}

export function addAttendanceActiveSubsectionToArray(newItem) {
  localStorage.setItem('AttendanceActiveSubsections', JSON.stringify(newItem));
}

export function getAttendanceActiveSubsectionArray() {
  const storedArray = localStorage.getItem('AttendanceActiveSubsections');
  return storedArray ? JSON.parse(storedArray) : [];
}

export function emptyAttendanceActiveSubsectionArray() {
  localStorage.removeItem('AttendanceActiveSubsections');
}

export function saveSubsecNameForMultipleUpload(subsectionName) {
  localStorage.setItem('subsectionNameForMulUpload', subsectionName);
}

export function getSubsecNameForMultipleUpload() {
  return localStorage.getItem('subsectionNameForMulUpload');
}

export function removeSubsecNameForMultipleUpload() {
  localStorage.removeItem('subsectionNameForMulUpload');
}

export function saveSubsecNameForWizardPopup(subsectionName) {
  localStorage.setItem('subsectionNameForWizardPopup', subsectionName);
}

export function getSubsecNameForWizardPopup() {
  return localStorage.getItem('subsectionNameForWizardPopup');
}

export function removeSubsecNameForWizardPopup() {
  localStorage.removeItem('subsectionNameForWizardPopup');
}



