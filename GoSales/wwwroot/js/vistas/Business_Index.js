
$(document).ready(function () {

    $(".card-body").LoadingOverlay("show");

    fetch("/Business/Get").then(response => {
        $(".card-body").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res) {
            const d = res.object;
            console.log(res);

            $("#txtDocNumber").val(d.docNumber)
            $("#txtBusinessName").val(d.name)
            $("#txtEmail").val(d.email)
            $("#txtAddress").val(d.address)
            $("#txPhone").val(d.phone)
            $("#txtTax").val(d.taxRate)
            $("#txtCurrencySymbol").val(d.currencySymbol)
            $("#imgLogo").attr("src",d.logoUrl)
        } else {
            swal("Lo sentimos :(", res.message, "error")
        }
    })


})

$("#btnSaveChanges").click(function () {
    const inputs = $("input.input-validar").serializeArray();
    const inputsNoValue = inputs.filter((item) => item.value.trim() == "")

    if (inputsNoValue.length > 0) {
        const message = `Debe completar el campo: ${inputsNoValue[0].name}`;
        toastr.warning("", message);
        $(`input[name="${inputsNoValue[0].name}"]`).focus();
        return;
    }

    const model = {
        docNumber: $("#txtDocNumber").val(),
        name: $("#txtBusinessName").val(),
        email: $("#txtEmail").val(),
        address: $("#txtAddress").val(),
        phone: $("#txPhone").val(),
        taxRate: $("#txtTax").val(),
        currencySymbol: $("#txtCurrencySymbol").val()

    }

    const logoInput = document.getElementById("txtLogo");

    const formData = new FormData();

    formData.append("logo", logoInput.files[0]);
    formData.append("model", JSON.stringify(model));


    $(".card-body").LoadingOverlay("show");

    fetch("/Business/SaveChanges", {
        method: "POST",
        body: formData
    }).then(response => {
        $(".card-body").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res) {
            console.log(res);
            const d = res.object;

            $("#imgLogo").attr("src",d.logoUrl)

        } else {
            swal("Lo sentimos :(", res.message, "error")
        }
    })

})