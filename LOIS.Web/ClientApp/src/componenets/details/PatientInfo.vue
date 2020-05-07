<template>
    <div class="card">
        <div class="card-header">
            <h4>Patient</h4>
        </div>
        <div class="card-body"  v-if="patient">
            <div class="row">
                <div class="col-sm-3">
                    <label for="">First</label>
                    <p>{{ patient.FirstName }}</p>
                </div>
                <div class="col-sm-3">
                    <label for="">Last</label>
                    <p>{{ patient.LastName }}</p>
                </div>
                <div class="col-sm-3">
                    <label for="">SSN</label>
                    <p>{{ patient.SSN }}</p>
                </div>
                <div class="col-sm-3">
                    <label for="">Id</label>
                    <p>{{ patient.Id }}</p>
                </div>
                <div class="col-sm-3">
                    <label for="">DOB</label>
                    <p>{{ patient.DateOfBirth ? new Date(patient.DateOfBirth).toLocaleString() : '' }}</p>
                </div>
                <div class="col-sm-3">
                    <label for="">Phone</label>
                    <p>{{ patient.Phone }}</p>
                </div>
                <div class="col">
                    <label for="">Address</label>
                    <p>{{ patient.Address }}</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        props: [ 'patient', 'requisition'],
        data: function () {
            return {

            }
        },
        methods: {},
        computed: {},
        components: {},
        created (){
            if(!this.patient){
                if(this.requisition){

                }
            }
        }
    }
</script>

<style scoped>
    label {
        font-weight: bold;
    }
    .card {
        width: 100%;
    }
    .card-header{
        padding-top: 10px;
        padding-bottom: 0px;
    }
    .card-body {
        padding-top: 10px;
        padding-bottom: 10px;
    }
    p {
        margin-bottom: 10px;
    }
    label {
        margin-bottom: 0px;
    }
</style>