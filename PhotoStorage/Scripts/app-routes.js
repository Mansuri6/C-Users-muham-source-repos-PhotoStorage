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

			    $locationProvider.hashPrefix('');
			    //$locationProvider.html5Mode({ enabled: true, requireBase: false, rewriteLinks: true })
			    //$locationProvider.hashPrefix('!');
			});
})();

