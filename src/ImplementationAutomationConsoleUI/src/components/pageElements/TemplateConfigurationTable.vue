<template>
    <div class="config-block mb-4">
        <!-- vue table component goes here -->
        <div class="table-responsive">
            <div class="table-outer-block">
                <div class="table-header d-flex justify-content-between align-items-center ">
                    <div>Showing 1 to 25 of 100 entries</div>
                    <div><input type="search" class="form-control" placeholder="Search">
                    </div>
                </div>
                <table class="table">
                    <thead>
                        <tr>
                            <th>Module Name</th>
                            <th>Section Title</th>
                            <th>Upload Excel</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>

                        <tr class="upload-success">
                            <td>EIM</td>
                            <td>Employee Data</td>
                            <td>
                                <div class="d-flex excel-upload"><button class="btn btn-link"><i
                                            class="ri-upload-cloud-line"></i><i class="ri-upload-cloud-fill"></i>
                                        Upload</button></div>
                            </td>
                            <td><span class="badge bg-success">Uploaded</span></td>
                        </tr>
                        <tr class="upload-success">
                            <td>EIM</td>
                            <td>Bank Details</td>
                            <td>
                                <div class="d-flex excel-upload"><button class="btn btn-link"><i
                                            class="ri-upload-cloud-line"></i><i class="ri-upload-cloud-fill"></i>
                                        Upload</button></div>
                            </td>
                            <td><span class="badge bg-success">Uploaded</span></td>
                        </tr>
                        <tr>
                            <td>Absence</td>
                            <td>Statutory Leave</td>
                            <td>
                                <div class="d-flex excel-upload"><button
                                        @click="this.$router.push({ name: 'template-upload', params: { subsection: 'Statutory Leave' } })"
                                        class="btn btn-link"><i class="ri-upload-cloud-line"></i><i
                                            class="ri-upload-cloud-fill"></i>
                                        Upload</button></div>
                            </td>
                            <td><span class="badge bg-danger">Rejected</span></td>
                        </tr>
                        <tr>
                            <td>Absence</td>
                            <td>Short Leave</td>
                            <td>
                                <div class="d-flex excel-upload"><button
                                        @click="this.$router.push({ name: 'template-upload', params: { subsection: 'Statutory Leave' } })"
                                        class="btn btn-link"><i class="ri-upload-cloud-line"></i><i
                                            class="ri-upload-cloud-fill"></i>
                                        Upload</button></div>
                            </td>
                            <td><span class="badge bg-secondary">Not Initiated</span></td>
                        </tr>
                    </tbody>
                </table>
                <div class="table-footer d-flex justify-content-between align-items-center">
                    <div>
                        <ul class="pagination">
                            <li class="page-item"><a class="page-link" href="#">Previous</a>
                            </li>
                            <li class="page-item"><a class="page-link" href="#">1</a></li>
                            <li class="page-item"><a class="page-link" href="#">2</a></li>
                            <li class="page-item"><a class="page-link" href="#">3</a></li>
                            <li class="page-item"><a class="page-link" href="#">Next</a>
                            </li>
                        </ul>
                    </div>
                    <div>
                        show 10 entries
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
</script>