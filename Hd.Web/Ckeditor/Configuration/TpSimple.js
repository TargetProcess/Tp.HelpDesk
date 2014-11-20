/*
Copyright (c) 2003-2009, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {
	config.enterMode = CKEDITOR.ENTER_BR
	config.language = 'en';
	config.toolbar = 'TpSimple';
	config.toolbar_TpSimple =
        [
            ['Source', 'Bold', 'Italic', 'Underline', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'RemoveFormat', 'TextColor', 'BGColor'],
	        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', 'Strike', 'Link', 'Unlink', 'Anchor', 'Image', 'Maximize']
        ];
	config.disableNativeSpellChecker = false;
	config.scayt_autoStartup = false;
	config.removePlugins = 'elementspath';
	config.resize_enabled = false;
	config.skin = 'v2';
	config.width = 635;
	config.resize_minWidth = 635;
};
