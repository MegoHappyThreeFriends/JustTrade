$.fn.insertInformBlock = function(type, caption, message) {

	return this.each(function() {
		$(this).html(
			'<div class="inform-block inform-block-' + (type === 'info' ? 'information' : 'danger') + '">' +
			'<h4>' + caption + '</h4>' +
			'<p>' + message + '</p>' +
			'</div>'
		);
	});

};

