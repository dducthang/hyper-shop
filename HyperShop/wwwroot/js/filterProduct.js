let color = [];
let brand = [];
let height = [];
let gender = [];

const btnApply = document.getElementsByClassName('btn-apply');


const brandForm = document.getElementById("brand_form");
const colorForm = document.getElementById("color_form");
const heightForm = document.getElementById("height_form");
const genderForm = document.getElementById("gender_form");

const url = "/Customer/Product/FilterProducts";

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

const loadProduct = async (event) => {
    event.preventDefault();
    let filters = getInputs();
    let data = {

    }
    await fetch(url, {
        method: "POST",
        body: JSON.stringify(data),
        headers: {
            "Content-type":"application/json"
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            productArea.innerHTML = "";
            response.json().then(result => {
                for (let i = 0; i < result.products.length;i++) {
                    const proBox = document.createElement("div");
                    proBox.classList.add('col-lg-4');
                    proBox.innerHTML = `<div class="product">
                                <div class="flip-container">
                                    <div class="flipper">
                                        <div class="front"><a><img src="${result.products[i].mainImage}" alt="" class="img-fluid"></a></div>
                                        <div class="back"><a><img src="${result.products[i].mainImage}" alt="" class="img-fluid"></a></div>
                                    </div>
                                </div>
                                <a href="detail.html" class="invisible"><img src="${result.products[i].mainImage}" alt="" class="img-fluid"></a>
                                <div class="text btn-primary">${result.color[i]} Color(s)</div>
                                <div class="text">
                                    <h3><a href="detail.html">${result.products[i].name}</a></h3>
                                    <p class="price">
                                        <del></del>$${result.products[i].price}.00
                                    </p>
                                    <p class="buttons"><a class="btn btn-outline-secondary">View detail</a><a class="btn btn-primary"><i class="fa fa-shopping-cart"></i>Add to cart</a></p>
                                </div>
                                <!--/.text-->
                            </div>
                            <!--/.product-->`

                    productArea.appendChild(proBox);
                }
            })
        }
        else {
            response.json().then(errData => {
                //throw new Error('Something went wrong');
            })
        }
    }).catch(error => {
        //await throw new Error("Something went wrong");
    })
}

for (let btn of btnApply) {
    btn.addEventListener('click', loadProduct);
}

//brandForm.onload = loadProduct();