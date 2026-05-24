const initiatorApiUrl = "https://localhost:7043/api/Initiator";
const adminApiUrl = "https://localhost:7043/api/Admin";

$(document).ready(function () {
    loadVisitorGrid();
});
// Load Visitor Grid table with data from API, also handle role-based access and actions
function loadVisitorGrid() {
    const user = getCurrentUser();

    if (!user || !user.userId) {
        alert("Please login first");
        window.location.href = "/Account/Login";
        return;
    }

    // Hide add button for Admin
    if (user.roleName === "Admin") {
        $("#addRequestBtn").hide();
    }

    // Determine API URL based on role
    let gridUrl = initiatorApiUrl + "/myrequests/" + user.userId;
    if (user.roleName === "Admin") {
        gridUrl = adminApiUrl + "/all";
    }

    $("#visitorGrid").jqGrid({
        url: gridUrl,
        datatype: "json",
        loadonce: true,
        ignoreCase: true,
        mtype: "GET",

        colModel: [
            { label: "ID", name: "visitorRequestId", width: 60, key: true, search: true },
            { label: "Visitor Name", name: "visitorName", width: 140, search: true },
            { label: "Mobile", name: "mobileNumber", width: 120, search: true },
            { label: "Company", name: "companyName", width: 140, search: true },
            { label: "Person To Meet", name: "personToMeet", width: 140, search: true },
            { label: "Purpose", name: "purposeOfVisit", width: 180, search: true },
            {
                label: "Visit Date",
                name: "visitDate",
                width: 120,
                search: true,
                formatter: function (cellValue) {
                    if (!cellValue) return "";
                    return cellValue.split("T")[0];
                }
            },
            { label: "Status", name: "status", width: 100, search: true },
            { label: "Remarks", name: "remarks", width: 150, search: true },
            { label: "Initiated By", name: "createdByName", width: 120, search: true, hidden: user.roleName !== "Admin" },
            {
                
                label: "Action",
                name: "action",
                width: 210,
                search: false,
                sortable: false,
                formatter: actionFormatter
            }
            
        ],

        jsonReader: {
            repeatitems: false,
            root: function (obj) {
                return obj;
            }
        },

        pager: "#visitorPager",
        rowNum: 10,
        rowList: [10, 20, 50],
        viewrecords: true,
        height: "auto",
        autowidth: true,
        caption: user.roleName === "Admin" ? "All Visitor Access Requests" : "My Visitor Requests",

        loadComplete: function () {
            console.log("Visitor data loaded successfully");
        },

        loadError: function (xhr) {
            console.log(xhr);
            alert("Failed to load visitor data");
        }
    });

    $("#visitorGrid").jqGrid("filterToolbar", {
        searchOperators: false,
        stringResult: false,
        defaultSearch: "cn"
    });
}
// Format action buttons based on user role and request status also handle locked state for approved/rejected requests
function actionFormatter(cellValue, options, rowObject) {
    const user = getCurrentUser();
    const status = (rowObject.status || "").toLowerCase();

    if (status === "approved" || status === "rejected") {
        return `<span class="badge bg-secondary action-locked">Locked</span>`;
    }

    if (user.roleName === "Admin") {
        return `
            <div class="grid-action-buttons">
                <button type="button" class="btn-grid btn-approve"
                    onclick="approveRequest(${rowObject.visitorRequestId})">Approve</button>

                <button type="button" class="btn-grid btn-reject"
                    onclick="rejectRequest(${rowObject.visitorRequestId})">Reject</button>
            </div>
        `;
    }

    return `
        <div class="grid-action-buttons">
            <button type="button" class="btn-grid btn-edit"
                onclick="editRequest(${rowObject.visitorRequestId})">Edit</button>

            <button type="button" class="btn-grid btn-delete"
                onclick="deleteRequest(${rowObject.visitorRequestId})">Delete</button>
        </div>
    `;
}
// Handle delete action with confirmation and API call, also reload grid after successful deletion
function deleteRequest(id) {
    if (!confirm("Are you sure you want to delete this request?")) {
        return;
    }

    $.ajax({
        url: initiatorApiUrl + "/delete/" + id,
        type: "DELETE",
        success: function (response) {
            alert(response.message || "Deleted successfully");
            $("#visitorGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger("reloadGrid");
        },
        error: function (xhr) {
            let msg = "Delete failed";
            if (xhr.responseJSON && xhr.responseJSON.message) {
                msg = xhr.responseJSON.message;
            }
            alert(msg);
        }
    });
}

// Open edit modal with pre-filled data, also handle locked state for approved/rejected requests
function editRequest(id) {
    const rowData = $("#visitorGrid").jqGrid("getRowData", id);

    $("#requestModalTitle").text("Edit Visitor Request");
    $("#requestId").val(id);
    $("#visitorName").val(rowData.visitorName);
    $("#mobileNumber").val(rowData.mobileNumber);
    $("#companyName").val(rowData.companyName);
    $("#personToMeet").val(rowData.personToMeet);
    $("#purposeOfVisit").val(rowData.purposeOfVisit);
    $("#visitDate").val(rowData.visitDate);

    $("#requestModalError").addClass("d-none").text("");
    $("#requestModal").modal("show");
}
// Open add modal with empty fields and default visit date, also handle locked state for approved/rejected requests
function openAddModal() {
    $("#requestModalTitle").text("Add Visitor Request");

    $("#requestId").val("");
    $("#visitorName").val("");
    $("#mobileNumber").val("");
    $("#companyName").val("");
    $("#personToMeet").val("");
    $("#purposeOfVisit").val("");

    const today = new Date().toISOString().split("T")[0];
    $("#visitDate").val(today);

    $("#requestModalError").addClass("d-none").text("");
    $("#requestModal").modal("show");
}

function saveRequest() {
    const user = getCurrentUser();
    const id = $("#requestId").val();
    const isEdit = id !== "";

    const requestData = {
        visitorName: $("#visitorName").val().trim(),
        mobileNumber: $("#mobileNumber").val().trim(),
        companyName: $("#companyName").val().trim(),
        personToMeet: $("#personToMeet").val().trim(),
        purposeOfVisit: $("#purposeOfVisit").val().trim(),
        visitDate: $("#visitDate").val()
    };

    if (
        requestData.visitorName === "" ||
        requestData.mobileNumber === "" ||
        requestData.companyName === "" ||
        requestData.personToMeet === "" ||
        requestData.purposeOfVisit === "" ||
        requestData.visitDate === ""
    ) {
        $("#requestModalError")
            .removeClass("d-none")
            .text("Please fill all fields.");
        return;
    }

    if (isEdit) {
        requestData.visitorRequestId = parseInt(id);
        requestData.modifiedBy = parseInt(user.userId) || 1;
    } else {
        requestData.createdBy = parseInt(user.userId) || 1;
    }

    $.ajax({
        url: isEdit ? (initiatorApiUrl + "/update/" + id) : (initiatorApiUrl + "/addRequest"),
        type: isEdit ? "PUT" : "POST",
        contentType: "application/json",
        data: JSON.stringify(requestData),
        success: function (response) {
            $("#requestModal").modal("hide");
            alert(response.message || (isEdit ? "Visitor request updated successfully" : "Visitor request added successfully"));
            $("#visitorGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger("reloadGrid");
        },
        error: function (xhr) {
            let msg = "Failed to save visitor request";
            if (xhr.responseJSON && xhr.responseJSON.message) {
                msg = xhr.responseJSON.message;
            }
            $("#requestModalError")
                .removeClass("d-none")
                .text(msg);
        }
    });
}
// Handle approve action with confirmation and API call, also reload grid after successful approval
function approveRequest(id) {
    const user = getCurrentUser();
    const adminId = parseInt(user.userId) || 1;

    if (!confirm("Are you sure you want to approve this request?")) {
        return;
    }

    $.ajax({
        url: adminApiUrl + "/approve/" + id,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ visitorRequestId: id, adminId: adminId }),
        success: function (response) {
            alert(response.message || "Approved successfully");
            $("#visitorGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger("reloadGrid");
        },
        error: function (xhr) {
            let msg = "Approval failed";
            if (xhr.responseJSON && xhr.responseJSON.message) {
                msg = xhr.responseJSON.message;
            }
            alert(msg);
        }
    });
}
// Handle reject action with remarks input, confirmation and API call, also reload grid after successful rejectio
function rejectRequest(id) {
    const remarks = prompt("Enter remarks for rejection:");
    if (remarks === null) return; // User cancelled
    if (remarks.trim() === "") {
        alert("Remarks are required for rejection");
        return;
    }

    $.ajax({
        url: adminApiUrl + "/reject/" + id,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ remarks: remarks }),
        success: function (response) {
            alert(response.message || "Rejected successfully");
            $("#visitorGrid").jqGrid('setGridParam', { datatype: 'json' }).trigger("reloadGrid");
        },
        error: function (xhr) {
            let msg = "Rejection failed";
            if (xhr.responseJSON && xhr.responseJSON.message) {
                msg = xhr.responseJSON.message;
            }
            alert(msg);
        }
    });
}