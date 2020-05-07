<template>
    <div id="main">
        <br>
        <button class="btn btn-light float-right" @click="resetSearch()">Reset</button>
        <v-client-table :data="reqs" :columns="columns" :options="options">
            <a slot="edit" slot-scope="props" @click="showDetails(props.row.RequisitionNo)"  class="btn btn-primary btn-sm" style="color: #fff">
                <i class="fas fa-eye"/> View
            </a>
        </v-client-table>
    </div>
</template>

<script>
    import { SET_REQS, DEFAULT_SEARCH} from "../../store/actions/reqs";

    export default {
        props: {},
        data: function () {
            return {
                options: {
                    headings: {
                        patientId: 'Patient Number',
                        CollectedDate: 'Collected Date',
                        PatientName: 'Patient Name',
                        RequisitionNo: 'Req #',
                        AccessionDate: 'Req Date',
                        PatientSSN: 'SSN',
                        EmrNo: 'External ID',
                        PayerName: 'Payer'
                    },
                    filterable: ['PatientName'],
                    pagination: { chunk:10, dropdown: false },
                    columnsDropdown: false,
                    sortable: ['PatientNumber', 'PatientName'],
                    dateColumns: ['CollectedDate', 'AccessionDate'],
                    dateFormat: 'DD/MM/YYYY'
                },
                columns: []
            }
        },
        methods: {
            showDetails(id){
                console.log(id);
                this.$router.push({ path: '/details/' + id});
            },
            resetSearch(){
                this.$store.dispatch(DEFAULT_SEARCH).then(() => {
                    this.$store.dispatch(SET_REQS)
                })
            }
        },
        computed: {
            reqs(){
                //Get the reqs from the DB then create the view models from them.
                let reqs =  this.$store.getters.GET_REQS;

                let output = [];
                for(let i = 0; i < reqs.length; i++) {
                    let temp = {
                        'RequisitionNo': reqs[i].RequisitionNo,
                        'EmrNo': reqs[i].EmrNo,
                        'AccessionDate': new Date(reqs[i].AccessionDate).toLocaleDateString('en-US'),
                        'PatientName': reqs[i].Patient ? reqs[i].Patient.LastName + ', ' + reqs[i].Patient.FirstName : ''
                    };

                    let searchModel = this.$store.getters.GET_SEARCH;
                    Object.keys(searchModel).forEach(function(key){
                        if(searchModel[key]){
                            if(key === 'CollectedDateStart' || key === 'CollectedDateEnd'){
                                temp['CollectedDate'] = new Date(reqs[i]['CollectedDate']).toLocaleDateString();
                            }
                            else if(key === 'AccessionDateStart' || key === 'AccessionDateEnd' || key === 'PatientFirstName' || key === 'PatientFirstName'){ //this is default.  Don't add
                            }
                            else if(key === 'ScannedDateStart' || key === 'ScannedDateEnd'){
                                temp['ScannedDate'] = new Date(reqs[i]['ScannedDate']).toLocaleString("en-US");
                            }
                            else if(key === 'PatientSSN'){
                                temp['PatientSSN'] = reqs[i].Patient ? reqs[i].Patient.SSN : '';
                            }
                            else{
                                //Check for null value
                                if(reqs[i][key]){
                                    temp[key] = reqs[i][key];
                                }
                                else{
                                    temp[key] = '';
                                }
                            }
                        }
                    });
                    output.push(temp);
                }

                //set columns
                let cols = [];
                if(output.length > 0){
                    Object.keys(output[0]).forEach(function (key) {
                        cols.push(key);
                    });
                }
                cols.push('edit');
                this.columns = cols;

                return output;
            }
        },
        created: function(){
            //The first time this module is ran, we need to check if there are any reqs.  If not, load from the DB.
            if(this.reqs.length === 0){
                this.$store.dispatch(SET_REQS);
            }
        }
    }
</script>

<style>
    #main{
        width:98%;
        margin-left: auto;
        margin-right: auto;
    }

    .VueTables__search{
        display: inline-block;
    }

    .VueTables__search label{
        display: none;
    }

    .VueTables__limit {
        display: inline-block;
        float: right;
    }

    .VueTables__limit label {
        display: inline-block;
        margin-right: 5px;
    }

</style>