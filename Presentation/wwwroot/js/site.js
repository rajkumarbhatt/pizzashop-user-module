$(document).ready(function () {
    $("#logoutBtn").click(function () {
        console.log("logout");
        // delete token from cookies
        document.cookie = "token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";

        window.location.href = "/Home";
    });
});
