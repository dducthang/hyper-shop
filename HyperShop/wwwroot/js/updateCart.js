const updateUrl = "/Customer/Cart/UpdateCartItem";
const deleteUrl = "/Customer/Cart/DeleteCartItem";

let quantityInputs = document.getElementsByClassName("quantity-input");
let deleteItemBtn = document.getElementsByClassName("delete-item-btn");

const UpdateQuantity = (event) => {
    let tableRow = event.currentTarget.closest("tr");
    let variationId = parseInt(tableRow.id);
    let quantity = parseInt(event.currentTarget.value);
    let totalElement = document.querySelector("#total")
    let total = parseInt(totalElement.dataset.total);
    let obj = {
        variationId,
        quantity,
        total
    };

    fetch(updateUrl, {
        method: "POST",
        body: JSON.stringify(obj),
        headers: {
            'Content-type':'application/json'
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            response.json().then(result => {
                totalElement.innerText = `$${result.total}.00`;
                totalElement.dataset.total = result.total;
                let subTotal = tableRow.querySelector(".sub-total");
                subTotal.innerText = `$${result.subTotal}.00`
            })
        }
        else {
            response.json().then(err => {
                console.log(err)
            })
        }
    }).catch(err => {
        console.log(err)
    })
}

const DeleteItem = (event) => {
    //get variationId
    let tableRow = event.currentTarget.closest('tr');
    let variationId = parseInt(tableRow.id);

    //get quantity
    let quantity = tableRow.querySelector('input').value;
    let totalElement = document.querySelector("#total")
    let total = parseInt(totalElement.dataset.total);

    let obj = {
        total,
        variationId,
        quantity
    }
    fetch(deleteUrl, {
        method: 'POST',
        body: JSON.stringify(obj),
        headers: {
            'Content-type':'application/json'
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            response.json().then(result => {
                let tableBody = tableRow.parentElement;
                console.log(tableBody);
                tableBody.removeChild(tableRow);
                totalElement.innerText = `$${result.total}.00`
                totalElement.dataset.total = result.total;
            })
        }
        else {
            response.json(err => {
                console.log(err);
            })
        }
    }).catch(err => {
        console.log(err)
    })
}

for (let input of quantityInputs) {
    input.addEventListener('change', UpdateQuantity);
}

for (let btn of deleteItemBtn) {
    btn.addEventListener('click', DeleteItem);
}