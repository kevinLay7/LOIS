<template>
  <div class="container-fluid">
      <app-nav></app-nav>
      <transition leave-active-class="fadeOut">
          <app-loading></app-loading>
      </transition>
      <transition enter-active-class="fadeIn" leave-active-class="hide">
          <router-view></router-view>
      </transition>
      <div class="versionInfo">
          <p class="float-right">v1.1.0</p>
      </div>
  </div>
</template>

<script type="text/javascript">
  import Nav from './componenets/navigation/Nav'
  import loading from './componenets/shared/loading'

  export default {
    name: 'app',
    components: {
        appNav: Nav,
        appLoading: loading
    },
    data() {
        return {

        }
    },
    computed: {
      loading(){
          return this.$store.getters.IsLoading;
      }
    },
      created(){
          this.$store.commit('setLoading', false);
      }
  }
</script>

<style>
  /*@import './styles/main.scss';*/
  /*@import '~bootstrap/scss/bootstrap.scss';*/
    @import '~bootstrap/dist/css/bootstrap.min.css';
    @import './styles/main.scss';
    .hide{
        display: none;
    }
    .versionInfo{
        position:absolute;
        bottom: 0;
        right: 0;
        color: #888;
        font-size: 9px;
    }
</style>