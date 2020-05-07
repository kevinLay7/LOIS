<template>
    <div class="container lightenUp">
        <br>
        <div class="row">
            <button @click="back()" class="btn btn-sm btn-outline-primary" style="margin-right: 20px;">< Back</button>
            <h2 style="margin: 0;">{{ id }}</h2>
        </div>
        <br>
        <patient-info :patient="Requisition.Patient"></patient-info>
        <br>
        <div v-for="doc in DocUrls" style="width: 100%;">
            <document v-bind:url="doc"></document>
            <br>
        </div>
    </div>
</template>

<script>
    import PatientInfo from './PatientInfo'
    import Document from './Document'
    import axios from 'axios'

    export default {
        props: ['id'],
        components: {
            PatientInfo,
            Document
        },
        data: function () {
            return {
                Documents:[],
                Requisition: '',
                DocUrls: []
            }
        },
        methods: {
            back(){
                this.$router.push('/');
            }
        },
        computed: {
        },
        created() {
            this.$store.commit('setLoading', true);
            let reqId = this.$route.params.id;
            axios.get('/requisition', {params:{id: reqId}}).then((resp) => {
                let req = resp.data;
                this.$store.commit('SET_REQ', req);
                this.Requisition = req;
                this.Documents = req.Documents;
            }).catch(err => {
                    console.log(err);
                });

            let url = '/requisition/' + reqId + '/DocumentUrls';
            axios.get(url).then((resp) => {
                for(let i = 0; i < resp.data.length; i++) {
                    let path = axios.defaults.baseURL.replace('api', 'Pdfs/') + resp.data[i];
                    this.DocUrls.push(path);

                    this.$store.commit('setLoading', false);
                }
            }).catch(err => {
                console.log(err);

                this.$store.commit('setLoading', false);
            });

        }
    }
</script>

<style scoped>
    .lightenUp{
        color: #555;
    }
</style>