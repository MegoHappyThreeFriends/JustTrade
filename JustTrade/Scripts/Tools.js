
var Tools = function () {
}

Tools.Guid = function () {
	function s4() {
		return Math.floor((1 + Math.random()) * 0x10000)
		  .toString(16)
		  .substring(1);
	}
	return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
	  s4() + '-' + s4() + s4() + s4();
}

Tools.StrFormat = function () {
	if (arguments.length > 1) {
		var result = arguments[0];
		for (var i = 1; i < arguments.length; i++) {
			result = result.replace("{" + (i - 1) + "}", arguments[i]);
		}
		return result;
	}
	return arguments[0];
}

