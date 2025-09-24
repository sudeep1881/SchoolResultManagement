
var FailName_Val = 0;
var Class_Val = null;
var Section_Val = null;


let dataTable;
$(function () {
    AdvanceSearch();
    deletehandlermethod1("/Admin/FailDeleteMethod?id=", dataTable);
})

function deletehandlermethod1(url, dataTable) {
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
        title: "Are you Sure",
        text: "You Wont Get return this Data",
        icon: "error",
        showCancelButton: true,
        cancelButtonColor: "red",
        confirmButtonColor: "skyblue",
        confirmButtonText: "Yes! ,Delete it"
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
                console.log("network issue", e)
            }
        }
    })
}



function AdvanceSearch()
{

    var studentNameVal = $("#studentDetailsReg_StudentId").val();
    if (studentNameVal > 0)
    {
        FailName_Val = studentNameVal;
    }

    var studentClassVal = $("#studentDetailsReg_Class").val();
    if (studentClassVal != null)
    {
        Class_Val = studentClassVal;
    }

    var StudentsectionVal = $("#studentDetailsReg_Section").val();
    if (StudentsectionVal != null)
    {
        Section_Val = StudentsectionVal;
    }

    loaddataTable(FailName_Val, Class_Val, Section_Val);

}

function loaddataTable(name,studentclass,section) {
    dataTable = $("#dataTable").DataTable({
        destroy: true,
        ajax: {
            url: "/Admin/FailStudentAdvnaceForm?studentnameid=" + name + "&studentclass=" + studentclass + "&studentsection=" + section,
            type: "POST",
        },
        columns: [
            { data: 'idDTO' },
            { data: 'studentNameDTO' },
            { data: 'classDTO' },
            { data: 'sectionDTO' },
            { data: 'subjectNameDTO' },
            { data: 'marksDTO' },
            { data: 'percentageDTO' },
            { data: 'resultNameDTO' },
            {
                data: "idDTO",
                width: "25%",
                render: function (data) {
                    return `<div>
                          <a href="/Admin/ResultEdition?id=${data}" class="btn btn-soft-info  ">
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