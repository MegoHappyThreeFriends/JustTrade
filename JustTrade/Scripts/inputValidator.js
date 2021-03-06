﻿function InputValidator(root) {
	this.rootInputSelector;

	this.Check = function (callback) {
		var result = true;
		this.rootInputSelector = root + " input";
		$(this.rootInputSelector).each(function (index, value) {
			var obj = $(value);
			var inputType = obj.attr("val-type");
			if (inputType != undefined) {
				if (!validate(obj)) {
					result = false;
				}
			}
		});

		if (!result) {
			$(".input-validate-information").fadeIn(500, function () {
				setTimeout(function () {
					$(".input-validate-information").fadeOut(500, function () {
						$(".input-validate-information").remove();
					});
				}, 5000);
			});
		}

		if (callback != undefined) {
			callback(result);
		}
		return result;
	}

	function addInformLabel(obj, text) {
		var errorMessage = obj.attr("val-error-msg");
		if (errorMessage != undefined) {
			text = errorMessage;
		}
		var parentObj = obj.parent();
		if ($(parentObj).hasClass("input-group")) {
			parentObj.before("<div class='input-validate-information'> " + text + " </div>");
		} else {
			obj.before("<div class='input-validate-information'> " + text + " </div>");
		}
	}

	function addInformLabelExtra(obj, text) {
		var parentObj = obj.parent();
		if ($(parentObj).hasClass("input-group")) {
			parentObj.before("<div class='input-validate-information'> " + text + " </div>");
		} else {
			obj.before("<div class='input-validate-information'> " + text + " </div>");
		}
	}


	function checkMin(obj) {
		var min = parseInt(obj.attr("val-min"));
		var val = obj.val();
		var valid = (val.length >= min);
		if (!valid) {
			addInformLabelExtra(obj, Tools.StrFormat(Language.Data["Minimum number of characters:{0}"], min));
		}
		return valid;
	}

	function checkMax(obj) {
		var max = parseInt(obj.attr("val-max"));
		var val = obj.val();
		var valid = (val.length <= max);
		if (!valid) {
			addInformLabelExtra(obj, Tools.StrFormat(Language.Data["Maximum number of characters:{0}"], max));
		}
		return valid;
	}

	function checkMatch(obj) {
		var matchId = obj.attr("val-match");
		var matchObj = $("#" + matchId);
		if (matchObj.val() == obj.val()) {
			return true;
		}
		addInformLabel(obj, Language.Data["Data not match."]);
		return false;
	}

	function checkKey(obj) {
		var regex = /^[a-zA-Z0-9-_]*$/;
		var val = obj.val();
		if (regex.test(val)) {
			return true;
		}
		addInformLabel(obj, Language.Data["Must be latin and number chars."]);
		return false;
	}

	function checkIP(obj) {
		var dataIsValid = true;
		var regexIp = /\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b$/;
		var val = obj.val().replace(/\s/g, '');
		var ipList = val.split(",");
		for (var i = 0; i < ipList.length; i++) {
			if (!(regexIp.test(val))) {
				dataIsValid = false;
				addInformLabel(obj, Language.Data["Must be IP"]);
				break;
			}
		}
		return dataIsValid;
	}

	function checkIPList(obj) {
		var dataIsValid = true;
		var regexIp = /\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b$/;
		var regexIpRange = /\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b-\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b$/;
		var val = obj.val().replace(/\s/g, '');
		var ipList = val.split(",");
		for (var i = 0; i < ipList.length; i++) {
			if (!regexIp.test(val) && !regexIpRange.test(val)) {
				dataIsValid = false;
				addInformLabel(obj, Language.Data["Must be IP or IP range"]);
				break;
			}
		}
		return dataIsValid;
	}

	function checkIPRange(obj) {
		var dataIsValid = true;
		var regexIpRange = /\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b-\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b$/;
		var val = obj.val().replace(/\s/g, '');
		var ipList = val.split(",");
		for (var i = 0; i < ipList.length; i++) {
			if (!(regexIpRange.test(val))) {
				dataIsValid = false;
				addInformLabel(obj, Language.Data["Must be IP range"]);
				break;
			}
		}
		return dataIsValid;
	}

	function checkInt(obj) {
		var regex = /^\d*$/;
		var min = obj.attr("val-min");
		min = min == undefined ? -100 : min;
		var max = obj.attr("val-max");
		max = max == undefined ? 100 : max;
		var val = obj.val();
		if (regex.test(val)) {
			var intVal = parseInt(val);
			if (intVal < min) {
				addInformLabel(obj, Tools.StrFormat(Language.Data["Minimum value: {0}"], min));
				return false;
			}
			if (intVal > max) {
				addInformLabel(obj, Tools.StrFormat(Language.Data["Maximum value: {0}"], min));
				return false;
			}
			return true;
		}
		addInformLabel(obj, Language.Data["Incorrect numeric."]);
		return false;
	}

	function checkFloat(obj) {
		var regex = /^-?\d*(\.\d+)?$/;
		var min = obj.attr("val-min");
		min = min == undefined ? -100 : min;
		var max = obj.attr("val-max");
		max = max == undefined ? 100 : max;
		var val = obj.val();
		if (regex.test(val)) {
			var floatVal = parseFloat(val);
			if (floatVal < min) {
				addInformLabel(obj, Tools.StrFormat(Language.Data["Minimum value: {0}"], min));
				return false;
			}
			if (floatVal > max) {
				addInformLabel(obj, Tools.StrFormat(Language.Data["Maximum value: {0}"], min));
				return false;
			}
			return true;
		}
		addInformLabel(obj, Language.Data["Incorrect float."]);
		return false;
	}

	function checkRegExp(obj) {
		var mask = obj.attr("val-mask");
		var maskExample = obj.attr("val-mask-example");
		var regex = new RegExp(mask);
		var val = obj.val();
		if (regex.test(val)) {
			return true;
		}
		addInformLabel(obj, Tools.StrFormat(Language.Data["Need format: {0}"], maskExample));
		return false;
	}

	
	function validate(obj) {
		var inputType = obj.attr("val-type");
		var result = false;
		var ignoreMinMax = false;
		switch (inputType) {
			case "int":
				result = checkInt(obj);
				ignoreMinMax = true;
				break;
			case "float":
				result = checkFloat(obj);
				ignoreMinMax = true;
				break;
			case "mask":
				result = checkRegExp(obj);
				break;
			case "key":
				result = checkKey(obj);
				break;
			case "ip":
				result = checkIP(obj);
				ignoreMinMax = true;
				break;
			case "iprange":
				result = checkIPRange(obj);
				ignoreMinMax = true;
				break;
			case "iplist":
				result = checkIPList(obj);
				ignoreMinMax = true;
				break;
			case "text":
				break;
			case "match":
				result = checkMatch(obj);
				break;
			default:
				throw new Error("Not found validate type: " + inputType);
		}

		if (!ignoreMinMax) {
			var min = obj.attr("val-min");
			if (min != undefined) {
				result = checkMin(obj);
			}

			var max = obj.attr("val-max");
			if (max != undefined) {
				result = checkMax(obj);
			}
		}

		return result;
	}

}
