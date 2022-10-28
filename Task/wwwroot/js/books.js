
var dataTable;


$(document).ready(function () {
    loadDataTable();
});



function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Book/GetAll"
        },
        "columns": [
            {"data": "title", "width": "15%"},
            {"data": "author", "width": "15%"},
            {"data": "releaseDateString", "width": "15%"},
            {"data": "description", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {

                    return `
                     <div class="group w-75 btn-group">
                    <a href="/Book/Upsert?id=${data}"
                     class="btn btn-primary mx-2"> Edit </a>
                    <a onclick=Delete('/Book/Delete/${data}')
                      class="btn btn-danger mx-2"> Delete </a>
                     <a href="/Book/Reservations?id=${data}"
                     class="btn btn-dark mx-2"> Reservations </a>
                      <a onclick=addReservation('/Book/AddReservation/${data}') "
                
                     class="btn btn-info mx-2"> Reserve </a>
                </div>
                     `
                },
                "width": "25%"
            },
        ]
    });
}

function  addReservation(url){
        Swal.fire({
            title: 'Are you sure?',
            text: "You wanna add this reservation ?",
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, add it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                        url: url,
                        type: 'POST',
                        success: function (data) {
                            if(data.success){
                                dataTable.ajax.reload();
                                toastr.success(data.message)
                            }
                            else{
                                toastr.error(data.message)
                            }
                        }
                    }
                )
            }
        })
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                    url: url,
                    type: 'DELETE',
                    success: function (data) {
                        if(data.success){
                            dataTable.ajax.reload();
                            toastr.success(data.message)
                        }
                        else{
                            toastr.error(data.message)
                        }
                    }
                }
            )
        }
    })
}