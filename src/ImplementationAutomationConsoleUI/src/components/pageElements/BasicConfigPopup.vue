<template>
    <button type="button" id="modal1Btn" class="btn btn-primary" data-bs-toggle="modal" data-bs-target=".myclass1" hidden>
    </button>

    <div class="modal fade myclass1" id="configIntro" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false"
        aria-labelledby="configIntroLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-body p-0 m-0">
                    <div class="step1">
                        <div class="intro-picture">
                            <img src="../../../images/intro-image-a001.jpg" alt="Intro">
                        </div>
                        <div class="intro-content text-center p-5">
                            <h2>Welcome to PeoplesHR Config Assist!</h2>
                            <div class="p-4">We are here to help you with the initial setting-up of PeoplesHR for you to
                                comfortably
                                onboard.</div>
                            <div class="text-center">
                                <button class="btn btn-primary btn-xl" data-bs-dismiss="modal"
                                    @click="showSecondModal()">Let's
                                    Begin</button>
                                <div class="text-center"><button id="closeBtn1" data-bs-dismiss="modal"
                                        class="btn btn-primary btn-xl" hidden>Continue</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <button type="button" id="modal2Btn" class="btn btn-primary" data-bs-toggle="modal" data-bs-target=".myclass" hidden>
    </button>

    <div class="modal fade myclass" id="configIntro2" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false"
        aria-labelledby="configIntroLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-body p-0 m-0">
                    <div class="step2">
                        <div class="intro-content p-5">
                            <h2 class="text-center">Let's get started with the following configurations</h2>
                            <div class="p-4">
                                <ul class="list-unstyled step-list">
                                    <li class="mt-4 item1">
                                        <i class="ri-checkbox-circle-fill check-fill"></i>
                                        <h4>Set-up Organization</h4>
                                        <ul>
                                            <li>Configure your organization settings
                                            </li>
                                        </ul>
                                    </li>
                                    <li class="mt-4 item1">
                                        <i class="ri-checkbox-circle-fill check-fill"></i>
                                        <h4>Configure Employee Information</h4>
                                        <ul>
                                            <li>Upload employee masters
                                            </li>
                                        </ul>
                                    </li>
                                    <li class="mt-4 item1">
                                        <i class="ri-checkbox-circle-fill check-fill"></i>
                                        <h4>Configure Absence Data</h4>
                                        <ul>
                                            <li>Create new leave types that comply with organizational policies
                                            </li>
                                        </ul>
                                    </li>
                                    <!-- <li class="mt-4 item2">
                                  <i class="ri-checkbox-circle-fill check-fill"></i>
                                  <h4>Post Configurations</h4>
                                  <ul>
                                    <li>Assist you in starting basic functions smoothly on initial onboarding
                                    </li>
                                    <li>Accommodates in maintaining a content library</li>
                                    <li>Links with PeoplesHR Learning Academy to enhance product knowledge</li>
                                  </ul>
                                </li> -->
                                </ul>
                            </div>

                            <div class="text-center"><button id="closeBtn2" @click="redirectPage" data-bs-dismiss="modal"
                                    class="btn btn-primary btn-xl">Continue</button>
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

export default {
    methods: {
        showSecondModal() {
            $('#modal2Btn').click();
        },

        redirectPage() {
            this.$router.push({
                name: 'home',
            });
        }
    },

    mounted() {
        //$('#closeBtn1').click();
        $('#modal1Btn').click();
    }
};
</script>

  