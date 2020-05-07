<template>
    <div id="loginDiv">
        <div id="loginBox" class="col-sm-8 col-md-6 col-lg-5">
            <div class="card">
                <h2 class="card-header">LOIS</h2>
                <div class="card-block">
                    <div class="col">
                        <div class="alert alert-danger" role="alert" v-if="unauthorized">
                            Please login again.
                        </div>
                        <div class="alert alert-danger" role="alert" v-if="!validation.isValid">
                            {{ validation.message }}
                        </div>

                        <form v-on:submit.prevent>
                            <div class="form-group">
                                <input v-validate.initial="'required|email'" type="text" class="form-control" placeholder="Email" name="email" v-model="user.username"/>
                                <span v-show="errors.has('email')" class="is-danger" style="margin-left: 10px;">  {{ errors.first('email') }}</span>
                            </div>
                            <div class="form-group">
                                <input v-validate.initial="'required'" type="password" class="form-control" placeholder="password" name="password" v-model="user.password">
                                <span v-show="errors.has('password')" class="is-danger" style="margin-left: 10px;"> {{ errors.first('password') }}</span>
                            </div>

                            <button :class="['btn' ,'float-right', {'btn-light' : !errors.any()}]" :disabled="errors.any()" @click="login()">Login</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>

    </div>
</template>

<script>
    import { Authlogin } from '../../store/actions/authlogin'
    import { LOGOUT } from "../../store/actions/authlogin";
    import { store  } from '../../store/index';

    export default {
        props: [
            'unauthorized'
        ],
        data: function () {
            return {
                user:{
                    username: '',
                    password: ''
                },
                validation: {
                    isValid: true,
                    message: ''
                }
            }
        },
        methods: {
            login(){
                //Reset the validation
                this.validation = {
                    isValid: true,
                    message: ''
                };

                this.$store.dispatch(Authlogin, this.user).then(() => {
                    this.$router.push("/");
                }).catch((err) => {
                   if(err.response){
                        if(err.response.status === 400){
                            this.validation.isValid = false;
                            this.validation.message = 'Incorrect username or password'
                        }
                        else{
                            this.validation.isValid = false;
                            this.validation.message = 'Something went wrong!'
                        }
                   }else if(err.request){
                       this.validation.isValid = false;
                       this.validation.message = 'Unable to reach the server'
                   }
                   else{
                       console.log(err)
                   }
                });
            }
        },
        computed: {

        },
        components: {},
        mounted(){
            //Since we have been directed to the login page, lets clear out any auth we have.
            this.$store.dispatch(LOGOUT);
        }
    }
</script>

<style scoped>
    #loginDiv{
        display: grid;
        place-items: center center;
        height: 80%;
        width: 100%;
    }

    .card-block{
        padding-top: 10px;
        padding-bottom: 10px;
    }

    .card-header{
        border-color: #eee;
    }

    .card, card-block, card-header {
        background: #396afc;  /* fallback for old browsers */
        background: -webkit-linear-gradient(to left, #007bff, #396afc);  /* Chrome 10-25, Safari 5.1-6 */
        background: linear-gradient(to left, #007bff, #396afc); /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */

    }

    .color {
        color:
    }

    .card-header{
        color: #fff;
        text-align: center;
    }

    .btn-light{
        background-color: #fff;
    }

    .is-danger {
        color: #fff;
    }

</style>