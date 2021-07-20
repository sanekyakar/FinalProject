let xhr = new XMLHttpRequest();
let jsonObjectArray = new Array();


xhr.onreadystatechange = function () {
    if (xhr.readyState == 4) {
        if (xhr.status == 200) {
            if (xhr.responseText != "") {
                let jsonObject = JSON.parse(xhr.responseText);

                for (let i of jsonObject) {
                    drawUserObject(i);

                    jsonObjectArray.push(i);
                }
            }
        }
    }
}

function ResponseNextAjax() {
    xhr.open("GET", '/Handlers/UserAjaxHandler.ashx');
    xhr.send(null);
}

NextAjaxBtn.onclick = ResponseNextAjax;
ResponseNextAjax();

function drawUserObject(UserObject) {
    let Id = document.createElement("td");
    Id.setAttribute("name", "id");
    Id.innerText = UserObject.Id;

    let Login = document.createElement("td");
    Login.setAttribute("name", "login");
    Login.innerText = UserObject.Login;

    let Role = document.createElement("td");
    Role.setAttribute("name", "role");
    Role.innerText = UserObject.Role.Name;

    let Name = document.createElement("td");
    Name.setAttribute("name", "name");
    Name.innerText = UserObject.Name;

    let ChangeBtn = document.createElement("button");
    ChangeBtn.setAttribute("name", "change-btn");
    ChangeBtn.innerText = "Изменить";

    let RemoveType = document.createElement("input");
    RemoveType.setAttribute("type", "text");
    RemoveType.setAttribute("hidden", "hidden");
    RemoveType.setAttribute("name", "type");
    RemoveType.value = "remove";

    let RemoveId = document.createElement("input");
    RemoveId.setAttribute("type", "text");
    RemoveId.setAttribute("hidden", "hidden");
    RemoveId.setAttribute("name", "id");
    RemoveId.value = UserObject.Id;

    let RemoveBtn = document.createElement("button");
    RemoveBtn.setAttribute("name", "remove-btn");
    RemoveBtn.innerText = "Удалить";

    let FormRemove = document.createElement("form");
    FormRemove.setAttribute("name", "remove-form");
    FormRemove.setAttribute("action", "#");
    FormRemove.setAttribute("method", "get");
    FormRemove.appendChild(RemoveId);
    FormRemove.appendChild(RemoveType);
    FormRemove.appendChild(RemoveBtn);

    let Btns = document.createElement("td");
    Btns.setAttribute("name", "btns");
    Btns.appendChild(ChangeBtn);
    Btns.appendChild(FormRemove);
    
    let Tr = document.createElement("tr");
    Tr.setAttribute("id", UserObject.Id);
    Tr.appendChild(Id);
    Tr.appendChild(Login);
    Tr.appendChild(Role);
    Tr.appendChild(Name);
    Tr.appendChild(Btns);

    TBody.appendChild(Tr);
}


ModalCloseBtn.onclick = function () {
    Modal.style.display = "none";
}

ModalAdd.onclick = function () {
    Modal.style.display = "flex";
    ModalAddBtn.innerText = "Добавить";

    ModalType.value = "add";
    ModalId.value = "-1";
    ModalLogin.value = ""
    ModalPassword.value = ""
    ModalName.value = "";

    for (var i of ModalRole.children) {
        i.removeAttribute("selected");
    }
}

TBody.onclick = function () {
    if (event.target.tagName == "BUTTON") {
        if (event.target.name == "change-btn") {
            ModalType.value = "change";
            ModalId.value = event.target.parentNode.parentNode.id;
            ModalLogin.value = event.target.parentNode.parentNode.children.namedItem("login").innerText;
            ModalName.value = event.target.parentNode.parentNode.children.namedItem("name").innerText;

            for (let Obj of jsonObjectArray) {
                if (Obj.Id == ModalId.value) {
                    ModalPassword.value = Obj.Password;
                }
            }

            for (let i of ModalRole.children) {
                if (i.textContent == event.target.parentNode.parentNode.children.namedItem("role").innerText) {
                    i.setAttribute("selected", "selected");
                }
                else {
                    i.removeAttribute("selected");
                }
            }

            ModalAddBtn.innerText = "Изменить";
            Modal.style.display = "flex";
        }
        if (event.target.name == "remove-btn") {
            let id = ModalId.value = event.target.parentNode.parentNode.parentNode.id;
            if (!confirm("Действительно хотите удалить ID = " + id + "?")) {
                event.preventDefault();
            }
        }
    }
}

ModalAddBtn.onclick = function () {
    if (ModalRole.value == -1) {
        event.preventDefault();
    }
}