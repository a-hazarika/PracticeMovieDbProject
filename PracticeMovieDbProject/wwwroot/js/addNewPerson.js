var producer = [];
var fname, mname, lname, bio, sex, dob;
var producerList;

var actors = [];
var actorCount = 0;
var afname, amname, alname, abio, asex, adob;
var actorList;

$("#producerAddedMsg").text("");
$("#actorAddedMsg").text("");

function showAddProducerDiv() {
    $("#addProducerDiv").slideDown(500);
    $("#producerAddedMsg").text("");
    console.log("Slide down");
}

function showNewActorDiv() {
    $("#addActorDiv").slideDown(500);
    $("#actorAddedMsg").text("");
    console.log("Slide down");
}

function SerializeAndStoreData(inputs) {
    if (inputs.length > 0) {
        return JSON.stringify(inputs);
    }

    return "";
}

// PRODUCER input

function hideProducerDiv() {
    producerList = SerializeAndStoreData(producer);

    $("#addProducerDiv").slideUp(500);

    var numProducers = producer.length;
    if (numProducers == 1) {
        $("#producerAddedMsg")
            .text(numProducers + " new producer added to be saved upon form submission.");
        $("#newProducerBtn").text("Change/Remove");
    }
    else {
        $("#producerAddedMsg").text("");
        $("#newProducerBtn").text("Add New");
    }

    console.log("Slide up");
}

function ClearProducerForm() {
    $("#pFName").val("");
    $("#pMName").val("");
    $("#pLName").val("");
    $("#pBio").val("");
    $("input[id='Sex']:checked").prop('checked', false);
    $("#pDob").val("");
}

function VerifyProducerInputs() {
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
    producer[0] = ({
        key: 0, firstName: fname, lastName: lname, middleName: mname, DOB: dob, gender: sex, Bio: bio
    });
}

function removeProducer() {
    producer.splice(0, 1);
    $("#tempProducer").remove();
    $("#addNewProducerBtn").prop('disabled', false);
}

function TempAddProducer() {
    if (mname != "") {
        $("#producerItem")
            .append("<li id=\"tempProducer\">" +
                "</span><i class=\"far fa-trash-alt\"></i></span>"
                + fname + " " + mname + " " + lname
                + "&nbsp;&nbsp;"
                + "<button type=\"button\" onclick=\"removeProducer()\" class=\"btn btn-danger btn-xs\">X</button>"
                + "</li>");
    }
    else {
        $("#producerItem")
            .append("<li id=\"tempProducer\">" +
                "<span><i class=\"far fa-trash-alt\"></i></span>"
                + fname + " " + lname
                + "&nbsp;&nbsp;"
                + "<button type=\"button\" onclick=\"removeProducer()\" class=\"btn btn-danger btn-xs\">X</button>"
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

    if (VerifyProducerInputs()) {
        StoreProducer();
        TempAddProducer();
        ClearProducerForm();
        $("#addNewProducerBtn").prop('disabled', true);        
    }
}

// ACTOR inputs

function hideActorDiv() {
    actorList = SerializeAndStoreData(actors);

    $("#addActorDiv").slideUp(500);

    var numActors = actors.length;
    $("#addActorBtn").text("Add/Remove");
    if (numActors == 1) {
        $("#actorAddedMsg")
            .text(numActors + " new actor added to be saved upon form submission.");
    }
    else if (numActors > 1) {
        $("#actorAddedMsg")
            .text(numActors + " new actors added to be saved upon form submission.");
    }
    else {
        $("#actorAddedMsg").text("");
        $("#addActorBtn").text("Add New");
    }

    console.log("Slide up");
}

function VerifyActorInputs() {
    if (afname == "" || alname == "" || !(asex === "Male" || asex === "Female")) {

        if (afname == "") {
            $("#aFNameReq").text("Required");
        }

        if (alname == "") {
            $("#aLNameReq").text("Required");
        }

        if (!(asex === "Male" || asex === "Female")) {
            $("#aSexReq").text("Required");
        }

        return false;
    }

    return true;
}

function StoreActor() {
    actors.push({
        key: actorCount,
        firstName: afname,
        lastName: alname,
        middleName: amname,
        DOB: adob,
        gender: asex,
        Bio: abio
    });
}

function removeActor(idVal) {
    var id = parseInt(idVal);
    if (id != NaN) {
        for (var i = 0; i <= actors.length - 1; i++) {
            if (actors[i].key == id) {
                actors.splice(i, 1);
                $("#" + "actor" + id.toString()).remove();
            }
        }
    }

    if (actors.length < 1) {
        actorCount = 0;
    }
}

function AddActorToHtmlList() {
    if (amname != "") {
        $("#actorItem")
            .append("<li id=\"actor" + actorCount + "\">" +
                "</span><i class=\"far fa-trash-alt\"></i></span>"
                + afname + " " + amname + " " + alname
                + "&nbsp;&nbsp;"
                + "<button type=\"button\" onclick=\"removeActor(" + actorCount + ")\" class=\"btn btn-danger btn-xs\">x</button>"
                + "</li>");
    }
    else {
        $("#actorItem")
            .append("<li id=\"actor" + actorCount + "\">" +
                "<span><i class=\"far fa-trash-alt\"></i></span>"
                + afname + " " + alname
                + "&nbsp;&nbsp;"
                + "<button type=\"button\" onclick=\"removeActor(" + actorCount + ")\" class=\"btn btn-danger btn-xs\">x</button>"
                + "</li>");
    }
}

function ClearActorForm() {
    $("#aFName").val("");
    $("#aMName").val("");
    $("#aLName").val("");
    $("#aBio").val("");
    $("input[id='aSex']:checked").prop('checked', false);
    $("#aDob").val("");
}

function addNewActor() {
    $("#aFNameReq").text("");
    $("#aLNameReq").text("");
    $("#aSexReq").text("");

    afname = $("#aFName").val();
    amname = $("#aMName").val();
    alname = $("#aLName").val();
    abio = $("#aBio").val();
    asex = $("input[id='aSex']:checked").val();
    adob = $("#aDob").val();

    if (VerifyActorInputs()) {
        StoreActor();
        AddActorToHtmlList();
        actorCount++;
        ClearActorForm();
    }
}