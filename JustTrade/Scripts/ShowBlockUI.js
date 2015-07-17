var ShowBlockUI = function () { }
ShowBlockUI.Show = function ()
{
	message = Language.Data["Executing"];
	$.blockUI({
		css: {
			border: 'none',
			padding: '15px',
			backgroundColor: '#000',
			'-webkit-border-radius': '10px',
			'-moz-border-radius': '10px',
			opacity: .5,
			color: '#fff'
		},
		message: '<img src="/Content/images/BusyGrid.gif" width="24px" /> ' + message
	});
}