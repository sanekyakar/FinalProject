let xhr = new XMLHttpRequest();

xhr.onreadystatechange = function () {
    if (xhr.readyState == 4) {
        if (xhr.status == 200) {
            if (xhr.responseText != "") {
                let jsonObject = JSON.parse(xhr.responseText);

                for (let i of jsonObject) {
                    drawUnitObject(i);
                }
            }
        }
    }
}

function ResponseNextAjax() {
    xhr.open("GET", '/Handlers/ProductAjaxHandler.ashx');
    xhr.send(null);
}

NextAjaxBtn.onclick = ResponseNextAjax;
ResponseNextAjax();

function drawUnitObject(UnitObject) {
    let Id = document.createElement("td");
    Id.setAttribute("name", "id");
    Id.innerText = UnitObject.Id;

    let Name = document.createElement("td");
    Name.setAttribute("name", "name");
    Name.innerText = UnitObject.Product.Name;

    let Price = document.createElement("td");
    Price.setAttribute("name", "price");
    Price.innerText = UnitObject.Price;

    let Category = document.createElement("td");
    Category.setAttribute("name", "category");
    Category.innerText = UnitObject.Product.Category.Name;

    let Store = document.createElement("td");
    Store.setAttribute("name", "store");
    Store.innerText = UnitObject.Store.Name;

    let Vendor = document.createElement("td");
    Vendor.setAttribute("name", "vendor");
    Vendor.innerText = UnitObject.Vendor.Name;

    let Status = document.createElement("td");
    Status.setAttribute("name", "status");
    Status.innerText = UnitObject.Status.Name;

    let ChangeBtn = document.createElement("button");
    ChangeBtn.setAttribute("name", "change-btn");
    ChangeBtn.innerText = "Изменить";

    let Btns = document.createElement("td");
    Btns.setAttribute("name", "btns");
    Btns.appendChild(ChangeBtn);

    let Tr = document.createElement("tr");
    Tr.setAttribute("id", UnitObject.Id);
    Tr.appendChild(Id);
    Tr.appendChild(Name);
    Tr.appendChild(Price);
    Tr.appendChild(Category);
    Tr.appendChild(Store);
    Tr.appendChild(Vendor);
    Tr.appendChild(Status);
    Tr.appendChild(Btns);

    TBody.appendChild(Tr);
}

ModalCloseBtn.onclick = function () {
    Modal.style.display = "none";
}

ModalAdd.onclick = function () {
    Modal.style.display = "flex";
    ModalAddBtn.innerText = "Добавить";

    ModalId.value = "-1";
    ModalPrice.value = "";

    for (var i of ModalProduct.children) {
        i.removeAttribute("selected");
    }

    for (var i of ModalStore.children) {
        i.removeAttribute("selected");
    }

    for (var i of ModalVendor.children) {
        i.removeAttribute("selected");
    }

    for (var i of ModalStatus.children) {
        i.removeAttribute("selected");
    }
}

TBody.onclick = function () {
    if (event.target.tagName == "BUTTON") {
        if (event.target.name == "change-btn") {
            ModalId.value = event.target.parentNode.parentNode.id;
            ModalPrice.value = event.target.parentNode.parentNode.children.namedItem("price").innerText;

            for (var i of ModalProduct.children) {
                if (i.textContent == event.target.parentNode.parentNode.children.namedItem("name").innerText) {
                    i.setAttribute("selected", "selected");
                }
                else {
                    i.removeAttribute("selected");
                }
            }

            for (var i of ModalStore.children) {
                if (i.textContent == event.target.parentNode.parentNode.children.namedItem("store").innerText) {
                    i.setAttribute("selected", "selected");
                }
                else {
                    i.removeAttribute("selected");
                }
            }

            for (var i of ModalVendor.children) {
                if (i.textContent == event.target.parentNode.parentNode.children.namedItem("vendor").innerText) {
                    i.setAttribute("selected", "selected");
                }
                else {
                    i.removeAttribute("selected");
                }
            }

            for (var i of ModalStatus.children) {
                if (i.textContent == event.target.parentNode.parentNode.children.namedItem("status").innerText) {
                    i.setAttribute("selected", "selected");
                }
                else {
                    i.removeAttribute("selected");
                }
            }

            ModalAddBtn.innerText = "Изменить";
            Modal.style.display = "flex";
        }
    }
}

ModalProduct.onchange = function () {
    if (event.target.value == -2) {
        ModalProductName.style.display = "flex";
        ModalProductName.setAttribute("required", "");

        ModalProductDiscription.style.display = "flex";
        ModalProductDiscription.setAttribute("required", "");

        ModalProductCategory.style.display = "flex";
        ModalProductCategory.setAttribute("required", "");

        ModalProductImage.style.display = "flex";
        ModalProductImage.setAttribute("required", "");
    }
    else {
        ModalProductName.style.display = "none";
        ModalProductName.removeAttribute("required");

        ModalProductDiscription.style.display = "none";
        ModalProductDiscription.removeAttribute("required");

        ModalProductCategory.style.display = "none";
        ModalProductCategory.removeAttribute("required");

        ModalProductImage.style.display = "none";
        ModalProductImage.removeAttribute("required");
    }
}

ModalAddBtn.onclick = function () {
    if (ModalProduct.value == -1 || ModalStore.value == -1 || ModalVendor.value == -1 || ModalStatus.value == -1) {
        event.preventDefault();
    }
}