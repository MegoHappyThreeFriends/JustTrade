
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

Tools.AjaxGetToBlock= function (rootBlockId, blockId, url, callback)
{
	$("#" + blockId).remove();
	ShowBlockUI.Show();
	$.ajax(
		 {
		 	url: url,
		 	success: function (data) {
		 		$(rootBlockId).append("<div id='" + blockId + "'>" + data + "</div>");
		 	},
		 	dataType: 'html',
		 	complete: function () {
		 		$.unblockUI();
				 if (callback !== undefined) {
					 callback();
				 }
			 },
		 	error: function (xhr, status, error) {
		 		var message = Language.Data["Error connect to server. Message:"];
		 		alert(message + error);
		 	}
		 });
}

Tools.AjaxPostToBlock = function (rootBlockId, blockId, url, data, callback) {
	$("#" + blockId).remove();
	$.ajax({
			type: "POST",
			url: url,
			data: data,
		success: function(response) {
			if (response.length > 0) {
				$(rootBlockId).append("<div id='" + blockId + "'>" + response + "</div>");
			} else {
				callback();
			}
		},
		complete: function() {
			$.unblockUI();
		},
		error: function(xhr, status, error) {
			var message = Language.Data["Error connect to server. Message:"];
			alert(message + error);
			callback();
		}
	});

}

$.fn.serializeObject = function () {
	var o = {};
	var a = this.serializeArray();
	$.each(a, function () {
		if (o[this.name]) {
			if (!o[this.name].push) {
				o[this.name] = [o[this.name]];
			}
			o[this.name].push(this.value || '');
		} else {
			o[this.name] = this.value || '';
		}
	});
	return o;
};
