const btnDarkMode = document.querySelector('#btn-dark-mode');
function darkMode() {
    const
        body1 = document.querySelector('body'),
        haeder1 = document.querySelector('.haeder'),
        btnHome = document.querySelector('#btnHome'),
        menu_con = document.querySelector('.menu-con'),
        menu = document.querySelector('.menu'),
        btnDarkMode = document.querySelector('#btn-dark-mode')
    ;
    body1.classList.toggle("bg-night");
    haeder1.classList.toggle("haeder-drck");
    btnHome.classList.toggle("svg-drck");
    btnHome.classList.toggle("btn-dark-settings");
    menu_con.classList.toggle("menu-con-dark");
    menu_con.classList.toggle("menu-con-bs-dark");
    menu.classList.toggle("menu-dark");
    btnDarkMode.classList.toggle("svg-drck");
}
btnDarkMode.addEventListener('click',darkMode);