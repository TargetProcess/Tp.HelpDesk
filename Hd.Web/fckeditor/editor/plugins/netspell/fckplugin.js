/*
 * FCKeditor - The text editor for Internet - http://www.fckeditor.net
 * Copyright (C) 2003-2008 Frederico Caldeira Knabben
 *
 * == BEGIN LICENSE ==
 *
 * Licensed under the terms of any of the following licenses at your
 * choice:
 *
 *  - GNU General Public License Version 2 or later (the "GPL")
 *    http://www.gnu.org/licenses/gpl.html
 *
 *  - GNU Lesser General Public License Version 2.1 or later (the "LGPL")
 *    http://www.gnu.org/licenses/lgpl.html
 *
 *  - Mozilla Public License Version 1.1 or later (the "MPL")
 *    http://www.mozilla.org/MPL/MPL-1.1.html
 *
 * == END LICENSE ==
 *
 * This plugin register Toolbar items for the combos modifying the style to
 * not show the box.
 */

// insertHtmlObject constructor
	var netSpellCheckToolbarCommand = function()
	{
	}

	// Register the command
	FCKCommands.RegisterCommand('netSpellCheck', new netSpellCheckToolbarCommand());

	// Create the toolbar  button
	var insertNetSpellCheckButton = new FCKToolbarButton('netSpellCheck', 'Check Spelling');
	insertNetSpellCheckButton.IconPath = FCKPlugins.Items['netspell'].Path + 'spellcheck.gif';
	FCKToolbarItems.RegisterItem('netSpellCheck', insertNetSpellCheckButton);

	// manage the plugins' button behavior
	netSpellCheckToolbarCommand.prototype.GetState = function()
	{
		return FCK_TRISTATE_OFF;
	}

	// what do we do when the button is clicked
	netSpellCheckToolbarCommand.prototype.Execute = function()
	{
        checkEditorSpelling(FCK.GetHTML(), FCKPlugins.Items['netspell'].Path + 'SpellCheck.aspx', FCK);
	}