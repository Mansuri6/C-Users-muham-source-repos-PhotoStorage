
(function () {
    "use strict";
    angular.module("PG")
			.controller("registerController", ["$scope", "$window", "loginFactory", registerController]);

    function registerController($scope, $window, loginFactory) {
        $scope.registerloader = false;
        $scope.user = {
            email: "",
            fullName: "",
            password: "",
            cpassword:""
        }
        $scope.register = function () {
            $scope.registerloader = true;

            if ($scope.user.password != $scope.user.cpassword) {
                swal("Pasword Mismatch", "please check your password", "warning");
                $scope.registerloader = false;
            } else {
                loginFactory.register($scope.user).then(function (res) {
                    $scope.user = {
                        email: "",
                        fullName: "",
                        password: "",
                        cpassword: ""
                    }
                    $scope.registerloader = false;
                    if (res.isSuccess) {
                        swal({
                            title: 'Success?',
                            text: "your account is successfuly created!",
                            type: 'success',
                            showCancelButton: true,
                            confirmButtonText: 'Sign in',
                            closeOnConfirm: true,
                            closeOnCancel: true
                        }, function (isConfirm) {
                            if (isConfirm) {
                                window.location.href = "/#/login";
                            }
                        });
                    } else {
                        if (res.errorMessage != "") {
                            swal("Timeout connection","please try to refresh and create again!","info")
                        } else {
                            swal("Email already use!","please try diffent email!","warning")
                        }
                        
                    }
                })
            }
        }
    }

})();

