import {GET_REQS, GET_REQ, SET_REQS, SET_SEARCH, GET_SEARCH, DEFAULT_SEARCH, CLEAR_SEARCH, CLEAR} from "../actions/reqs";
import axios from 'axios'
let moment = require('moment');

class SearchModel {
    constructor(){
        this.RequisitionId = '';
        this.EMRNo = '';
        this.AccessionDateStart= '';
        this.AccessionDateEnd= '';
        this.CollectedDateStart= '';
        this.CollectedDateEnd='';
        this.PatientId= '';
        this.PatientFirstName= '';
        this.PatientLastName= '';
        this.PatientSSN= '';
        this.PayerName='';
        this.ScannedDateStart= moment().format('YYYY-MM-DD');
        this.ScannedDateEnd = '';
    }
}

export default {
    state: {
        reqs: [],
        searchModel: new SearchModel(),
        columns: [],
        loading: false
    },
    mutations: {
        SET_REQS(state, reqs){
            state.reqs = reqs;
        },
        'SET_REQ'(state, req){
            let r = state.reqs.filter(item => item.Id === req.Id);
            if(r){
                r = req;
            }else{
                state.reqs.push(req);
            }
        },
        SET_SEARCH(state, search){
            state.searchModel = search;
        },
        SET_COLUMNS(state, columns){
            state.columns = columns;
        },
        'setLoading'(state, isLoading){
            state.loading = isLoading
        }
    },
    actions: {
        [SET_REQS]:({commit, state}) =>
        {
            return new Promise((resolve, reject) => {
                if(state.searchModel) {
                    var d = JSON.stringify(state.searchModel);
                }else{
                    var d = new SearchModel();
                }

                axios.get('/requisition', {params: {data: d}}).then((response) => {
                    let reqs = JSON.parse(response.data);
                    commit(SET_REQS, reqs);
                    resolve();
                }).catch((err) => {
                    reject();
                });
            });
        },
        [DEFAULT_SEARCH]({commit}) {
            let model = new SearchModel();

            commit(SET_SEARCH, model);
        },
        [CLEAR_SEARCH]({commit}) {
            let model = new SearchModel();
            model.ScannedDateStart = '';

            commit(SET_SEARCH, model);
        },
        [CLEAR]({commit}, store){
            store.dispatch(DEFAULT_SEARCH);
            store.dispatch(SET_REQS);
        }
    },
    getters: {
        [GET_REQS] : state => {
            return state.reqs;
        },
        [GET_REQ]: (state) => (id) => {
            return state.reqs.find(item => {
               return item.RequisitionId === id
            });
        },
        [GET_SEARCH]: state => {
            return state.searchModel;
        },
        IsLoading: state => {
            return state.loading;
        }

    }
}