
(function () {
    "use strict";
    angular.module("PG")
			.controller("loginController", ["$scope", "$window", "loginFactory", loginController]);

    function loginController($scope, $window, loginFactory) {
        $scope.loginLoader = false
        $scope.user = {
            email: "",
            password:""
        }
        $scope.login = function () {
            $scope.loginLoader = true;
            loginFactory.login($scope.user).then(function (res) {
                console.log(res);
                $scope.loginLoader = false
                $scope.user = {
                    email: "",
                    password: ""
                }
                if (res.isSuccess) {
                    window.localStorage["utoken"] = res.token;
                    window.location.href = '/#/main';
                } else {
                    if (res.errorMessage != "") {
                        swal("Timeout connection", "Please try to refresh and create again!", "info")
                    } else {
                        swal("User not found", "Please ensure that the user is registered!", "warning");
                    }
                }
            })
        }
    }

})();

