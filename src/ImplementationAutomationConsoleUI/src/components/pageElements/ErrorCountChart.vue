<template>
  <div>
    <h2>Error Count</h2>
  </div>
  <div>
    <vc-donut background="white" foreground="grey" :size="150" unit="px" :thickness="25" :sections="sections"
      :total="total" :start-angle="0" :auto-adjust-text-size="true">
      <h1>{{ percentage }}%</h1>
    </vc-donut>
  </div>
</template>
<script>
export default {
  data() {
    return {
      sections: [
        { label: 'Red section', value: this.errorCount, color: 'red' },
      ]
    };
  },

  props: {
    errorCount: {
      type: Number,
      required: true,
    },

    total: {
      type: Number,
      required: true,
    },
  },
  
  computed: {
    percentage() {
      return Math.round((this.errorCount / this.total) * 100);
    },
  },
};
</script>