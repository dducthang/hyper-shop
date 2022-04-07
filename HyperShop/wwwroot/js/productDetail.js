let productId = document.querySelector("#content").dataset.productId;
const url = "/Customer/Product/GetVariations";
const cartUrl = "/Customer/Cart/AddToCart";

let primaryImages = document.getElementsByClassName("primary-image");
let addToCartBtn = document.getElementById("addtocart_btn");


const ToggleActiveImage = (event) => {
    //reset image active css
    for (let img of primaryImages) {
        if (img.classList.contains('image-active')) {
            img.classList.remove('image-active')
        }
    }

    //add image active css to currently choosen image
    let primaryImage = event.currentTarget;
    if (!primaryImage.classList.contains('image-active')) {
        primaryImage.classList.add('image-active')
    }
    else {
        primaryImage.classList.remove('image-active')
    }
}

const ToggleActiveClass = (event) => {
    let sizeBox = event.currentTarget;

    if (!sizeBox.classList.contains('size-box-active')) {
        sizeBox.classList.add('size-box-active')
    }
    else {
        sizeBox.classList.remove('size-box-active')
    }
}

const GetVariationImages = (data) => {
    let temp = data;
    fetch(url, {
        method: "POST",
        body: JSON.stringify(data),
        headers: {
            "Content-type":"application/json"
        }
    }).then(response => {
        if (response.status >= 200 && response.status < 300) {
            response.json().then(result => {

                let thumbs = document.getElementsByClassName("owl-thumb-item");
                let owlItems = document.getElementsByClassName("owl-item");

                //reset carousel images
                for (let item of thumbs) {
                    item.querySelector("img").setAttribute("src", "");
                }
                for (let item of owlItems) {
                    item.querySelector(".item").querySelector("img").setAttribute("src", "");
                }

                //add new images to carousel 
                for (let i = 0; i < result.images.length; i++) {
                    thumbs[i].querySelector("img").setAttribute("src", result.images[i].imageUrl);
                }
                for (let i = 0; i < owlItems.length; i++) {
                    let index = (i + result.images.length - 4) % result.images.length ;
                    owlItems[i].querySelector(".item").querySelector("img").setAttribute("src", result.images[index].imageUrl);
                }

                let sizeArea = document.querySelector(".size-area").querySelector(".size-table");
                //delete old size area 
                sizeArea.innerHTML = "";

                //add size to size area
                for (let i = 0; i < result.sizes.length; i++) {
                    let sizeBox = document.createElement("div");
                    sizeBox.classList.add("col-md-3");
                    sizeBox.classList.add("size-box");
                    sizeBox.innerText = result.sizes[i].sizeValue;
                    if (result.variationSizes.includes(result.sizes[i].sizeValue)) {
                        sizeBox.classList.add("size-box-avai");
                    }
                    sizeBox.addEventListener('click', ToggleActiveClass);
                    sizeArea.appendChild(sizeBox);
                    
                }
            })
        }
    }).catch(err => {

    })
}

const AddToCart = async () => {
    let variation;
    let sizeList =[]
    for (let img of primaryImages) {
        if (img.classList.contains('image-active')){
            variation = {
                productId: img.dataset.productId,
                colorId: img.dataset.colorId
            }
        }
    }

    let sizeBoxes = document.getElementsByClassName("size-box");
    for (let sizeBox of sizeBoxes) {
        if (sizeBox.classList.contains('size-box-active')) {
            sizeList.push(parseFloat(sizeBox.innerText));
        }
    }

    if (!variation) {
        alert("Please shoes variation");
    }
    else if (sizeList.length == 0) {
        alert('Please choose shoes size');
    }
    else {
        await fetch(cartUrl, {
            method: "POST",
            body: JSON.stringify({
                productId: variation.productId,
                colorId: variation.colorId,
                sizeList
            }),
            headers: {
                'Content-type': 'application/json'
            }
        }).then(response => {
            if (response.status >= 200 && response.status < 300) {
                response.json().then(result => {
                    console.log(result);
                    alert("Add Product To Cart Successfully");
                })
            }
        })
    }
    
}

addToCartBtn.addEventListener('click', AddToCart);

for (let img of primaryImages) {
    img.addEventListener('click', ToggleActiveImage);
    let productId = img.dataset.productId;
    let colorId = img.dataset.colorId;
    img.addEventListener('click', GetVariationImages.bind(null, { productId: productId, colorId: colorId }))
}

window.onload = GetVariationImages({ productId:parseInt(productId), colorId:-1 });
