/*
Copyright (c) 2003-2009, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {
	config.enterMode = CKEDITOR.ENTER_BR
	config.language = 'en';
	config.toolbar = 'TpSimpleWithTables';
	config.toolbar_TpSimpleWithTables =
        [
            ['Source', 'Bold', 'Italic', 'Underline', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'RemoveFormat', 'TextColor', 'BGColor'],
	        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', 'Strike', '-', "Table", 'Image', 'Maximize', 'Link', 'Unlink']
        ];
	config.disableNativeSpellChecker = false;
	config.scayt_autoStartup = false;
	config.removePlugins = 'elementspath';
	config.resize_enabled = false;
	config.skin = 'v2';
	config.width = 610;
	config.resize_minWidth = 610;
};
