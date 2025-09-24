

let dataTable;
$(function () {
    loadDataTable();
    DataHandler("/Admin/RoleDeleteHandler?id=", dataTable);

})

function DataHandler(url, dataTable) {
    const table = document.querySelector("#dataTable");
    table.addEventListener("click", (e) => {
        let id = e.target.dataset.deleteId ?? e.target.parentElement.dataset.deleteId;
        if (id) {
            deletehandle(`${url}${id}`, dataTable);
        }
    })

}

function deletehandle(url, dataTable) {
    Swal.fire({
        title: "Are you Sure!!",
        text: "You won't get return Data",
        icon: "warning",
        showCancelButton: true,
        cancelButtonColor: "#f52f41",
        confirmButtonColor: "#351171",
        confirmButtonText: "Yes,Delete it!!"
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
                console.log("Network Slow", e);
            }
        }
    })
}

function loadDataTable() {
    dataTable = $("#dataTable").DataTable({
        ajax: {
            url: "/Admin/RoleFetchMethod",
            type: "POST"
        },
        columns: [
            { data: "id" },
            { data: "name" },
            

            {
                data: "id",
                width: "25%",
                render: function (data) {
                    return `<div>
                             <a href="/Admin/Role?id=${data}" class="btn btn-soft-info  ">
                               <i class="bx bx-edit"></i>
                                    </a>
                               <a   class="btn btn-soft-danger   delete-btn" data-delete-id="${data}">
                             <i class="bx bxs-trash"></i>
                            </a>
                           </div>`;
                }
            }

        ]


    })
}