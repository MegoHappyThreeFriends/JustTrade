function MessageBox(msg, caption) {
	this.message = msg;
	this.caption = caption;

	this.Alert = function (callback) {
		
		var sourceDialogId = createDialog(this.message, this.caption);
		var dialogId = "#dialog_" + sourceDialogId;

		$(dialogId).dialog({
			resizable: true,
			height: 200,
			width: 500,
			modal: true,
			close: function () {
				clear(sourceDialogId);
				callback();
			},
			buttons: [
				{
					text: Language.Data["Ok"],
					icons: {
						primary: "ui-icon-check"
					},
					click: function () {
						$(dialogId).dialog("close");
					    $(dialogId).dialog("destroy");
					}
				}
			]
		});
		$(".ui-dialog").css("font-size", "12px");

	}

	this.Confirm = function (callback) {

		var sourceDialogId = createDialog(this.message, this.caption);
		var dialogId = "#dialog_" + sourceDialogId;

		$(dialogId).dialog({
			resizable: true,
			height: 200,
			width: 500,
			modal: true,
			close: function () {
				clear(sourceDialogId);
				
			},
			buttons: [
				{
					text: Language.Data["Yes"],
					icons: {
						primary: "ui-icon-check"
					},
					click: function () {
						callback(true);
						$(dialogId).dialog("close");
						$(dialogId).dialog("destroy");
					}
				},
				{
					text: Language.Data["No"],
					icons: {
						primary: "ui-icon-close"
					},
					click: function () {
						callback(false);
						$(dialogId).dialog("close");
						$(dialogId).dialog("destroy");
					}
				}
			]
		});

		$(".ui-dialog").css("font-size","12px");
	}

	
	function createDialog(message, caption) {
		var dialogid = Tools.Guid();
		var dialogId = "dialog_" + dialogid;
		var divDialog = "<div id='" + dialogId + "' class='' title='" + caption + "'> <div class='js-message-text'> " + message + " </div> </div>";
		$("body").append(divDialog);
		return dialogid;
	}

	function clear(dialogid) {
		$("#dialog_" + dialogid).parent().remove();
		$("#dialog_" + dialogid).remove();
	}

}
