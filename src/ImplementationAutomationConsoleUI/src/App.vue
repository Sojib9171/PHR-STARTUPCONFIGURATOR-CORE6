<template>
  <div id="app">
    <div class="main-wrapper h-100">
          <div class="container-fluid px-0">
            <div class="row g-0">
              <router-view></router-view>
            </div>
          </div>
      </div>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>