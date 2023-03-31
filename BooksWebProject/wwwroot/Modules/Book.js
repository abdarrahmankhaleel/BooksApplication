$(document).ready(function () {
    $('#tableLogSubCategory').DataTable({
        "autoWidth": false,
        "responsive": true
    })

    $('#tableSubCategory').DataTable({
        "autoWidth": false,
        "responsive": true
    })

});
// declarations
let subCategoryId = document.getElementById("subCategoryId")
let subCurrentState = document.getElementById("subCurrentState")
let subCategoryName = document.getElementById("subCategoryName")
let categoryIdOfSubCateg = document.getElementById("categoryIdOfSubCateg")
let titleModel = document.getElementById("titleModel")

function Edit(id, name, catIdOfSub, currentState) {
    subCategoryId.value = id
    subCurrentState.value = currentState
    subCategoryName.value = name
    categoryIdOfSubCateg.value = catIdOfSub
    titleModel.innerText = lbEditSubCateg
}


function Delete(id) {
    Swal.fire({
        title: lbTitleMsgDelete,
        text: lbTextMsgDelete,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: lbconfirmButtonText,
        cancelButtonText: lbcancelButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/Admin/SubCategories/Delete?id=${id}`;
            Swal.fire(
                lbTitleDeletedOk,
                lbMsgDeletedOkSubCategory,
                lbSuccess
            )
        }
    })
}

DeleteLog = (id) => {
    Swal.fire({
        title: lbTitleMsgDelete,
        text: lbTextMsgDelete,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: lbconfirmButtonText,
        cancelButtonText: lbcancelButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/Admin/SubCategories/DeleteLog?id=${id}`;
            Swal.fire(
                lbTitleDeletedOk,
                lbMsgDeletedOk,
                lbSuccess
            )
        }
    })
}

function Reset() {
    subCategoryId.value = "";
    subCurrentState.value = "";
    subCategoryName.value = ""
    categoryIdOfSubCateg.value = "";
    titleModel.innerText = lbTitleAddSubCategory
}

document.getElementById("defaultOpen").click();
function openTab(evt, tapName) {

    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(tapName).style.display = "block";
    evt.currentTarget.className += " active";
}