var Debug = function () { }

Debug.Write = function (message) {
	console.log(" - " + message);
};

Debug.WriteError = function (message) {
	console.log('%c ' + message , ' color: red');
};

