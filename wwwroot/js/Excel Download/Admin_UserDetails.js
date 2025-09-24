
// User Details  Excel Download
$("#btnDownLoadExcel").click(function () {
    $.ajax({
        url: "/Admin/UserDetailsDownload",
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.downloadAllow === 1) {
                var jdata = JSON.parse(JSON.stringify(data));
                var json = JSON.stringify(jdata.data.result);
                var myJsonArray = json;

                var myTestXML = new myExcelXML(myJsonArray);

                const dynamicFileName = "User_Regitration_Details";
                myTestXML.downLoad(dynamicFileName);

                toastr.success("Excel Downloaded Successfully!");
            } else {
                toastr.error("Download not allowed!");
            }
        },
        error: function () {
            toastr.error("Error while downloading Excel");
        }
    });
});

//  Pass Student Excel Download
$("#btndownloadExcel").click(function () {
    $.ajax({
        url: "/Admin/PassStudentDownLoadExcel",
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.downloadAllow === 1) {
                var jdata = JSON.parse(JSON.stringify(data));
                var json = JSON.stringify(jdata.data.result);
                var myJsonArray = json;


                var myTestXML = new myExcelXML(myJsonArray);

                const dynamicFileName = "Pass Student";
                myTestXML.downLoad(dynamicFileName);

                toastr.success("Excel Downloaded Successfully!");
            } else {
                toastr.error("Download not allowed!");
            }
        },
        error: function () {
            toastr.error("Error while downloading Excel");
        }
    });
});



//--------Fail Student Download Formate---------
$("#btnDownloadExcel1").click(function () {
    $.ajax({
        url: "/Admin/FailStudentDownloadExecl",
        type: "POST",
        dataType: "json",

        success: function (data) {
            if (data.downloadAllow === 1) {
                var jdata = JSON.parse(JSON.stringify(data));
                var json = JSON.stringify(jdata.data.result);
                var myjsonarray = json;

                var mydownloadXml = new myExcelXML(myjsonarray);

                const myTextFileName = "Fail Student List";
                mydownloadXml.downLoad(myTextFileName);

                toastr.success("Excel Download Successfully");



            }
            else {
                toastr.error("Execel not Downloaded");
            }
        },
        error: function () {
            toastr.error("Error while DownLoading Execl");
        }

    });
});


//-------Role Download Excel-------

$("#downloadExecl").click(function () {
    $.ajax({
        url: "/Admin/RoleExecldownload",
        type: "POST",
        dataType: "json",

        success: function (data) {
            if (data.downloadAllow === 1) {
                var jdata = JSON.parse(JSON.stringify(data));
                var json = JSON.stringify(jdata.data.result);
                var myjsonList = json;

                var MydownloadXML = new myExcelXML(myjsonList);

                const mytextfilename = "Role Name";
                MydownloadXML.downLoad(mytextfilename);

                toastr.success("Excel Download Successfully");

            }
            else {
                toastr.error("Excel Not downloaded");
            }
        },
        error: function () {
            toastr.error("Error while downloading Excel");
        }
    })
})



//-----------------Student List------------------------

$("#StudentdownloadExcel").click(function () {
    $.ajax({
        url: "/Admin/studentDownloadExcel",
        type: "POST",
        dataType: "json",

        success: function (data) {
            if (data.downloadAllow === 1) {


                var jdata = JSON.parse(JSON.stringify(data));
                var json = JSON.stringify(jdata.data.result);
                var myjsonList = json;

                var mydownloadExcelXML = new myExcelXML(myjsonList);

                const myfileName = "Student List";
                mydownloadExcelXML.downLoad(myfileName);

                toastr.success("Excel Download successfully");
            }
            else {
                toastr.error("Excel Not Downloaded");
            }

        },
        error: function () {
            toastr.error("error while Excel Downloading");
        }
        

    })
})
//------------------Teacher List Download Excel--------------------

$("#Teacherdownloadexcel").click(function(){
    $.ajax({
        url: "/Admin/TeacherDownloadexcelFormat",
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.downloadAllow === 1) {
                var jdata = JSON.parse(JSON.stringify(data));
                var json = JSON.stringify(jdata.data.result);
                var myjsonList = json;

                var mydownloadXML = new myExcelXML(myjsonList);

                const myexcelfilename = "Teacher Lists";
                mydownloadXML.downLoad(myexcelfilename);

                toastr.success("Excel Download Successfully");
            }
            else {
                toastr.error("Excel Download Fail");
            }
        },
        error: function () {
            toastr.error("Error while Excel Downloading")
        }
    })
})



