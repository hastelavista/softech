
//only did for employee list rn

function openEmployeeForm(employeeData = null) {
    $("#employeePopup").dxPopup({
        title: employeeData ? "Edit Employee" : "Add Employee",
        visible: true,
        width: 700,
        height: "auto",
        showTitle: true,
        dragEnabled: true,
        closeOnOutsideClick: false,
        contentTemplate: function (contentElement) {
            renderEmployeeForm(contentElement, employeeData);
        }
    }).dxPopup("instance");
}

function renderEmployeeForm(container, data) {
    $("<div>").attr("id", "employeeForm").appendTo(container).dxForm({
        formData: data || {},
        colCount: 2,
        items: [
            { dataField: "name", label: { text: "Name" }, isRequired: true }, 
            { dataField: "age", editorType: "dxNumberBox" },  
            {
                dataField: "gender",
                editorType: "dxSelectBox",
                editorOptions: {
                    items: ["Male", "Female", "Other"],
                    value: ""
                }
            },  
            { dataField: "contact", label: { text: "Contact" } },  
            {
                itemType: "button",
                horizontalAlignment: "right",
                buttonOptions: {
                    text: "Save",
                    type: "default",
                    onClick: function () {
                        saveEmployeeForm();
                    }
                }
            },
            {
                itemType: "button",
                horizontalAlignment: "right",
                buttonOptions: {
                    text: "Cancel",
                    type: "danger",
                    onClick: function () {
                        closeEmployeeForm();
                    }
                }
            }
        ]
    });
}

function saveEmployeeForm() {
    const formData = $("#employeeForm").dxForm("instance").option("formData");

    $.ajax({
        url: '/Employee/Save',  
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),  
        success: function (response) {
            console.log("Employee Data Saved:", response);
            
            closeEmployeeForm();
            reloadEmployeeGrid();
        },
        error: function (xhr, status, error) {
            console.error("Error saving employee data:", error);
            alert("Failed to save employee data. Please try again.");
        }
    });
}

function reloadEmployeeGrid() {
    $("#dataGrid").dxDataGrid("instance").refresh();
}

function closeEmployeeForm() {
    $("#employeePopup").dxPopup("instance").hide();
}
















//let experienceList = [];
//let experienceCounter = 1;

//function openEmployeeForm(employeeData = null) {
//    experienceList = employeeData?.experiences?.map((e, i) => ({ ...e, id: i + 1 })) || [];
//    experienceCounter = experienceList.length + 1;

//    $("#employeePopup").dxPopup({
//        title: employeeData ? "Edit Employee" : "Add Employee",
//        visible: true,
//        width: 700,
//        height: "auto",
//        showTitle: true,
//        dragEnabled: true,
//        closeOnOutsideClick: false,
//        contentTemplate: function (contentElement) {
//            renderForm(contentElement, employeeData);
//        }
//    }).dxPopup("instance");
//}

//function renderForm(container, data) {
//    $("<div>").attr("id", "employeeForm").appendTo(container).dxForm({
//        formData: data || {},
//        colCount: 2,
//        items: [
//            { dataField: "name", label: { text: "Name" }, isRequired: true },
//            { dataField: "age", editorType: "dxNumberBox", editorOptions: { min: 18, max: 100 } },
//            {
//                dataField: "gender",
//                editorType: "dxSelectBox",
//                editorOptions: {
//                    items: ["Male", "Female", "Other"],
//                    value: ""
//                }
//            },
//            { dataField: "contact" },
//            {
//                itemType: "group",
//                colSpan: 2,
//                caption: "Experiences",
//                template: function (e) {
//                    const $container = $(e.container);
//                    $("<div>").attr("id", "experienceSection").appendTo($container);

                    
//                    $("<div>").dxButton({
//                        text: "Add Experience",
//                        icon: "add",
//                        onClick: function () {
//                            addExperienceRow();
//                        }
//                    }).appendTo("#experienceSection");

//                    $("<div>").attr("id", "experienceTable").appendTo("#experienceSection");
//                    renderExperienceGrid(); 
//                }
//            }
//        ]
//    });
//}

//function addExperienceRow() {
//    experienceList.push({
//        id: experienceCounter++, 
//        company: "",
//        title: "",
//        years: 0
//    });
//    renderExperienceGrid();
//}

//function renderExperienceGrid() {
//    if ($("#experienceTable").dxDataGrid("instance")) {
//        $("#experienceTable").dxDataGrid("instance").dispose(); 
//    }

//    $("#experienceTable").dxDataGrid({
//        dataSource: experienceList,
//        keyExpr: "id",
//        editing: {
//            mode: "row",
//            allowUpdating: true,
//            allowDeleting: true,
//            useIcons: true
//        },
//        columns: [
//            { dataField: "company", caption: "Company", validationRules: [{ type: "required" }] },
//            { dataField: "title", caption: "Title", validationRules: [{ type: "required" }] },
//            {
//                dataField: "years",
//                caption: "Years",
//                dataType: "number",
//                editorType: "dxNumberBox",
//                editorOptions: { min: 0 },
//                validationRules: [{ type: "required" }]
//            }
//        ]
//    });
//}
