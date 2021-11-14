
(function () {
    "use strict";
    angular.module("PG")
			.controller("imageController", ["$scope", "$window", "imageFactory", "loginFactory", imageController]);

    function imageController($scope, $window, imageFactory, loginFactory) {
        console.log("main");
    }

})();

