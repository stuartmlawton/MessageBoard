//myapp.js
//SELF EXECUTING ANONYMOUS FUNCTION - err a what now? to be fair is does what it says on the tin... create anonymous function then call it immediately
(function (app) {
    app.isDebug = true;
    app.log = function (msg) {
        if(app.isDebug){
            console.log(msg);
        }
    };
})(window.app = window.app || {});//set window.app to esixting else set empty then use it as parameter(something like that)