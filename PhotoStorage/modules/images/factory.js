(function () {
    "use strict";
    angular.module("PG")
			.factory("imageFactory", ["$http", "$q", "UserAuthorizationFactory", imageFactory]);

    function imageFactory($http, $q, UserAuthorizationFactory) {
        var imageFactory = {};
        var u = UserAuthorizationFactory.geturl();
        var c = {
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            }
        };

        imageFactory.login = function () {
            var de = $q.defer();
            $http.get(u + 'api/login', c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        imageFactory.createAlbum = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/createAlbum', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        imageFactory.getAlbumByUser = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/getAlbumByUser', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        imageFactory.getAlbumByShared = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/getAlbumByShared', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        imageFactory.deleteImage = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/deleteImage', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        imageFactory.DeleteAlbum = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/DeleteAlbum', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        imageFactory.SharedImagebyEmail = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/SharedImagebyEmail', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        imageFactory.SharedAlbumbyEmail = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/SharedAlbumbyEmail', d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        imageFactory.getImagesByAlbumAndUser = function (d) {
            var de = $q.defer();
            $http.post(u + 'api/getImagesByAlbumAndUser',d, c).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        }

        imageFactory.AddUpdateImages = function (formData, id, albumId, geolocation, tags, capturedBy, capturedDate) {
            var de = $q.defer();
            var request = {
                method: 'POST',
                url: u + 'api/AddUpdateImages?id=' + id.toString() + '&albumId=' + albumId.toString() + "&geolocation=" + geolocation + "&tags=" + tags + "&capturedBy=" + capturedBy + "&capturedDate=" + capturedDate,
                data: formData,
                headers: {
                    'Content-Type': undefined
                }
            }
            console.log(request.url);

            $http(request).then(
                function success(res, status) { de.resolve(res.data) },
                function error(res, status) { de.resolve(res.data) }
            )
            return de.promise;
        };

        return imageFactory;
    }
})();