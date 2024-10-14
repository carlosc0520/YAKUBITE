const executeView = () => {
    const uisApis = {
        ORI: "/Auth/Register/Index?handler",
    }

    // * TABLAS

  
    // * FORMULARIOS
    const loginCrud = {
        init: () => {

        },
        globales: () => {
            configFormVal("formAuthentication", loginCrud.validaciones.autenticar, () => loginCrud.eventos.agregar());
        },
        eventos: {
            agregar: () => {
                let formData = new FormData();
                formData.append("USUARIO", $("#RegisterUser #USUARIO").val());
                formData.append("PASSWORD", $("#RegisterUser #PASSWORD").val());
                
                swalFire.cargando(["Espere un momento", "Estamos validando sus credenciales"]);
                $.ajax({
                    url: uisApis.ORI + '=AutoLogin',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN", "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX")
                    },
                    type: 'POST',
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (data) {
                        console.log(data);
                        // if (data?.codEstado > 0) {
                        //     swalFire.success("Usuario registrado correctamente", "", {
                        //         1: () => {
                        //             $("#RegisterUser")[0].reset();
                        //             window.location.href = "/Auth/Login/Cover";
                        //         }
                        //     });
                        // }

                        // if (data?.codEstado <= 0) swalFire.error(data.mensaje);
                    },
                    error: function (xhr, status, error) {
                        swalFire.error("Ocurrió un error al validar sus credenciales");
                    }
                });
            },
        },
        validaciones: {
            autenticar: {
                "USUARIO": agregarValidaciones({
                    required: true,
                    minlength: 10,
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
            loginCrud.init();
            loginCrud.globales();
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
