
(function () {
    angular.module("PG")
        .factory("UserAuthorizationFactory", ["$http", "$q", "$window", UserAuthorizationFactory]);

    function UserAuthorizationFactory($http, $q, $window) {
        var StorageData = {};
        var url = "http://photostorage2021-001-site1.itempurl.com/";
        var c = {
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            }
        };

        StorageData.geturl = function () {
            return url;
        }

        return StorageData;
    }
})();
