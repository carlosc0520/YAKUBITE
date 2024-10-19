/**
 *  Pages Authentication
 */

'use strict';
const formAuthentication = $('#formAuthentication');
const uisApis = {
  SEG: "/Auth/Login/Login?handler",
  LOGIN: "/Auth/Login/Cover?handler",
  INICIO: "/Inicio/Home",
}

document.addEventListener('DOMContentLoaded', function (e) {
  (function () {
    localStorage.clear();
    
    const validacionesAdd = {
      "password": agregarValidaciones({
        minlength: 6,
        required: true
      }),
      "email": agregarValidaciones({
        required: true,
      }),
    }

    if (formAuthentication) {
      configFormVal("formAuthentication", validacionesAdd, () => eventos.autenticar());
    }

    const eventos = {
      autenticar: () => {

        // swalFire.cargando(["Espere un momento", "Estamos agregando el rol"]);

        var token = $('input[name="__RequestVerificationToken"]').val();

        let url = uisApis.LOGIN + '=MiAccion';
        let formData = postForm(formAuthentication);

        $.ajax({
          url,
          beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", token);
          },
          type: 'POST',
          dataType: 'json',
          contentType: 'application/json',
          data: JSON.stringify(formData),
          success: function (data) {
            console.log(data)
            if (data.succeeded && !data.cambiarContrasena && data.accessToken) {
              localStorage.setItem("accessToken", data.accessToken);
              window.location.href = uisApis.INICIO;
            } else if (data.succeeded && data.cambiarContrasena) {
              window.location.href = "/Auth/Login/ChangePassword";
            } else {
              swalFire.error(data.errorMessage);
            }
          },
          error: function (jqXHR, textStatus, errorThrown) {
            swalFire.error(errorThrown);
          },
          complete: function () {
            // swalFire.close();
          }
        });
      }
    }
  })();
});
