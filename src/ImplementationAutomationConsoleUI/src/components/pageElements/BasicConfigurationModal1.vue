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
                                <button @click="this.showSecondModal=true"
                                    class="btn btn-primary btn-xl" data-bs-dismiss="modal">Let's Begin</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div v-if="showSecondModal">
        <basic-Configuration-Modal2></basic-Configuration-Modal2>
    </div>
</template>

<script>
import $ from 'jquery';
import BasicConfigurationModal2 from "./BasicConfigurationModal2.vue";
export default (
    {
        data() {
            return {
                showSecondModal:false
            }
        },
        components:{
            "basic-Configuration-Modal2":BasicConfigurationModal2
        },
        mounted() {
            $('#modal1Btn').click();
        },
    })
</script>