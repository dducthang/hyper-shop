//------------------filter products---------------------------
let color = [];
let brand = [];
let height = [];
let gender = [];

const btnApply = document.getElementsByClassName('btn-apply');

const brandForm = document.getElementById("brand_form");
const colorForm = document.getElementById("color_form");
const heightForm = document.getElementById("height_form");
const genderForm = document.getElementById("gender_form");

//---------------get available product -----------------------
let productArea = document.getElementById("product_area");

const url = "/Customer/Product/FilterProducts";

let itemPerPage = 6;

//--------------------Fetch Available Product----------------------
let getInputs = () => {
    height = [];
    gender = [];
    color = [];
    brand = [];

    const brandInputs = brandForm.getElementsByTagName('input');
    const colortInputs = colorForm.getElementsByTagName('input');
    const gendertInputs = genderForm.getElementsByTagName('input');
    const heightInputs = heightForm.getElementsByTagName('input');
    for (let input of brandInputs) {
        if (input.checked) {
            brand.push(input.value);
        }
    }
    for (let input of colortInputs) {
        if (input.checked) {
            color.push(input.value);
        }
    }
    for (let input of gendertInputs) {
        if (input.checked) {
            gender.push(input.value);
        }
    }
    for (let input of heightInputs) {
        if (input.checked) {
            height.push(input.value);
        }
    }
    return {
        brand,
        color,
        gender,
        height
    }
}

const AddToCart = async (id) => {
    let productId = parseInt(id)
    await fetch(cartUrl, {
        method: "POST",
        body: JSON.stringify(productId),
        headers: {
            'Content-type':'application/json'
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            response.json().then(result => {
                console.log(result)
            })
        }
        else {
            response.json().then(errData => {
                //throw new Error('Something went wrong');
            })
        }
    }).catch(error => {
        //throw new Error('Something went wrong');
    })
}

async function FetchAvailableProduct(event, config) {
    if (event != null) {
        event.preventDefault();
    }
    let filter = getInputs();
    let data = {
        itemPerPage: config ? config.itemPerPage : itemPerPage,
        pageNumber: config ? config.pageNumber : 1,
        search: config.search,
        ...filter
    }
    await fetch("/Customer/Product/GetAvailableProducts", {
        method: "POST",
        body: JSON.stringify(data),
        headers: {
            'Content-type': 'application/json'
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            response.json().then(result => {
                //-------------------render
                document.querySelector("#products_showing").innerText = result.products.length < result.itemPerPage ? result.products.length : result.itemPerPage;

                document.querySelector("#products_total").innerText = result.quantity;

                //------------------display available products
                productArea.innerHTML = "";
                for (let i = 0; i < result.products.length; i++) {
                    const proBox = document.createElement("div");
                    proBox.classList.add('col-lg-4');
                    let colour = result.color[i] > 1 ? result.color[i] + " Colours" : result.color[i] + " Colour";
                    proBox.innerHTML = `<div class="product">
                                <div class="flip-container">
                                    <div class="flipper">
                                        <div class="front"><a href="Product/ProductDetail?productid=${result.products[i].id}"><img src="${result.products[i].mainImage}" alt="" class="img-fluid"></a></div>
                                        <div class="back"><a href="Product/ProductDetail?productid=${result.products[i].id}"><img src="${result.products[i].mainImage}" alt="" class="img-fluid"></a></div>
                                    </div>
                                </div>
                                <a class="invisible"><img src="${result.products[i].mainImage}" alt="" class="img-fluid"></a>
                                <div class="text btn-primary">${colour}</div>
                                <div class="text">
                                    <h3><a href="Product/ProductDetail?productid=${result.products[i].id}">${result.products[i].name}</a></h3>
                                    <p class="price">
                                        <del></del>$${result.products[i].price}.00
                                    </p>
                                    <p class="buttons">
                                        <a class="btn btn-outline-secondary">View detail</a>
                                        <a class="btn btn-primary">
                                            <i class="fa fa-shopping-cart"></i>Add to cart
                                        </a>
                                    </p>
                                </div>
                                <!--/.text-->
                            </div>
                            <!--/.product-->`
                    productArea.appendChild(proBox);
                }

                //--------------------pagination---------------
                var pageItems = document.getElementsByClassName("page-item");
                //-----remove old page item-----
                for (let i = 0; i < pageItems.length; i++) {
                    if (pageItems[i].id == "") {
                        document.getElementById("pagination").removeChild(pageItems[i]);
                        i--;
                    }
                }

                //-----add new page item-----
                itemPerPage = data.itemPerPage;
                let pre = document.getElementById("previous");
                for (let i = Math.ceil(result.quantity / itemPerPage); i >= 1 ; i--) {
                    let pageItem = document.createElement("li");
                    pageItem.classList.add("page-item");
                    pageItem.innerHTML = `<a href="#" class="page-link" onclick='FetchAvailableProduct(null,{itemPerPage, pageNumber:${i}})'>${i}</a>`;
                    pre.parentElement.insertBefore(pageItem, pre.nextSibling);
                }
            });
        }
        else {
            response.json().then(errData => {
                //throw new Error('Something went wrong');
            })
        }
    }).catch(error => {
        //throw new Error('Something went wrong');
    })
}



const SearchProducts = async () => {
    //event.preventDefault();
    let data = document.querySelector("#search_input").value;
    FetchAvailableProduct(null, { itemPerPage, pageNumber: 1, search: data });
}

window.onload = FetchAvailableProduct(null, { itemPerPage, pageNumber: 1 });


for (let btn of btnApply) {
    btn.addEventListener('click', FetchAvailableProduct);
}
