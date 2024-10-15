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

const configTable = (domHelp = null, mensaje = '', lengthMenu = [10, 15, 20], all = false) => {
    return {
      order: [[1, 'desc']],
      dom:
        domHelp != null
          ? domHelp
          : '<"mx-0 d-flex flex-wrap flex-column flex-sm-row gap-2 py-4 py-sm-0"' +
            '<"d-flex align-items-center me-auto"l>' +
            '<"dt-action-buttons text-xl-end text-lg-start text-md-end text-start d-flex flex-sm-row align-items-center justify-content-md-end gap-2 ms-n2 ms-md-2 flex-wrap flex-sm-nowrap"fB>' +
            '>t' +
            '<"row mx-4"' +
            '<"col-sm-12 col-md-6"i>' +
            '<"col-sm-12 col-md-6 pb-3 ps-0"p>' +
            '<"d-flex justify-content-center w-100"r>' +
            '>',
      // 10,15,20 y Todos
      lengthMenu: lengthMenu,
      displayLength: all ? -1 : lengthMenu?.[0] || 10,
      processing: true,
      serverSide: true,
      responsive: true,
      language: {
        processing:
          '<div class="sk-wave mx-auto"><div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div></div>',
        emptyTable: 'No hay registros para mostrar',
        searchPlaceholder: 'Buscar..',
        search: '',
        lengthMenu: '_MENU_',
        info: 'Mostrando _START_ a _END_ de _TOTAL_ registros',
        infoEmpty: 'Mostrando 0 a 0 de 0 registros',
        infoFiltered: '(filtrado de _MAX_ registros totales)',
        paginate: {
          previous: '<',
          next: '>',
          first: 'Primero',
          last: 'Último',
          zeroRecords: 'No se encontraron registros'
        }
      },
      responsive: {
        details: {
          display: $.fn.dataTable.Responsive.display.modal({
            header: function (row) {
              var data = row.data();
              var $content = $(data[2]);
              // Extract the value of data-user-name attribute (User Name)
              var userName = $content.find('[class^="user-name-full-"]').text();
              return 'Details of ' + userName;
            }
          }),
          type: 'column',
          renderer: function (api, rowIdx, columns) {
            var data = $.map(columns, function (col, i) {
              // Exclude the last column (Action)
              if (i < columns.length - 1) {
                return col.title !== ''
                  ? '<tr data-dt-row="' +
                      col.rowIndex +
                      '" data-dt-column="' +
                      col.columnIndex +
                      '">' +
                      '<td>' +
                      col.title +
                      ':' +
                      '</td> ' +
                      '<td>' +
                      col.data +
                      '</td>' +
                      '</tr>'
                  : '';
              }
              return '';
            }).join('');
  
            return data ? $('<table class="table"/><tbody />').append(data) : false;
          }
        }
      },
      rowReorder: {
        selector: 'td:nth-child(2)'
      }
    };
  };

const select_estados =
  '<div class="radio-buttons">' +
  '<select class="form-select" id="radioGroup_estado" name="radioGroup_estado">' +
  '<option selected value="">Todos</option>' +
  '<option value="A">Activo</option>' +
  '<option value="I">Inactivo</option>' +
  '</select>' +
  '</div>';

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

const redirect = (isView = false, selector = '', valor) => {
    try {
      const button = document.querySelector(`.nav-tabs button[data-bs-target="#${selector}"]`);
      if (isView && valor) {
        button.removeAttribute('disabled');
        button.click();
      } else {
        button.setAttribute('disabled', 'disabled');
      }
    } catch (error) {}
};


const createModalImage = src => {
    let modal = document.createElement('div');
    modal.classList.add('modal', 'fade');
    modal.setAttribute('tabindex', '-1');
    modal.setAttribute('role', 'dialog');
  
    let modalDialog = document.createElement('div');
    modalDialog.classList.add('modal-dialog', 'modal-dialog-centered');
    modalDialog.classList.add('modal-xl');
    modalDialog.setAttribute('role', 'document');
    let modalContent = document.createElement('div');
    modalContent.classList.add('modal-content');
    let modalBody = document.createElement('div');
    modalBody.classList.add('modal-body', 'text-center');
    let image = document.createElement('img');
    image.src = src;
    image.classList.add('img-fluid');
    modalBody.appendChild(image);
    modalContent.appendChild(modalBody);
    modalDialog.appendChild(modalContent);
    modal.appendChild(modalDialog);
    document.body.appendChild(modal);
    $(modal).modal('show');
    $(modal).on('hidden.bs.modal', function () {
      $(this).remove();
    });
  };


const previewTemplateImage = (tipo = 'Imagen') => `
  <div class="dz-preview dz-file-preview" style="width: 50%; height: 80%;">
    <div class="dz-details">
        <img data-dz-thumbnail class="centered-image" style="width: 100%; height: 100%;">
        <div class="progress">
          <div class="progress-bar progress-bar-primary" role="progressbar" aria-valuemin="0" aria-valuemax="100" data-dz-uploadprogress></div>
        </div>
    </div>
    <div class="dz-remove">
      <button class="btn btn-danger btn-sm" data-dz-remove>Quitar ${tipo}</button>
    </div>
  </div>
`;

const previewTemplate = tipo => `
  <div class="dz-preview dz-file-preview">
    <div class="dz-details">
      <div class="dz-thumbnail">
        <img data-dz-thumbnail class="centered-image" style="max-width: 200px; max-height: 200px;">
        <span class="dz-nopreview">No preview</span>
        <div class="dz-success-mark"></div>
        <div class="dz-error-mark"></div>
        <div class="dz-error-message"><span data-dz-errormessage></span></div>
        <div class="progress">
          <div class="progress-bar progress-bar-primary" role="progressbar" aria-valuemin="0" aria-valuemax="100" data-dz-uploadprogress></div>
        </div>
      </div>
      <div class="dz-filename" data-dz-name></div>
      <div class="dz-size" data-dz-size></div>
    </div>
    <div class="dz-remove">
      <button class="btn btn-danger btn-sm" data-dz-remove>Quitar ${tipo}</button>
    </div>
  </div>
`;


const func = {
    getURLParameter: name => {
      return decodeURI((RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]);
    },
    formatFecha: (fecha, config) => (!fecha ? '' : moment(fecha).format(config)),
    actualizarForm: (form, rowData) => {
      const fechasKeys = ['FEDCN', 'FCRCN', 'FESTDO'];
      Object.keys(rowData).forEach(key => {
        const uppercaseKey = key.toUpperCase();
        if (fechasKeys.includes(uppercaseKey)) {
          if (uppercaseKey === 'FESTDO')
            $(`#${form} [name="${uppercaseKey}"]`).val(func.formatFecha(new Date(), 'DD-MM-YYYY HH:mm a').toString());
          if (uppercaseKey === 'FEDCN')
            $(`#${form} [name="${uppercaseKey}"]`).val(func.formatFecha(rowData[key], 'DD-MM-YYYY HH:mm a').toString());
          if (uppercaseKey === 'FCRCN')
            $(`#${form} [name="${uppercaseKey}"]`).val(func.formatFecha(rowData[key], 'DD-MM-YYYY HH:mm a').toString());
          return;
        } else {
          const value = rowData[key];
          const typeInput = $(`#${form} [name="${uppercaseKey}"]`).attr('type');
          if (uppercaseKey !== 'UEDCN') {
            if (typeInput === 'checkbox') $(`#${form} [name="${uppercaseKey}"]`).prop('checked', value);
            else if (typeInput === 'radio') {
              $(`#${form} [name="${uppercaseKey}"][value="${value}"]`).prop('checked', true);
            } else
              $(`#${form} [name="${uppercaseKey}"]`).val(['undefined', undefined, null].includes(value) ? '' : value);
  
            try {
              if ($(`#${form} [name="${uppercaseKey}"]`).hasClass('dob-picker')) {
                $(`#${form} [name="${uppercaseKey}"]`).flatpickr({
                  value: value
                });
              }
            } catch (error) {}
          } else {
            $(`#${form} [name="${uppercaseKey}"]`).val(['undefined', undefined, null].includes(value) ? '' : value);
          }
          if ($(`#${form} [name="${uppercaseKey}"]`).hasClass('select2')) {
            $(`#${form} [name="${uppercaseKey}"]`).val(value).trigger('change');
          }
        }
  
        // actualziar select2
      });
    },
    generateQRCode: async (url, width = 128, margin = 1) => {
      return await new Promise((resolve, reject) => {
        QRCode.toDataURL(url, { width, margin }, function (err, url) {
          if (err) reject(err);
          resolve(url);
        });
      });
    },
    generateChart: async (data, index) => {
      const MESES = ['ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN', 'JUL', 'AGO', 'SEP', 'OCT', 'NOV', 'DIC'];
  
      const formatLabel = period => {
        const monthIndex = parseInt(period.split('-')[1], 10) - 1;
        return MESES[monthIndex] || period;
      };
  
      const labels = data.map(item => formatLabel(item.PRDO));
      const values = data.map(item => item.CNSMO);
  
      const container = document.createElement('div');
      container.style.position = 'absolute'; // Posición absoluta para que no afecte el layout
      container.style.top = '-10000px'; // Fuera de la vista
      container.style.left = '-10000px'; // Fuera de la vista
      container.style.border = '2px solid black';
      container.style.padding = '10px';
      container.style.margin = '10px';
      container.style.width = '800px'; // Ancho en píxeles
      container.style.height = '400px'; // Alto en píxeles
      container.style.overflow = 'hidden'; // Evita que el canvas se desborde
  
      const canvas = document.createElement('canvas');
      canvas.id = `chart-${index}`;
      canvas.width = 800; // Ancho en píxeles
      canvas.height = 400; // Alto en píxeles
      container.appendChild(canvas);
  
      document.body.appendChild(container); // Añadir al body para evitar conflictos de CSS
  
      const baseColor = 'gray';
      const lastBarColor = 'darkgray';
      const backgroundColors = values.map((value, i) => (i === values.length - 1 ? baseColor : lastBarColor));
  
      const maxValue = Math.max(...values);
  
      const chart = new Chart(canvas, {
        type: 'bar',
        data: {
          labels: labels,
          datasets: [
            {
              label: 'CNSMO',
              data: values,
              backgroundColor: backgroundColors,
              borderColor: 'black',
              borderWidth: 1
            }
          ]
        },
        options: {
          responsive: false, // Desactiva la responsividad
          plugins: {
            legend: {
              display: false
            },
            title: {
              display: true,
              text: 'ULT. CARGOS',
              color: 'black',
              font: {
                size: 24,
                weight: 'bold'
              },
              padding: {
                top: 10,
                bottom: 10
              }
            }
          },
          scales: {
            x: {
              beginAtZero: true,
              ticks: {
                color: 'black',
                font: {
                  size: 24
                }
              }
            },
            y: {
              beginAtZero: true,
              min: 0,
              max: maxValue,
              ticks: {
                color: 'black',
                font: {
                  size: 24
                },
                callback: function (value) {
                  return `S/.${value.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}`;
                }
              }
            }
          }
        }
      });
  
      await new Promise(resolve => setTimeout(resolve, 1000));
  
      return new Promise(resolve => {
        html2canvas(canvas).then(canvas => {
          const img = canvas.toDataURL('image/png');
          // Limpiar el contenedor después de la conversión
          document.body.removeChild(container);
          resolve(img);
        });
      });
    },
    sumarMeses: meses => {
      let fecha = new Date();
      fecha.setMonth(fecha.getMonth() + meses);
      return moment(fecha).format('YYYY-MM-DD');
    },
    resetAll: (form, all = true) => {
      const fieldsToReset = all
        ? 'input:not(.no-reset), textarea:not(.no-reset), select:not(.no-reset), checkbox:not(.no-reset)'
        : 'input, textarea, select, checkbox';
      $(`${form}`).find(fieldsToReset).val('');
    },
    obtenerCESTDO: (tableName, all = false) => {
      const existe = $(`#${tableName}_filter .radio-buttons #radioGroup_estado`);
      if (existe.length == 0) {
        return all ? '' : '';
      }
  
      return existe.val() || null;
    },
    titleTable: (referencia, array = []) => {
      $(referencia).html('');
      let html = '';
      array.forEach(element => {
        let split = element.split(':') || [];
        if (split.length == 2) {
          html += `<p class="text-lg mb-1"><span class="fw-bold">${split[0]}:</span> ${split[1]}</p>`;
        }
      });
      $(referencia).html(html);
    },
    p: (NOBJTO = null) => {
      let decrypted = CryptoJS.AES.decrypt($('#TK_PRMSS_USER').val(), window.location.pathname).toString(
        CryptoJS.enc.Utf8
      );
  
      try {
        let data = JSON.parse(decrypted);
        return NOBJTO ? data.find(x => x.NOBJTO === NOBJTO && x.ESTDO == 1) : data;
      } catch (error) {
        return [];
      }
    },
    condominio: () => {
      try {
        let condominioStore = localStorage.getItem('CNDMNIOGBL');
        condominioStore = JSON.parse(condominioStore);
        return condominioStore?.ID || null;
      } catch (error) {
        return null;
      }
    },
    selects2: () => {
      const select2 = $('.select2');
      if (select2.length) {
        select2.each(function () {
          var $this = $(this);
          $this.wrap('<div class="position-relative"></div>').select2({
            placeholder: 'Seleccione',
            dropdownParent: $this.parent()
          });
        });
      }
    },
    limitarCaracteres: () => {
      // LONGITUD CARACTERES
      let inputs = $('input[type="text"].max-length, textarea.max-length');
      inputs.each(function () {
        let max = parseInt($(this).data('maxlength'));
        $(this).on('input', function () {
          let value = $(this).val();
          if (value.length > max) {
            $(this).val(value.substring(0, max));
          }
        });
      });
  
      let sinEspacios = $('input[type="text"].sin-espacios, textarea.sin-espacios');
      sinEspacios.each(function () {
        $(this).on('input', function () {
          let value = $(this).val();
          if (value.includes(' ')) {
            $(this).val(value.replace(/\s/g, ''));
          }
        });
      });
  
      // LONGITUD DECIMALES
      let soloDecimles = $('input[type="text"].solo-decimales, textarea.solo-decimales');
      soloDecimles.each(function () {
        let max = parseInt($(this).data('maxlength'));
        $(this).on('input', function () {
          let value = $(this).val();
          if (value.includes('.')) {
            let decimal = value.split('.')[1];
            if (decimal.length > max) {
              $(this).val(value.substring(0, value.length - 1));
            }
          }
        });
      });
    },
    llenarcombos: async (url, referencia = [], removeAll = []) => {
      try {
        const response = await fetch(url);
        const data = await response.json();
        referencia.forEach(selectRef => {
          const select = $(selectRef);
          select.empty();
          select.append($('<option>').text('- Seleccione -').val(''));
          data.forEach(item => {
            if (removeAll.includes(item.dscrpcn)) return;
  
            const option = $('<option>').text(item.dscrpcn).val(item.value);
            select.append(option);
          });
        });
      } catch (error) {
        swalFire.error('Ocurrió un error al cargar los datos');
      }
    },
    actualizarAuditoria: (form, rowData) => {
      // selecconar input y select
      let $inputs = $(`#${form} #auditoria-collapse input, #${form} #auditoria-collapse select`);
      let idRowData = ![undefined, null, ''].includes(rowData?.id);
      let localUsuario = localStorage.getItem('usuario');
      if (localUsuario !== null) {
        localUsuario = JSON.parse(localUsuario);
      } else {
        localUsuario = null;
      }
  
      $inputs.each(function () {
        let name = $(this).attr('name');
        switch (name) {
          case 'CESTDO':
            $(this).val(idRowData ? rowData[name.toLowerCase()] : 'V');
            break;
          case 'UCRCN':
            $(this).val(idRowData ? rowData[name.toLowerCase()] : localUsuario?.['user']);
            break;
          case 'UEDCN':
            $(this).val(localUsuario?.['user']);
            break;
          case 'FCRCN':
            $(this).val(
              idRowData
                ? func.formatFecha(rowData[name.toLowerCase()], 'DD-MM-YYYY HH:mm:ss a')
                : func.formatFecha(new Date(), 'DD-MM-YYYY HH:mm:ss a')
            );
            break;
          case 'FEDCN':
            $(this).val(func.formatFecha(new Date(), 'DD-MM-YYYY HH:mm:ss a'));
            break;
          case 'FESTDO':
            $(this).val(func.formatFecha(new Date(), 'DD-MM-YYYY HH:mm:ss a'));
            break;
          default:
            break;
        }
      });
    },
    downloadFiles: files => {
      files.forEach(file => {
        $(`${file}`).on('click', function () {
          new Promise((resolve, reject) => {
            swalFire.cargando();
            resolve();
          })
            .then(() => {
              swalFire.success('Descarga exitosa', 'El archivo se ha descargado correctamente');
            })
            .catch(() => {
              swalFire.error('Ocurrió un error al descargar el archivo');
            });
        });
      });
    },
    formatMonto: (monto, moneda = 'PEN') => {
      return new Intl.NumberFormat('es-PE', {
        style: 'currency',
        currency: moneda
      }).format(monto);
    },
    datepickerListModify: () => {
      const datepickerListModify = document.querySelectorAll('.dob-picker-format');
      datepickerListModify.forEach(function (datepicker) {
        datepicker.placeholder = 'DD-MM-YYYY';
        if (datepicker._flatpickr) {
          datepicker._flatpickr.destroy();
        }
  
        flatpickr(datepicker, {
          altFormat: 'd-m-Y',
          dateFormat: 'd-m-Y',
          altInput: true,
          allowInput: true,
          disableMobile: true,
          locale: {
            firstDayOfWeek: 1,
            weekdays: {
              shorthand: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
              longhand: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado']
            },
            months: {
              shorthand: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
              longhand: [
                'Enero',
                'Febrero',
                'Marzo',
                'Abril',
                'Mayo',
                'Junio',
                'Julio',
                'Agosto',
                'Septiembre',
                'Octubre',
                'Noviembre',
                'Diciembre'
              ]
            }
          },
          onClose: function (selectedDates, dateStr, instance) {
            if (dateStr === '') {
              instance.setDate(null);
            }
          }
        });
      });
    },
    formatoSolesPEN: monto => {
      if (isNaN(monto)) {
        monto = 0;
      }
  
      try {
        return new Intl.NumberFormat('es-PE', {
          style: 'currency',
          currency: 'PEN'
        }).format(monto);
      } catch (error) {
        return monto;
      }
    }
  };

  
  function agregarArchivoADropzone(rutaArchivo, dropzoneInstance) {
    if (!rutaArchivo || !dropzoneInstance) {
      console.error('Se requiere una ruta válida y una instancia de Dropzone.');
      return;
    }
  
    let filename = 'archivo_' + new Date().getTime() + '.jpg';
    let fileOfBlob = new File([], filename, { type: 'image/jpeg' });
    dropzoneInstance.files.push(fileOfBlob);
    dropzoneInstance.emit('addedfile', fileOfBlob);
    dropzoneInstance.emit('thumbnail', fileOfBlob, '');
    dropzoneInstance.emit('complete', fileOfBlob);
  
    setTimeout(() => {
      dropzoneInstance.emit('thumbnail', fileOfBlob, rutaArchivo);
      fileOfBlob.dataURL = rutaArchivo;
      fileOfBlob.isExist = true;
    }, 500);
  }