
var name_val = null;
var email_val = null;
var role_val = 0;
var fromdate_val = null;
var todate_val = null;

 
let dataTable;
$(function () {
    AdvSearch();
    DataHandler("/Admin/DeleteHandler?id=", dataTable);

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

function AdvSearch() {

    var roleidval = $("#userrolid").val();

    if (roleidval > 0) {
        role_val = roleidval;
    }
    var nameVal = $("#registerName").val();
    if (nameVal != null) {
        name_val = nameVal;
    }

    var emailVal = $("#registerEmail").val();
    if (emailVal != null) {
        email_val = emailVal;
    }

    var fromdateVal = $("#fromRegisterdate").val();
    if (fromdateVal != null)
    {
        fromdate_val = fromdateVal;
    }

    var TodateVal = $("#toregisterdate").val();
    if (TodateVal != null)
    {
        todate_val = TodateVal;
    }

    
    loadDataTable(name_val, email_val, role_val, fromdate_val, todate_val);
}




function loadDataTable(name, email, roleid,fromdate,Todate) {
    dataTable = $("#dataTable").DataTable({
        destroy: true,
        ajax: {
            url: "/Admin/RegistrationAdvSearch?name=" + name + "&email=" + email + "&roleid=" + roleid + "&fromregisterDate=" + fromdate + "&ToregisterDate=" + Todate,
            type: "POST"
        },
        columns: [
            { data: "id" },
            { data: "role" },
            { data: "name" },
            { data: "email" },
            { data: "password" },
            { data: "registrationDate" },

            {
                data: "imageUpload",
                orderable: false,
                render: function (data) {
                    return `<a class="image-popup" href="${data}" >
                               <img src="${data}" alt="profile Img" class="img-fluid img-thumbnail" style="border-radius: 0;width:100px;height:50px; box-shadow: 0 0 6px #AFC5E7;">
                           </a>`;
                }
            },


            {
                data: "id",
                width: "25%",
                render: function (data) {
                    return `<div>
                             <a href="/Admin/Registration?id=${data}" class="btn btn-soft-info  ">
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