// Excel Download
$("#btnDownloadExcel").click(function () {
    $.ajax({
        url: "/Teacher/PassStudentDownload",
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.downloadAllow === 1) {
                var jdata = JSON.parse(JSON.stringify(data));
                var json = JSON.stringify(jdata.data.result);
                var myJsonArray = json;

                // This uses same logic your sir gave (myExcelXML)
                var myTestXML = new myExcelXML(myJsonArray);
                
                const dynamicFileName = "Pass_Student_List";
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
