
let dataTable;
$(document).ready(function () {

    $.datepicker.setDefaults($.datepicker.regional["es"]);

    $("#txtStartDate").datepicker({ dateFormat: "dd/mm/yy" })
    $("#txtEndDate").datepicker({ dateFormat: "dd/mm/yy" })


    dataTable = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Reports/SaleReport?startDate=01/01/1991&endDate=01/01/1991',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "registryDate" },
            { "data": "saleNumber" },
            { "data": "docType" },
            { "data": "clientDocument" },
            { "data": "clientName" },
            { "data": "saleSubTotal" },
            { "data": "totalSalesTax" },
            { "data": "totalSale" },
            { "data": "product" },
            { "data": "quantity" },
            { "data": "price" },
            { "data": "total" }
        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte de Ventas'
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

})


$("#btnSearch").click(function () {
    if ($("#txtStartDate").val().trim() == "" || $("#txtEndDate").val().trim() == "") {
        toastr.warning("", "Debe ingresar un rango de fechas válido");
        return;
    }

    let startDate = $("#txtStartDate").val();
    let endDate = $("#txtEndDate").val();

    let newUrl = `/Reports/SaleReport?startDate=${startDate}&endDate=${endDate}`;

    dataTable.ajax.url(newUrl).load();
})