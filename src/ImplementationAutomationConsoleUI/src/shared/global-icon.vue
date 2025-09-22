<template>
  <div>
    <v-icon>
      {{ appIcon }}
    </v-icon>
  </div>
</template>

<script>
import { icon } from "../favicon.ico";

export default {
  data: () => ({
    icons: {
      appIcon: icon,
    },
  })
};
