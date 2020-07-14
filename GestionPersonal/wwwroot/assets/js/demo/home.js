$(function(){

	var id_nav = '#cm-navbar-configuration'
    var btn_cc = 'btn-'+$(id_nav).attr('color');
    var navbar_cc = 'cm-navbar-'+$(id_nav).attr('color');


    $('#demo-buttons').on('click', 'button', function(event) {
		var color = $(this).data('switch-color');
		$('.cm-navbar').removeClass(navbar_cc);
		navbar_cc = 'cm-navbar-' + color;
		$('.cm-navbar').addClass(navbar_cc);
		$('.cm-navbar .btn').removeClass(btn_cc);
		btn_cc = 'btn-' + color;
		$('.cm-navbar .btn').addClass(btn_cc);

	    GlobalAjax(
	       "../../models/Usuarios/Usuarios.php",
	       "get",
	       "json",
	       {Action: "cambiarColorMenuUser" , GP_ColorMenuUser: color },
	       function(response){
	           
	       }
	   );

  });

});
