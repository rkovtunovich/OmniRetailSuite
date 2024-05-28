window.MudBlazorOpenMenu = (menuId) => {
    const menu = document.getElementById(menuId);
    if (menu) {
        menu.style.display = "block";
    }
};
