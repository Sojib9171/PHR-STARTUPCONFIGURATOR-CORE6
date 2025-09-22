<template>
  <button type="button" id="advanceModal1Btn" class="btn btn-primary" data-bs-toggle="modal" data-bs-target=".myclass2"
    hidden>
  </button>

  <div class="modal fade myclass2" id="configIntro" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false"
    aria-labelledby="configIntroLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-body p-0 m-0">
          <div class="step1">
            <div class="intro-picture">
              <img src="../../images/intro-image-a002-advance.jpg" alt="Intro">
            </div>
            <div class="intro-content text-center p-5">
              <h2>Preferred Option selection for Advanced Configurations</h2>
              <div class="p-4">Please click "Excel" button, If you wish to continue the configurations via
                an excel upload. To continue via a wizard, please click "Wizard" button.</div>
              <div class="text-center">
                <button @click="goToExcel" class="btn" data-bs-dismiss="modal"><i class="ri-file-excel-2-fill me-1"></i>
                  Excel</button>
                <button @click="goToWizard" class="btn btn-primary" style="margin-left: 5px;" data-bs-dismiss="modal"><i
                    class="ri-bubble-chart-fill me-1"></i> Wizard</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
  
<script>
import $ from 'jquery';
import { getSubsecNameForWizardPopup } from '@/localStorageUtils';
export default {
  data() {
    return {
      subsection: '',
      redirect: false
    }
  },

  methods: {
    goToExcel() {
      this.redirect = true;
      this.$router.push({ name: 'upload', params: { subsection: getSubsecNameForWizardPopup() } })
    },

    goToWizard() {
      this.redirect = true;
      this.$router.push({ name: 'absence-wizard', params: { subsection: getSubsecNameForWizardPopup() } })
    }
  },

  beforeRouteLeave(to, from, next) {
    if (this.redirect == false) {
      next(false);
    } else {
      setTimeout(() => {
        next();
      }, 300);
    }
  },

  components: {
  },

  mounted() {
    this.redirect = false;
    this.subsection = this.$route.params.subsection;
    $('#advanceModal1Btn').click();
  },
}
</script>
