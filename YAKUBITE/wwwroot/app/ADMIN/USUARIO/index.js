/**
 * PERSONAL CRUD JS
 */

'use strict';

const executeView = () => {
    const uisApis = {
        API: '/Admin/Usuarios/Index?handler',
        GD: '/Auth/Login/Index?handler'
    };

    // * VARIABLES
    let usuariosTable = 'usuariosTable';
    let CusuariosTable = null;

    // * files
    let myDropzoneAddUsuario = null;
    let myDropzoneEditUsuario = null;

    // * TABLAS
    const usuarioCrud = {
        init: () => {
            usuarioCrud.eventos.TABLE();
        },
        globales: () => {
            let dropzoneBasic = $('#AddUsuario #dropzone-area');
            if (dropzoneBasic) {
                myDropzoneAddUsuario = new Dropzone(dropzoneBasic[0], {
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

            let dropzoneBasicEdit = $('#EditUsuario #dropzone-area');
            if (dropzoneBasicEdit) {
                myDropzoneEditUsuario = new Dropzone(dropzoneBasicEdit[0], {
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
            $('#modalAddUsuario').on('show.bs.modal', function (e) {
                myDropzoneAddUsuario.removeAllFiles();
                configFormVal('AddUsuario', usuarioCrud.validaciones.INSERT, () => usuarioCrud.eventos.INSERT());
            });

            $('#modalEditUsuario').on('show.bs.modal', function (e) {
                myDropzoneEditUsuario.removeAllFiles();
                configFormVal('EditUsuario', usuarioCrud.validaciones.UPDATE, () => usuarioCrud.eventos.UPDATE());
                func.actualizarForm('EditUsuario', usuarioCrud.variables.rowEdit);
                agregarArchivoADropzone(usuarioCrud.variables.rowEdit?.ruta, myDropzoneEditUsuario);
            });

            //   // * FORMULARIOS
            $(`#${usuariosTable}`).on('click', '.edit-row-button', function () {
                const data = CusuariosTable.row($(this).parents('tr')).data();
                if (!data.id) return swalFire.error('No se encontró el restaurant seleccionado');
                usuarioCrud.variables.rowEdit = data;
                $('#modalEditUsuario').modal('show');
            });

            $(`#${usuariosTable}`).on('click', '.delete-row-button', function () {
                const data = CusuariosTable.row($(this).parents('tr')).data();
                if (!data.id) return swalFire.error('No se encontró el restaurant seleccionado');
                swalFire.confirmar('¿Está seguro de eliminar el Restaurant?', {
                    1: () => usuarioCrud.eventos.DELETE(data.id)
                });
            });
        },
        variables: {
            rowEdit: {}
        },
        eventos: {
            TABLE: () => {
                if (!CusuariosTable) {
                    CusuariosTable = $(`#${usuariosTable}`).DataTable({
                        ...configTable(),
                        ajax: {
                            url: uisApis.API + '=Buscar',
                            type: 'GET',
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader('Authorization', 'Bearer ' + (localStorage.getItem('accessToken') || null));
                            },
                            data: function (d) {
                                delete d.columns;
                                d.ESTADO = func.obtenerCESTDO(usuariosTable);
                            }
                        },
                        columns: [
                            { data: 'rn', title: '' },
                            { data: null, title: 'Nombres', render: data => `${data?.nombres || ""} ${data?.apellidos || ""}` },
                            { data: null, title: "Usuario", render: data => data?.usuario ? data.usuario.toUpperCase() : '' },
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
                            if ($(`#${usuariosTable}`).find('.radio-buttons').length == 0) {
                                $(`#${usuariosTable}_filter`).append(select_estados);

                                $(`#${usuariosTable}_filter .radio-buttons #radioGroup_estado`).on('change', function () {
                                    $(`#${usuariosTable}`).DataTable().ajax.reload();
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

                                    $('#modalAddUsuario').modal('show');
                                }
                            });

                            return buttons;
                        })()
                    });
                } else {
                    CusuariosTable.ajax.reload();
                }
            },
            INSERT: () => {

                let file = myDropzoneAddUsuario.files[0];
                if (!file) return swalFire.error('Debe seleccionar una imagen');

                let formData = new FormData();
                formData.append('NOMBRES', $('#AddUsuario #NOMBRES').val());
                formData.append('APELLIDOS', $('#AddUsuario #APELLIDOS').val());
                formData.append('USUARIOADM', $('#AddUsuario #USUARIO').val());
                formData.append('CORREO', $('#AddUsuario #CORREO').val());
                formData.append('TELEFONO', $('#AddUsuario #TELEFONO').val());
                formData.append('PASSWORD', $('#AddUsuario #PASSWORD').val());
                formData.append('ROL', $('#AddUsuario #ROL').val());
                formData.append('FILE', file);
                formData.append('ESTADO', $('#AddUsuario #ESTADO').val());

                swalFire.cargando(['Espere un momento', 'Estamos registrando el usuario']);
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
                            swalFire.success('usuario registrado correctamente', '', {
                                1: () => {
                                    $('#modalAddUsuario').modal('hide');
                                    CusuariosTable.ajax.reload();
                                }
                            });
                        }

                        if (data?.codEstado <= 0) swalFire.error(data.mensaje);
                    },
                    error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al agregar el usuario')
                });
            },
            UPDATE: () => {

                let file = myDropzoneEditUsuario.files[0];
                if (!file) return swalFire.error('Debe seleccionar una imagen');

                let formData = new FormData();
                formData.append('ID', usuarioCrud.variables.rowEdit.id);
                formData.append('NOMBRES', $('#EditUsuario #NOMBRES').val());
                formData.append('APELLIDOS', $('#EditUsuario #APELLIDOS').val());
                formData.append('USUARIOADM', $('#EditUsuario #USUARIO').val());
                formData.append('CORREO', $('#EditUsuario #CORREO').val());
                formData.append('ROL', $('#EditUsuario #ROL').val());
                formData.append('TELEFONO', $('#EditUsuario #TELEFONO').val());
                formData.append('PASSWORD', $('#EditUsuario #PASSWORD').val());
                formData.append('FILE', file?.isExist ? null : file);
                formData.append('RUTA', usuarioCrud.variables.rowEdit.ruta);
                formData.append('ESTADO', $('#EditUsuario #ESTADO').val());

                swalFire.cargando(['Espere un momento', 'Estamos actualizando el usuario']);
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
                            swalFire.success('usuario actualizado correctamente', '', {
                                1: () => {
                                    $('#modalEditUsuario').modal('hide');
                                    CusuariosTable.ajax.reload();
                                }
                            });
                        }

                        if (data?.codEstado <= 0) swalFire.error(data.mensaje);
                    },
                    error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al actualizar el usuario')
                });
            },
            DELETE: id => {
                let formData = new FormData();
                formData.append('ID', id);

                swalFire.cargando(['Espere un momento', 'Estamos eliminando el usuario']);
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
                            swalFire.success('usuario eliminado correctamente', '', {
                                1: () => $(`#${usuariosTable}`).DataTable().ajax.reload()
                            });
                        }

                        if (data?.codEstado <= 0) swalFire.error(data.mensaje);
                    },
                    error: (jqXHR, textStatus, errorThrown) => swalFire.error('Ocurrió un error al eliminar el usuario')
                });
            }
        },
        formularios: {},
        validaciones: {
            INSERT: {
                NOMBRES: agregarValidaciones({
                    required: true,
                }),
                APELLIDOS: agregarValidaciones({
                    required: true
                }),
                USUARIO: agregarValidaciones({
                    required: true
                }),
                CORREO: agregarValidaciones({
                    required: true,
                    regexp: /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/
                }),
                ROL: agregarValidaciones({
                    required: true
                }),
                PASSWORD: agregarValidaciones({
                    required: true,
                    minlength: 8
                }),
            },
            UPDATE: {
                NOMBRES: agregarValidaciones({
                    required: true,
                }),
                APELLIDOS: agregarValidaciones({
                    required: true
                }),
                USUARIO: agregarValidaciones({
                    required: true
                }),
                CORREO: agregarValidaciones({
                    required: true,
                    regexp: /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/
                }),
                ROL: agregarValidaciones({
                    required: true
                }),
                PASSWORD: agregarValidaciones({
                    required: true,
                    minlength: 8
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
                let selects = ["ROL"]

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

            usuarioCrud.init();
            usuarioCrud.globales();

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
