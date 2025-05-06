var EmployeeList = [];
var dataGrid;


function loadEmployee(onSuccess) {
    $.ajax({
        url: '/Employee/ListEmployees',
        type: 'GET',
        success: function (res) {
        
            const merged = {};
            res.forEach(entry => {
                const emp = entry.employee;
                const empId = emp.employeeID;

             
                if (!merged[empId]) {
                    merged[empId] = {
                        employeeID: emp.employeeID,
                        name: emp.name,
                        age: emp.age,
                        gender: emp.gender,
                        contact: emp.contact,
                        years: 0 
                    };
                }
                entry.experiences.forEach(exp => {
                    merged[empId].years += exp.years;
                });
            });

           
            EmployeeList = Object.values(merged);
          

            onSuccess();
            if (dataGrid) {
                dataGrid.refresh();
            }
            else {
                console.error("datagrid is not initalized");
            }
        },
        error: function (err) {
            alertify.error("Error loading employees");
            console.log(err);
        }
    });
}

function datagridinit() {
     dataGrid = $("#dataGrid").dxDataGrid({

        dataSource: EmployeeList,
        keyExpr: "employeeID",

       

        columnFixing: { enabled: true },
        columns: [
            { dataField: "name", caption: "नाम",allowgrouping: false },
            { dataField: "age", allowgrouping: true },
            { dataField: "gender" },
            { dataField: "contact"},
            { dataField: 'years'},
            {
                type: "buttons",
                buttons: [
                        {
                            hint: "Edit",
                            icon: "edit",
                        onClick: function (e) {
                            const id = e.row.data.employeeID;

                            $("#employeePopup").dxPopup({
                                title: "Edit Employee",
                                visible: true,
                                width: 700,
                                height: "auto",
                                showTitle: true,
                                dragEnabled: true,
                                closeOnOutsideClick: false,
                                contentTemplate: function (contentElement) {
                                    renderForm(contentElement); 

                                 
                                        $.ajax({
                                            url: '/Employee/GetEmployee/' + id,
                                            type: 'GET',
                                            success: function (response) {
                                                loadEmployeeForm(response); 
                                            }
                                        });
                                   
                                }
                            }).dxPopup("instance");
                        }

                        },
                        {
                            hint: "Delete",
                            icon: "trash",
                            onClick: function (e) {
                                const empId = e.row.data.employeeID;

                                alertify.confirm("Confirm Deletion", "Are you sure you want to delete this employee?",
                                    function () {
                                        $.ajax({
                                            url: `/Employee/Delete/${empId}`,
                                            type: 'POST',
                                            success: function () {
                                                alertify.success("Employee Deleted");
                                               
                                                loadEmployee(datagridinit);
                                            },
                                            error: function () {
                                                alertify.error("Delete failed.");
                                            }
                                        });
                                    },
                                    function () {
                                        alertify.message('Canceled');
                                    }
                                );
                            }
                        }


                ]

            }
        ],

        filterRow: { visible: true },
        searchPanel: { visible: true },
        groupPanel: { visible: true },
        allowColumnReordering: true,
        allowColumnResizing: true,
        columnAutoWidth: true,
        selection: { mode: "single" },

         //custom toolbar to add
         onToolbarPreparing: function (e) {
             e.toolbarOptions.items.unshift({
                 location: "after",
                 widget: "dxButton",
                 options: {
                     icon: "add",
                     text: "",
                     type: "default",
                     onClick: function () {
                             experienceList = [];
                             experienceCounter = 1;

                             $("#employeePopup").dxPopup({
                                 title: "Add Employee",
                                 visible: true,
                                 width: 700,
                                 height: "auto",
                                 showTitle: true,
                                 dragEnabled: true,
                                 closeOnOutsideClick: false,
                                 contentTemplate: function (contentElement) {
                                     renderForm(contentElement);
                                 }
                             }).dxPopup("instance");
                     }
                 }
             });
         },

    
    }).dxDataGrid("instance");

}

//$(() => {
//    loadEmployee(datagridinit);
//});
loadEmployee(datagridinit);







