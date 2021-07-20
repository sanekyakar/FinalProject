ModalCategoryCloseBtn.onclick = function () {
    ModalCategory.style.display = "none";
}

ModalCategoryAdd.onclick = function () {
    ModalCategory.style.display = "flex";
    ModalCategoryType.value = "add";
    ModalCategoryName.value = "";
    ModalCategoryAddBtn.innerText = "Добавить";
}

CategoryTBody.onclick = function () {
    if (event.target.tagName == "BUTTON") {
        if (event.target.name == "changeCategory-btn") {
            ModalCategoryType.value = "change";
            ModalCategoryName.value = event.target.parentNode.parentNode.children.namedItem("name").innerText;
            ModalCategoryId.value = event.target.parentNode.parentNode.id;
            ModalCategoryAddBtn.innerText = "Изменить";
            ModalCategory.style.display = "flex";
        }
        if (event.target.name == "removeCategory-btn") {
            let id = ModalCategoryId.value = event.target.parentNode.parentNode.parentNode.id;
            if (!confirm("Действительно хотите удалить id =" + id + "?")) {
                event.preventDefault();
            }
        }
    }
}