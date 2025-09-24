
var StudentName_val = 0;
var Class_val = null;
var section_val = null;
var result_val = 0;




let dataTable;
$(function () { 
    Fulldetsilsadvancefetchmethod();
    deletehandlermethod1("/Teacher/StudentListDeleteMethod?id=", dataTable);
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

function Fulldetsilsadvancefetchmethod()
{
    var Studentnameval = $("#studentDetailsReg_StudentId").val();
    if (Studentnameval > 0)
    {
        StudentName_val = Studentnameval;
    }
    var Studentclassval = $("#studentDetailsReg_Class").val();
    if (Studentclassval !=null)
    {
        Class_val = Studentclassval;
    }
    var Studentsectionval = $("#studentDetailsReg_Section").val();
    if (Studentsectionval !=null)
    {
        section_val = Studentsectionval;
    }
    var Studentresultval = $("#studentDetailsReg_ResultId").val();
    if (Studentresultval > 0)
    {
        result_val = Studentresultval;
    }

    loaddataTable(StudentName_val, Class_val, section_val, result_val);

}


function loaddataTable(name,studentclass,section,result) {
    dataTable = $("#dataTable").DataTable({
        destroy:true,
        ajax: {
            url: "/Teacher/ResultDetailsAdvnaceFetchMethod?StudentName=" + name + "&StudentClass=" + studentclass + "&section=" + section + "&result=" + result,
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
                          <a href="/Teacher/StudentDetails?id=${data}" class="btn btn-soft-info  ">
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