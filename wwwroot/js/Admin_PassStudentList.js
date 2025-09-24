
var StudentNameID_Val = 0;
var StudentClass_Val = null;
var StudentSection = null;
var StudentFromMarks = 0;
var StudentToMarks = 0;


let dataTable;
$(function () {
    advanceSearchButton();
    deletehandlermethod1("/Admin/PassDeleteHandler?id=", dataTable);
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

function advanceSearchButton()
{
    var Studentnameval = $("#studentDetailsReg_StudentId").val();
    if (Studentnameval > 0)
    {
        StudentNameID_Val = Studentnameval;
    }
    var Studentclass = $("#studentDetailsReg_Class").val();
    if (Studentclass != null)
    {
        StudentClass_Val = Studentclass;
    }

    var StudentSectionval = $("#studentDetailsReg_Section").val();
    if (StudentSectionval != null)
    {
        StudentSection = StudentSectionval;
    }
    var StudentFrommarks = $("#FromMarks").val();
    if (StudentFrommarks > 0)
    {
        StudentFromMarks = StudentFrommarks;  
    }

    var Studenttomarks = $("#ToMarks").val();
    if (Studenttomarks > 0)
    {
        StudentToMarks = Studenttomarks;
    }

    loaddataTable(StudentNameID_Val, StudentClass_Val, StudentSection, StudentFromMarks, StudentToMarks);

}



function loaddataTable(name,studentclass,section,frommarks,tomarks) 
{
    dataTable = $("#dataTable").DataTable({
        destroy: true,
        ajax: {
            url: "/Admin/PassStudentAdvanceSearch?studentnameId=" + name + "&studentclass=" + studentclass + "&StudentSection=" + section + "&frommarks=" + frommarks + "&toMarks=" + tomarks,
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