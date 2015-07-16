var Language = function() {
}

Language.Data = null;

Language.Get = function (name, callback) {
	if (Language.Data == null) {
		Debug.Write("Load language");
		console.log($.get("Language/GetLanguageJson", function (data) {
			try {
				Language.Data = jQuery.parseJSON(data);
			} catch (e) {
				Debug.WriteError("Error in parse language data! Info:" + e.message);
				return;
			} 
			callback(Language.Data[name]);
		}));
	} else {
		callback(Language.Data[name]);
	}
};
