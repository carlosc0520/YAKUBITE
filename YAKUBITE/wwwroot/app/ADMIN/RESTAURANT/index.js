/**
 * PERSONAL CRUD JS
 */

'use strict';

const executeView = () => {
  const uisApis = {
    API: '/Admin/Restaurant/Index?handler',
    GD: '/Auth/Login/Index?handler'
  };

  // * VARIABLES
  let restaurantTable = 'restaurantTable';
  let menuTable = 'menuTable';
  let CrestaurantTable = null;
  let CmenuTable = null;

  // * files
  let myDropzoneAddRestaurant = null;
  let myDropzoneEditRestaurant = null;

  // * TABLAS
  const restaurantCrud = {
    init: () => {
      restaurantCrud.eventos.TABLE();
    },
    globales: () => {
        let dropzoneBasic = $('#AddRestaurant #dropzone-area');
        if (dropzoneBasic) {
            myDropzoneAddRestaurant = new Dropzone(dropzoneBasic[0], {
              previewTemplate: previewTemplate('imagen'),
              parallelUploads: 1,
              maxFilesize: 5,
              maxFiles: 1,
              acceptedFiles: 'image/*',
              init: function () {
                this.on('addedfile', function (file) {
                  if (this.files.length > 1) {
                    this.removeFile(this.files[0]);
                  }
                });
              }
            });
          }

        let dropzoneBasicEdit = $('#EditRestaurant #dropzone-area');
        if (dropzoneBasicEdit) {
        myDropzoneEditRestaurant= new Dropzone(dropzoneBasicEdit[0], {
            previewTemplate: previewTemplateImage('imagen'),
            createImageThumbnails: false,
            parallelUploads: 1,
            maxFilesize: 5,
            maxFiles: 1,
            acceptedFiles: 'image/*',
            init: function () {
            this.on('addedfile', async function (file) {
                if (this.files.length > 1) {
                this.removeFile(this.files[0]);
                }

                dropzoneBasicEdit.find('.centered-image').off('click');
                dropzoneBasicEdit.find('.dz-preview').css('cursor', 'pointer');

                let filePreview = this.files[0];
                if (filePreview.isExist) {
                let img = dropzoneBasicEdit.find('.dz-preview').find('.dz-details').find('img');
                img.attr('src', filePreview.dataURL);
                return;
                }

                let reader = new FileReader();
                reader.readAsDataURL(filePreview);
                reader.onload = function () {
                let img = dropzoneBasicEdit.find('.dz-preview').find('.dz-details').find('img');
                img.attr('src', reader.result);
                file.dataURL = reader.result;
                img.on('click', function () {
                    createModalImage($(this).attr('src'));
                });
                };
            });
            }
        });
        }
    
      // * MODALES
      $('#modalAddRestaurant').on('show.bs.modal', function (e) {
        configFormVal('AddRestaurant', restaurantCrud.validaciones.INSERT, () => restaurantCrud.eventos.INSERT());
      });

      $('#modalEditRestaurant').on('show.bs.modal', function (e) {
        configFormVal('EditRestaurant', restaurantCrud.validaciones.UPDATE, () => restaurantCrud.eventos.UPDATE());
        func.actualizarForm('EditRestaurant', restaurantCrud.variables.rowEdit);
        agregarArchivoADropzone(restaurantCrud.variables.rowEdit?.ruta, myDropzoneEditRestaurant);
      });

    //   // * FORMULARIOS
      $(`#${restaurantTable}`).on('click', '.edit-row-button', function () {
        const data = CrestaurantTable.row($(this).parents('tr')).data();
        if (!data.id) return swalFire.error('No se encontró el restaurant seleccionado');
        restaurantCrud.variables.rowEdit = data;
        $('#modalEditRestaurant').modal('show');
      });

      $(`#${restaurantTable}`).on('click', '.delete-row-button', function () {
        const data = CrestaurantTable.row($(this).parents('tr')).data();
        if (!data.id) return swalFire.error('No se encontró el restaurant seleccionado');
        swalFire.confirmar('¿Está seguro de eliminar el Restaurant?', {
          1: () => restaurantCrud.eventos.DELETE(data.id)
        });
      });

    //   $(`#${restaurantTable}`).on('click', '.view-grupodato-button', function () {
    //     const data = CrestaurantTable.row($(this).parents('tr')).data();
    //     if (!data.id) return swalFire.error('No se encontró el grupo dato seleccionado');
    //     restaurantCrud.variables.rolEdit = data;
    //     redirect(true, 'navs-detalle-gd', data.id);
    //   });
    },
    variables: {
        rowEdit: {}
    },
    eventos: {
      TABLE: () => {
        if (!CrestaurantTable) {
          CrestaurantTable = $(`#${restaurantTable}`).DataTable({
            ...configTable(),
            ajax: {
              url: uisApis.API + '=Buscar',
              type: 'GET',
              beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + (localStorage.getItem('accessToken') || null));
              },
              data: function (d) {
                delete d.columns;
                d.ESTADO = func.obtenerCESTDO(restaurantTable);
              }
            },
            columns: [
              { data: 'rn', title: '' },
              { data: 'ruc', title: 'RUC' },
              { data: 'alias', title: 'Abrev.' },
              {
                data: null,
                title: 'Estado',
                className: 'text-center',
                render: data => {
                  return `<span><i class="fa fa-circle ${data.estado == 'A' ? 'text-success' : 'text-danger'}" title=${
                    data.estado == 'A' ? 'Activo' : 'Inactivo'
                  }></i></span>`;
                }
              },
              { data: 'usuarioe', title: 'U. Edición' },
              { data: null, title: 'F. Edición', render: data => func.formatFecha(data.fedicion, 'DD-MM-YYYY HH:mm a') },
              {
                data: null,
                title: '',
                className: 'text-center',
                render: data => {
                  return `<div class="d-flex justify-content-center m-0 p-0">
                        <button name="EDITAR" class="btn btn-sm btn-icon edit-row-button" title="Editar"><i class="bx bx-edit"></i></button>
                        <button name="ELIMINAR" class="btn btn-sm btn-icon delete-row-button" title="Eliminar"><i class="bx bx-trash"></i></button>
                        <button name="VER" class="btn btn-sm btn-icon view-row-button" title="Ver"><i class="bx bx-show"></i></button>
                     </div>`;
                }
              }
            ],
            initComplete: function (settings, json) {
              if ($(`#${restaurantTable}`).find('.radio-buttons').length == 0) {
                $(`#${restaurantTable}_filter`).append(select_estados);

                $(`#${restaurantTable}_filter .radio-buttons #radioGroup_estado`).on('change', function () {
                  $(`#${restaurantTable}`).DataTable().ajax.reload();
                });
              }
            },
            columnDefs: [],
            buttons: (() => {
                let buttons = [];

              buttons.unshift({
                text: '<i class="bx bx-plus me-0 me-md-2"></i><span class="d-none d-md-inline-block">Agregar</span>',
                className: 'btn btn-label-primary btn-add-new',
                action: function (e, dt, node, config) {

                    $('#modalAddRestaurant').modal('show');
                }
              });

              return buttons;
            })()
          });
        } else {
          CrestaurantTable.ajax.reload();
        }
      },
      INSERT: () => {

        let file = myDropzoneAddRestaurant.files[0];
        if (!file) return swalFire.error('Debe seleccionar una imagen');

        let formData = new FormData();
        formData.append('RUC', $('#AddRestaurant #RUC').val());
        formData.append('DESCRIPCION', $('#AddRestaurant #DESCRIPCION').val());
        formData.append('ALIAS', $('#AddRestaurant #ALIAS').val());
        formData.append('DIRECCION', $('#AddRestaurant #DIRECCION').val());
        formData.append('CATEGORIAGD', $('#AddRestaurant #CATEGORIAGD').val());
        formData.append('FILE', file);
        formData.append('ESTADO', $('#AddRestaurant #ESTADO').val());

        swalFire.cargando(['Espere un momento', 'Estamos registrando el grupo dato']);
        $.ajax({
          url: uisApis.API + '=Add',
          beforeSend: function (xhr) {
            xhr.setRequestHeader('XSRF-TOKEN', localStorage.getItem('accessToken'));
          },
          type: 'POST',
          dataType: 'json',
          contentType: false,
          processData: false,
          data: formData,
          success: function (data) {
            if (data?.codEstado > 0) {
              swalFire.success('Restaurant registrado correctamente', '', {
                1: () => {
                  $('#modalAddRestaurant').modal('hide');
                  CrestaurantTable.ajax.reload();
                }
              });
            }

            if (data?.codEstado <= 0) swalFire.error(data.mensaje);
          },
          error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al agregar el restaurant')
        });
      },
      UPDATE: () => {

        let file = myDropzoneEditRestaurant.files[0];
        if (!file) return swalFire.error('Debe seleccionar una imagen');

        let formData = new FormData();
        formData.append('ID', restaurantCrud.variables.rowEdit.id);
        formData.append('RUC', $('#EditRestaurant #RUC').val());
        formData.append('DESCRIPCION', $('#EditRestaurant #DESCRIPCION').val());
        formData.append('ALIAS', $('#EditRestaurant #ALIAS').val());
        formData.append('DIRECCION', $('#EditRestaurant #DIRECCION').val());
        formData.append('CATEGORIAGD', $('#EditRestaurant #CATEGORIAGD').val());
        formData.append('FILE', file?.isExist ? null : file);
        formData.append('RUTA', restaurantCrud.variables.rowEdit.ruta);
        formData.append('ESTADO', $('#EditRestaurant #ESTADO').val());

        swalFire.cargando(['Espere un momento', 'Estamos actualizando el Restaurant']);
        $.ajax({
          url: uisApis.API + '=Update',
          beforeSend: function (xhr) {
            xhr.setRequestHeader('XSRF-TOKEN', localStorage.getItem('accessToken'));
          },
          type: 'POST',
          dataType: 'json',
          contentType: false,
          processData: false,
          data: formData,
          success: function (data) {
            if (data?.codEstado > 0) {
              swalFire.success('Restaurant actualizado correctamente', '', {
                1: () => {
                  $('#modalEditRestaurant').modal('hide');
                  CrestaurantTable.ajax.reload();
                }
              });
            }

            if (data?.codEstado <= 0) swalFire.error(data.mensaje);
          },
          error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al actualizar el restaurant')
        });
      },
   
    DELETE: id => {
        let formData = new FormData();
        formData.append('ID', id);

        swalFire.cargando(['Espere un momento', 'Estamos eliminando el restaurant']);
        $.ajax({
          url: uisApis.API + '=Delete',
          beforeSend: function (xhr) {
            xhr.setRequestHeader('XSRF-TOKEN', localStorage.getItem('accessToken'));
          },
          type: 'POST',
          dataType: 'json',
          contentType: false,
          processData: false,
          data: formData,
          success: function (data) {
            if (data?.codEstado > 0) {
              swalFire.success('Restaurant eliminado correctamente', '', {
                1: () => $(`#${restaurantTable}`).DataTable().ajax.reload()
              });
            }

            if (data?.codEstado <= 0) swalFire.error(data.mensaje);
          },
          error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al eliminar el restaurant')
        });
      }
    },
    formularios: {},
    validaciones: {
      INSERT: {
        RUC: agregarValidaciones({
          required: true,
          regexp: /^[0-9]{11}$/,
          
        }),
        DESCRIPCION: agregarValidaciones({
          required: true
        }),
        ALIAS: agregarValidaciones({
          required: true
        }),
        DIRECCION: agregarValidaciones({
          required: true
        }),
        CATEGORIAGD: agregarValidaciones({
            required: true
        }),
      },
      UPDATE: {
        RUC: agregarValidaciones({
            required: true,
            regexp: /^[0-9]{11}$/,
            
          }),
          DESCRIPCION: agregarValidaciones({
            required: true
          }),
          ALIAS: agregarValidaciones({
            required: true
          }),
          DIRECCION: agregarValidaciones({
            required: true
          }),
          CATEGORIAGD: agregarValidaciones({
              required: true
          }),
      }
    }
  };

//   const detallerestaurantCrud = {
//     init: () => {
//       detallerestaurantCrud.eventos.TABLEGRUPODATODETALLE();
//     },
//     globales: () => {
//       // * MODALES
//       $('#modalAddDetGrupoDato').on('show.bs.modal', function (e) {
//         configFormVal('AddDetGrupoDato', detallerestaurantCrud.validaciones.INSERT, () =>
//           detallerestaurantCrud.eventos.INSERT()
//         );
//         $('#AddDetGrupoDato #GDPDRE').val(restaurantCrud.variables.rolEdit.dtlle);
//       });

//       $('#modalEditDetGrupoDato').on('show.bs.modal', function (e) {
//         configFormVal('EditDetGrupoDato', detallerestaurantCrud.validaciones.UPDATE, () =>
//           detallerestaurantCrud.eventos.UPDATE()
//         );
//         func.actualizarForm('EditDetGrupoDato', detallerestaurantCrud.variables.rolEdit);
//       });

//       // * FORMULARIOS
//       $(`#${menuTable}`).on('click', '.edit-grupodato-button', function () {
//         const data = CmenuTable.row($(this).parents('tr')).data();
//         if (!data.id) return swalFire.error('No se encontró el detalle del grupo dato seleccionado');
//         detallerestaurantCrud.variables.rolEdit = data;
//         $('#modalEditDetGrupoDato').modal('show');
//       });

//       $(`#${menuTable}`).on('click', '.delete-grupodato-button', function () {
//         const data = CmenuTable.row($(this).parents('tr')).data();
//         if (!data.id) return swalFire.error('No se encontró el detalle del grupo dato seleccionado');
//         swalFire.confirmar('¿Está seguro de eliminar el detalle del grupo dato?', {
//           1: () => detallerestaurantCrud.eventos.DELETE(data.id)
//         });
//       });
//     },
//     variables: {
//       rolEdit: {}
//     },
//     eventos: {
//       TABLEGRUPODATODETALLE: () => {
//         $(`#${menuTable}_filter .radio-buttons #radioGroup_2`).prop('checked', true);
//         $(`#${menuTable}_title`).text('GRUPO DATO: ' + restaurantCrud.variables.rolEdit?.dgdtlle || '');

//         if (!CmenuTable) {
//           CmenuTable = $(`#${menuTable}`).DataTable({
//             ...configTable(),
//             ajax: {
//               url: uisApis.API + '=BuscarDetalle',
//               type: 'GET',
//               beforeSend: function (xhr) {
//                 xhr.setRequestHeader('Authorization', 'Bearer ' + (localStorage.getItem('accessToken') || null));
//               },
//               data: function (d) {
//                 delete d.columns;
//                 d.GDPDRE = restaurantCrud.variables.rolEdit.dtlle;
//                 d.CESTDO = func.obtenerCESTDO(menuTable);
//               }
//             },
//             columns: [
//               { data: 'rn', title: '' },
//               { data: 'dtlle', title: 'Descripción' },
//               { data: null, title: 'Valor 1', className: 'text-center', render: data => data?.vlR1 || '' },
//               { data: null, title: 'Valor 2', className: 'text-center', render: data => data?.vlR2 || '' },
//               { data: null, title: 'Valor 3', className: 'text-center', render: data => data?.vlR3 || '' },
//               {
//                 data: null,
//                 title: 'Estado',
//                 className: 'text-center',
//                 render: data => {
//                   return `<span><i class="fa fa-circle ${data.cestdo == 'A' ? 'text-success' : 'text-danger'}" title=${
//                     data.cestdo == 'A' ? 'Activo' : 'Inactivo'
//                   }></i></span>`;
//                 }
//               },
//               { data: 'uedcn', title: 'U. Edición' },
//               { data: null, title: 'F. Edición', render: data => func.formatFecha(data.fedcn, 'DD-MM-YYYY HH:mm a') },
//               {
//                 data: null,
//                 title: '',
//                 className: 'text-center',
//                 render: data => {
//                   return `<div class="d-flex justify-content-center m-0 p-0">
//                           <button name="EDITAR" class="btn btn-sm btn-icon edit-grupodato-button" title="Editar"><i class="bx bx-edit"></i></button>
//                           <button name="ELIMINAR" class="btn btn-sm btn-icon delete-grupodato-button" title="Eliminar"><i class="bx bx-trash"></i></button>
//                        </div>`;
//                 }
//               }
//             ],
//             initComplete: function (settings, json) {
//               if ($(`#${menuTable}`).find('.radio-buttons').length == 0) {
//                 $(`#${menuTable}_filter`).append(radio_group_estados);

//                 $(`#${menuTable}_filter .radio-buttons #radioGroup_estado`).on('change', function () {
//                   $(`#${menuTable}`).DataTable().ajax.reload();
//                 });
//               }
//             },
//             columnDefs: [],
//             buttons: (() => {
//               let buttons = [
//                 {
//                   extend: 'collection',
//                   className: 'btn btn-label-secondary dropdown-toggle ms-2 me-0 mx-sm-3',
//                   text: '<i class="bx bx-export me-2"></i>Exportar',
//                   buttons: [
//                     {
//                       extend: 'pdf',
//                       title: 'Suministros',
//                       text: '<i class="bx bxs-file-pdf me-2"></i>Pdf',
//                       className: 'dropdown-item',
//                       exportOptions: {
//                         columns: [0, 1, 2, 3, 4],
//                         format: {
//                           body: function (data, row, column, node) {
//                             return data;
//                           }
//                         }
//                       }
//                     }
//                   ]
//                 }
//               ];

//               // AGREGAR al inicio PLANTILLA
//               buttons.unshift({
//                 text: '<i class="bx bx-plus me-0 me-md-2"></i><span class="d-none d-md-inline-block">Agregar</span>',
//                 className: 'btn btn-label-primary btn-add-new',
//                 action: function (e, dt, node, config) {
//                   $('#modalAddDetGrupoDato').modal('show');
//                 }
//               });

//               return buttons;
//             })()
//           });
//         } else {
//           CmenuTable.ajax.reload();
//         }
//       },
//       INSERT: () => {
//         let formData = new FormData();
//         formData.append('GDPDRE', $('#AddDetGrupoDato #GDPDRE').val());
//         formData.append('DTLLE', $('#AddDetGrupoDato #DTLLE').val());
//         formData.append('VLR1', $('#AddDetGrupoDato #VLR1').val());
//         formData.append('VLR2', $('#AddDetGrupoDato #VLR2').val());
//         formData.append('VLR3', $('#AddDetGrupoDato #VLR3').val());
//         formData.append('VLR4', $('#AddDetGrupoDato #VLR4').val());
//         formData.append('CESTDO', $('#AddDetGrupoDato #CESTDO').val());

//         swalFire.cargando(['Espere un momento', 'Estamos registrando el detalle del grupo dato']);
//         $.ajax({
//           url: uisApis.API + '=AddDetalle',
//           beforeSend: function (xhr) {
//             xhr.setRequestHeader('XSRF-TOKEN', localStorage.getItem('accessToken'));
//           },
//           type: 'POST',
//           dataType: 'json',
//           contentType: false,
//           processData: false,
//           data: formData,
//           success: function (data) {
//             if (data?.codEstado > 0) {
//               swalFire.success('Detalle de Grupo dato registrado correctamente', '', {
//                 1: () => {
//                   $('#modalAddDetGrupoDato').modal('hide');
//                   CmenuTable.ajax.reload();
//                 }
//               });
//             }

//             if (data?.codEstado <= 0) swalFire.error(data.mensaje);
//           },
//           error: (jqXHR, textStatus, errorThrown) =>
//             swalFire.error('Ocurrió un error al agregar el detalle del grupo dato')
//         });
//       },
//       UPDATE: () => {
//         let formData = new FormData();
//         formData.append('ID', detallerestaurantCrud.variables.rolEdit.id);
//         formData.append('GDPDRE', detallerestaurantCrud.variables.rolEdit.gdpdre);
//         formData.append('DTLLE', $('#EditDetGrupoDato #DTLLE').val());
//         formData.append('VLR1', $('#EditDetGrupoDato #VLR1').val());
//         formData.append('VLR2', $('#EditDetGrupoDato #VLR2').val());
//         formData.append('VLR3', $('#EditDetGrupoDato #VLR3').val());
//         formData.append('VLR4', $('#EditDetGrupoDato #VLR4').val());
//         formData.append('CESTDO', $('#EditDetGrupoDato #CESTDO').val());

//         swalFire.cargando(['Espere un momento', 'Estamos actualizando el detalle del Grupo Dato']);
//         $.ajax({
//           url: uisApis.API + '=UpdateDetalle',
//           beforeSend: function (xhr) {
//             xhr.setRequestHeader('XSRF-TOKEN', localStorage.getItem('accessToken'));
//           },
//           type: 'POST',
//           dataType: 'json',
//           contentType: false,
//           processData: false,
//           data: formData,
//           success: function (data) {
//             if (data?.codEstado > 0) {
//               swalFire.success('Detalle de Grupo Dato actualizado correctamente', '', {
//                 1: () => {
//                   $('#modalEditDetGrupoDato').modal('hide');
//                   CmenuTable.ajax.reload();
//                 }
//               });
//             }

//             if (data?.codEstado <= 0) swalFire.error(data.mensaje);
//           },
//           error: (jqXHR, textStatus, errorThrown) =>
//             swalFire.error('Ocurrió un error al actualizar el detalle del Grupo Dato')
//         });
//       },
//       DELETE: id => {
//         let formData = new FormData();
//         formData.append('ID', id);

//         swalFire.cargando(['Espere un momento', 'Estamos eliminando el Grupo Dato']);
//         $.ajax({
//           url: uisApis.API + '=DeleteDetalle',
//           beforeSend: function (xhr) {
//             xhr.setRequestHeader('XSRF-TOKEN', localStorage.getItem('accessToken'));
//           },
//           type: 'POST',
//           dataType: 'json',
//           contentType: false,
//           processData: false,
//           data: formData,
//           success: function (data) {
//             if (data?.codEstado > 0) {
//               swalFire.success('Grupo Dato eliminado correctamente', '', {
//                 1: () => $(`#${menuTable}`).DataTable().ajax.reload()
//               });
//             }

//             if (data?.codEstado <= 0) swalFire.error(data.mensaje);
//           },
//           error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al eliminar el Grupo Dato')
//         });
//       }
//     },
//     formularios: {},
//     validaciones: {
//       INSERT: {
//         GDPDRE: agregarValidaciones({
//           required: true
//         }),
//         DGDTLLE: agregarValidaciones({
//           required: true
//         }),
//         GDTPO: agregarValidaciones({
//           required: true
//         }),
//         DTLLE: agregarValidaciones({
//           required: true
//         }),
//         VLR1: agregarValidaciones({
//           required: true
//         })
//       },
//       UPDATE: {
//         DGDTLLE: agregarValidaciones({
//           required: true
//         }),
//         GDTPO: agregarValidaciones({
//           required: true
//         }),
//         DTLLE: agregarValidaciones({
//           required: true
//         }),
//         VLR1: agregarValidaciones({
//           required: true
//         })
//       }
//     }
//   };

//   const globalCrud = {
//     init: () => {
//       globalCrud.eventos.selects();
//       //   globalCrud.eventos.selects2();
//     },
//     eventos: {
//       selects: () => {
//         let GRUPODATOS = 'GDTPO';
//         if (!GRUPODATOS) return;

//         $.ajax({
//           url: uisApis.GD + '=ObtenerAll',
//           beforeSend: function (xhr) {
//             xhr.setRequestHeader('XSRF-TOKEN', localStorage.getItem('accessToken'));
//           },
//           type: 'GET',
//           data: {
//             GDTOS: GRUPODATOS
//           },
//           success: function (response) {
//             if (response?.data) {
//               // todos los selects dentro EditPlantilla AddPlantilla, que no sea CESTDO
//               let selects = document.querySelectorAll("select");
//               selects = Array.from(selects).filter(select => select.getAttribute('name') != 'CESTDO');

//               selects.forEach(select => {
//                 const name = select.getAttribute('name');
//                 const data = response.data.filter(d => d.gdpdre == name);
//                 select.innerHTML = `<option value="">-- Seleccione</option>`;

//                 if (data.length > 0) {
//                   data.forEach(d => {
//                     select.innerHTML += `<option value="${d.vlR1}">${d.dtlle}</option>`;
//                   });
//                 }
//               });
//             }
//           },
//           error: error => swalFire.error('Ocurrió un error al cargar los módulos')
//         });
//       },
//       selects2: () => {
//         let GDTPO = [
//           { dtlle: 'Grupo Dato', vlr1: 'G' },
//           { dtlle: 'Parametro', vlr1: 'P' }
//         ];

//         let selects = document.querySelectorAll('#EditGrupoDato select, #AddGrupoDato select');
//         selects = Array.from(selects).filter(select => select.getAttribute('name') == 'GDTPO');

//         selects.forEach(select => {
//           select.innerHTML = `<option value="">-- Seleccione</option>`;

//           if (GDTPO.length > 0) {
//             GDTPO.forEach(d => {
//               select.innerHTML += `<option value="${d.vlr1}">${d.dtlle}</option>`;
//             });
//           }
//         });
//       }
//     }
//   };


    const globales = {
        init: () => {
            globales.eventos.selects();
        },
        eventos: {
            selects: () => {
                let selects = ["CATEGORIAGD"]

                selects.forEach(selectAll => {
                    $.ajax({
                        url: uisApis.GD + '=Combo&GD=' + selectAll,
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('XSRF-TOKEN', localStorage.getItem('accessToken'));
                        },
                        type: 'GET',
                        success: function (response) {
                            if (response?.data) {
                                let select = document.querySelectorAll(`select[name=${selectAll}]`);

                                select.forEach(s => {
                                    s.innerHTML = `<option value="">-- Seleccione</option>`;
                                    response.data.forEach(d => {
                                        s.innerHTML += `<option value="${d.value}">${d.label}</option>`;
                                    });
                                });
                            }
                        },
                        error: error => swalFire.error('Ocurrió un error al cargar los módulos')
                    });
                });
            }
        }
    }
  return {
    init: async () => {
      await globales.init();

        restaurantCrud.init();
        restaurantCrud.globales();
        // detallerestaurantCrud.globales();

        var myTabs = document.querySelectorAll('.nav-tabs button');
        myTabs.forEach(function (tab) {
            tab.addEventListener('click', function () {
            const tabPane = tab.getAttribute('data-bs-target');
            if (tabPane === '#navs-restaurant') {
                redirect(false, 'navs-menu', 0);
                restaurantCrud.eventos.TABLE();
            }

            if (tabPane === '#navs-menu') {
                // detallerestaurantCrud.eventos.TABLEGRUPODATODETALLE();
            }
            });
        });
      
    }
  };
};



executeView().init();

// const useContext = async () => {
//   $.ajax({
//     url: '/Login/Index?handler=Validate&accessToken=' + localStorage.getItem('accessToken'),
//     type: 'GET',
//     success: data => (data?.success ? executeView().init() : (window.location.href = '/Login')),
//     error: error => (window.location.href = '/Login')
//   });
// };

// useContext();
