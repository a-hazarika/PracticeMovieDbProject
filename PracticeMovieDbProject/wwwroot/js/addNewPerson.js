var producer = [];
var fname, mname, lname, bio, sex, dob;
var producerList;

var actors = [];
var actorCount = 0;
var afname, amname, alname, abio, asex, adob;
var actorList;

$("#producerAddedMsg").text("");
$("#actorAddedMsg").text("");

$(document).ready(function () {
    UpdateCurrentManuallyAddedProducerOnPageReload();
    UpdateCurrentManuallyAddedActorsOnPageReload();
});


function showAddProducerDiv() {
    $("#producerPlaceHolder").append($("#addProducerDiv"));
    $("#addProducerDiv").slideDown(300);
    $("#producerAddedMsg").text("");

    console.log("Slide down");
}

function showNewActorDiv() {
    $("#actorPlaceHolder").append($("#addActorDiv"));
    $("#addActorDiv").slideDown(300);
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
function UpdateCurrentManuallyAddedProducerOnPageReload() {
    var producerDetails = $("#newProducerValue").val();

    if (producerDetails == "") {
        return;
    }

    producer = JSON.parse(producerDetails);
    if (producer.length > 0) {

        fname = producer[0].FirstName;
        lname = producer[0].LastName;
        mname = producer[0].MiddleName;
        dob = producer[0].DOB;
        sex = producer[0].Sex;
        bio = producer[0].Bio;

        if (VerifyProducerInputs()) {
            TempAddProducer();
            UpdateNewProducerDisplayDivVisibility();
            ClearProducerForm();
            $("#addNewProducerBtn").prop('disabled', true);
            hideProducerDiv();
        }
    }
}

function update_newProducerValueDiv() {
    producerList = SerializeAndStoreData(producer);
    $("#newProducerValue").val(producerList);
}

function UpdateNewProducerDisplayDivVisibility() {
    if (producer.length > 0) {
        $("#newProducerDisplay").slideDown(300);
    }
    else {
        $("#newProducerDisplay").slideUp(300);
    }
}

function hideProducerDiv() {
    $("#addProducerDiv").slideUp(300);
    $("#producerPlaceHolder addProducerDiv").remove();
    var numProducers = producer.length;
    if (numProducers == 1) {
        $("#producerAddedMsg")
            .text(numProducers + " new producer added to be saved upon form submission.");
        $("#newProducerBtn").text("Change/Remove");
        $("#producer").prop("disabled", true);
        $("#Producers").text("");
    }
    else {
        $("#producerAddedMsg").text("");
        $("#newProducerBtn").text("Add New");
        $("#producer").prop("disabled", false);
    }

    console.log("Slide up");
}

function ClearProducerForm() {
    $("#pFName").val("");
    $("#pMName").val("");
    $("#pLName").val("");
    $("#pBio").val("");
    $("input[id='sex']:checked").prop('checked', false);
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
        Key: 0,
        FirstName: fname,
        LastName: lname,
        MiddleName: mname,
        DOB: dob,
        Sex: sex,
        Bio: bio
    });
}

function removeProducer() {
    producer.splice(0, 1);
    $("#tempProducer").remove();
    $("#addNewProducerBtn").prop('disabled', false);
    UpdateNewProducerDisplayDivVisibility();
    update_newProducerValueDiv();
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
    sex = $("input[id='sex']:checked").val();
    dob = $("#pDob").val() == "" ? "0001-01-01" : $("#pDob").val();

    if (VerifyProducerInputs()) {
        StoreProducer();
        TempAddProducer();
        UpdateNewProducerDisplayDivVisibility();
        ClearProducerForm();
        update_newProducerValueDiv();
        $("#addNewProducerBtn").prop('disabled', true);
    }
}

// ACTOR inputs

function UpdateCurrentManuallyAddedActorsOnPageReload() {
    actorList = $("#newActorValue").val();

    if (actorList == "") {
        return;
    }

    actors = JSON.parse(actorList);
    if (actors.length > 0) {

        var i = 0;
        for (i = 0; i < actors.length; i++) {
            actors[i].Key = i;
            actorCount = i;
            afname = actors[i].FirstName;
            alname = actors[i].LastName;
            amname = actors[i].MiddleName;
            adob = actors[i].DOB;
            asex = actors[i].Sex;
            abio = actors[i].Bio;

            if (VerifyActorInputs()) {
                AddActorToHtmlList();
                UpdateNewActorsDisplayDivVisibility();
                ClearActorForm();
                hideActorDiv();
            }
        }

        actorCount = i;
    }
}

function update_newActorValueDiv() {
    actorList = SerializeAndStoreData(actors);
    $("#newActorValue").val(actorList);
}

function UpdateNewActorsDisplayDivVisibility() {
    if (actors.length > 0) {
        $("#newActorsDisplay").slideDown(300);
    }
    else {
        $("#newActorsDisplay").slideUp(300);
    }
}

function hideActorDiv() {
    actorList = SerializeAndStoreData(actors);

    $("#addActorDiv").slideUp(300);
    $("#actorPlaceHolder addActorDiv").remove();

    var numActors = actors.length;
    $("#addActorBtn").text("Add/Remove");
    if (numActors == 1) {
        $("#actorAddedMsg")
            .text(numActors + " new actor added to be saved upon form submission.");
        $("#MovieActors").text("");
    }
    else if (numActors > 1) {
        $("#actorAddedMsg")
            .text(numActors + " new actors added to be saved upon form submission.");
        $("#MovieActors").text("");
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
        Key: actorCount,
        FirstName: afname,
        LastName: alname,
        MiddleName: amname,
        DOB: adob,
        Sex: asex,
        Bio: abio
    });
}

function removeActor(idVal) {
    var id = parseInt(idVal);
    if (id != NaN) {
        for (var i = 0; i <= actors.length - 1; i++) {
            if (actors[i].Key == id) {
                actors.splice(i, 1);
                $("#" + "actor" + id.toString()).remove();
            }
        }
    }

    if (actors.length < 1) {
        actorCount = 0;
    }

    UpdateNewActorsDisplayDivVisibility();
    update_newActorValueDiv();
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
    adob = $("#aDob").val() == "" ? "0001-01-01" : $("#aDob").val();

    if (VerifyActorInputs()) {
        StoreActor();
        AddActorToHtmlList();
        actorCount++;
        UpdateNewActorsDisplayDivVisibility();
        update_newActorValueDiv();
        ClearActorForm();
    }
}