<template>
    <div class="search-component ">
        <div class="container">
            <form v-on:submit.prevent class="card">
                <div class="container">
                    <div class="row">
                        <div class="col">
                            <h5>Accession</h5>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 col-sm-4">
                            <div class="form-group">
                                <label>Accession Id</label>
                                <input type="text"  class="form-control form-control-sm" v-model="RequisitionSearch.RequisitionId">
                            </div>
                            <div class="form-group">
                                <label>EMR #</label>
                                <input type="text" class="form-control form-control-sm" v-model="RequisitionSearch.EMRNo">
                            </div>
                        </div>
                        <div class="col-12 col-sm-4">
                            <div class="form-group">
                                <label for="">Req Start Date</label>
                                <input type="date" class="form-control form-control-sm" v-model="RequisitionSearch.AccessionDateStart">
                            </div>
                            <div class="form-group">
                                <label for="">Req End Date</label>
                                <input type="date" class="form-control form-control-sm" v-model="RequisitionSearch.AccessionDateEnd">
                            </div>
                        </div>
                        <div class="col-12 col-sm-4">
                            <div class="form-group">
                                <label for="">Collected Date</label>
                                <input type="date" class="form-control form-control-sm" v-model="RequisitionSearch.CollectedDateStart">
                            </div>
                            <div class="form-group">
                                <label for="">Collected Date End</label>
                                <input type="date" class="form-control form-control-sm" v-model="RequisitionSearch.CollectedDateEnd">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 col-sm-4">
                            <div class="form-group">
                                <label for="">Scanned Date</label>
                                <input type="date" class="form-control form-control-sm" v-model="RequisitionSearch.ScannedDateStart">
                            </div>
                            <div class="form-group">
                                <label for="">Scanned Date End</label>
                                <input type="date" class="form-control form-control-sm" v-model="RequisitionSearch.ScannedDateEnd">
                            </div>
                        </div>
                    </div>

                    <hr/>

                    <div class="row">
                        <div class="col">
                            <h5>Patient</h5>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 col-sm-4">
                            <div class="form-group">
                                <label for="" >Number</label>
                                <input type="text" class="form-control form-control-sm" v-model="RequisitionSearch.PatientId">
                            </div>
                            <div class="form-group">
                                <label for="">SSN</label>
                                <input type="number" class="form-control form-control-sm" v-model="RequisitionSearch.PatientSSN">
                            </div>
                        </div>
                        <div class="col-12 col-sm-4">
                            <div class="form-group">
                                <label for="">First Name</label>
                                <input type="text" class="form-control form-control-sm" v-model="RequisitionSearch.PatientFirstName">
                            </div>
                        </div>
                        <div class="col-12 col-sm-4">
                            <div class="form-group">
                                <label for="">Last Name</label>
                                <input type="text" class="form-control form-control-sm" v-model="RequisitionSearch.PatientLastName">
                            </div>
                        </div>
                    </div>

                    <hr/>

                    <div class="row">
                        <div class="col">
                            <h5>Billing</h5>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 col-sm-4">
                            <div class="form-group">
                                <label for="" >Payer</label>
                                <input type="text" class="form-control form-control-sm" v-model="RequisitionSearch.PayerName">
                            </div>

                        </div>
                    </div>

                    <div class="float-right">
                        <button class="btn btn-danger btn-sm" @click="clearSearch()" ><span class="fas fa-times"></span> Clear</button>
                        <button class="btn btn-default btn-sm" @click="defaultSearch()">Default</button>
                        <button class="btn btn-success " @click="search()"><span class="fas fa-search"></span> Search</button>
                    </div>

                </div>
            </form>
        </div>

    </div>
</template>

<script>
    import {SET_REQS, DEFAULT_SEARCH, CLEAR_SEARCH, GET_SEARCH} from "../../store/actions/reqs";

    export default {
        props: {
        },
        data: function () {
            return {
            }
        },
        methods: {
            search(){
                this.$store.dispatch(SET_REQS).then(() => {
                    this.$router.push('/#');
                })
            },
            defaultSearch(){
                this.$store.dispatch(DEFAULT_SEARCH)
            },
            clearSearch(){
                this.$store.dispatch(CLEAR_SEARCH);
            }

        },
        computed: {
            RequisitionSearch(){
                return this.$store.getters.GET_SEARCH;
            }
        },
        components: {}
    }
</script>

<style scoped>
    .form-group {
        margin-bottom: 5px;
    }

    .card{
        padding-top: 10px;
        padding-bottom: 10px;
    }

    label {
        font-size: .8em;
        margin-bottom: 0px;
    }
</style>