<template>
    <div class="container" style="margin-top: 10px !important;">
        <div class="row">
            <button @click="addingNewUser = !addingNewUser" class="btn btn-light float-left">New User</button>
            <button class="btn btn-light ml-auto" @click="getUsers">Refresh</button>
        </div>

        <add-user v-if="addingNewUser"></add-user>

        <div class="row">
            <v-client-table :data="users" :columns="columns" :options="options" style="width: 100%;">
                <template slot="enabled" slot-scope="props">
                    <input type="checkbox" v-model="props.row.enabled" disabled/>
                </template>
                <template slot="admin" slot-scope="props">
                    <input type="checkbox" v-model="props.row.admin" disabled/>
                </template>
                <template slot="Groups" slot-scope="props">
                    <p v-for="group in props.row.Groups">
                        {{ group }}
                    </p>
                </template>
            </v-client-table>
        </div>
    </div>
</template>

<script>
    import AddUser from "./AddUser"
    import { GET_USERS } from "../../store/actions/admin";

    export default {
        props: [],
        data: function () {
            return {
                options: {
                    headings: {
                    },
                    pagination: { chunk:10, dropdown: false },
                    columnsDropdown: false,
                    filterable: false
                },
                columns: [
                    'id',
                    'firstname',
                    'lastname',
                    'email',
                    'enabled',
                    'Groups'
                ],
                addingNewUser: false
            }
        },
        methods: {
            getUsers(){
                this.$store.dispatch('LOAD_USERS');
            }
        },
        computed: {
            users() {
                let u = this.$store.getters.GET_USERS;
                if(u.length === 0){
                    this.$store.dispatch('LOAD_USERS');
                }

                return this.$store.getters.GET_USERS;
            }
        },
        components: {
            AddUser
        }
    }
</script>

<style scoped>
    .row {
        margin-top: 10px;
    }
</style>