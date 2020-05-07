import {LOAD_LOGS, LOAD_USERS, GET_USERS} from '../actions/admin'
import axios from 'axios';

export default {
    state : {
        logs: [],
        users: []
    },
    mutations : {
        [LOAD_LOGS]: (state, logs) => {
            state.logs = logs;
        },
        [LOAD_USERS]: (state, users) => {
            state.users = users;
        }
    },
    actions : {
        [LOAD_LOGS]: ({commit}, logs) => {
            if(logs){
                commit(LOAD_LOGS, logs);
            }else{
                //Todo implement this
            }
        },
        [LOAD_USERS]: ({commit}) => {
            return new Promise((resolve, reject) => {
               axios.get('/admin/users').then((response) => {
                    let returnedUsers = response.data;
                    //Commit to store
                   commit(LOAD_USERS, returnedUsers);
                    resolve();
               }).catch((err) => {
                   reject(err);
               })
            });
        }
    },
    getters : {
        [GET_USERS]: state => {
            return state.users;
        }
    }
}