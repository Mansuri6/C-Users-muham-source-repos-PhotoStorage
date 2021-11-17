
(function () {
    "use strict";
    angular.module("PG")
			.controller("imageListController", ["$scope", "$window", "imageFactory", "loginFactory","$location", "$http", imageListController]);

    function imageListController($scope, $window, imageFactory, loginFactory, $location, $http) {
        var formData = new FormData();
        $scope.imgLoader = false;
        $scope.imgSrc = "";
        $scope.isEdit = false;

        $scope.editImage = function (d) {
            $scope.isEdit = true;
            d.capturedDate = new Date(d.capturedDate);
            $scope.img = angular.copy(d);
            $scope.imgSrc = '../images/' + d.albumId + '/' + d.id + d.imgEx;
            $("#addImages").modal("show");
        }

        $scope.param = $location.search();
        var d = {
            token: window.localStorage["utoken"]
        }
        loginFactory.checkIfLogin(d).then(function (res) {
            if (!res) {
                window.location.href = "/login";
                window.localStorage["utoken"] = undefined;
            }
        });
        $scope.imgList = [];
        $scope.SetImage = function (files, id) {
            console.log($scope.files)
            if ($scope.files != undefined) {
                var file = $scope.files;
                var reader = new FileReader();
                reader.onload = function (evt) {
                    $scope.$apply(function () {
                        console.log(evt.target.result);
                        $scope.imgSrc = evt.target.result;
                    });
                };
                reader.readAsDataURL(file);
            }
        }
        $scope.sharLoader = false;

        $scope.shareImages = function () {
            $scope.sharLoader = true;
            imageFactory.SharedImagebyEmail($scope.share).then(function (res) {
                if (res.isSuccess) {
                    swal("Success", "Successfuly share your photo to " + $scope.share.sharedTo);
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

        $scope.download = function (d) {
            var anchor = angular.element('<a/>');
            anchor.attr({
                href: '../images/' + d.albumId + '/' + d.id + d.imgEx,
                target: '_blank',
                download: d.tags + d.imgEx
            })[0].click();
        }

        $scope.addImages = function () {
            $scope.img = {
                id: 0,
                geolocation: "",
                tags: "",
                capturedDate: new Date(),
                capturedDateBy: ""
            }
            $scope.isEdit = false;
            $("#addImages").modal("show");
        }

        $scope.SaveImages = function (d) {
            $scope.imgLoader = true;
            formData = new FormData();
            formData.append("", $scope.files)
            var capturedDate = d.capturedDate.getFullYear() + "/" + (d.capturedDate.getMonth() + 1) + "/" + d.capturedDate.getDate();
            imageFactory.AddUpdateImages(formData, d.id, $scope.param.id, d.geolocation, d.tags, d.capturedBy, capturedDate).then(
                function (res) {
                    $scope.imgLoader = false;
                    console.log(res);
                    if (res.isSuccess) {
                        swal("Success", "successfuly saved!", "success");
                        getImagesByAlbumAndUserList();
                        $("#addImages").modal("hide");
                    } else {
                        if (res.errorMessage != "") {
                            swal("Timeout connection", "please try to refresh and create again!", "info")
                        }
                        $("#addImages").modal("hide");
                    }
                })
        }

        $scope.viewButton = function (i) {
            angular.forEach($scope.imgList, function (d) {
                document.getElementById("hover" + d.id).style.display = "none"
            })
            document.getElementById("hover" + i.id).style.display = "block"
        }

        $scope.imgList = [];
        var getImagesByAlbumAndUserList = function () {
            $scope.imgList = [];
            var d = {
                id:$scope.param.id,
                type: $scope.param.type,
                tokenId: window.localStorage["utoken"]
            }
            imageFactory.getImagesByAlbumAndUser(d).then(function (res) {
                $scope.imgList = res;
                console.log($scope.imgList);
            })
        }
        getImagesByAlbumAndUserList();


        $scope.deleteImage = function (d) {
            swal({
                title: 'Warning?',
                text: "are you sure you want to delete this photo?",
                type: 'info',
                showCancelButton: true,
                confirmButtonText: 'Yes',
                closeOnConfirm: true,
                closeOnCancel: true
            }, function (isConfirm) {
                if (isConfirm) {
                    imageFactory.deleteImage(d).then(function (res) {
                        if (res.isSuccess) {
                            swal("Success", "image successfuly deleted", "success");
                            getImagesByAlbumAndUserList();
                        } else {
                            swal("Timeout connection", "please try to refresh and create again!", "info")
                        }
                    })
                    window.location.reload();
                }
            });
        }
        
    }

})();

