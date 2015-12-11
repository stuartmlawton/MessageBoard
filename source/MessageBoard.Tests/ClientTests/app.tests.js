/// <reference path="c:\users\stuart lawton\documents\github\pluralsight\messageboard\messageboard.tests\scripts\jasmine\jasmine.js" />
/// <reference path="../../messageboard/js/myapp.js" />
//hutzpah uses the reference paths above plus it yields intellisense

//describe("test class description", testClassImplemantation) this is yer test class
//it("test method description", testMethodImplementation) these are yer test methods

//jasmine tests will have to run all the tests within a describe

describe("myapp tests", function () {
    it("isDebug", function () {
        expect(app.isDebug).toEqual(true);
    });
    it("log", function () {
        expect(app.log).toBeDefined();
        app.log("testing");
    });
});