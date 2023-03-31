$(document).ready(function () {
    $('#tableRole').DataTable();
});
function Delete(id) {
    console.log("ggg")
    Swal.fire({
        title: 'Are you sure?',
        text: lbTextMsgDelete,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: lbcancelButtonText,
        confirmButtonText: lbconfirmButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href=`/Admin/Accounts/DeleteRole?id=${id}`
            Swal.fire(
                lbTitleDeletedOk,
                lbMsgDeletedOkRole,
                lbSuccess
            )
        }
    })
}

Edit = (id,name) => {
    document.getElementById("roleId").value = id
    document.getElementById("roleName").value = name

    document.getElementById("btnSubmit").value = lbEdit
    document.getElementById("title").innerHTML = lbTitleEdit
    
}

Reset = ()=>{
    document.getElementById("roleName").value =""
    document.getElementById("btnSubmit").value = lbbtnSave
    document.getElementById("title").innerHTML = lbAddNewRole
}