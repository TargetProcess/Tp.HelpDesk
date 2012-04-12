/****************************************************
* Spell Checker Client JavaScript Code
****************************************************/

// spell checker constants
var spellURL;
var showCompleteAlert = true;
var TextToCheck;
var FCKToEdit;

var checkElements = new Array();

function getText(index)
{
    return TextToCheck;
}

function setText(index, text)
{
    window.setTimeout(function(){FCKToEdit.SetHTML(text);},250);
}

function checkEditorSpelling(Html, spellcheckurl, FCK)
{
    spellURL = spellcheckurl;
    FCKToEdit = FCK;
    checkElements = new Array();
    checkElements[checkElements.length] = FCK;
    TextToCheck = Html;
    openSpellChecker();
}

function openSpellChecker()
{
    if (window.showModalDialog)
        var result = window.showModalDialog(spellURL + "?Modal=true", window, "dialogHeight:320px; dialogWidth:400px; edge:Raised; center:Yes; help:No; resizable:No; status:No; scroll:No");
    else
        var newWindow = window.open(spellURL, "newWindow", "height=300,width=400,scrollbars=no,resizable=no,toolbars=no,status=no,menubar=no,location=no");
}


/****************************************************
* Spell Checker Suggestion Window JavaScript Code
****************************************************/
var iElementIndex = -1;
var parentWindow;

function initialize()
{
    iElementIndex = parseInt(document.getElementById("ElementIndex").value);

    if (parent.window.dialogArguments)
        parentWindow = parent.window.dialogArguments;
    else if (top.opener)
        parentWindow = top.opener;

    var spellMode = document.getElementById("SpellMode").value;

    switch (spellMode)
    {
        case "start" :
            //do nothing client side
            break;
        case "suggest" :
            //update text from parent document
            //updateText();
            //wait for input
            break;
        case "end" :
            //update text from parent document
            updateText();
            //fall through to default
            endCheck();
            break;
        default :
            //get text block from parent document
            if(loadText())
                document.SpellingForm.submit();
            else
            {
                endCheck();
            }

            break;
    }
}

function loadText()
{
    if (!parentWindow.document)
        return false;

    // check if there is any text to spell check
    for (++iElementIndex; iElementIndex < parentWindow.checkElements.length; iElementIndex++)
    {
        var newText = parentWindow.getText();
        if (newText.length > 0)
        {
			updateSettings(newText, 0, iElementIndex, "start");
			document.getElementById("StatusText").innerText = "Spell Checking Text ...";
			return true;
        }
    }

    return false;
}

function updateSettings(currentText, wordIndex, elementIndex, mode)
{
    document.getElementById("CurrentText").value = currentText;
    document.getElementById("WordIndex").value = wordIndex;
    document.getElementById("ElementIndex").value = elementIndex;
    document.getElementById("SpellMode").value = mode;
}

function updateText()
{
    if (!parentWindow.document)
        return false;

	var newText = document.getElementById("CurrentText").value;
    parentWindow.setText(iElementIndex, newText);
}

function endCheck()
{
    if (showCompleteAlert)
    	alert("Spell Check Complete");
    closeWindow();
}

function closeWindow()
{
    if (top.opener || parent.window.dialogArguments)
	   self.close();
}

function changeWord(oElement)
{
    var k = oElement.selectedIndex;
    oElement.form.ReplacementWord.value = oElement.options[k].value;
}
