import Vue from 'vue';
import App from './App.vue';
import router from './router'
import { store } from './store/index.js';
import VeeValidate from 'vee-validate';
import axios from 'axios';
import {ClientTable} from 'vue-tables-2';
import 'bootstrap';

require('vue2-animate/dist/vue2-animate.min.css');

Vue.use(ClientTable);
Vue.use(VeeValidate);

let v = new Vue({
    el: '#app',
    router,
    store,
    render: h => h(App)
});



axios.defaults.baseURL = __API__;
axios.defaults.headers.common['Access-Control-Allow-Headers'] = 'Origin, X-Requested-With, Content-Type, Accept';
axios.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
axios.defaults.headers.common['Access-Control-Allow-Methods'] = 'GET, PUT, POST, DELETE, OPTIONS';

axios.interceptors.request.use(function(config){
    const token = v.$store.getters.token;
    if(token){
        config.headers['Token'] = token;
    }
    v.$store.commit('setLoading', true);
    return config;
});

axios.interceptors.response.use(function(resp){
    v.$store.commit('setLoading', false);
    return resp;
}, function(error){
    v.$store.commit('setLoading', false);
    if(error.response.status === 401){
        try {
            v.$store.dispatch("LOGOUT");
            v.$router.push({ name: 'login', params: { unauthorized : true }});
        }catch(err){
            console.log(err);
        }
    }
    v.$store.commit('setLoading', false);
    return error;
});



