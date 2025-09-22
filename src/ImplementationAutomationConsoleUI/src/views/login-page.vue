<template>
  <div>
    <head>
      <meta charset="utf-8" />
      <meta name="viewport" content="width=device-width, initial-scale=1" />
      <title>PeoplesHR Config Assist</title>
    </head>
    <div class="main-wrapper h-100">
      <div class="login-container h-100">
        <div class="container-fluid h-100">
          <div class="row">
            <div class="col-md-12">
              <div class="branding mb-4">
                <img src="../../images/logo.png" alt="Peoples HR Logo" class="logo" />
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-6">
              <div class="intro-contents">
                <h1>PeoplesHR Config Assist</h1>
                <p class="welcome-text">Welcome to PeoplesHR Config Assist! </p>
                <p class="sub-title" style="text-align: justify;">Onboard and set up your configurations for seamless integration with
                  PeoplesHR. Log in to access your personalized dashboard and effortlessly do your
                  configurations. Experience the convenience of a self-guided onboarding process,
                  maximizing the benefits of PeoplesHR within minutes. Start configuring with ease using
                  our intuitive Log in UI provided by PeoplesHR Config Assist.</p>
              </div>
            </div>
            <login-comp />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import loginComp from "../components/auth/login-comp.vue";

export default {
  components: {
    "login-comp": loginComp,
  },
};
</script>