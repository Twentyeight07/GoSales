

const baseModel = {
    userId: 0,
    name: "",
    email: "",
    phone: "",
    roleId: 0,
    isActive: 1,
    picUrl: ""
}

let dataTable;

$(document).ready(function () {
    fetch("/Users/RoleList").then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res.length > 0) {
            res.forEach((item) => {
                $("#cboRole").append(
                    $("<option>").val(item.roleId).text(item.description)
                )
            })
        }
    })


    dataTable = $('#tbdata').DataTable({
        responsive: true,
         "ajax": {
             "url": '/Users/List',
             "type": "GET",
             "datatype": "json"
         },
         "columns": [
             { "data": "userId","visible":false,"searchable":false },
             {
                 "data": "picUrl", render: function (data) {
                     return `<img style="height:60px" src=${data} class="rounded mx-auto d-block"/>`
                 }
             },
             { "data": "name" },
             { "data": "email" },
             { "data": "phone" },
             { "data": "roleName" },
             {
                 "data": "isActive", render: function (data) {
                     if (data == 1)
                         return '<span class="badge badge-info">Activo</span>';
                     else
                         return '<span class="badge badge-danger">No Activo</span>';
                 }
             },
             {
                 "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                     '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                 "orderable": false,
                 "searchable": false,
                 "width": "80px"
             }
         ],
         order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte Usuarios',
                exportOptions: {
                    columns: [2,3,4,5,6]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

function showModal(model = baseModel) {
    $("#txtId").val(model.userId)
    $("#txtName").val(model.name)
    $("#txtEmail").val(model.email)
    $("#txtPhone").val(model.phone)
    $("#cboRole").val(model.roleId == 0 ? $("#cboRole option:first").val() : model.roleId)
    $("#cboState").val(model.isActive)
    $("#txtPicture").val("")
    $("#userPicture").attr("src", model.picUrl)

    $("#modalData").modal("show")
}

$("#btnNew").click(function () {
    showModal()
})

$("#btnSave").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputsNoValue = inputs.filter((item) => item.value.trim() == "")

    if (inputsNoValue.length > 0) {
        const message = `Debe completar el campo: ${inputsNoValue[0].name}`;
        toastr.warning("", message);
        $(`input[name="${inputsNoValue[0].name}"]`).focus();
        return;
    }

    const model = structuredClone(baseModel);
    model["userId"] = parseInt($("#txtId").val())
    model["name"] = $("#txtName").val()
    model["email"] = $("#txtEmail").val()
    model["phone"] = $("#txtPhone").val()
    model["roleId"] = $("#cboRole").val()
    model["isActive"] = $("#cboState").val()

    const pictureInput = document.getElementById("txtPicture");

    const formData = new FormData();

    formData.append("picture", pictureInput.files[0])
    formData.append("model", JSON.stringify(model))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (model.userId == 0) {
        fetch("/Users/Create", {
            method: "POST",
            body: formData
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        }).then(res => {
            if (res.state) {
                dataTable.row.add(res.object).draw(false)
                $("#modalData").modal("hide")
                swal("Listo!", "Se ha creado el usuario satisfactoriamente", "success")
            } else {
                console.log(res)
                swal("Lo sentimos :(", res.message, "error")
            }
        }).catch(err => {
            console.log(err);
        })
    } else {
        fetch("/Users/Edit", {
            method: "PUT",
            body: formData
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        }).then(res => {
            if (res.state) {
                dataTable.row(selectedRow).data(res.object).draw(false);
                selectedRow = null;
                $("#modalData").modal("hide")
                swal("Listo!", "Se ha modificado el usuario satisfactoriamente", "success")
            } else {
                console.log(res)
                swal("Lo sentimos :(", res.message, "error")
            }
        }).catch(err => {
            console.log(err);
        })
    }


})

let selectedRow;

$("#tbdata tbody").on("click", ".btn-editar", function () {

    if ($(this).closest("tr").hasClass("child")) {
        selectedRow = $(this).closest("tr").prev();
    } else {
        selectedRow = $(this).closest("tr");

    }

    const data = dataTable.row(selectedRow).data();

    showModal(data);

})

$("#tbdata tbody").on("click", ".btn-eliminar", function () {

    let row;

    if ($(this).closest("tr").hasClass("child")) {
        row = $(this).closest("tr").prev();
    } else {
        row = $(this).closest("tr");

    }

    const data = dataTable.row(row).data();

    swal({
        title: "¿Estás seguro?",
        text: `Eliminar al usuario "${data.name}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "Cancelar",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (res) {
            if (res) {
                $(".showSweetAlert").LoadingOverlay("show")

                fetch(`/Users/Delete?userId=${data.userId}`, {
                    method: "DELETE"
                }).then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide")
                    return response.ok ? response.json() : Promise.reject(response);
                }).then(res => {
                    if (res.state) {
                        dataTable.row(row).remove().draw(false);
       
                        swal("Listo!", "Se ha eliminado el usuario satisfactoriamente", "success")
                    } else {
                        console.log(res)
                        swal("Lo sentimos :(", res.message, "error")
                    }
                }).catch(err => {
                    console.log(err);
                })
            }
        }
    )

})