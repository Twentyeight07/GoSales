

$(document).ready(function () {

    $(".container-fluid").LoadingOverlay("show");

    fetch("/Home/GetUser").then(response => {
        $(".container-fluid").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res) {
            console.log("Hola")
            const d = res.object;

            $("#imgFoto").attr("src", d.picUrl)

            $("#txtName").val(d.name)
            $("#txtEmail").val(d.email)
            $("#txtPhone").val(d.phone)
            $("#txtRole").val(d.roleName)

        } else {
            swal("Lo sentimos :(", res.message, "error")
        }
    });
})


$("#btnSaveChanges").click(function() {
    if ($("#txtEmail").val().trim() == "") {
        toastr.warning("", "Debe completar el campo Correo");
        $("#txtEmail").focus();
        return;
    }

    if ($("#txtPhone").val().trim() == "") {
        toastr.warning("", "Debe completar el campo Correo");
        $("#txtPhone").focus();
        return;
    }


    swal({
        title: "¿Desea guardar los cambios?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-primary",
        confirmButtonText: "Si",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (res) {
            if (res) {
                $(".showSweetAlert").LoadingOverlay("show")

                let model = {
                    email: $("#txtEmail").val().trim(),
                    phone: $("#txtPhone").val().trim()
                }

                fetch(`/Home/SaveProfile`, {
                    method: "POST",
                    headers: { "Content-type": "application/json; charset=utf-8" },
                    body: JSON.stringify(model)
                }).then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide")
                    return response.ok ? response.json() : Promise.reject(response);
                }).then(res => {
                    if (res.state) {
                        swal("Listo!", "Los cambios han sido guardados", "success")
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


$("#btnChangePassword").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputsNoValue = inputs.filter((item) => item.value.trim() == "")

    if (inputsNoValue.length > 0) {
        const message = `Debe completar el campo: ${inputsNoValue[0].name}`;
        toastr.warning("", message);
        $(`input[name="${inputsNoValue[0].name}"]`).focus();
        return;
    }

    if ($("#txtNewPassword").val().trim() != $("#txtConfirmPassword").val().trim()) {
        toastr.warning("", "Las contraseñas no coinciden");
        return;
    }

    let model = {
        actualPassword: $("#txtActualPassword").val().trim(),
        newPassword: $("#txtNewPassword").val()
    }


    fetch(`/Home/ChangePassword`, {
        method: "POST",
        headers: { "Content-type": "application/json; charset=utf-8" },
        body: JSON.stringify(model)
    }).then(response => {
        $(".showSweetAlert").LoadingOverlay("hide")
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res.state) {
            swal("Listo!", "Se ha actualizado la contraseña", "success")
            $("input.input-validar").val("");
        } else {
            swal("Lo sentimos :(", res.message, "error")
        }
    }).catch(err => {
        console.log(err);
    })

})