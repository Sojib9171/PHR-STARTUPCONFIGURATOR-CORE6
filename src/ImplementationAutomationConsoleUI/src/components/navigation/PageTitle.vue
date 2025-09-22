<template>
    <div>
        <div class="page-title">
            <!--Page title-->
            <h2>{{ pageName }}</h2>

            <!--Breadcrumb-->
            <nav style="--bs-breadcrumb-divider: '|';" aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><router-link to="/home"><i class="ri-home-4-line"></i></router-link></li>
                    <li class="breadcrumb-item active" aria-current="page">{{ pageName }}</li>

                </ol>
            </nav>
        </div>
    </div>
</template>

<script>
export default {
    name: 'pageTitle',
    props: {
        'pageName': String
    },
}
</script>