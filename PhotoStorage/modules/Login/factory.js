(function () {
    "use strict";
    angular.module("PG")
			.factory("loginFactory", ["$http", "$q","UserAuthorizationFactory", loginFactory]);

    function loginFactory($http, $q, UserAuthorizationFactory) {
        var loginFactory = {};
        var u = UserAuthorizationFactory.geturl();
        var c = {
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            }
        };

        loginFactory.login = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/login', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        loginFactory.register = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/register', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        loginFactory.checkIfLogin = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/checkIfLogin', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        return loginFactory;
    }
})();