

let dataTable;
$(function () {
    loadDataTable();
    DeleteHAndlerData("/Admin/SubjectDelete?id=", dataTable);

})

function DeleteHAndlerData(url, dataTable)
{
    const table = document.querySelector("#dataTable");
    table.addEventListener("click", (e) => {
        let id = e.target.dataset.deleteId ?? e.target.parentElement.dataset.deleteId;
        if (id) {
            deletehandlerlightshows(`${url}${id}`, dataTable);
        }
    })

}

function deletehandlerlightshows(url, dataTable)
{
    Swal.fire({
        title: "Are You Sure!",
        text: "You Won't get Return Data",
        icon: "warning",
        showCancelButton: true,
        cancelButtonColor: "Red",
        confirmButtonColor: "blue",
        confirmButtonText: "Yes!Delete it"
    }).then(async(result) => {
        if(result.isConfirmed){
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
            url: "/Admin/subjectFetchMethod",
            type:"POST",
        },
        columns: [
            { data: "id" },
            { data: "subjectName" },

            {
                data: "id",
                width: "25%",
                render: function (data) {
                    return `<div>
                                  <a href="/Admin/Subject?id=${data}" class="btn btn-soft-info  ">
                                <i class="bx bx-edit"></i>
                                    </a>
                                 <a   class="btn btn-soft-danger   delete-btn" data-delete-id="${data}">
                                     <i class="bx bxs-trash"></i>
                                           </a>
                            </div>`;
                }
            }
        ],
    })
}