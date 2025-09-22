<template>
  <div></div>
</template>

<script>
export default {
  async created() {
    await this.CheckLog();
  },
  methods: {
    async CheckLog() {
      if (
        this.$cookies.get("enc") == null &&
        this.$cookies.get("encVal") == null
      ) {
        await this.$http
          .post("/LogOut")
          .then(() => {
            this.$router.replace({
              path: "/",
            });
          })
          .catch((error) => {
            console.log(error.message);
          });
      }
    },
  },
};
</script>
