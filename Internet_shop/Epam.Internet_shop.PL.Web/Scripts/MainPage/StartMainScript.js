window.onload = function () {
    initialize();
}

function initialize() {
    CatalogBtn.onclick = onClickCataloge;
}

function onClickCataloge() {
    if (window.getComputedStyle(NavCatologue, null).display == 'none') {
        NavCatologue.style.display = "flex";
    }
    else {
        NavCatologue.style.display = "none";
    }
}