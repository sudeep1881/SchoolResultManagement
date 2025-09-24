// Excel Download
$("#btnDownLoadExcel").click(function () {
    $.ajax({
        url: "/Teacher/FailStudentDownload",
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.downloadAllow === 1) {
                var jdata = JSON.parse(JSON.stringify(data));
                var json = JSON.stringify(jdata.data.result);
                var myJsonArray = json;

                
                var myTestXML = new myExcelXML(myJsonArray);

                const dynamicFileName = "Fail_Student_List";
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
