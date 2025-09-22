<template>
    <button type="button" id="modal2Btn" class="btn btn-primary" data-bs-toggle="modal" data-bs-target=".myclass"
        hidden>
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
                                        <h4>Pre Configurations</h4>
                                        <ul>
                                            <li>Assist you in configuring preliminary steps of PeoplesHR in order to
                                                onboard
                                            </li>
                                        </ul>
                                    </li>
                                    <li class="mt-4 item2">
                                        <i class="ri-checkbox-circle-fill check-fill"></i>
                                        <h4>Post Configurations</h4>
                                        <ul>
                                            <li>Assist you in starting basic functions smoothly on initial onboarding
                                            </li>
                                            <li>Accommodates in maintaining a content library</li>
                                            <li>Links with PeoplesHR Learning Academy to enhance product knowledge</li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                            
                            <div class="text-center"><button data-bs-dismiss="modal"
                                    class="btn btn-primary btn-xl">Continue</button></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import $ from 'jquery';
export default (
    {
        mounted() {
            $('#modal2Btn').click();
        },
    })
</script>