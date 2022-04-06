let dataTable;
let productId = document.getElementById('tblData').dataset.productid;

$(document).ready(function () {
    loadDataTable(productId);

});

function loadDataTable(productId){
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/ProductVariation/GetAll?productId=" + productId
        },
        "columns": [
            /*{"data":"productVariation.id", "width":"15%"},*/
            {
                "data": "imageUrl",
                "render": function (data) {
                    return `
                            <div class="image" >
                                <img src="${data}" style="width: 100px;" alt="Product Image">
                            </div>
                        `
                },
                "width": "5%"
            },
            {"data":"color.colorValue", "width":"15%"},
            {
                "data": "product_Id",
                "render": function (data, type, row, meta) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/ProductVariation/Edit?productId=${row.product_Id}&colorId=${row.color.id}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onClick=Delete('/Admin/ProductVariation/Delete?productId=${row.product_Id}&colorId=${row.color.id}') class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
					    </div>
                        `
                },
                "width": "10%"
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