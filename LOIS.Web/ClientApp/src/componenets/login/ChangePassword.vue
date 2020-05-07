<template>
    <div class="container">
        <h3>Change Password</h3>
        <div class="row">
            <div class="col">
                <form v-on:submit.prevent>
                    <div class="form-group">
                        <input type="text" class="form-control" placeholder="Old Password" v-model="user.oldPassword">
                    </div>
                    <br>
                    <br>
                    <div class="form-group">
                        <input v-validate.initial="'required|min:8'" type="password" class="form-control" placeholder="New Password" name="password" v-model="user.newPassword"/>
                        <span v-show="errors.has('password')" class="is-danger" style="margin-left: 10px;">  {{ errors.first('password') }}</span>
                    </div>
                    <div class="form-group">
                        <input v-validate="'required|confirmed:password'" type="password" class="form-control" placeholder="New Password, Again" name="password_confirmation" data-vv-as="password">
                        <span v-show="errors.has('password_confirmation')" class="is-danger" style="margin-left: 10px;"> {{ errors.first('password_confirmation') }}</span>
                    </div>

                    <button :disabled="errors.any()" class="btn btn-sm btn-success float-right" @click="changePassword()">Change</button>
                </form>
            </div>
        </div>

    </div>
</template>

<script>
    import {EMAIL} from '../../store/actions/authlogin';
    import axios from 'axios';

    export default {
        props: [],
        data: function () {
            return {
                user: {
                    newPassword: '',
                    oldPassword: '',
                    email: ''
                }
            }
        },
        methods: {
            changePassword(){
                if(!this.errors.any()){
                    this.user.email = this.$store.getters.EMAIL;
                    axios.post('Manage', this.user).then(resp => {
                        if(resp.status === 200){
                            alert('Password changed');
                            this.$router.push('/');
                        }else{
                            alert('Error changing password');
                        }
                    }).catch(err => {
                    });
                }
            }
        },
        computed: {},
        components: {}
    }
</script>

<style scoped>
    .container {
        margin-top: 10px;
    }
</style>