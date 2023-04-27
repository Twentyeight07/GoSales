const baseModel = {
    productId: 0,
    barCode: "",
    brand: "",
    description: "",
    categoryId: 0,
    stock: 0,
    picUrl: "",
    price: 0,
    isActive: 1,
}



let dataTable;

$(document).ready(function () {
    fetch("/Categories/List").then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res.data.length > 0) {
            res.data.forEach((item) => {
                $("#cboCategory").append(
                    $("<option>").val(item.categoryId).text(item.description)
                )
            })
        }
    })


    dataTable = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Products/List',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "productId", "visible": false, "searchable": false },
            {
                "data": "picUrl", render: function (data) {
                    return `<img style="height:60px" src=${data} class="rounded mx-auto d-block"/>`
                }
            },
            { "data": "barCode" },
            { "data": "brand" },
            { "data": "description" },
            { "data": "categoryName" },
            { "data": "stock" },
            { "data": "price" },
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
                filename: 'Reporte Productos',
                exportOptions: {
                    columns: [2, 3, 4, 5, 6]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})


function showModal(model = baseModel) {
    $("#txtId").val(model.productId)
    $("#txtBarCode").val(model.barCode)
    $("#txtBrand").val(model.brand)
    $("#txtDescription").val(model.description)
    $("#cboCategory").val(model.categoryId == 0 ? $("#cboCategory option:first").val() : model.categoryId)
    $("#txtStock").val(model.stock)
    $("#txtPrice").val(model.price)
    $("#cboState").val(model.isActive)
    $("#txtPicture").val("")
    $("#imgProduct").attr("src", model.picUrl)

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
    model["productId"] = parseInt($("#txtId").val())
    model["barCode"] = $("#txtBarCode").val()
    model["brand"] = $("#txtBrand").val()
    model["description"] = $("#txtDescription").val()
    model["categoryId"] = $("#cboCategory").val()
    model["stock"] = $("#txtStock").val()
    model["price"] = $("#txtPrice").val()
    model["isActive"] = $("#cboState").val()

    const pictureInput = document.getElementById("txtPicture");

    const formData = new FormData();

    formData.append("picture", pictureInput.files[0])
    formData.append("model", JSON.stringify(model))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (model.productId == 0) {
        fetch("/Products/Create", {
            method: "POST",
            body: formData
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        }).then(res => {
            if (res.state) {
                dataTable.row.add(res.object).draw(false)
                $("#modalData").modal("hide")
                swal("Listo!", "Se ha creado el producto satisfactoriamente", "success")
            } else {
                swal("Lo sentimos :(", res.message, "error")
            }
        }).catch(err => {
            console.log(err);
        })
    } else {
        fetch("/Products/Edit", {
            method: "PUT",
            body: formData
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        }).then(res => {
            console.log(pictureInput.value, pictureInput.files[0])
            if (res.state) {
                dataTable.row(selectedRow).data(res.object).draw(false);
                selectedRow = null;
                $("#modalData").modal("hide")
                swal("Listo!", "Se ha modificado el producto satisfactoriamente", "success")
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
        text: `Eliminar el producto "${data.description}"`,
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

                fetch(`/Products/Delete?ProductId=${data.productId}`, {
                    method: "DELETE"
                }).then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide")
                    return response.ok ? response.json() : Promise.reject(response);
                }).then(res => {
                    if (res.state) {
                        dataTable.row(row).remove().draw(false);

                        swal("Listo!", "Se ha eliminado el producto satisfactoriamente", "success")
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