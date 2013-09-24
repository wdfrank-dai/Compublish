//人员选择控件

$(document).ready(function () {
    $("#PersonTree").jstree({
        "json_data": {
            "ajax": {
                "url": "/Scheduling/GetPersonTreeData",
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json charset=utf-8"
            }
        },

        "themes": { "theme": "default", "dots": true, "icons": true },
        "plugins": ["themes", "json_data", "ui", "checkbox"]
    });
});
