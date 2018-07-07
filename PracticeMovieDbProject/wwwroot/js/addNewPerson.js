var producers = [];
var producerCount = 0;
var fname, mname, lname, bio, sex, dob;
var producerList;

$("#producerAddedMsg").text("");

function showNewProducer() {
    $("#addProducerDiv").slideDown(500);
    $("#producerAddedMsg").text("");
    console.log("Slide down");
}

function SerializeAndStoreData() {
    if (producers.length > 0) {
        producerList = JSON.stringify(producers);
    }    
}

function hideProducerDiv() {
    SerializeAndStoreData();

    $("#addProducerDiv").slideUp(500);

    var numProducers = producers.length;
    if (numProducers == 1) {
        $("#producerAddedMsg")
            .text(numProducers + " new actor added to be saved upon form submission.");
    }
    else if (numProducers == 1) {
        $("#producerAddedMsg")
            .text(numProducers + " new actors added to be saved upon form submission.");
    }
    else {
        $("#producerAddedMsg").text("");
    }
    
    console.log("Slide up");
}

function ClearForm() {
    $("#pFName").val("");
    $("#pMName").val("");
    $("#pLName").val("");
    $("#pBio").val("");
    $("input[id='Sex']:checked").prop('checked', false);
    $("#pDob").val("");
}

function VerifyInputs() {
    if (fname == "" || lname == "" || !(sex === "Male" || sex === "Female")) {

        if (fname == "") {
            $("#pFNameReq").text("Required");
        }

        if (lname == "") {
            $("#pLNameReq").text("Required");
        }

        if (!(sex === "Male" || sex === "Female")) {
            $("#pSexReq").text("Required");
        }

        return false;
    }

    return true;
}

function StoreProducer() {
    producers.push({
        key: producerCount, firstName: fname, lastName: lname, middleName: mname, DOB: dob, gender: sex, Bio: bio
    });
}

function removeProducer(idVal) {
    var id = parseInt(idVal);
    if (id != NaN) {
        for (var i = 0; i <= producers.length - 1; i++) {
            if (producers[i].key == id) {
                producers.splice(i, 1);
                $("#" + id.toString()).remove();
            }
        }
    }

    if (producers.length < 1) {
        producerCount = 0;
    }
}

function AddNewProducerToList() {
    if (mname != "") {
        $("#producerItem")
            .append("<li id=\"" + producerCount + "\">" +
                "</span><i class=\"far fa-trash-alt\"></i></span>"
                + fname + " " + mname + " " + lname
                + "&nbsp;&nbsp;"
                + "<button type=\"button\" onclick=\"removeProducer(" + producerCount + ")\" class=\"btn btn-danger btn-xs\">X</button>"
                + "</li>");
    }
    else {
        $("#producerItem")
            .append("<li id=\"" + producerCount + "\">" +
                "<span><i class=\"far fa-trash-alt\"></i></span>"
                + fname + " " + lname
                + "&nbsp;&nbsp;"
                + "<button type=\"button\" onclick=\"removeProducer(" + producerCount + ")\" class=\"btn btn-danger btn-xs\">X</button>"
                + "</li>");
    }
}

function addNewProducer() {
    $("#pFNameReq").text("");
    $("#pLNameReq").text("");
    $("#pSexReq").text("");

    fname = $("#pFName").val();
    mname = $("#pMName").val();
    lname = $("#pLName").val();
    bio = $("#pBio").val();
    sex = $("input[id='Sex']:checked").val();
    dob = $("#pDob").val();

    if (VerifyInputs()) {

        StoreProducer();

        AddNewProducerToList()

        producerCount++;

        ClearForm();
    }
}