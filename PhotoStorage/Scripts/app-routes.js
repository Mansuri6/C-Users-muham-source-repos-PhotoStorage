(function () {

    angular.module("PG", ["ui.router", "ngMessages", "ngFileUpload"])
			.config(function ($locationProvider, $stateProvider, $urlRouterProvider, $qProvider) {
			    $qProvider.errorOnUnhandledRejections(false);
			    $urlRouterProvider.otherwise('/login');

			    $stateProvider

                .state("login", {
                    url: "/login",
                    templateUrl: "../modules/login/login.html",
                    controller: 'loginController'
                })
                .state("main", {
                    url: "/main",
                    templateUrl: "../modules/images/image.html",
                    controller: 'imageController'
                })
                    .state("register", {
                        url: "/register",
                        templateUrl: "../modules/Login/Register/register.html",
                        controller:"registerController"
                    })
                    .state("images", {
                        url: "/captured_images",
                        templateUrl: "../modules/images/imageList/imageList.html",
                        controller: 'imageListController'
                    })

                    .state("albumShare", {
                        url: "/shared_album",
                        templateUrl: "../modules/images/sharedAlbum/sharedAlbum.html",
                        controller: 'sharedAlbumController'
                    })

			    $locationProvider.hashPrefix('');
			    //$locationProvider.html5Mode({ enabled: true, requireBase: false, rewriteLinks: true })
			    //$locationProvider.hashPrefix('!');
			});
})();

