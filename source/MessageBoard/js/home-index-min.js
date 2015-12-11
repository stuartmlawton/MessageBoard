function newTopicController(e,o,t,n){e.newTopic={},e.save=function(){n.addTopic(e.newTopic).then(function(){t.location="#/"},function(){alert("Could not save new topic")})}}function singleTopicController(e,o,t,n,i){e.topic=null,e.newReply={},n.getTopicById(i.id).then(function(o){e.topic=o},function(){$windows.location="#/"}),e.addReply=function(){n.saveReply(e.topic,e.newReply).then(function(){e.newReply.body=""},function(){alert("Could not save the reply")})}}var module=angular.module("homeIndex",["ngRoute"]);module.config(["$routeProvider",function(e){e.when("/",{controller:"topicsController",templateUrl:"/templates/topicsView.html"}),e.when("/newmessage",{controller:"newTopicController",templateUrl:"/templates/newTopicView.html"}),e.when("/message/:id",{controller:"singleTopicController",templateUrl:"/templates/singleTopicView.html"}),e.otherwise({redirectTo:"/"})}]),module.factory("dataService",["$http","$q",function(e,o){function t(e){var o=null;return $.each(n,function(t,n){return n.id==e?(o=n,!1):void 0}),o}var n=[],i=!1,r=function(){var t=o.defer();return e.get("/api/topics?includeReplies=true").then(function(e){angular.copy(e.data,n),i=!0,t.resolve()},function(){t.reject()}),t.promise},l=function(t){var i=o.defer();return e.post("/api/topics",t).then(function(e){var o=e.data;n.splice(0,0,o),i.resolve(o)},function(){i.reject()}),i.promise},c=function(){return i},p=function(e){var n=o.defer();if(c()){var i=t(e);i?n.resolve(i):n.reject()}else r().then(function(){var o=t(e);o?n.resolve(o):n.reject()},function(){n.reject()});return n.promise},u=function(t,n){var i=o.defer();return e.post("/api/topics/"+t.id+"/replies",n).then(function(e){null==t.replies&&(t.replies=[]),t.replies.push(e.data),i.resolve(e.data)},function(){i.reject()}),i.promise};return{topics:n,getTopics:r,addTopic:l,isReady:c,getTopicById:p,saveReply:u}}]),module.controller("topicsController",topicsController),module.controller("newTopicController",newTopicController),module.controller("singleTopicController",singleTopicController);var topicsController=["$scope","$http","dataService",function(e,o,t){e.data=t,e.isBusy=!1,0==t.isReady()&&(e.isBusy=!0,t.getTopics().then(function(){},function(){alert("Failed to load topics")}).then(function(){e.isBusy=!1},function(){}))}];