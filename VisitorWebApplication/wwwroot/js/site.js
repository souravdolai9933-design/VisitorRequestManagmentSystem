const apiBaseUrl = "https://localhost:7043/api";
// get user data from current session storage
function getCurrentUser() {
    return {
        userId: localStorage.getItem("userId"),
        fullName: localStorage.getItem("fullName"),
        email: localStorage.getItem("email"),
        roleId: localStorage.getItem("roleId"),
        roleName: localStorage.getItem("roleName")
    };
}
// logout functionality clear session storage and redirect to login page
function logout() {
    localStorage.clear();
    window.location.href = "/Account/Login";
}