/*Si clicamos en el botón del sol, borrarémos la clase css dark-mode del div 
con id page y se aplicará el estilo active al sol*/
const sun = document.getElementById('id-sun');
const moon = document.getElementById('id-moon');

// Guardar un valor en el localStorage
const key = 'darkMode';  // Clave bajo la cual se almacenará el valor
const value = 'true';    // Valor que se almacenará

document.addEventListener('DOMContentLoaded', function () {
    const isDarkMode = localStorage.getItem('darkMode') === 'true';
    if (isDarkMode) {
        document.getElementById('wrapper').classList.add('dark-mode');
        moon.classList.add('active');
    } else {
        sun.classList.add('active');
    }
});

sun.onclick = function () {
    localStorage.removeItem(key, value);
    document.getElementById('wrapper').classList.remove('dark-mode')
    document.getElementById('id-moon').classList.remove('active')
    this.classList.add('active')
}
/*Si clicamos en el botón de la luna, añadiremos la clase css dark-mode del div 
con id page y se aplicará el estilo active a la luna*/
moon.onclick = function () {
    localStorage.setItem(key, value);
    document.getElementById('wrapper').classList.add('dark-mode')
    document.getElementById('id-sun').classList.remove('active')
    this.classList.add('active')
    console.log("Hola");
}
