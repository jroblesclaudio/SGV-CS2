// Función para cambiar el idioma

const en = {
    "Dashboard": "Dashboard",
    "Inventario": "Inventory",
}

const es = {
    "Dashboard": "Panel principal",
    "Inventory": "Inventario",
}

const checkbox = document.getElementById("idCheckbox");
console.log("Enlazado");


checkbox.addEventListener("change", () => {
    if (checkbox.checked) {
        console.log("Marcado");
        aEn(en);
    } else {
        console.log("No marcado");
        aEs(es);
    }
})
function aEs(lang) {
    document.getElementById('invent').textContent = lang.Inventory;
}

function aEn(lang) {
    document.getElementById('invent').textContent = lang.Inventario;
}

// Detectar el idioma preferido del usuario
//const userLanguage = navigator.language || navigator.userLanguage;