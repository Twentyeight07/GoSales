const baseModel = {
    categoryId: 0,
    description: "",
    isActive: 1
}


let dataTable;

$(document).ready(function () {
   


    dataTable = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Categories/List',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoryId", "visible": false, "searchable": false },
            { "data": "description" },
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
                filename: 'Reporte Categorías',
                exportOptions: {
                    columns: [1, 2]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });


})


function showModal(model = baseModel) {
    $("#txtId").val(model.categoryId)
    $("#txtDescription").val(model.description)
    $("#cboStatus").val(model.isActive)

    $("#modalData").modal("show")
}


$("#btnNew").click(function () {
    showModal()
})


$("#btnSave").click(function () {

    if ($("#txtDescription").val().trim() == "") {
        toastr.warning("", "Debe completar el campo Descripción");
        $("#txtDescription").focus();
        return;
    }

    const model = structuredClone(baseModel);
    model["categoryId"] = parseInt($("#txtId").val())
    model["description"] = $("#txtDescription").val()
    model["isActive"] = $("#cboStatus").val()


    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (model.categoryId == 0) {
        fetch("/Categories/Create", {
            method: "POST",
            headers: { "Content-type":"application/json; charset=utf-8" },
            body: JSON.stringify(model)
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        }).then(res => {
            if (res.state) {
                dataTable.row.add(res.object).draw(false)
                $("#modalData").modal("hide")
                swal("Listo!", "Se ha creado la categoría satisfactoriamente", "success")
            } else {
                swal("Lo sentimos :(", res.message, "error")
            }
        }).catch(err => {
            console.log(err);
        })
    } else {
        fetch("/Categories/Edit", {
            method: "PUT",
            headers: { "Content-type": "application/json; charset=utf-8" },
            body: JSON.stringify(model)
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        }).then(res => {
            if (res.state) {
                dataTable.row(selectedRow).data(res.object).draw(false);
                selectedRow = null;
                $("#modalData").modal("hide")
                swal("Listo!", "Se ha modificado la categoría satisfactoriamente", "success")
            } else {
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
        text: `Eliminar la categoría "${data.description}"`,
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

                fetch(`/Categories/Delete?categoryId=${data.categoryId}`, {
                    method: "DELETE"
                }).then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide")
                    return response.ok ? response.json() : Promise.reject(response);
                }).then(res => {
                    if (res.state) {
                        dataTable.row(row).remove().draw(false);

                        swal("Listo!", "Se ha eliminado la categoría satisfactoriamente", "success")
                    } else {
                        swal("Lo sentimos :(", res.message, "error")
                    }
                }).catch(err => {
                    console.log(err);
                })
            }
        }
    )

})