import Vue from 'vue';
import Router from 'vue-router'
import { store } from './store/index.js'

import Home from './componenets/home/Home'
import Details from './componenets/details/Details'
import Login from './componenets/login/login'
import Search from './componenets/search/Search'
import Admin from './componenets/admin/admin'
import changePassword from './componenets/login/ChangePassword'

Vue.use(Router);

const ifNotAuthenticated = (to, from, next) => {
    if(!store.getters.isLoggedIn){
        next();
        return
    }
    next('/');
};

const ifAuthenticated = (to, from, next) => {
    if(store.getters.isLoggedIn){
        next();
        return
    }
    next('/login');
};

const ifAdmin = (to, from, next) => {
    if(store.getters.isAdmin && store.getters.isLoggedIn){
        next();
        return
    }
    next('/login');
};


export default new Router({
    mode: 'hash',
    routes: [
        {
            path: '/',
            name: 'home',
            components:{
                default: Home
            },
            beforeEnter: ifAuthenticated
        },
        {
            path: '/login',
            name: 'login',
            component: Login,
            props: true,
            beforeEnter: ifNotAuthenticated
        },
        {
            path: '/admin',
            name: 'admin',
            component: Admin,
            beforeEnter: ifAdmin
        },
        {
            path: '/details/:id',
            component: Details,
            name: 'details',
            props: true,
            beforeEnter: ifAuthenticated
        },
        {
            path: '/search',
            component: Search,
            name: 'search',
            beforeEnter: ifAuthenticated
        },
        {
            path: '/changePassword',
            component: changePassword,
            name: 'changePassword',
            beforeEnter: ifAuthenticated
        }
    ]
})

