const executeView = () => {
    const uisApis = {
        ORI: "/Auth/Register/Index?handler",
    }

    // * TABLAS

  
    // * FORMULARIOS
    const registerCrud = {
        init: () => {

        },
        globales: () => {
            configFormVal("RegisterUser", registerCrud.validaciones.autenticar, () => registerCrud.eventos.agregar());
        },
        eventos: {
            agregar: () => {
                if(!$("#RegisterUser #TERMS").is(":checked")) {
                    swalFire.warning("Debe aceptar los términos y condiciones para poder registrarse");
                    return;
                }

                let formData = new FormData();
                formData.append("NOMBRES", "");
                formData.append("APELLIDOS", "");
                formData.append("USUARIO", $("#RegisterUser #USUARIO").val());
                formData.append("CORREO", $("#RegisterUser #CORREO").val());
                formData.append("ROL", "2");
                formData.append("TELEFONO", $("#RegisterUser #TELEFONO").val());
                formData.append("PASSWORD", $("#RegisterUser #PASSWORD").val());
                
                swalFire.cargando(["Espere un momento", "Estamos registrando su usuario"]);
                $.ajax({
                    url: uisApis.ORI + '=Register',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN", "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX")
                    },
                    type: 'POST',
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (data) {
                        if (data?.codEstado > 0) {
                            swalFire.success("Usuario registrado correctamente", "", {
                                1: () => {
                                    $("#RegisterUser")[0].reset();
                                    window.location.href = "/Auth/Login";
                                }
                            });
                        }

                        if (data?.codEstado <= 0) swalFire.error(data.mensaje);
                    },
                    error: function (xhr, status, error) {
                        swalFire.error("Ocurrió un error al querer registrar su usuario");
                    }
                });
            },
        },
        validaciones: {
            autenticar: {
                "CORREO": agregarValidaciones({
                    required: true,
                }),
                "TELEFONO": agregarValidaciones({
                    required: true,
                }),
                "USUARIO": agregarValidaciones({
                    required: true,
                    minlength: 5,
                }),
                "PASSWORD": agregarValidaciones({
                    required: true,
                    minlength: 8,
                }),
            },
        },
        variables: {
    
        },
    }

    return {
        init: async (params = null) => {
            registerCrud.init();
            registerCrud.globales();
        }
    }
}

const initGlobal = executeView();
const preloader = document.getElementById('preloader');

if(!preloader) {
    initGlobal.init();
}else{
    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            if (mutation.attributeName === 'style' && preloader.style.display === 'none') {
                initGlobal.init();
            } else {
                initGlobal.init(true);
            }
        });
    });
    
    observer.observe(preloader, { attributes: true });
}
