$(document).ajaxSend(function (event, xhr, settings) {
xhr.setRequestHeader('Authorization', 'Bearer ' + (localStorage.getItem('accessToken') || null));
});


const swalFire = {
cargando: (mensaje = [], isClose = true) => {
    Swal.fire({
    title: "Cargando...",
    html: `
    ${mensaje.length > 1 ? mjsArraySwal(mensaje) : '<p class="text-lg">Espere un momento por favor.</p>'}
    <div class="spinner-container">
        <div class="spinner-border text-primary" role="status">
        <span class="sr-only"></span>
        </div>
    </div>`
    ,
    buttonsStyling: false,
    showConfirmButton: false,
    allowOutsideClick: isClose,
    });
},
success: (title, mensaje = "", eventos = {}, textmsj = "") => {
    Swal.fire({
    title: title,
    text: mensaje,
    icon: 'success',
    confirmButtonText: 'Ok',
    confirmButton: false,
    customClass: {
        confirmButton: 'btn btn-success'
    }
    })
    .then((result) => {
        for (const key in eventos) {
        if (Object.hasOwnProperty.call(eventos, key)) {
            eventos[key]();
        }
        }
    })
},
error: (mensaje = "", eventos = {}) => {
    Swal.fire({
    icon: 'error',
    title: mensaje,
    showConfirmButton: true,
    }).then((result) => {
    for (const key in eventos) {
        if (Object.hasOwnProperty.call(eventos, key)) {
        eventos[key]();
        }
    }
    })
},
warning: (mensaje = "") => {
    Swal.fire({
    icon: 'warning',
    title: mensaje,
    showConfirmButton: true,
    });
},
cerrar: () => Swal.close(),
cancelar: (mensaje) => {
    Swal.fire({
    title: 'Cancelado',
    html: `<p>¡No se ha eliminado!</p>`,
    icon: 'error',
    confirmButtonText: 'Ok',
    customClass: {
        confirmButton: 'btn btn-success'
    }
    });
},
delete: (mensaje, eventos) => {
    Swal.fire({
    title: "",
    html: `<p class="">${mensaje}<br></p>`,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Eliminar',
    cancelButtonText: 'Cancelar',
    customClass: {
        confirmButton: 'btn btn-primary',
        cancelButton: 'btn btn-secondary'
    }
    }).then(result => {
    if (result.isConfirmed) {
        for (const key in eventos) {
        if (Object.hasOwnProperty.call(eventos, key)) {
            eventos[key]();
        }
        }
    } else {
        swalFire.cancelar()
    }
    });
},
confirmar: (mensaje, eventos = {}) => {
    Swal.fire({
    title: "",
    html: `<p class="">${mensaje}<br></p>`,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Aceptar',
    cancelButtonText: 'Cancelar',
    customClass: {
        confirmButton: 'btn btn-primary',
        cancelButton: 'btn btn-secondary'
    }
    }).then(result => {
    if (result.isConfirmed) {
        for (const key in eventos) {
        if (Object.hasOwnProperty.call(eventos, key)) {
            eventos[key]();
        }
        }
    }
    });
}
}

const plugins = {
    trigger: (typeof FormValidation != "undefined") ? new FormValidation.plugins.Trigger() : null,
    bootstrap5: (typeof FormValidation != "undefined") ? new FormValidation.plugins.Bootstrap5({
      eleValidClass: '',
      rowSelector: '.col-12'
    }) : null,
    submitButton: (typeof FormValidation != "undefined") ? new FormValidation.plugins.SubmitButton() : null,
    autoFocus: (typeof FormValidation != "undefined") ? new FormValidation.plugins.AutoFocus() : null,
  };
  

const configFormVal = (form, campos, thisEvent = () => { }) => {
    try {
        FormValidation
        .formValidation(document.getElementById(form))
        .destroy();

        $(`#${form}`).trigger('reset');
        Object.keys(campos).forEach(key => {
        $(`#${form}`).find(`[name=${key}]`).removeClass('is-invalid');
        $(`#${form}`).find(`[data-field=${key}]`).remove();
        })

        FormValidation.formValidation(document.getElementById(form), {
        fields: campos,
        plugins: plugins
        }).on('core.form.valid', function (e) {
        thisEvent()
        }).on('core.form.invalid', function () {
        });
    } catch (error) {
    }
}

const agregarValidaciones = (valid = {}) => {
    let validators = {};
  
    if (valid?.required) {
      validators = { ...validators, notEmpty: { message: 'El campo es requerido' } }
    }
  
    if (valid?.minlength) {
      validators = { ...validators, stringLength: { min: valid.minlength, message: `El campo debe tener al menos ${valid.minlength} caracteres` } }
    }
  
    if (valid?.maxlength) {
      if (validators.stringLength) {
        validators.stringLength.max = valid.maxlength;
      } else {
        validators = { ...validators, stringLength: { max: valid.maxlength, message: `El campo no puede tener más de ${valid.maxlength} caracteres` } }
      }
    }
    
    if (valid?.regexp) {
      validators = { ...validators, regexp: { regexp: valid.regexp, message: valid?.message || `El campo no cumple con el formato requerido` } }
    }
  
    if (valid?.email) {
      validators = { ...validators, emailAddress: { message: `El campo no es un correo válido` } }
    }
  
    if (valid?.number) {
      validators = { ...validators, numeric: { message: `El campo debe ser un número` } }
    }
  
    if (valid?.numberMin) {
      validators = { ...validators, greaterThan: { value: valid.numberMin, message: `El campo debe ser mayor a ${valid.numberMin}` } }
    }
  
    return { validators };
};

const mjsArraySwal = (array) => {
let cant = array.length;
var html = `<div class='slide-vertical m-0 p-0 elements-${cant}'><ul>`;
for (var i = 0; i < array.length; i++) {
    // tamaño 1.6rem
    html += "<li class='text-lg'>" + array[i] + "</li>";
}
html += "</ul></div>";
return html;
}