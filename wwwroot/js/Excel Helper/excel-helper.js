// excel-helper.js
(function (global) {
    function escapeHtml(str) {
        if (str === null || str === undefined) return "";
        return String(str)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#39;");
    }

    function myExcelXML(json) {
        var data = typeof json === "string" ? JSON.parse(json) : (json || []);
        this.downLoad = function (fileName) {
            if (!data || !data.length) {
                alert("No data to export");
                return;
            }

            // Column order from first object keys
            var cols = Object.keys(data[0]);

            // Build header row
            var header = "<tr>";
            for (var i = 0; i < cols.length; i++) {
                header += "<th>" + escapeHtml(cols[i]) + "</th>";
            }
            header += "</tr>";

            // Build data rows
            var rows = "";
            for (var r = 0; r < data.length; r++) {
                var row = "<tr>";
                for (var c = 0; c < cols.length; c++) {
                    var val = data[r][cols[c]];
                    row += "<td>" + escapeHtml(val) + "</td>";
                }
                row += "</tr>";
                rows += row;
            }

            var table = "<table>" + header + rows + "</table>";

            // Use Blob + URL to avoid data URI length issues
            var blob = new Blob([table], { type: "application/vnd.ms-excel" });
            var url = URL.createObjectURL(blob);

            var link = document.createElement("a");
            link.href = url;
            link.download = (fileName || "export") + ".xls";
            document.body.appendChild(link);
            link.click();

            // cleanup
            setTimeout(function () {
                URL.revokeObjectURL(url);
                document.body.removeChild(link);
            }, 100);
        };
    }

    // expose globally
    global.myExcelXML = myExcelXML;
})(window);
