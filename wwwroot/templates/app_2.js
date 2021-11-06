$(function () {
    scalesArr = Object.keys(SCALES_MAP);
    tuningsArr = Object.keys(TUNING_MAP);
    scalesArrVal = Object.values(SCALES_MAP);
    tuningsArrVal = Object.values(TUNING_MAP);
    currentScale = SCALES_MAP[scalesArr[0]];
    keytone = KEYTONES_ARR[0];
    savedTones = [];
    initColorMap();
    initSelectList("#tunings", tuningsArr);
    initSelectList("#frets", FRET_WIRES_ARR);
    initSelectList("#scales", scalesArr);
    initSelectList("#key", KEYTONES_ARR);
    initGlobal();
    setBtnEvent();

    $("#frets").on('selectmenuchange', function () {
        currentFrets = this.value;
        setCookie(scalesArrVal.indexOf(currentScale), tuningsArrVal.indexOf(currentTuning), FRET_WIRES_ARR.indexOf(currentFrets), savedTones, KEYTONES_ARR.indexOf(keytone));
        updateProgram();
        initRestoreSavedTones();
        foundScales();
    });

    $("#tunings").on('selectmenuchange', function () {
        currentTuning = TUNING_MAP[this.value];
        setCookie(scalesArrVal.indexOf(currentScale), tuningsArrVal.indexOf(currentTuning), FRET_WIRES_ARR.indexOf(currentFrets), savedTones, KEYTONES_ARR.indexOf(keytone));
        updateProgram();
        initRestoreSavedTones();
        foundScales();
    });

    $("#scales").on('selectmenuchange', function () {
        currentScale = SCALES_MAP[this.value];
        savedTones = [];
        setCookie(scalesArrVal.indexOf(currentScale), tuningsArrVal.indexOf(currentTuning), FRET_WIRES_ARR.indexOf(currentFrets), savedTones, KEYTONES_ARR.indexOf(keytone));
        updateScale();
        foundScales();
    });

    $("#key").on('selectmenuchange', function () {
        keytone = this.value;
        savedTones = [];
        if (keytone !== "No keytone" && currentScale.length === 0) {
            updateKey(keytone);
            resetScale();
            updateButton(keytone, "#FFFF00");
            isClickAllowed = false;
        } else if (keytone !== "No keytone" && currentScale.length !== 0) {
            var newScale = getModifiedDictKey(SCALES_MAP, currentScale, keytone);
            currentScale = SCALES_MAP[newScale];
            updateKey(keytone);
            updateScale();
            $('#scales').val(newScale);
            $('#scales').selectmenu("refresh");
        } else if (keytone === "No keytone" && currentScale.length === 0) {
            initScalesStart();
            resetScale();
            isClickAllowed = true;
            initScalesEnd();
        } else {
            initScalesStart();
            initScalesEnd();
        }
        foundScales();
        setCookie(scalesArrVal.indexOf(currentScale), tuningsArrVal.indexOf(currentTuning), FRET_WIRES_ARR.indexOf(currentFrets), savedTones, KEYTONES_ARR.indexOf(keytone));
    });

    $(document).ready(function () {
        var du = 1500;
        $(document).tooltip({
            show: {
                effect: 'slideDown'
            },
            track: true,
            open: function (event, ui) {
                setTimeout(function () {
                    $(ui.tooltip).hide();
                }, du);
            }
        });
    });
    $('#info').html('<p></p><font size="3"><strong>Source: </strong><a href="https://github.com/nuggetNascher/GuitarHelperWebVS"><i>https://github.com/nuggetNascher/GuitarHelperWebVS</i></a><br><strong>Info: </strong><i>This site uses cookies and all information is without guarantee of correctness and completeness.</i><br><strong>Contact: </strong><i>vpick87@gmail.com</i></font>');
});


function foundScales() {
    $('#found').html("");
    if (savedTones !== undefined && savedTones.length >= 3 && savedTones.length <= 7 && isClickAllowed) {
        var foundScales = getfoundScales(savedTones, false);
        foundScales = foundScales.replace(", end", "").replace("end", "");
        if (foundScales !== "") {
            $('#found').html('<font size="3"><strong>Tones included in:</strong><br>' + foundScales + "</font>");
        }
    } else {
        $('#found').html("");
    }
}

function getfoundScales(list, isFromScale) {
    var retVal = "";
    var foundKeys = [];
    var br = 5;
    if (isFromScale) {
        br++;
    }
    for (var key in SCALES_MAP) {
        var cnt = 0;
        var tmpScale = SCALES_MAP[key];
        for (var i = 0; i < list.length; i++) {
            for (var j = 0; j < tmpScale.length; j++) {
                if (list[i] === tmpScale[j]) {
                    cnt++;
                }
            }
        }
        if (cnt === list.length && !foundKeys.includes(key)) {
            if (foundKeys.length >= br) {
                br += 5;
                retVal += "<br>";
            }
            if (isFromScale && tmpScale.length === list.length && list !== tmpScale) {
                retVal += key + ", ";
                foundKeys.push(key);
            } else if (!isFromScale) {
                retVal += key + ", ";
                foundKeys.push(key);
            }
        }
    }
    return retVal + "end";
}

function isContains(note, myArray) {
    var retVal = false;
    for (var i = 0; i < myArray.length; i++) {
        if (myArray[i] === note) {
            retVal = true;
            break;
        }
    }
    return retVal;
}

function getDictKey(dict, val) {
    var retVal = "";
    for (var key in dict) {
        if (dict[key] === val) {
            retVal = key;
            break;
        }
    }
    return retVal;
}

function getModifiedDictKey(dict, val, keyTone) {
    var retVal = getDictKey(dict, val);
    if (retVal.charAt(1) === " ") {
        retVal = retVal.replace(retVal.charAt(0), keyTone);
    } else {
        retVal = retVal.replace(getNewKeyTone(retVal), keyTone);
    }
    return retVal;
}

function getNewKeyTone(retVal) {
    var newRetVal = "";
    for (var i = 0; i < MAX_NOTE_LEN; i++) {
        newRetVal += retVal.charAt(i);
    }
    return newRetVal;
}

function setMatchingChords(scaleIndex) {
    var msg = "";
    var steps = "";
    var name = "";

    if (scaleIndex > 0) {

        if (scalesArr[scaleIndex].includes("Chord")) {
            name = scalesArr[scaleIndex];

            if (name.includes("Major") | name.includes("Minor") && !name.includes("7") && !name.includes("9") && !name.includes("6") && !name.includes("Slash")) {
                name = name.replace("Chord", "Scale");

                if (name.includes("Major")) {
                    steps = getSteps(SCALES_MAP[name], MAJOR_STEPS_DICT, name);
                } else if (name.includes("Minor")) {
                    steps = getSteps(SCALES_MAP[name], MINOR_STEPS_DICT, name);
                }

            } else {
                steps = getfoundChords(SCALES_MAP[name], name);
            }

            if (steps.length > 4) {
                msg = '<font size="3"><strong>Possible Backingtrack content:</strong><br>' + steps + "</font >";
            }

        } else {
            name = scalesArr[scaleIndex];
            var scale = SCALES_MAP[name];
            var foundScale = "<strong>[</strong> " + getfoundScales(scale, true).replace("<br>end", "end") + "<strong>]</strong>";
            foundScale = foundScale.replace(", end<strong>]</strong>", "<strong> ]</strong>");
            foundScale = foundScale.replace("<strong>[</strong> end<strong>]</strong>", "");

            if (name.includes("Pentatonic")) {
                name = name.replace("Pentatonic", "Scale");
                scale = SCALES_MAP[name];
            }

            steps = getfoundChords(scale, name);
            msg = '<font size="3"><strong>Possible Backingtrack content:</strong><br>' + steps + foundScale + "</font >";
        }
    } else {
        msg = "";
    }
    $('#matchingChords').html(msg);
}

function getSteps(scale, stepsDict, name) {
    var retVal = "";
    if (Object.keys(stepsDict).length === 7) {
        if (scale.length === 7) {
            for (var i = 0; i < 7; i++) {
                retVal += '<strong>' + STEPS[i] + ':</strong> ' + scale[i] + ' ' + stepsDict[STEPS[i]] + ' ';
            }
        }
        return retVal;
    } else {
        return "";
    }
}

function getStepStr(scale, name, steps) {
    if (steps === "") {
        return getfoundChords(scale, name);
    } else {
        return steps + "<br>" + getfoundChords(scale, name);
    }
}

function getfoundChords(list) {
    var retVal = "";
    var foundKeys = [];
    var cnt = 0;
    for (var key in SCALES_MAP) {
        if (key.includes("Chord")) {
            cnt = 0;
            var tmpChord = SCALES_MAP[key];
            for (var i = 0; i < list.length; i++) {
                for (var j = 0; j < list.length; j++) {
                    if (list[i] === tmpChord[j]) {
                        cnt++;
                    }
                }
            }
            if (cnt === tmpChord.length && !foundKeys.includes(key) && list !== tmpChord) {
                foundKeys.push(key.replace("Chord", ""));
            }
        }
    }
    for (var i = 0; i < list.length; i++) {
        var isMatch = false;
        if (list.length === 7) {
            retVal += '<strong>' + STEPS[i] + ':</strong> ';
        }
        for (var k = 0; k < foundKeys.length; k++) {
            if (list[i].length === 1) {
                if (foundKeys[k].charAt(0) === list[i] && foundKeys[k].charAt(1) === " ") {
                    retVal += foundKeys[k] + " <strong>|</strong> ";
                    isMatch = true;
                }
            } else {
                if (foundKeys[k].includes(list[i])) {
                    retVal += foundKeys[k] + " <strong>|</strong> ";
                    isMatch = true;
                }
            }
        }
        if (isMatch) {
            retVal += "<br>";
        }
        retVal = retVal.replace("<br><br>", "<br>");
        retVal = retVal.replace(" <strong>|</strong> <br>", "<br>");
    }
    retVal += "*end*";
    retVal = retVal.replace("<br>*end*", "*end*");
    return retVal.replace("*end*", "<br>");
}

function setBtnEvent() {
    for (var i = 0; i < btnIdArr.length; i++) {
        document.getElementById(btnIdArr[i]).onclick = clickButton;
    }
}

function setCookie(scaleIndex, tuningsIndex, fretsIndex, savedTones, keytoneIndex) {
    document.cookie = "";
    var sb = [];
    sb[0] = scaleIndex;
    sb[1] = tuningsIndex;
    sb[2] = fretsIndex;
    sb[3] = keytoneIndex;
    var index = 4;
    if (savedTones !== undefined && savedTones.length > 0) {
        for (var i = 0; i < savedTones.length; i++) {
            sb[index] = savedTones[i];
            index++;
        }
    }
    document.cookie = sb.join().toString();
    setMatchingChords(scaleIndex);
}

function setColor(btnId, note) {
    var len = currentScale.length;
    switch (len) {
        case 2:
            document.getElementById(btnId).style.backgroundColor = CHORD5_COLOR_ARR[currentScale.indexOf(note)];
            break;
        case 3:
            document.getElementById(btnId).style.backgroundColor = CHORD_COLOR_ARR[currentScale.indexOf(note)];
            break;
        case 4:
            document.getElementById(btnId).style.backgroundColor = CHORD7_COLOR_ARR[currentScale.indexOf(note)];
            break;
        case 5:
            document.getElementById(btnId).style.backgroundColor = PENTA_COLOR_ARR[currentScale.indexOf(note)];
            break;
        case 6:
            document.getElementById(btnId).style.backgroundColor = BLUES_COLOR_ARR[currentScale.indexOf(note)];
            break;
        case 7:
            document.getElementById(btnId).style.backgroundColor = SCALE_COLOR_ARR[currentScale.indexOf(note)];
            break;
        case 12:
            document.getElementById(btnId).style.backgroundColor = TONE_COLOR_ARR[currentScale.indexOf(note)];
            break;
        default:
            document.getElementById(btnId).style.backgroundColor = TONE_COLOR_ARR[currentScale.indexOf(note)];
    }
}

function initScalesStart() {
    $('#scales').find('option').remove().end();
    $('#scales').selectmenu('destroy').selectmenu({ style: 'dropdown' });
    initSelectList("#scales", scalesArr);
}

function initScalesEnd() {
    $('#scales').val(scalesArr[scalesArrVal.indexOf(currentScale)]);
    $('#scales').selectmenu("refresh");
}

function initScales(tone) {
    $('#scales').find('option').remove().end();
    $('#scales').selectmenu('destroy').selectmenu({ style: 'dropdown' });
    $("#scales").append($("<option></option>").attr("value", scalesArr[0]).text(scalesArr[0]));
    for (var i = 1; i < scalesArr.length; i++) {
        if (tone.length === 1) {
            if (tone === scalesArr[i].charAt(0) && scalesArr[i].charAt(1) === " ") {
                $("#scales").append($("<option></option>").attr("value", scalesArr[i]).text(scalesArr[i]));
            }
        } else if (tone.length === MAX_NOTE_LEN) {
            if (tone === getNewKeyTone(scalesArr[i])) {
                $("#scales").append($("<option></option>").attr("value", scalesArr[i]).text(scalesArr[i]));
            }
        }
    }
    $('#scales').selectmenu().selectmenu("menuWidget").addClass("overflow");
    $('#scales').selectmenu();
    $('#scales').selectmenu("refresh");
}

function initGlobal() {
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookies = decodedCookie.split(',');
    if (cookies !== undefined && cookies.length === 4) {
        initGUISearchPanels(Number(cookies[0]), Number(cookies[1]), Number(cookies[2]), Number(cookies[3]));
        initDefault();
        initWithKeytone(Number(cookies[0]), Number(cookies[3]));
        initWithScale(Number(cookies[0]));
    } else if (cookies !== undefined && cookies.length > 4) {
        initGUISearchPanels(0, Number(cookies[1]), Number(cookies[2]), Number(cookies[3]));
        initDefault();
        initSavedToneFromCookie(cookies);
        initRestoreSavedTones();
        foundScales();
    } else {
        initGUISearchPanels(0, 3, 0, 0);
        initDefault();
    }
}

function initWithKeytone(scaleIndex, keytoneIndex) {
    if (keytoneIndex !== 0) {
        initScales(KEYTONES_ARR[keytoneIndex]);
        $('#scales').val(scalesArr[scaleIndex]);
        $('#scales').selectmenu("refresh");
        if (scaleIndex === 0) {
            updateButton(keytone, "#FFFF00");
        }
    }
}

function initRestoreSavedTones() {
    if (savedTones.length > 0) {
        for (var i = 0; i < savedTones.length; i++) {
            for (var j = 0; j < btnIdArr.length; j++) {
                if (document.getElementById(btnIdArr[j]).value === savedTones[i]) {
                    document.getElementById(btnIdArr[j]).style.backgroundColor = colorMap[savedTones[i]];
                }
            }
        }
    }
}

function initSelectList(element, arr) {
    for (var i = 0; i < arr.length; i++) {
        $(element).append($("<option></option>").attr("value", arr[i]).text(arr[i]));
    }
    $(element).selectmenu().selectmenu("menuWidget").addClass("overflow");
    $(element).selectmenu();
}

function initColorMap() {
    for (var i = 0; i < TONES_ARR.length; i++) {
        colorMap[TONES_ARR[i]] = TONE_COLOR_ARR[i];
    }
}

function initGUISearchPanels(scaleIndex, tuningsIndex, fretsIndex, keytoneIndex) {
    $('#scales').val(scalesArr[scaleIndex]);
    $('#tunings').val(tuningsArr[tuningsIndex]);
    $('#frets').val(FRET_WIRES_ARR[fretsIndex]);
    $('#key').val(KEYTONES_ARR[keytoneIndex]);
    $('#scales').selectmenu("refresh");
    $('#tunings').selectmenu("refresh");
    $('#frets').selectmenu("refresh");
    $('#key').selectmenu("refresh");
    currentTuning = TUNING_MAP[tuningsArr[tuningsIndex]];
    currentFrets = FRET_WIRES_ARR[fretsIndex];
    currentScale = SCALES_MAP[scalesArr[scaleIndex]];
    keytone = KEYTONES_ARR[keytoneIndex];
    setMatchingChords(scaleIndex);
}

function initSavedToneFromCookie(cookies) {
    for (var i = 4; i < cookies.length; i++) {
        savedTones.push(cookies[i]);
    }
}

function initDefault() {
    initFretBoard(TONES_ARR, currentTuning, currentFrets);
    createFretBoard();
    checkButtons();
    checkUserInfo();
    updateLegendBtn();
}

function initFretBoard(tones, tuning, frets) {
    var strings = tuning.length;
    frets++;
    var fretboard = initFretBoardMatrix();
    for (var i = 0; i < MAX_STR; i++) {
        if (i < strings) {
            fretboard[i][0] = tuning[i];
        }
        var startToneIndex = tones.indexOf(fretboard[i][0]);
        startToneIndex++;
        for (var j = 1; j < MAX_FRETS; j++) {
            if (startToneIndex >= 12) {
                startToneIndex = 0;
            }
            if (i < strings && j < frets) {
                fretboard[i][j] = tones[startToneIndex];
            } else {
                fretboard[i][j] = "";
            }
            startToneIndex++;
        }
    }
    fretBoard = fretboard;
}

function initFretBoardMatrix() {
    var arr = [];
    for (var i = 0; i < MAX_STR; i++) {
        arr[i] = [];
        for (var j = 0; j < MAX_FRETS; j++) {
            arr[i][j] = "";
        }
    }
    return arr;
}

function initScale() {
    for (var i = 0; i < btnIdArr.length; i++) {
        var note = document.getElementById(btnIdArr[i]).value;
        if (isContains(note, currentScale) && !document.getElementById(btnIdArr[i]).hidden) {
            setColor(btnIdArr[i], note);
        }
    }
}

function initWithScale(scaleIndex) {
    if (scaleIndex > 0) {
        isClickAllowed = false;
        savedTones = [];
        updateScale();
    }
}

function updateKey(tone) {
    initScales(tone);
}

function updateButton(target, color) {
    for (var i = 0; i < btnIdArr.length; i++) {
        var note = document.getElementById(btnIdArr[i]).value;
        if (note === target) {
            if (document.getElementById(btnIdArr[i]).style.backgroundColor === DEFAULT_RGB) {
                if (color.length === 0) {
                    document.getElementById(btnIdArr[i]).style.backgroundColor = colorMap[note];
                } else {
                    document.getElementById(btnIdArr[i]).style.backgroundColor = color;
                }
            } else {
                document.getElementById(btnIdArr[i]).style.backgroundColor = DEFAULT_GRAY;
            }
        }
    }
}

function updateSavedTones(target) {
    if (!isContains(target, savedTones)) {
        savedTones.push(target);
    } else if (isContains(target, savedTones)) {
        savedTones.splice(savedTones.indexOf(target), 1);
    }
}

function updateProgram() {
    initFretBoard(TONES_ARR, currentTuning, currentFrets);
    updateButtons();
    checkButtons();
    checkUserInfo();
    updateScale();
}

function updateScale() {
    if (currentScale.length === 0 && keytone === "No keytone") {
        resetScale();
        hideLegendBtn();
        isClickAllowed = true;
    } else if (currentScale.length === 0 && keytone !== "No keytone") {
        resetScale();
        hideLegendBtn();
        updateButton(keytone, "#FFFF00");
        isClickAllowed = false;
    } else {
        isClickAllowed = false;
        resetScale();
        initScale();
        updateLegendBtn();
    }
}

function updateButtons() {
    var btnId = 0;
    for (var i = 0; i < MAX_STR; i++) {
        for (var j = 0; j < MAX_FRETS + 1; j++) {
            if (j > 0) {
                document.getElementById(btnIdArr[btnId]).value = fretBoard[i][j - 1];
            }
            btnId++;
        }
    }
}

function updateLegendBtn() {
    var len = currentScale.length;
    if (len > 0) {
        document.getElementById("legendButtons").hidden = false;
        for (var i = 0; i < 7; i++) {
            var btn = "buttonLegend".concat("", i);
            if (i < len) {
                document.getElementById(btn).hidden = false;
                document.getElementById(btn).value = currentScale[i];
                setColor(btn, currentScale[i]);
            } else {
                document.getElementById(btn).hidden = true;
                document.getElementById(btn).style.backgroundColor = DEFAULT_GRAY;
                document.getElementById(btn).value = "";
            }
        }
    } else {
        hideLegendBtn();
    }
}

function resetScale() {
    for (var i = 0; i < btnIdArr.length; i++) {
        if (!document.getElementById(btnIdArr[i]).disabled) {
            document.getElementById(btnIdArr[i]).style.backgroundColor = DEFAULT_GRAY;
        }
    }
}

function resetFretboard() {
    document.cookie = "";
    location.reload(true);
}

function clearFretboard() {
    initScalesStart();
    initScalesEnd();
    resetScale();
    hideLegendBtn();
    isClickAllowed = true;
    currentScale = SCALES_MAP[scalesArr[0]];
    keytone = KEYTONES_ARR[0];
    $('#scales').val(scalesArr[0]);
    $('#scales').selectmenu("refresh");
    $('#key').val("No keytone");
    $('#key').selectmenu("refresh");
    savedTones = [];
    $('#matchingChords').html("");
    $('#found').html("");
    setCookie(0, tuningsArrVal.indexOf(currentTuning), FRET_WIRES_ARR.indexOf(currentFrets), savedTones, KEYTONES_ARR.indexOf(keytone));
}

function checkButtons() {
    for (var i = 0; i < btnIdArr.length; i++) {
        if (document.getElementById(btnIdArr[i]).value === "") {
            document.getElementById(btnIdArr[i]).hidden = true;
        } else {
            document.getElementById(btnIdArr[i]).hidden = false;
        }
    }
}

function checkUserInfo() {
    enablerStrBtn();
    enablerFretBtn();
}

function clickButton(e) {
    e = e || window.event;
    var target = e.target.value;
    if (isClickAllowed) {
        updateSavedTones(target);
        updateButton(target, "");
    }
    setCookie(scalesArrVal.indexOf(currentScale), tuningsArrVal.indexOf(currentTuning), FRET_WIRES_ARR.indexOf(currentFrets), savedTones, KEYTONES_ARR.indexOf(keytone));
    foundScales();
}

function createFretBoard() {
    var btnId = 0;
    createFretBtn();
    createLegendBtn();
    disablerFretBtn();
    hideLegendBtn();
    for (var i = 0; i < MAX_STR; i++) {
        $('#pageButtons').append('<div>');
        btnId = createNoteBtnRow(i, btnId);
        $('#pageButtons').append('</div>');
    }
    disablerStrBtn();
}

function createNoteBtnRow(row, btnId) {
    for (var i = 0; i < MAX_FRETS + 1; i++) {
        if (i === 0) {
            $('#pageButtons').append(
                '<input type="button" tabindex="-1" style="font-weight: bold;border: none;background-color: Transparent;' +
                'text-align:center;height:30px;width:50x;border-style:none;margin-bottom:2px;margin-right:10px;" id="button' + btnId + '" value="' + (row + 1) + ' -' + '"/>'
            );
            strBtnId.push("button".concat("", btnId));
        } else {
            $('#pageButtons').append(
                '<input type="button" tabindex="-1" style="font-weight: bold;text-align:center;height:30px;width:50px;' +
                'margin-bottom:2px;margin-right:2px;border-style:solid;border-width:1px;border-color:#C5C5C5;background-color:#F6F6F6;" id="button' + btnId + '" value="' + fretBoard[row][i - 1] + '"/>'
            );
        }
        btnIdArr.push("button".concat("", btnId));
        btnId++;
    }
    return btnId;
}

function createLegendBtn() {
    $('#legendButtons').append("<strong>Sequence: </strong>");
    for (var i = 0; i < 7; i++) {
        $('#legendButtons').append('<input type="button" tabindex="-1" style="font-weight: bold;border:none;' +
            'text-align:center;height:30px;width:50px;border-style:none;" id="buttonLegend' + i + '" value="' + i + '"/>');
    }
}

function createFretBtn() {
    for (var i = 0; i < MAX_FRETS + 1; i++) {
        $('#fretsButtons').append('<input type="button" tabindex="-1" style="font-weight: bold;border: none;' +
            'background-color: Transparent;text-align:center;height:30px;width:50px;border-style:none;margin-bottom:2px;' +
            'margin-right: 2px;" id="buttonFret' + i + '" value="' + (i - 1) + '"/>');
    }
}

function hideLegendBtn() {
    for (var i = 0; i < 7; i++) {
        var btn = "buttonLegend".concat("", i);
        document.getElementById(btn).hidden = true;
        document.getElementById(btn).style.backgroundColor = DEFAULT_GRAY;
        document.getElementById(btn).value = "";
        document.getElementById("legendButtons").hidden = true;
    }
}

function disablerFretBtn() {
    for (var i = 0; i < MAX_FRETS + 1; i++) {
        document.getElementById("buttonFret".concat("", i)).disabled = true;
        if (i - 1 === -1) {
            document.getElementById("buttonFret".concat("", i)).value = "";
        }
    }
}

function disablerStrBtn() {
    for (var i = 0; i < strBtnId.length; i++) {
        document.getElementById(strBtnId[i]).disabled = true;
    }
}

function enablerStrBtn() {
    for (var i = 0; i < strBtnId.length; i++) {
        if (i < currentTuning.length) {
            document.getElementById(strBtnId[i]).hidden = false;
        } else {
            document.getElementById(strBtnId[i]).hidden = true;
        }
    }
}

function enablerFretBtn() {
    var end = currentFrets;
    end++;
    for (var i = 0; i < MAX_FRETS + 1; i++) {
        if (i <= end) {
            document.getElementById("buttonFret".concat("", i)).hidden = false;
        } else {
            document.getElementById("buttonFret".concat("", i)).hidden = true;
        }
    }
}