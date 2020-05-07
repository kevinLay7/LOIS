import { Authlogin, LOGIN_ERROR, LOGIN_SUCCESS, LOGOUT, EMAIL } from '../actions/authlogin';
import axios from 'axios';

export default {
    state:{
        auth: '',
        status: '',
        admin: false
    },
    mutations: {
        [Authlogin]: (state) => {
            state.status = 'loading';
        },
        [LOGIN_SUCCESS]: (state, resp) => {
            state.status = 'success';
            state.auth = resp.data;
        },
        [LOGIN_ERROR]: (state) => {
            state.status = 'error';
        },
        [LOGOUT]:(state) => {
            state.auth = ''
        }
    },
    actions: {
        [Authlogin]: ({commit, dispatch}, user) => {
            return new Promise((resolve, reject) => {
                commit(Authlogin);
                axios.post('/auth', user).then((resp) => {
                    if(resp.status === 200){
                        commit(LOGIN_SUCCESS, resp);
                        resolve();
                    }else{
                        reject(resp);
                    }
                }).catch((err) => {
                    reject(err);
                });
            })
        },
        [LOGOUT]: ({commit, dispatch}) => {
            return new Promise((resolve, reject) => {
                commit(LOGOUT);
                localStorage.clear();
                resolve();
            })
        }
    },
    getters:{
        isLoggedIn: state => {
            let expiration = state.auth.Expiration;
            if(expiration){
                let date = new Date(Date.parse(expiration));
                if(date <= Date.now()){
                    return false;
                }
            }
            return !!state.auth.Token;
        },
        token: state => {
            return state.auth.Token;
        },
        [EMAIL] : state => {
            return state.auth.email;
        },
        isAdmin: state => {
            try {
                return state.auth.UserGroups.includes('Admin');
            }catch (e) {
                return false
            }
        }
    }
}
