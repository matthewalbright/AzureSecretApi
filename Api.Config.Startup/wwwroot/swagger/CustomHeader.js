(function()
{
	$(function()
	{
		console.info( 'Changing Swagger header' );

		$("#header").css("background-color", "rgb(64,121,86)");
		$("#select_document").css("background-color", "#4c4903");

		var anchor = 
			'<img class="logo__img" alt="" height="30" width="30" src="/swagger/images/itworks-logo.png"><span class="logo__title">It Works!</span>';

		$('#logo').html(anchor);
		$('#logo').attr('href', 'http://www.itworks.com');
	});
})();