let dataTable;
$(function () {
    loadDataTable();
    HandleDeleteTable("/Admin/resultDeleteHandler?id=", dataTable);
})

function HandleDeleteTable(url, dataTable) {
    const table = document.querySelector("#dataTable");
    table.addEventListener("click", (e) => {
        let id = e.target.dataset.deleteId ?? e.target.parentElement.dataset.deleteId;
        if (id) {
            deleteHandlerHover(`${url}${id}`, dataTable);
        }
    })

}

function deleteHandlerHover(url, dataTable) {
    Swal.fire({
        title: "Are you sure",
        text: "You won't return Data",
        icon: "question",
        showCancelButton: true,
        cancelButtonColor: "red",
        confirmButtonColor: "skyblue",
        confirmButtonText: "Yes! Delete it"
    }).then(async (result) => {
        if (result.isConfirmed) {
            try {
                const { success, message } = await fetch(url, { method: "DELETE" }).then(res => res.json());
                if (success) {
                    dataTable.ajax.reload();
                    toastr.success(message);
                }
                else {
                    toastr.error(message);
                }

            }
            catch (e) {
                console.log("Network issue", e);
            }
        }
    })
}

function loadDataTable() {
    dataTable = $("#dataTable").DataTable({
        ajax: {
            url:"/Admin/ResultFetchMethod",
            type:"POST",
        },
        columns: [
            {data:"id"},
            { data:"examResult"},
            {
                data: "id",
                width: "25%",
                render: function (data) {
                    return `<div>
                        <a href="/Admin/Result?id=${data}" class="btn btn-soft-info  ">
                            <i class="bx bx-edit"></i>
                        </a>
                        <a class="btn btn-soft-danger   delete-btn" data-delete-id="${data}">
                            <i class="bx bxs-trash"></i>
                        </a>
                    </div>`;
                }
            },
           
        ]
    })
}