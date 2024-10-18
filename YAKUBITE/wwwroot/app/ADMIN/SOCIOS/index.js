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
    let sociosTable = 'sociosTable';
    let CsociosTable = null;

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
                myDropzoneEditRestaurant = new Dropzone(dropzoneBasicEdit[0], {
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
                myDropzoneAddRestaurant.removeAllFiles();
                configFormVal('AddRestaurant', restaurantCrud.validaciones.INSERT, () => restaurantCrud.eventos.INSERT());
            });

            $('#modalEditRestaurant').on('show.bs.modal', function (e) {
                myDropzoneEditRestaurant.removeAllFiles();
                configFormVal('EditRestaurant', restaurantCrud.validaciones.UPDATE, () => restaurantCrud.eventos.UPDATE());
                func.actualizarForm('EditRestaurant', restaurantCrud.variables.rowEdit);
                agregarArchivoADropzone(restaurantCrud.variables.rowEdit?.ruta, myDropzoneEditRestaurant);
            });

            //   // * FORMULARIOS
            $(`#${sociosTable}`).on('click', '.edit-row-button', function () {
                const data = CsociosTable.row($(this).parents('tr')).data();
                if (!data.id) return swalFire.error('No se encontró el restaurant seleccionado');
                restaurantCrud.variables.rowEdit = data;
                $('#modalEditRestaurant').modal('show');
            });

            $(`#${sociosTable}`).on('click', '.delete-row-button', function () {
                const data = CsociosTable.row($(this).parents('tr')).data();
                if (!data.id) return swalFire.error('No se encontró el restaurant seleccionado');
                swalFire.confirmar('¿Está seguro de eliminar el Restaurant?', {
                    1: () => restaurantCrud.eventos.DELETE(data.id)
                });
            });
        },
        variables: {
            rowEdit: {}
        },
        eventos: {
            TABLE: () => {
                if (!CsociosTable) {
                    CsociosTable = $(`#${sociosTable}`).DataTable({
                        ...configTable(),
                        ajax: {
                            url: uisApis.API + '=BuscarAll',
                            type: 'GET',
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader('Authorization', 'Bearer ' + (localStorage.getItem('accessToken') || null));
                            },
                            data: function (d) {
                                delete d.columns;
                                d.ESTADO = func.obtenerCESTDO(sociosTable);
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
                                    return `<span><i class="fa fa-circle ${data.estado == 'A' ? 'text-success' : 'text-danger'}" title=${data.estado == 'A' ? 'Activo' : 'Inactivo'
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
                                    </div>`;
                                }
                            }
                        ],
                        initComplete: function (settings, json) {
                            if ($(`#${sociosTable}`).find('.radio-buttons').length == 0) {
                                $(`#${sociosTable}_filter`).append(select_estados);

                                $(`#${sociosTable}_filter .radio-buttons #radioGroup_estado`).on('change', function () {
                                    $(`#${sociosTable}`).DataTable().ajax.reload();
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
                    CsociosTable.ajax.reload();
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

                swalFire.cargando(['Espere un momento', 'Estamos registrando el Socio']);
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
                            swalFire.success('Socio registrado correctamente', '', {
                                1: () => {
                                    $('#modalAddRestaurant').modal('hide');
                                    CsociosTable.ajax.reload();
                                }
                            });
                        }

                        if (data?.codEstado <= 0) swalFire.error(data.mensaje);
                    },
                    error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al agregar el socio')
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

                swalFire.cargando(['Espere un momento', 'Estamos actualizando el Socio']);
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
                            swalFire.success('Socio actualizado correctamente', '', {
                                1: () => {
                                    $('#modalEditRestaurant').modal('hide');
                                    CsociosTable.ajax.reload();
                                }
                            });
                        }

                        if (data?.codEstado <= 0) swalFire.error(data.mensaje);
                    },
                    error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al actualizar el Socio')
                });
            },
            DELETE: id => {
                let formData = new FormData();
                formData.append('ID', id);

                swalFire.cargando(['Espere un momento', 'Estamos eliminando el Socio']);
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
                            swalFire.success('Socio eliminado correctamente', '', {
                                1: () => $(`#${sociosTable}`).DataTable().ajax.reload()
                            });
                        }

                        if (data?.codEstado <= 0) swalFire.error(data.mensaje);
                    },
                    error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al eliminar el Socio')
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
                        error: error => swalFire.error('Ocurrió un error al cargar los datos')
                    });
                });
            }
        }
    }
    return {
        init: async () => {
            await globales.init();
            await func.limitarCaracteres();

            restaurantCrud.init();
            restaurantCrud.globales();

            setTimeout(() => {
                $('.dataTables_filter .form-control').removeClass('form-control-sm');
                $('.dataTables_length .form-select').removeClass('form-select-sm');
                $('.dt-buttons').addClass('d-flex align-items-center gap-3 gap-md-0');
            }, 300);
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
