function InputValidator(root) {
	this.rootInputSelector;

	this.Check = function (callback) {
		var result = true;
		this.rootInputSelector = root + " input";
		$(this.rootInputSelector).each(function (index, value) {
			var id = $(value).attr("id");
			var inputType = $("#" + id).attr("val-type");
			if (inputType != undefined) {
				if (!validate(id)) {
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
				}, 1500);
			});
		}

		callback(result);
	}

	function validate(id) {
		var inputType = $("#" + id).attr("val-type");
		switch (inputType) {
			case "int":
				return checkInt(id);
			case "text":
				return checkText(id);
			case "float":
				return checkFloat(id);
			case "mask":
				return checkRegExp(id);
			case "login":
				return checkLogin(id);
			default:
				throw new Error("Not found validate type: "+inputType);
		}
	}

	function checkLogin(id) {
		var regex = /^[a-zA-Z0-9]*$/;
		var obj = $("#" + id);
		var min = obj.attr("val-min");
		min = min == undefined ? -100 : min;
		var max = obj.attr("val-max");
		max = max == undefined ? 100 : max;
		var val = obj.val();
		if (regex.test(val)) {
			if (val.length >= min && val.length <= max) {
				return true;
			}
		}
		addInformLabel(id, Tools.StrFormat(Language.Data["Must be latin and number chars. Min:{0} Max:{1}"], min, max));
		return false;
	}


	function checkInt(id) {
		var regex = /^\d*$/;
		var obj = $("#" + id);
		var min = obj.attr("val-min");
		min = min == undefined ? -100 : min;
		var max = obj.attr("val-max");
		max = max == undefined ? 100 : max;
		var val = obj.val();
		if (regex.test(val)) {
			var intVal = parseInt(val);
			if (intVal >= min && intVal <= max) {
				return true;
			}
		}
		addInformLabel(id, Tools.StrFormat(Language.Data["Incorrect numeric. Min:{0} Max:{1}"], min, max));
		return false;
	}

	function checkText(id) {
		var obj = $("#" + id);
		var min = obj.attr("val-min");
		min = min == undefined ? 0 : min;
		var max = obj.attr("val-max");
		max = max == undefined ? 5000 : max;
		var val = obj.val();
		if (val.length >= min && val.length <= max) {
			return true;
		}
		addInformLabel(id, Tools.StrFormat(Language.Data["Incorrect text. Min:{0} Max:{1}"], min, max));
		return false;
	}

	function checkFloat(id) {
		var regex = /^-?\d*(\.\d+)?$/;
		var obj = $("#" + id);
		var min = obj.attr("val-min");
		min = min == undefined ? -100 : min;
		var max = obj.attr("val-max");
		max = max == undefined ? 100 : max;
		var val = obj.val();
		if (regex.test(val)) {
			var intVal = parseFloat(val);
			if (intVal >= min && intVal <= max) {
				return true;
			}
		}
		addInformLabel(id, Tools.StrFormat(Language.Data["Incorrect float. Min:{0} Max:{1}"], min, max));
		return false;
	}

	function checkRegExp(id) {
		var obj = $("#" + id);
		var min = obj.attr("val-min");
		min = min == undefined ? -100 : min;
		var max = obj.attr("val-max");
		max = max == undefined ? 100 : max;
		var mask = obj.attr("val-mask");
		var maskExample = obj.attr("val-mask-example");
		var regex = new RegExp(mask);
		var val = obj.val();
		if (regex.test(val)) {
			if (val.length >= min && val.length <= max) {
				return true;
			}
		}
		addInformLabel(id, Tools.StrFormat(Language.Data["Need format: {0}"], maskExample));
		return false;
	}


	function addInformLabel(id, text) {
		var obj = $("#" + id).parent();
		obj.before("<div class='input-validate-information'> " + text + " </div>");
	}


}
