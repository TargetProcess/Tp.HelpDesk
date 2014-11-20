/*
Copyright (c) 2003-2009, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {
	config.enterMode = CKEDITOR.ENTER_BR
	config.language = 'en';
	config.toolbar = 'TpFull';
	config.toolbar_TpFull =
        [
            ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
            ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
            ['JustifyLeft', 'JustifyCenter', 'JustifyRight'],
            ['Link', 'Unlink', 'Anchor'],
            ['Image', 'Table', 'HorizontalRule', 'SpecialChar'],
            '/',
            ['Source', 'Format', 'Font', 'FontSize'],
            ['Replace', '-', 'RemoveFormat', 'TextColor', 'BGColor', '-', 'Templates'],
            ['Maximize']
        ];
	config.disableNativeSpellChecker = false;
	config.scayt_autoStartup = false;
	config.removePlugins = 'elementspath';
	config.resize_enabled = false;
	config.skin = 'v2';
	config.width = 710;
	config.resize_minWidth = 710;
};
