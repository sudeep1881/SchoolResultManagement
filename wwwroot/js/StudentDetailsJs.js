
var name_val = 0;
var class_val=null;
var Section_val = null;
var result_val = 0;




let dataTable;
$(function () {
    studentresult()
    deletehandlermethod1("/Teacher/StudentDetailsDeleteMethod?id=", dataTable);
})

function deletehandlermethod1(url, dataTable)
{
    const table = document.querySelector("#dataTable");
    table.addEventListener("click", (e) => {
        let id = e.target.dataset.deleteId ?? e.target.parentElement.dataset.deleteId;
        if (id) {
            deleteHandlerHover(`${url}${id}`, dataTable);
        }
    })
}

function deleteHandlerHover(url, dataTable)
{
    Swal.fire({
        title: "Are you Sure!!",
        text: "You Won't Get return this Data",
        icon: "error",
        showCancelButton: true,
        cancelButtonColor:"red",
        confirmButtonColor:"blue",
        confirmButtonText: "Yes! ,Delete it"
    }).then(async(result) => {
        if (result.isConfirmed)
        {
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
                console.log("network issue",e)
            }
        }
    })  



}


function studentresult() {
    var nameidvalue = $("#StudentName").val();

    if (nameidvalue > 0) {
        name_val = nameidvalue;
    }
    var classvalue = $("#ClassId").val();
 
    if (classvalue != null) {
        class_val = classvalue;
    }
    var sectionval = $("#sectionSearch").val();
    if (sectionval != null) {
        Section_val = sectionval;
    }
    var resval = $("#ResultSearch").val();
    if (resval > 0) {
        result_val = resval;
    }
    //alert(classvalue);
    //alert(class_val);
    //return true; 
    loaddataTable(name_val, class_val, Section_val, result_val);

}

function loaddataTable(name,stuclass,section,resullt) {
    dataTable = $("#dataTable").DataTable({
        destroy: true,
        ajax: {
            url: "/Teacher/AdvnaceSearchFormStudentDetails?studentNameId=" + name + "&studentclass=" + stuclass + "&section=" + section + "&result=" + resullt,
            type:"POST",
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