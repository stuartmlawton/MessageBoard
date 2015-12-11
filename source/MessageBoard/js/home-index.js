// home-index
var homeIndexModule = angular.module("homeIndex", ["ngRoute"]);//[] configuration data

//page centric routes - change views on a page
homeIndexModule.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/", {
        controller: "topicsController",
        templateUrl: "/templates/topicsView.html"//html fragment for view
    });
    $routeProvider.when("/newmessage", {
        controller: "newTopicController",
        templateUrl: "/templates/newTopicView.html"//html fragment for view
    });
    $routeProvider.when("/message/:id", {//parameterise this route
        controller: "singleTopicController",
        templateUrl: "/templates/singleTopicView.html"
    });
    $routeProvider.otherwise({ redirectTo: "/" });//if uri falls through all routes, use the default
}]);

//this is interesting... if you refresh #/ the route will re-instantiate the data service via the factory (fair 'nuff)
// however, if you use the dataservice without refresh of #/ it is singleton/shared between the 2 controllers
homeIndexModule.factory("dataService", ["$http", "$q", function ($http, $q) {//use a factory to define a named injectable service.
    //this callback function will be called the FIRST TIME the service is needed - it must return objectg representing the service
    var _topics = [];
    var _isInit = false;
    var _getTopics = function () {//$q provides deferral object with a promise
        var deferred = $q.defer();
        $http.get("/api/topics?includeReplies=true")//async returns a promise object with a then method
        .then(function (result) {//then yeilds a result to the callback
            //_topics = result.data; not so hot with angular as the UI won't receive an update notification
            angular.copy(result.data, _topics);//use angular copy incase the UI needs updating
            _isInit = true;
            deferred.resolve();//we are successful - can pass data if needed
        },
        function () {
            //alert('Failed to load topics');// no dataservice mustn't interact with the end client
            deferred.reject();
        })
        return deferred.promise;//return a promise that will be either successful or not
    };
    var _addTopic = function (newTopic) {
        var deferred = $q.defer();
        $http.post("/api/topics",newTopic)//add to server
        .then
        (
            function (result) {
                var createdTopic = result.data;//this is the new topic passed back by the server
                _topics.splice(0, 0, createdTopic);//add to internal collection on singleton service
                deferred.resolve(createdTopic);
            },
            function () {
                deferred.reject();
            }
        )
        return deferred.promise;
    };
    var _isReady = function () {
        return _isInit;
    };
    var _getTopicById = function (id) {
        var deferred = $q.defer();
        if (_isReady()) {
            var topic = _findTopic(id);
            if (topic) {
                deferred.resolve(topic);
            }
            else {
                deferred.reject();
            }
        }
        else {
            _getTopics()
            .then
            (
                function () {//refactor extraction missed here
                    var topic = _findTopic(id);
                    if (topic) {
                        deferred.resolve(topic);
                    }
                    else {
                        deferred.reject();
                    }
                },
                function () {
                    deferred.reject();
                }
            );
        }
        return deferred.promise;
    };
    //private - assumed that isReady ok
    function _findTopic(id) {
        var found = null;
        $.each(_topics, function (index, item) {
            if (item.id == id) {
                found = item;
                return false;//use the each to implement a "break". Other options in js - dictionary etc
            }
        });
        return found;
    }
    var _saveReply = function (topic, reply) {
        var deferred = $q.defer();
        $http.post("/api/topics/" + topic.id + "/replies", reply)
        .then
        (
            function (result) {
                //push the created reply object into the topic's replies collection
                if (topic.replies == null) {
                    topic.replies = [];
                }
                topic.replies.push(result.data);
                deferred.resolve(result.data);//just the reply returned - not really needed though.
            },
            function () {
                deferred.reject();
            }
        );
        return deferred.promise;
    }
    //return an object with public properties - this IS the service interface
    return {
        topics: _topics,
        getTopics: _getTopics,
        addTopic: _addTopic,
        isReady: _isReady,
        getTopicById: _getTopicById,
        saveReply: _saveReply
    };
}]);


//additional minify issues here - need object with named params again
homeIndexModule.controller("topicsController", topicsController);
homeIndexModule.controller("newTopicController", newTopicController);
homeIndexModule.controller("singleTopicController", singleTopicController);

//injecting services by NAME of the object = minify issues. code change required as AngularJS uses named properties
// so make me an array and use string values to identify the named parameters (services). Angularjs will infer by param location
//var topicsController = ["$scope", "$http", "dataService",
function topicsController($scope, $http, dataService) {//angular injects scope service - container for data / dom manipulation
    $scope.data = dataService;
    $scope.isBusy = false;
    if (dataService.isReady() == false) {
        $scope.isBusy = true;
        dataService.getTopics()//async returns a promise object with a then method
        .then
        (
            function () {//then yeilds a result to the callback
            },
            function () {
                alert("Failed to load topics");// i am responsible for UI interaction - NOT the dataService
            }
        )
        .then
        (
            function () {//the then call also returns a promise so can chain - similar to a try catch finally... this will always be done
                $scope.isBusy = false;
            },
            function () {
                //nothin here.
            }
        );
    }
}//]

function newTopicController($scope, $http, $window, dataService) {
    $scope.newTopic = {};//this bit - right here, what is this all about then? surely this initialises newTopic to empty? Some angular magic happening here
    //"a place for the form to fill in the new data - a placeholder"

    $scope.save = function () {
        dataService.addTopic($scope.newTopic)
        .then
        (
            function () { $window.location = "#/"; },
            function () { alert("Could not save new topic"); }
        )
    };
}

function singleTopicController($scope, $http, $window, dataService, $routeParams) {
    $scope.topic = null;
    $scope.newReply = {};
    dataService.getTopicById($routeParams.id)//$routeParams service
    .then
    (
    function (topic) { $scope.topic = topic; },
    function () { $windows.location = "#/";}
    );
    $scope.addReply = function () {
        dataService.saveReply($scope.topic, $scope.newReply)
        .then
        (
        function () {
            //don't need to do anything here as angular will databind the topic.replies and the #/ will re-get data
            $scope.newReply.body = "";//clean out the scope reply
        },  
        function () { alert("Could not save the reply"); }
        );
    };
}