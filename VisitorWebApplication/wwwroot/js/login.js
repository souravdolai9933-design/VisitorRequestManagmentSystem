const authApiUrl = "https://localhost:7043/api/auth";

// User Login Functionality
function loginUser() {
    const email = $("#email").val().trim();
    const password = $("#password").val().trim();

    if (email === "" || password === "") {
        alert("Please enter email and password");
        return;
    }

    const loginData = {
        email: email,
        password: password
    };

    $.ajax({
        url: authApiUrl + "/login",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(loginData),

        success: function (response) {
            if (response.success === true) {
                // after User login user data store into session storage
                localStorage.setItem("userId", response.data.userId);
                localStorage.setItem("fullName", response.data.fullName);
                localStorage.setItem("email", response.data.email);
                localStorage.setItem("roleId", response.data.roleId);
                localStorage.setItem("roleName", response.data.roleName);
                localStorage.setItem("token", response.data.token);

                alert("Login successful");

                window.location.href = "/Visitor/Index";
            }
        },

        error: function (xhr) {
            let msg = "Invalid email or password";

            if (xhr.responseJSON && xhr.responseJSON.message) {
                msg = xhr.responseJSON.message;
            }

            alert(msg);
        }
    });
}

$(document).ready(function () {
    $("#login-btn").click(function () {
        loginUser();
    });
});