import Vue from 'vue';
import Vuex from 'vuex';
import auth from './modules/auth'
import reqs from './modules/reqs'
import admin from './modules/admin'
import VuexPersistence from 'vuex-persist'

Vue.use(Vuex);

export const store = new Vuex.Store({
    modules:{
        auth,
        reqs,
        admin
    },
    plugins: [(new VuexPersistence('vuex')).plugin]
});