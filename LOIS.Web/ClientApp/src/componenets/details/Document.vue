<template>
    <div v-if="!Deleted" class="card" :class="[{expanded: isExpanded, default: !isExpanded}, 'lightenUp']">
        <div class="card-body" style="height: 100%;">
            <div style="width:100% ;height: 30px;" class="row">
                <div class="col"></div>
                <div class="col">
                    <span style="margin-left: 50%; height: 100%;" v-show="!isExpanded" @click="toggleExpand"><i class="toggle fas fa-chevron-down"></i></span>
                    <span style="margin-left: 50%; height: 100%;" v-show="isExpanded" @click="toggleExpand"><i class="toggle fas fa-chevron-up"></i></span>
                </div>
                <div class="col">
                    <button class="btn btn-sm btn-danger float-right" @click="removePdf()" style="margin-top: -10px"><span class="fa fa-trash"></span></button>
                </div>
            </div>
            <div class="float-left" style="width: 100%; height: 92%;">
                <object style="height: 100%; width: 100%;" :data="this.url" type="application/pdf"></object>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        props: ['doc', 'url'],
        data: function () {
            return {
                isExpanded: false,
                Deleted: false
            }
        },
        methods: {
            toggleExpand(){
                this.isExpanded = !this.isExpanded;
            },
            removePdf(){
                let result = confirm('Are you sure you want to delete this?');
                if(!result){
                    return;
                }

                let split = this.url.split('/');
                let id = split[split.length - 1].replace('.pdf', '');
                let url = '/document/' + id + '/Delete';
                axios.post(url).then(resp => {
                    this.Deleted = true;
                    alert('Deleted');
                }).catch(err => {
                    alert('Unable to delete document');
                })
            }
        },
        computed: {
            chevron(){
                return{
                    'fas fa-chevron-up': this.isExpanded,
                    'fas fa-chevron-down': !this.isExpanded
                }
            }
        },
        components: {
        }
    }
</script>

<style scoped>

    .expanded{
        height: 80vh;
    }
    .default {
        height: 40vh;
    }
    .toggle{
        width: 25px;
        height: 25px;
        margin-top: -10px;
        color: #888;
    }
</style>