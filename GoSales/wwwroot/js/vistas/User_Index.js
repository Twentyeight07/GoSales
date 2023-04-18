﻿

const baseModel = {
    userId: 0,
    name: "",
    email: "",
    phone: "",
    idRole: 0,
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
    $("#cboRole").val(model.idRole == 0 ? $("#cboRole option:first").val() : model.idRole)
    $("#cboState").val(model.isActive)
    $("#txtPicture").val("")
    $("#userPicture").attr("src", model.picUrl)

    $("#modalData").modal("show")
}

$("#btnNew").click(function () {
    showModal()
})