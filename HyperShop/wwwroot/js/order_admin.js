const GetOrdersWithStatus = (event) => {
    let select = event.currentTarget;
    let filterStatus = select.value;

    let bodyTable = document.getElementById('tblData').querySelector('tbody');
    let tableRows = bodyTable.querySelectorAll('tr');
    for (let i of tableRows) {
        i.remove();
    }

    fetch(`/Admin/Order/GetAll?orderStatus=${filterStatus}`, {
        method: "GET",
        headers: {
            'Content-type':'application'
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            response.json().then(result => {
                for (let i = 0; i < result.data.length; i++) {
                    let tableRow = document.createElement('tr');
                    if (i % 2 == 0) {
                        tableRow.className = "even";
                    }
                    else {
                        tableRow.className = "odd";
                    }
                    tableRow.id=(result.data[i].id);

                    let badge = "badge-warning";
                    let orderStatus = result.data[i].orderStatus.status;
                    if (orderStatus == "Delivered") {
                        badge = "badge-success";
                    }
                    else if (orderStatus == "Delivering") {
                        badge = "badge-info";
                    }
                    tableRow.innerHTML = `
                            <td class="sorting_1">${result.data[i].id}</td>
                            <td>${result.data[i].receiver}</td>
                            <td>${result.data[i].email}</td>
                            <td>${result.data[i].phone}</td>
                            <td>${result.data[i].address}</td>
                            <td>${result.data[i].cityShipCost.cityName}</td>
                            <td>${result.data[i].orderDate}</td>
                            <td>${result.data[i].totalCost}</td>
                            <td>
                                <span class="badge ${badge}">${result.data[i].orderStatus.status}</span>
                            </td>
                            <td>
                                <select class="btn btn-primary update-status-btn">
                                    <option disabled selected>--Update Status--</option>
                                    <option value="Pending">Pending</option>
                                    <option value="Delivering">Delivering</option>
                                    <option value="Delivered">Delivered</option>
                                </select>
                            </td>
                    `
                    bodyTable.appendChild(tableRow);
                    if (filterStatus == 'All') {
                        tableRow.querySelector('select').addEventListener('change', UpdateStatus.bind(null,false));
                    }
                    else {
                        tableRow.querySelector('select').addEventListener('change', UpdateStatus.bind(null,true));
                    }
                    /*let oldScript = document.getElementById('rerun_id');
                    let source = oldScript.src;
                    oldScript.remove();
                    let rerunScript = document.createElement("script");
                    rerunScript.src = source;
                    rerunScript.id = 'rerun_id';
                    document.querySelector('body').appendChild(rerunScript);*/
                }
            })
        }
    }).catch(err => {
        console.log(err);
    })
}

const UpdateStatus = (isRemoved =false, event) => {
    let select = event.currentTarget;
    let orderStatus = select.value;

    let tableRow = select.closest('tr');
    let orderId = parseInt(tableRow.id);

    fetch("/Admin/Order/UpdateStatus", {
        method: "POST",
        body: JSON.stringify({ orderId, orderStatus }),
        headers: {
            'Content-type':'application/json'
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            response.json().then(result => {
                alert(result.message);
                if (isRemoved != false && result.err==false) {
                    tableRow.remove();
                }
                else {
                    let span = tableRow.querySelector('span');
                    let badge = '';
                    let orderStatus = result.status;
                    if (orderStatus == "Delivered") {
                        badge = "badge badge-success";
                    }
                    else if (orderStatus == "Delivering") {
                        badge = "badge badge-info";
                    }
                    else {
                        badge = "badge badge-warning"
                    }
                    span.className = badge;
                    span.innerText = orderStatus;
                }
            })
        }
    }).catch(err => {
        console.log(err);
    })
}

let select = document.querySelector('select');
let updatesStatusBtns = document.getElementsByClassName('update-status-btn');

for (let btn of updatesStatusBtns) {
    btn.addEventListener('change', UpdateStatus.bind(null, false));
}
select.addEventListener('change', GetOrdersWithStatus);
