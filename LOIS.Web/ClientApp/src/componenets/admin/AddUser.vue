<template>
    <div class="row">
        <form action="" v-on:submit.prevent>
            <label v-model="validation"></label>
            <div class="row">
                <div class="col-6">
                    <div class="form-group">
                        <label for="firstNameInput">First Name</label>
                        <input name="first" id="firstNameInput" type="text" class="form-control" v-model="user.firstName" v-validate="'required'">
                        <i v-show="errors.has('first')" class="fa fa-warning"></i>
                        <span v-show="errors.has('first')" class="help is-danger">{{ errors.first('first') }}</span>

                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group">
                        <label for="lastNameInput">Last Name</label>
                        <input name="last" id="lastNameInput" type="text" class="form-control" v-model="user.lastName" v-validate="'required'">
                        <i v-show="errors.has('last')" class="fa fa-warning"></i>
                        <span v-show="errors.has('last')" class="help is-danger">{{ errors.first('last') }}</span>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-6">
                    <div class="form-group">
                        <label for="emailInput">Email</label>
                        <input name="email" id="emailInput" type="email" class="form-control" v-model="user.email" v-validate="'required|email'">
                        <i v-show="errors.has('email')" class="fa fa-warning"></i>
                        <span v-show="errors.has('email')" class="help is-danger">{{ errors.first('email') }}</span>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group">
                        <label for="passwordInput">Password</label>
                        <input name="pass" id="passwordInput" type="password" class="form-control" v-model="user.password" v-validate="'required'">
                        <i v-show="errors.has('pass')" class="fa fa-warning"></i>
                        <span v-show="errors.has('pass')" class="help is-danger">{{ errors.first('pass') }}</span>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-6">
                    <div class="form-check">
                        <input id="isAdminInput" type="checkbox" class="form-check-input" v-model="user.isAdmin">
                        <label for="isAdminInput">Admin</label>
                    </div>
                </div>
                <div class="col-6">
                    <button @click="createUser" class="btn btn-success float-right">Create</button>
                </div>
            </div>
        </form>
    </div>
</template>

<script>
    import axios from 'axios'

    export default {
        props: [],
        data: function () {
            return {
                user: {
                    firstName: '',
                    lastName: '',
                    email: '',
                    password: '',
                    isAdmin: false
                },
                validation: ''
            }
        },
        methods: {
            createUser() {
                this.$validator.validateAll().then((result) => {
                   if(result){
                       //send to server
                       axios.post('admin/CreateUser', this.user).then((result) => {
                            alert('User created');
                       }).catch((err) => {
                            alert('Error creating user: ' + err);
                       });
                       return;
                   }

                   alert('Could not create user.  Fix the errors.');
                });

                this.$store.dispatch('SetLoading', false);
            }
        },
        computed: {},
        components: {}
    }
</script>

<style scoped>
    form {
        border: solid 1px #CCCCCC;
        width: 100%;
        padding: 10px;
    }

    .is-danger{
        color: #ff6352;
    }
</style>