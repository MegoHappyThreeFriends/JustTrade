
function GetPartial(url, elementId){
	$.ajax({
		url: url,
	  	context: document.body
	}).done(function(data) {

	  	//callback(data);
	}).fail(function(errorData) {
    	alert(errorData.Message);
  	});
}

