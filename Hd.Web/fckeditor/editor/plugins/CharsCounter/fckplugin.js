/*
 * FCKeditor - The text editor for internet
 * Copyright (C) 2003-2004 Frederico Caldeira Knabben
 * 
 * Licensed under the terms of the GNU Lesser General Public License:
 * 		http://www.opensource.org/licenses/lgpl-license.php
 * 
 * For further information visit:
 * 		http://www.fckeditor.net/
 * 
 * File Name: fckplugin.js
 * 	This is the Chars Counter plugin definition file.
 * 
 * Version:  2.0 FC
 * Modified: 2005-05-25
 * 
 * File Authors:
 * 		Luigi Maniscalco (l.maniscalco@visioni.info)
 * FCK 2.3 Fix :
 * Tamir Mordo (tamir@tetitu.co.il)
 */
 
// Define the command.
var CharsCounterLoaded=false;
var FCKCharsCounterCommand = function( maxlength, countername )
{
	this.Name = 'CharsCounter' ;
	this.TextLength = 0 ;
	this.HTMLLength = 0 ;
	this.MaxLength = maxlength ;
	this.LeftChars = maxlength ;
	this.CounterName = countername ;
}

FCKCharsCounterCommand.prototype.Execute = function()
{
	var DOMDocument = FCK.EditorDocument ;
	// The are two diffent browser specific ways to get the text.
	// I also use a trick to count linebreaks (<br>/</p>) as one-stroke.
	if ( FCKBrowserInfo.IsIE ) {
		var HTMLText = DOMDocument.body.innerHTML ;
		var linebreaks = HTMLText.length - HTMLText.replace(/<(br|\/p)/gi,'**').length;
		this.TextLength = DOMDocument.body.innerText.length + linebreaks;
		this.HTMLLength = HTMLText.length ;
	} else {
		var r = DOMDocument.createRange() ;
		r.selectNodeContents( DOMDocument.body ) ;
		var HTMLText = r.startContainer.innerHTML ;
		var linebreaks = HTMLText.length - HTMLText.replace(/<(br|\/p)/gi,'**').length;
		this.TextLength = r.toString().length + linebreaks;
		this.HTMLLength = HTMLText.length ;
	}
	// MaxLength is optional: if undefined, LeftChars is always 1
	//this.LeftChars = this.MaxLength ? this.MaxLength - this.TextLength : 1;
	this.LeftChars = this.MaxLength ? this.MaxLength - this.HTMLLength : 1;	
	if ( this.LeftChars < 0 ) {
		// What will I do if I reached MaxLength ?
		// By now I simply force an 'Undo' command, but often this result in a big step back!
		// For example: if I type a text, once I reach the limit,
		// the whole last sentence will be removed, instead of the last char typed only.
		// You may want to update values in the counters always,
		// then you can block too long texts simply adding an external check on the counter.
        //		FCKCommands.GetCommand( 'Undo' ).Execute() ;
	    //		this.Execute() ;
	    var trimmed_max = DOMDocument.body.innerHTML.substring(0, DOMDocument.body.innerHTML.length + this.LeftChars - 1 );
	    DOMDocument.body.innerHTML = '';
	    FCK.InsertHtml(trimmed_max);

	} else {
		// Update values in the toolbar button, if defined.
		this.DOMLabel = document.getElementById( 'CharsCounterLabel' ) ;
		if ( this.DOMLabel ) {
			this.DOMLabel.innerHTML = this.TextLength + ' / ' + this.HTMLLength + ' / ' + this.LeftChars ;
		}
		// Update value in the external counter, if defined.
		this.Counter = FCK.LinkedField.form[this.CounterName];
		if ( this.Counter ) {
			this.Counter.value = this.LeftChars ;
		}
	}
}

FCKCharsCounterCommand.prototype.GetState = function()
{
	return FCK_TRISTATE_OFF ;
}

// Register the related command.
FCKCommands.RegisterCommand( 'CharsCounter', new FCKCharsCounterCommand( FCK.Config['MaxLength'], FCK.Config['CounterName'] ) ) ;

// Create the "CharsCounter" toolbar button.
var oCharsCounterItem = new FCKToolbarButton( 'CharsCounter', '<label id="CharsCounterLabel">0 / 0 / ' + FCK.Config['MaxLength'] + '</label>', 'Text length / HTML length / Left chars', FCK_TOOLBARITEM_ONLYTEXT, false, true ) ;
FCKToolbarItems.RegisterItem( 'CharsCounter', oCharsCounterItem ) ;

// Define the event handler.
function CharsCounterEventHandler()
{
FCKCommands.GetCommand( 'CharsCounter' ).Execute() ;
}




function CharsCounter_SetListeners()
{
// First time execution.
	// Attach events "OnSelectionChange" "onPaste" and "onKeyUp", this is browser specific.
	if (CharsCounterLoaded == false) // I don’t know the code so in that way I disable double event attachment
	{
		CharsCounterLoaded == true
		if (FCKBrowserInfo.IsIE) 
		{
			FCK.Events.AttachEvent( 'OnSelectionChange', CharsCounterEventHandler ) ;
			FCK.EditorDocument.attachEvent( 'onkeyup', CharsCounterEventHandler ) ;
			FCK.EditorDocument.attachEvent( 'onkeydown', CharsCounterEventHandler ) ;
		} 
		else 
		{
			FCK.Events.AttachEvent( 'OnSelectionChange', CharsCounterEventHandler ) ;
			FCK.EditorDocument.addEventListener( 'focus', CharsCounterEventHandler, true ) ;
			FCK.EditorDocument.addEventListener( 'keyup', CharsCounterEventHandler, true ) ;
			FCK.EditorDocument.addEventListener( 'keydown', CharsCounterEventHandler, true ) ;
		}
		FCKCommands.GetCommand( 'CharsCounter' ).Execute() ;	
	}	
}


function CharsCounter_CheckEditorStatus( sender, status )
{
	// check if the fckdocument is loaded and if so I can attach events to the code
	if ( status == FCK_STATUS_COMPLETE )
		CharsCounter_SetListeners() ;
}

FCK.Events.AttachEvent( 'OnStatusChange', CharsCounter_CheckEditorStatus ) ; // attach to load status event
