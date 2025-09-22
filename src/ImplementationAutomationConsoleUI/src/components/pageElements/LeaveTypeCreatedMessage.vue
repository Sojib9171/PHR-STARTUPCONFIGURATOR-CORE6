<template>
    <div class="config-block">
        <div class="row p-4 border-0">
            <div class="col-sm-8 col-12 offset-2">
                <div class="config-message success text-center">
                    <div class="message-image">
                        <div>
                            <svg version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"
                                x="0px" y="0px" width="50px" height="50px" viewBox="0 0 24 24"
                                enable-background="new 0 0 24 24" xml:space="preserve">
                                <g id="Filled_Icon">
                                    <g class="fill-white">
                                        <path d="M12,20c3.691,0,6.725-2.38,6.982-6H5.018C5.275,17.62,8.308,20,12,20z" />
                                        <circle cx="7.5" cy="7.5" r="1" />
                                        <circle cx="16.5" cy="7.5" r="1" />
                                        <path d="M12,0C5.383,0,0,5.383,0,12s5.383,12,12,12s12-5.383,12-12S18.617,0,12,0z M16.5,5.5c1.103,0,2,0.897,2,2
                                                    c0,1.102-0.897,2-2,2s-2-0.898-2-2C14.5,6.397,15.398,5.5,16.5,5.5z M7.5,5.5c1.102,0,2,0.897,2,2c0,1.102-0.898,2-2,2
                                                    s-2-0.898-2-2C5.5,6.397,6.398,5.5,7.5,5.5z M12,21c-4.411,0-8-3.096-8-7.506c0-0.277,0.224-0.5,0.5-0.5h15
                                                    c0.276,0,0.5,0.223,0.5,0.5C20,17.904,16.411,21,12,21z" />
                                    </g>
                                </g>
                                <g id="Invisible_Shape">
                                    <rect fill="none" width="24" height="24" />
                                </g>
                            </svg>
                        </div>
                    </div>
                    <div class="message-content">
                        <div class="message-title">
                            Excellent!
                        </div>
                        <p class="sub-title">You have successfully added a leave type</p>
                        <p class="help-info"><i class="ri-information-fill"></i>
                            <span>This will go through the relevant approval flow and you
                                will be notified via an email as soon as the approving
                                person takes the action.</span>
                        </p>

                    </div>
                    <hr />
                    <div class="quis-block">
                        <div class="quis-q">
                            Do you wish to create more Leave Types?
                        </div>
                        <div class="quis-a">
                            <div class="d-flex justify-content-center w-100">
                                <button class="btn btn-secondary" @click="this.$router.push({ name: 'leave-type-summary', params:{subsection:this.subsectionName} })">No</button>
                                <div class="position-relative">
                                    <div class="callout-box active">
                                        <i class="ri-checkbox-blank-circle-fill icon-blinking"></i>
                                        <span>
                                            Click on this to continue with the wizard
                                        </span>
                                    </div>
                                </div>
                                <button @click="createAnotherLeaveType"
                                    class="btn btn-info">Yes</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import {emptyLeaveWizardOptionsInLocalStorage, removeAbsenceRowID, removeAbsenceSubsectionName} from '@/localStorageUtils'
export default
{
    methods:{
        createAnotherLeaveType()
        {
            removeAbsenceRowID();
            removeAbsenceSubsectionName();
            this.$router.push({ name: 'absence-wizard', params:{subsection:this.subsectionName} });
        }
    },

    mounted(){
        emptyLeaveWizardOptionsInLocalStorage();
    },

    props:{
        subsectionName:String
    }
}
</script>
