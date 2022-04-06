let dataTable;


$(document).ready(function () {
    loadDataTable();

});

function loadDataTable(){
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Product/GetAll"
        },
        "columns": [
            {"data":"id", "width":"15%"},
            {"data":"name", "width":"15%"},
            {
                "data": "mainImage",
                "render": function (data) {
                    return `
                            <div class="image" >
                                <img src="${data}" style="width: 100px;" alt="Product Image">
                            </div>
                        `
                },
                "width": "15%"
            },
            {"data":"price", "width":"15%"},
            {"data":"gender", "width":"15%"},
            {"data":"shoesHeight", "width":"15%"},
            {"data":"closureType", "width":"15%"},
            {"data":"brand.name", "width":"15%"},
            {"data":"category.name", "width":"15%"},
            {
                "data": "publishedDate",
                "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="/Admin/ProductVariation/Index?productId=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Variations
                            </a>
                            <a onClick=Delete('/Admin/Product/Delete?id=${data}') class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
					    </div>
                        `
                },
                "width": "15%"
            },

        ]
    });
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
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}

/*let deleteProductBtn = document.getElementById("delete_product_btn");
let url = `/Admin/Product/Delete?id=${deleteProductBtn.dataset.productId}`;

deleteProductBtn.addEventListener("click", deleteProduct.bind(null, url));

function deleteProduct(url) {
    return fetch(url, {
        method: "DELETE",
        headers: {
            "Content-type": "application/json"
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            return response.json();
        }
        else {
            response.json().then(errData => {
                console.log(errData);
                throw new Error("Something went wrong");
            })
        }
    }).catch(err => {
        console.log(err);
        throw new Error('Something went wrong');
    })
}*/