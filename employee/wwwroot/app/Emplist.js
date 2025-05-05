var EmployeeList = [];
var dataGrid;
function loadEmployee(onSuccess) {
    $.ajax({
        url: '/Employee/ListEmployees',
        type: 'GET',
        success: function (res) {
            //EmployeeList = res
            //res.forEach(emp => {
            //    emp.experiences.forEach(exp => {
            //        EmployeeList.push({
            //            employeeID: emp.employee.employeeID,
            //            name: emp.employee.name,
            //            age: emp.employee.age,
            //            gender: emp.employee.gender,
            //            contact: emp.employee.contact,
            //            company: exp.company,
            //            department: exp.department,
            //            years: exp.years
            //        });
            //    });
            //});
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

        
         //custom toolbar to add
         //onToolbarPreparing: function (e) {
         //    e.toolbarOptions.items.unshift({
         //        location: "after",
         //        widget: "dxButton",
         //        options: {
         //            icon: "add",
         //            text: "",
         //            onClick: function () {
         //                $.get('/Employee/GetEmployee', function (response)
         //                {
         //                    $('#createEmployeeModal .modal-content').html(response);
         //                    $('#createEmployeeModal').modal('show');
         //                    initializemodal();
         //                });
         //            }
         //        }
         //    });
         //},

         editing: {},
         form: {},

        columnFixing: { enabled: true },
        columns: [
            {
                dataField: "name",
                caption: "नाम",
                allowgrouping: false
            },
            {
                dataField: "age",
                allowgrouping: true
            },
            {
                dataField: "gender",
            },
            {
                dataField: "contact",
            },
            {
                dataField: 'years'
                
            },
            {
                type: "buttons",
                buttons: [
                        {
                            hint: "Edit",
                            icon: "edit",
                            onClick: function (e){
                                    const id = e.row.data.employeeID;

                                 
                                $.ajax({
                                    url: '/Employee/GetEmployee/' + id,
                                    type: 'GET',
                                    success: function (response) {


                                        $('#createEmployeeModal .modal-content').html(response);
                                        $('#createEmployeeModal').modal('show');
                                        initializemodal();

                                    }
                                });

                            
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


        summary: {
            groupItems: [{
                summaryType: "count"
            }]
        },

    
    }).dxDataGrid("instance");

}






$(() => {
    loadEmployee(datagridinit);
});

