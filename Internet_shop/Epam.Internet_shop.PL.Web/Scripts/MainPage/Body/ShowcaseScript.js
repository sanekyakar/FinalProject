let xhr = new XMLHttpRequest();
let jsonObjectArray = new Array();

xhr.onreadystatechange = function () {
    if (xhr.readyState == 4) {
        if (xhr.status == 200) {
            if (xhr.responseText != "") {
                let jsonObject = JSON.parse(xhr.responseText);

                for (let i of jsonObject) {
                    drawShowcaseObject(i);

                    jsonObjectArray.push(i);
                }
            }
        }
    }
}

function ResponseNextAjax() {
    xhr.open("GET", '/Handlers/ShowcaseAjaxHandler.ashx');
    xhr.send(null);
}

NextAjaxBtn.onclick = ResponseNextAjax;
ResponseNextAjax();

CloseWindowBtn.onclick = function () {
    Modal.style.display = "none";
}

function drawShowcaseObject(ShowcaseObject) {

    let Image = document.createElement("img");
    Image.setAttribute("src", ShowcaseObject.Product.ImageInBase64Src);

    let Name = document.createElement("h4");
    Name.textContent = ShowcaseObject.Product.Name;

    let Price = document.createElement("h4");
    Price.textContent = ShowcaseObject.Price + " руб";

    let Point = document.createElement("li");
    Point.textContent = ShowcaseObject.Product.Discription;

    let List = document.createElement("ul");
    List.appendChild(Point);

    let BayBtn = document.createElement("button");
    BayBtn.textContent = "Купить";

    let FormButton = document.createElement("form");
    FormButton.setAttribute("action", "");
    FormButton.setAttribute("method", "get");
    FormButton.appendChild(BayBtn);

    let Div = document.createElement("div");
    Div.setAttribute("id", ShowcaseObject.Id);
    Div.setAttribute("name", "element");
    Div.setAttribute("class", "showcase-element");
    Div.appendChild(Image);
    Div.appendChild(Name);
    Div.appendChild(Price);
    Div.appendChild(List);
    Div.appendChild(FormButton);

    Showcase.appendChild(Div);
}

function onClichShowcase() {
    Modal.style.display = "flex";

    var a = event.target;

    while (a) {
        if (a.getAttribute("name") == 'element') {
            let id = a.getAttribute("id");
            for (let item of jsonObjectArray) {
                if (item.Id == id) {
                    ModalShowcaseProductImage.setAttribute("src", item.Product.ImageInBase64Src);
                    ModalShowcaseProductName.textContent = item.Product.Name;
                    ModalShowcasePrice.textContent = item.Price + ' руб';
                    ModalShowcaseProductDiscription.textContent = item.Product.Discription;
                    ModalShowcaseCategoryName.textContent = item.Product.Category.Name;
                    ModalShowcaseStatusName.textContent = item.Status.Name;
                    ModalShowcaseStoreName.textContent = item.Store.Name;
                    ModalShowcaseVendorName.textContent = item.Vendor.Name;
                    
                    break;
                }
            }
            //ModalShowcaseName.textContent = jsonObjectArray[id].Product.Name;

            break;
        }
        else {
            a = a.parentNode;
        }
    }
}

Showcase.addEventListener("click", onClichShowcase, false)


