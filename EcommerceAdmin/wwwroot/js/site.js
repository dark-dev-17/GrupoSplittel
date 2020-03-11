// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//var app_log = new Vue({
//    el: '#app_navBar',
//    data: {
//        userData: {},
//        Error: {
//            Error: false,
//            Description: ""
//        },
//        loginSuccess: false,
//        loading: false
//    },
//    mounted() {
//        axios.get('./Empleado/GetInformation').then(response => {
            
//            this.userData = response.data.data;
//            console.log(this.userData);
//            //this.Error.Error = response.data.error;
//            //this.Error.Description = response.data.description;
//            //if (response.data.error == false) {
//            //    this.loginSuccess = true;
//            //    //router.go('');
//            //    window.location = '../Home';
//            //} else {
//            //    this.loginSuccess = false;
//            //}
//        }).catch(error => {
//            console.log(error);
//            //this.Error.Error = true;
//            //this.loginSuccess = false;
//            //this.Error.Description = error.message;
//        }).finally(() => this.loading = false);
//    },
//    methods: {
        
//    }
//})
