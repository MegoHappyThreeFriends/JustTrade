
$.fn.proggressBar = function (settings) {

	return this.each(function () {
		var progressbarId = ("progressbar_" + Math.random()).replace(".","");
		$(this).html(
		'<div id=' + progressbarId + '>' +
		'<div class="progress">' +
			
			'<div id="progress" class="progress-bar progress-bar-success progress-bar-striped active" ' +
				'role="progressbar" style="width: 0%">' +
			'<span ></span>' +
			'</div>' +
			' <div id="info"></div> ' +
			'</div>');
		var url = settings.Url;
		window.setTimeout(function () {
			ProgressBarRefresh(progressbarId, url, settings.Complete);
		}, 1500);
	});

};

function ProgressBarRefresh(prgbId, url, completeCallback) {
	$.getJSON(url, function (data) {
		var progressbar = $("#" + prgbId);
		var progress = data.Progress;
		var info = data.Information;
		if (progress === -1) {
			progressbar.find("#progress").removeClass("progress-bar-success");
			progressbar.find("#progress").addClass("progress-bar-danger");
			progressbar.find("#progress").removeClass("active");
			var msg = new MessageBox(info, data.Error);
			msg.Alert();
			return;
		}
		progressbar.find("#progress").css("width", progress + "%");
		progressbar.find("#info").html(info);
		//progressbar.find("span").html(info);
		if (data.IsRunning === true) {
			//console.log("[" + url + "] Refresh " + progress);
			window.setTimeout(function() {
				ProgressBarRefresh(prgbId, url, completeCallback);
			}, 500);
		} else {
			progressbar.find("#progress").removeClass("active");
			//console.log("[" + url + "] Stop refresh");
			progressbar.remove();
			if (completeCallback != undefined) {
				completeCallback();
			}
		}
	});
}
