<template>
    <nav class="navbar navbar-expand-sm  navbar-dark bg-primary">
        <a class="navbar-brand" href="#/">LOIS</a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent" v-if="isLoggedIn">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item active">
                    <router-link class="nav-link" :to="{name: 'home'}">Home </router-link>
                </li>
                <li class="nav-item">
                    <router-link class="nav-link" :to="{ path: 'admin'}" v-if="this.$store.getters.isAdmin">Admin</router-link>
                </li>
            </ul>
            <form class="form-inline my-2 my-lg-0">
                <button class="btn btn-outline-light btn-sm" type="button" @click="GoToSearch()">Search</button>
            </form>
            <div class="nav-item dropdown" >
                <a class="nav-link dropdown-toggle myAccount" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <span class="fas fa-user-circle" style="height: 30px; width: 30px;"></span>
                </a>
                <div class="dropdown-menu" style="margin-left: -95px;">
                    <a class="dropdown-item"><router-link :to="{name: 'changePassword'}" class="btn btn-sm btn-outline-success">Change Password</router-link></a>
                    <a class="dropdown-item"><button class="btn btn-outline-danger btn-sm"  @click="logout()">Logout</button></a>
                </div>
            </div>
         </div>
    </nav>
</template>

<script>
    import { LOGOUT } from "../../store/actions/authlogin";

    export default {
        props: {},
        data: function () {
            return {}
        },
        methods: {
            logout(){
                this.$store.dispatch(LOGOUT).then(() => {
                    this.$router.push('/login');
                });
            },
            GoToSearch(){
                this.$router.push('/search');
            }
        },
        computed: {
            isLoggedIn(){
                return this.$store.getters.isLoggedIn;
            }
        },
        components: {}
    }
</script>

<style scoped>
    .myAccount{
        color: #fff;
    }
</style>