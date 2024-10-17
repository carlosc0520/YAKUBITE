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
      // Generar un toast loading
      autenticar: async (mensaje = "Espero un momento...") => {
        eventos.toasts.info(mensaje, 'Autenticando');

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
            toastr.clear();

            if (data.succeeded && !data.cambiarContrasena && data.accessToken) {
              eventos.toasts.success(data.message, 'Bienvenido', () => {
                
              });

              localStorage.setItem("accessToken", data.accessToken);
              window.location.href = uisApis.INICIO;

            } else if (data.succeeded && data.cambiarContrasena) {
              window.location.href = "/Auth/Login/ChangePassword";
            } else {
              let mensajeError = data.errorMessage ? data.errorMessage.replace('ACCESO DENEGADO:', '') : 'Error al autenticar';
              eventos.toasts.error(mensajeError, 'Error');
            }
          },
          error: function (jqXHR, textStatus, errorThrown) {
            eventos.toasts.error('Error al autenticar', 'Error');
          },
          complete: function () {
          }
        });
      },

      toasts: {
        error: (mensaje, title) => {
          toastr.error(mensaje, title, {
            timeOut: 5000,
            extendedTimeOut: 1000,
            tapToDismiss: true,
            closeButton: true,
            progressBar: true,
            positionClass: 'toast-top-right',
            preventDuplicates: true,
          });
        },
        success: (mensaje, title, evet = () => {}) => {
          toastr.success(mensaje, title, {
            timeOut: 5000,
            extendedTimeOut: 1000,
            tapToDismiss: true,
            closeButton: true,
            progressBar: true,
            positionClass: 'toast-top-right',
            preventDuplicates: true,
            onclick: () => {
              evet();
            },
          });
        },
        info: (mensaje, title) => {
          toastr.info(mensaje, title, {
            tapToDismiss: true,
            closeButton: true,
            progressBar: true,
            positionClass: 'toast-top-right',
            preventDuplicates: true,
          });
        },
      }
    }
  })();
});
