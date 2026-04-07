let dataTable;

$(function () {
    loadDataTable();
    DeleteHandlerMethod("/Admin/DeletAdminListMethod?id=", dataTable);
})

function DeleteHandlerMethod(url, dataTable) {
    const table = document.querySelector('#dataTable');
    table.addEventListener("click", (e) => {
        let id = e.target.dataset.deleteId ?? e.target.parentElement.dataset.deleteId;
        if (id) {
            DeleteHandlerHover(`${url}${id}`, dataTable);
        }
    })
}

function DeleteHandlerHover(url, dataTable) {
    Swal.fire({
        title: "Are You Sure!",
        text:"You Wont Return this data",
        icon:"warning",
        showCancelButton: true,
        cancelButtonColor:"red",
        confirmButtonColor:"green",
        confirmButtonText:"Yes! Delte It"
    }).then(async (res) => {
        if (res.isConfirmed) {
            try {
                const { success, message } = await fetch(url, { method : "Delete" }).then(s => s.json());
                if (success) {
                    dataTable.ajax.reload();
                    toastr.success(message);
                }
                else {
                    toastr.error(message);
                }
            }
            catch (e){
                console.log("error while", e);
            }
        }
    })
}

function loadDataTable() {
    dataTable = $("#dataTable").DataTable({
        ajax: {
            url: "/Admin/AdminListAll",
            type:"POST"
        },
        columns: [
            { data: "id" },
            { data: "role" },
            { data: "name" },
            { data: "email" },
            { data: "password" },
            {
                data: "imageUpload",
                width: "25%",
                render: function (data) {
                    return `<a class="image-popup" href="${data}">
                    <img src="${data}" class="img-fluild img-thumbnail" style="Box-shadow:0 0 6px #AFC5E7">
                    </a>`
                }
            },
            {
                data: "id",
                width: "25%",
                render: function (data) {
                    return `<div>
                    <a href="/Admin/Registration?id=${data}" class="btn btn-soft-info">
                    <i class="bx bx-edit"></i>
                    </a>
                    <a class="btn btn-soft-danger  delete-btn" data-delete-id="${data}">
                    <i class="bx bxs-trash"></i>
                    <a>
                    </div>`
                }
            },
           

        ]
    })
}