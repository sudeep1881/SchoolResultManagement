$("#Admindownloadexcel").click(function () {
    $.ajax({
        url: "/Admin/DownloadExcelAdminList",
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.dataAllows === 1) {
                let jdata = JSON.parse(JSON.stringify(data));
                let json = JSON.stringify(jdata.data);
                let jArraysList = json;

                let myEXCELXML = new myExcelXML(jArraysList);

                const dynamicFileName = "Admin Lists";
                myEXCELXML.downLoad(dynamicFileName);

                toastr.success("Downloaded Excel Formate  ");
            }
            else {
                toastr.error("Error Download Excel Formate")
            }
        },                                                                             
        error: function () {
            toastr.error("Error while Downloading Excel Formate");
        }
    })
})