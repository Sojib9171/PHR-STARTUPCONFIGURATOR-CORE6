import { defineStore } from 'pinia';

export const useMyStore = defineStore({
  id: 'myStore',
  state: () => ({
    data: null,
    leaveTypeName: null,
    option2: null,
    option3: null,
    option4: null,
    option5: null,
    option6: null,
    leaveWizardOptions:[]
  }),
  actions: {
    setData(value) {
      this.data = value;
    },

    addLeaveOption(val)
    {
      this.leaveWizardOptions.push(val);
    },
    
    setLeaveTypeName(value) {
      this.leaveTypeName = value;
    },
    setOption2(value) {
      this.option2 = value;
    },
    setOption3(value) {
      this.option3 = value;
    },
    setOption4(value) {
      this.option4 = value;
    },
    setOption5(value) {
      this.option5 = value;
    },
    setOption6(value) {
      this.option6 = value;
    },
  },
});