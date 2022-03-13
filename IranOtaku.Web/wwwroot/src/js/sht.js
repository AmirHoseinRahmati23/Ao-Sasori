const 
    btn_search = document.querySelector('.btn-search'),
    menu = document.querySelector('.menu'),
    btn_close = document.querySelector('.btn-close'),
    menu_con = document.querySelector('.menu-con')
;
function show(btn,con,close,s) {
    btn.onclick = () => {
        con.classList.toggle("d-block");
        s.classList.remove("menu-con-bs");
    }
    close.onclick = () => {
        con.classList.toggle("d-block");
        s.classList.add("menu-con-bs");
    }
}
show(btn_search,menu,btn_close,menu_con);