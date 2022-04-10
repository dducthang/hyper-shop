const url = "/Customer/Checkout/CalShipCost"

const selectCity = document.querySelector("#select_city");
const createOrderBtn = document.getElementById("create_order_btn");

const SelectShipCity = (event) => {
    var selectForm = event.currentTarget;
    var cityId = selectForm.value;

    fetch(url, {
        method: "POST",
        body: JSON.stringify(cityId),
        headers: {
            'Content-type': 'application/json'
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            response.json().then(result => {
                let totalElement = document.querySelector('.total');
                let totalValue = totalElement.dataset.total;
                let thTotal = totalElement.querySelector('th');
                thTotal.innerText = `$${parseInt(totalValue) + result.shipCost}.00`;

                let shipCostElement = document.querySelector('.ship-cost');
                //let shipValue = shipCostElement.dataset.shipCost = result.shipCost;
                let thShipCost = shipCostElement.querySelector('th');
                thShipCost.innerText = `$${result.shipCost}.00`;
            })
        }
    })
}

const CreateOrder = (event) => {
    event.preventDefault();

}

selectCity.addEventListener("change", SelectShipCity)

