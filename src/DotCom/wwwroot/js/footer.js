var footer = footer || {}

footer.initialize = function(){
	$.get( "/version", function( data ) {
		$( "#version" ).html( data );
	});
}