let dataTable;


$(function () {
    loaddataTable();
    
})

function loaddataTable() {
    dataTable = $("#dataTable").DataTable({
        ajax: {
            url: "/Student/FetchMethod",
            type: "POST",
            responsive: true,
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
            
        ]

    })
}

