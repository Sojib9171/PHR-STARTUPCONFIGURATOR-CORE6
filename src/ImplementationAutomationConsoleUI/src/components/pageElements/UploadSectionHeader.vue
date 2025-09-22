<template>
    <h3 class="section-title">Upload Data or Download Templates</h3>
    <h4 class="subtitle">For {{ subsectionName }}</h4>
</template>
<script>
export default {
    name: 'UploadSectionHeader',
    props: {
        'subsectionName': String
    }
}
</script>