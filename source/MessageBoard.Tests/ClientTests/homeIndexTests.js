/// <reference path="c:\users\stuart lawton\documents\github\pluralsight\messageboard\messageboard.tests\scripts\jasmine\jasmine.js" />
/// <reference path="../../messageboard/scripts/angular.js" />
/// <reference path="../../messageboard/js/home-index.js" />
/// <reference path="../../messageboard/scripts/angular-route.js" />
/// <reference path="../../messageboard/scripts/angular-mocks.js" />

//this is a hierarchy the callbackj in the outer describe creates another describe with its inside it.. jasmine handles the nesting
describe("home-index tests->", function () {
    beforeEach(function () {//this is yer SetUp method ran before each of the describes below
        module("homeIndex");//dunno?!?! basically a using statement - using the module "homeIndex"
    });

    var $httpBackend;//angular mock for a back end server
    beforeEach(inject(function ($injector) {//injecting the $injector Angular service!
        $httpBackend = $injector.get("$httpBackend");//WTF? .. so, use the angular injector to set the $httpBackend variable to an instance of the angular.mock httpbackend object;
        $httpBackend.when("GET", "/api/topics?includeReplies=true")
        .respond
        ([
            {
            title: "Mocked Message 1",
            body: "This is the body of the mocked message",
            id: 1,
            created: "20151209"
            },
            {
                title: "Mocked Message 2",
                body: "This is the body of the mocked message",
                id: 1,
                created: "20151209"
            },
            {
                title: "Mocked Message 3",
                body: "This is the body of the mocked message",
                id: 1,
                created: "20151209"
            }
        ]);
    }));

    afterEach(function () {//clear out the back end mock
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    describe("dataService->", function () {
        it("can load topics", inject(function (dataService) {//inject is part of AngularJS.mock this yields access to the dataService - how it matches params i do NOT know - looks stringy!
            expect(dataService.topics).toEqual([]);//without the get call the array exposed on the interface should be empty
            //by mocking the back end we are ONLY testing the dataService - avoids the server side api logic altogether
            $httpBackend.expectGET("/api/topics?includeReplies=true");//tell httpbackend to listen for a get on this url
            dataService.getTopics();
            $httpBackend.flush();
            expect(dataService.topics.length).toEqual(3);
        }));
    });

    describe("topicsController->", function () {
        it("loads data", inject(function ($controller, $http, dataService) {//can't controllers directly as they are globally scoped. So inject the controller service and use that to get at required controler
            var theScope = {};//just need an empty holder
            $httpBackend.expectGET("/api/topics?includeReplies=true");//tell httpbackend to listen for a get on this url
            var ctrl = $controller("topicsController", {$scope: theScope, $http: $http, dataService: dataService});//use controller service to construct the topicsController with dependencies
            $httpBackend.flush();

            expect(ctrl).not.toBeNull();
            expect(theScope.data).toBeDefined();

        }));
    })
});