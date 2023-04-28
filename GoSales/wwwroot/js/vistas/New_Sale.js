



let taxValue = 0;

$(document).ready(function () {
    fetch("/Sale/ListSaleDocType").then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res.length > 0) {
            res.forEach((item) => {
                $("#cboSaleDocType").append(
                    $("<option>").val(item.saleDocTypeId).text(item.description)
                )
            })
        }
    })

    fetch("/Business/Get").then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res.state) {
            const d = res.object

            $("#inputGroupSubTotal").text(`SubTotal - ${d.currencySymbol}`)
            $("#inputGroupIGV").text(`IVA ${d.taxRate}% - ${d.currencySymbol}`)
            $("#inputGroupTotal").text(`Total - ${d.currencySymbol}`)

            taxValue = parseFloat(d.taxRate);
        }
    })


    $("#cboSearchProducts").select2({
        ajax: {
            url: "/Sale/GetProducts",
            dataType: 'json',
            contentType:"application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                return {
                    search: params.term // search term
                };
            },
            processResults: function (data) {

                return {
                    results: data.map((item) => (
                        {
                        id: item.productId,
                        text: item.description,
                        brand: item.brand,
                        category: item.categoryName,
                        picUrl: item.picUrl,
                        price: parseFloat(item.price) 
                        }
                    ))
                };
            }
        },
        language: "es",
        placeholder: 'Buscar Producto',
        minimumInputLength: 1,
        templateResult: resFormat
    });


})


function resFormat(data) {
    // This shows "loading"
    if (data.loading)
        return data.text;


    var container = $(`
    <table width="100%">
        <tr>
            <td style="width:60px">
                <img style="height:60px; width:60px; margin-right: 10px;" src="${data.picUrl}" />
            </td>
            <td>
                <p style="font-weight: bolder; margin:2px">${data.brand}</p>
                <p style="margin:2px">${data.text}</p>
            </td>
        </tr>
    </table>    
    `)

    return container;
}


$(document).on("select2:open", function () {
    document.querySelector(".select2-search__field").focus();

})


let productsToSale = [];
$("#cboSearchProducts").on("select2:select", function (e) {
    const data = e.params.data;


    let productFound = productsToSale.filter(p => p.productId == data.id)
    if (productFound.length > 0) {
        $("#cboSearchProduct").val("").trigger("change")
        toastr.warning("", "El producto ya fue agregado");
        return false;
    }

    swal({
        title: data.brand,
        text: data.text,
        picUrl: data.picUrl,
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        inputPlaceholder: "Ingrese la cantidad"
    },
        function (value) {
           
            if (value === false)
                return false;

            if (value === "") {
                toastr.warning("", "Necesita ingresar la cantidad");
                return false;
            }

            if (isNaN(parseInt(value))) {
                toastr.warning("", "La cantidad debe ser en valor numérico");
                return false;
            }

            let product = {
                productId: data.id,
                productBrand: data.brand,
                productDescription: data.text,
                productCategory: data.category,
                quantity: parseInt(value),
                price: data.price.toString(),
                total: (parseFloat(value) * data.price).toString()
            }
            productsToSale.push(product); 
            showProductPrices();
            $("#cboSearchProducts").val("").trigger("change")
            swal.close();
        }
    )

})



function showProductPrices() {
    let total = 0;
    let igv = 0;
    let subtotal = 0;
    let percentage = taxValue/100;


    $("#tbProduct tbody").html("");

    productsToSale.forEach((item) => {
        total = total + parseFloat(item.total);
        $("#tbProduct tbody").append(
            $("<tr>").append(
                $("<td>").append(
                    $("<button>").addClass("btn btn-danger btn-eliminar btn-sm").append(
                        $("<i>").addClass("fas fa-trash-alt")
                    ).data("id", item.productId)
                ),
                $("<td>").text(item.productDescription),
                $("<td>").text(item.quantity),
                $("<td>").text(item.price),
                $("<td>").text(item.total)
            )
        )
    })

    subtotal = total / (1 + percentage);
    igv = total - subtotal;

    $("#txtSubTotal").val(subtotal.toFixed(2))
    $("#txtIGV").val(igv.toFixed(2))
    $("#txtTotal").val(total.toFixed(2))

}

$(document).on("click","button.btn-eliminar", function () {
    const _productId = $(this).data("id")
    productsToSale = productsToSale.filter(p => p.productId != _productId);

    showProductPrices();

})



$("#btnEndSale").click(function () {
    if (productsToSale < 1) {
        toastr.warning("", "Debe ingresar productos");
        return;
    }

    const vmSaleDetail = productsToSale;

    const sale = {
        saleDocTypeId: $("#cboSaleDocType").val(),
        clientDoc: $("#txtClientDoc").val(),
        clientName: $("#txtClientName").val(),
        subTotal: $("#txtSubTotal").val(),
        totalTax: $("#txtIGV").val(),
        total: $("#txtTotal").val(),
        saleDetail: vmSaleDetail
    }
    
    $("#btnEndSale").LoadingOverlay("show");

    fetch("/Sale/RecordSale", {
        method: "POST",
        headers: { "Content-type": "application/json; charset=utf-8" },
        body: JSON.stringify(sale)
    }).then(response => {
        $("#btnEndSale").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res) {
            productsToSale = [];
            showProductPrices();

            $("#txtClientDoc").val("");
            $("#txtClientName").val("");
            $("#cboSaleDocType").val($("#cboSaleDocType option:first").val());

            swal("Registrado!", `Número de venta: ${res.object.saleNumber}`, "success")

        } else {
            swal("Lo sentimos!", "No se pudo completar la venta", "error")
        }

    })

})