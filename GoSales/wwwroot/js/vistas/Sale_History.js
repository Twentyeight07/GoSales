

const searchView = {
    searchDate: () => {
        $("#txtStartDate").val("")
        $("#txtEndDate").val("")
        $("#txtSaleNumber").val("")

        $(".busqueda-fecha").show()
        $(".busqueda-venta").hide()
    },
    searchSale: () => {
        $("#txtStartDate").val("")
        $("#txtEndDate").val("")
        $("#txtSaleNumber").val("")

        $(".busqueda-fecha").hide()
        $(".busqueda-venta").show()
    }
}


$(document).ready(function () {
    searchView["searchDate"]();

    $.datepicker.setDefaults($.datepicker.regional["es"]);

    $("#txtStartDate").datepicker({dateFormat: "dd/mm/yy"})
    $("#txtEndDate").datepicker({ dateFormat: "dd/mm/yy" })

})

$("#cboSearchBy").change(function () {
    if ($("#cboSearchBy").val() == "fecha") {
        searchView["searchDate"]();
    } else {
        searchView["searchSale"]();
    }



})

$("#btnSearch").click(function () {
    if ($("#cboSearchBy").val() == "fecha") {   

        if ($("#txtStartDate").val().trim() == "" || $("#txtEndDate").val().trim() == "") {
            toastr.warning("", "Debe ingresar un rango de fechas válido");
            return;
        } 

    } else {
        if ($("#txtSaleNumber").val().trim() == "") {
            toastr.warning("", "Debe ingresar un número de venta");
            return;
        }

    }

    let saleNumber = $("#txtSaleNumber").val();
    let startDate = $("#txtStartDate").val();
    let EndDate = $("#txtEndDate").val();


    $(".card-body").find("div.row").LoadingOverlay("show");

    fetch(`/Sale/History?saleNumber=${saleNumber}&startDate=${startDate}&endDate=${EndDate}`).then(response => {
        $(".card-body").find("div.row").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {

        $("#tbSale tbody").html("");

        if (res.length > 0) {
            res.forEach((sale) => {
                $("#tbSale tbody").append(
                    $("<tr>").append(
                        $("<td>").text(sale.registryDate),
                        $("<td>").text(sale.saleNumber),
                        $("<td>").text(sale.saleDocType),
                        $("<td>").text(sale.clientDoc),
                        $("<td>").text(sale.clientName),
                        $("<td>").text(sale.total),
                        $("<td>").append(
                            $("<button>").addClass("btn btn-info btn-sm").append(
                                $("<i>").addClass("fas fa-eye")
                            ).data("sale", sale)
                        )
                    )
                )
            })
        } else {
            swal("Lo sentimos :(", res.message, "error")
        }
    })
})



$("#tbSale tbody").on("click", ".btn-info", function () {
    let d = $(this).data("sale");

    $("#txtRecordDate").val(d.registryDate)
    $("#txtSaleNum").val(d.saleNumber)
    $("#txtRecordUser").val(d.user)
    $("#txtDocType").val(d.saleDocType)
    $("#txtClientDoc").val(d.clientDoc)
    $("#txtClientName").val(d.clientName)
    $("#txtSubTotal").val(d.subTotal)
    $("#txtIGV").val(d.totalTax)
    $("#txtTotal").val(d.total)


    $("#tbProducts tbody").html("");

    d.saleDetail.forEach((item) => {
        $("#tbProducts tbody").append(
            $("<tr>").append(
                $("<td>").text(item.productDescription),
                $("<td>").text(item.quantity),
                $("<td>").text(item.price),
                $("<td>").text(item.total)
            )
        )
    })

    $("#linkPrint").attr("href", `/Sale/ShowSalePDF?saleNumber=${d.saleNumber}`)

    $("#modalData").modal("show");


})