
(function () {
    "use strict";
    angular.module("PG")
			.controller("sharedAlbumController", ["$scope", "$window", "imageFactory", "loginFactory", sharedAlbumController]);

    function sharedAlbumController($scope, $window, imageFactory, loginFactory) {
        $scope.album = {
            tokenId: window.localStorage["utoken"],
            fName:""
        }

        $scope.AlbumLoader = false;

        var d = {
            token: window.localStorage["utoken"]
        }
        loginFactory.checkIfLogin(d).then(function (res) {
            if (!res) {
                window.location.href = "/#/login";
                window.localStorage["utoken"] = undefined;
            }
        });


        $scope.viewButton = function (i) {
            angular.forEach($scope.albumList, function (d) {
                document.getElementById("hover" + d.id).style.display = "none"
            })
            document.getElementById("hover" + i.id).style.display = "block"
        }

        $scope.albumList = [];
        var getAlbumByShared = function () {
            $scope.albumList = [];
            imageFactory.getAlbumByShared($scope.album).then(function (res) {
                $scope.albumList = res;
            })
        }
        $scope.isAdd = true;
        $scope.addModal = function () {
            $scope.isAdd = true;
            $scope.album = {
                tokenId: window.localStorage["utoken"],
                id:0,
                fName: ""
            }
            $("#addAlbum").modal("show");
        }

        $scope.deleteImage = function (d) {
            swal({
                title: 'Warning?',
                text: "are you sure you want to delete this Album?, all photos in this album will be delete also",
                type: 'info',
                showCancelButton: true,
                confirmButtonText: 'Yes',
                closeOnConfirm: true,
                closeOnCancel: true
            }, function (isConfirm) {
                if (isConfirm) {
                    imageFactory.DeleteAlbum(d).then(function (res) {
                        if (res.isSuccess) {
                            swal("Success", "Album successfuly deleted", "success");
                            getImagesByAlbumAndUserList();
                        } else {
                            swal("Timeout connection", "please try to refresh and create again!", "info")
                        }
                    })
                    window.location.reload();
                }
            });
        }
        $scope.sharLoader = false;
        $scope.shareImages = function () {
            $scope.sharLoader = true;
            imageFactory.SharedAlbumbyEmail($scope.share).then(function (res) {
                if (res.isSuccess) {
                    swal("Success", "you success share your photo to " + $scope.share.sharedTo);
                    $scope.sharLoader = false;
                    $("#shareModal").modal("hide");
                } else {
                    swal("Oops!", res.errorMessage, "warning")
                    $scope.sharLoader = false;
                    $("#shareModal").modal("hide");
                }
            })
        }

        $scope.sharedModal = function (d) {
            $scope.share = {
                sharedTo: "",
                id: d.id
            };
            $("#shareModal").modal("show");
        }

        $scope.ediModal = function (d) {
            $scope.album = {
                tokenId: window.localStorage["utoken"],
                id: d.id,
                fName: d.fName
            }
            $scope.isAdd = false;
            $("#addAlbum").modal("show");
        }
        getAlbumByShared();
        $scope.createAlbumFolder = function () {
            var invalid = false;
            if ($scope.album.fName.toUpperCase() == ("Photo's shared to you").toUpperCase()) {
                swal("Folder already exists", "please input different folder name", "warning");
                invalid = true;
            } else if ($scope.album.fName.toUpperCase() == ("Photo's you shared").toUpperCase()) {
                swal("Folder already exists", "please input different folder name", "warning");
                invalid = true;
            }
            if (!invalid) {
                $scope.AlbumLoader = true;
                imageFactory.createAlbum($scope.album).then(function (res) {
                    $scope.AlbumLoader = false;
                    if (res.isSuccess) {
                        swal("Success", "successfuly saved!", "success");
                        $scope.album = {
                            tokenId: window.localStorage["utoken"],
                            fName: ""
                        }
                        getAlbumsById();
                        $("#addAlbum").modal("hide");
                    } else {
                        if (res.errorMessage != "") {
                            swal("Timeout connection", "please try to refresh and create again!", "info")
                            $("#addAlbum").modal("hide");
                        } else {
                            if (res.album.id == 0) {
                                swal("Not Login", "please go to login page and try to login!", "info");
                                $("#addAlbum").modal("hide");
                            } if (res.album.id == 1) {
                                swal("Folder name exists", "please input different folder name", "warning");
                                $("#addAlbum").modal("hide");
                            }

                        }
                    }
                })
            }
        }
        

        $scope.gotoimages = function (id,fName, type) {
            window.location.href = "/#/captured_images?type=" + type + "&id=" + id + "&album=" + fName + "&isShared=1";
        }
    }

})();

