/*
 * @file image paste plugin for CKEditor
    Feature introduced in: https://bugzilla.mozilla.org/show_bug.cgi?id=490879
    doesn't include images inside HTML (paste from word): https://bugzilla.mozilla.org/show_bug.cgi?id=665341
 * Copyright (C) 2011 Alfonso Mart�nez de Lizarrondo
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
 */

 // Handles image pasting in Firefox
CKEDITOR.plugins.add( 'imagepaste',
{
    init : function( editor )
    {
        CKEDITOR.document.appendStyleSheet(this.path + 'css/imagepaste.css');

        var dataURItoBlob = function(dataURI) {
           var binary = atob(dataURI.split(',')[1]);
           var array = [];
           for(var i = 0; i < binary.length; i++) {
               array.push(binary.charCodeAt(i));
           }
           return new Blob([new Uint8Array(array)], {
               type: 'image/jpeg'
           });
       };

        var doSend = function(editor, file, id){
            var url= editor.config.filebrowserImageUploadUrl + '&CKEditor=' + editor.name + '&CKEditorFuncNum=2&langCode=' + editor.langCode;
            var form = new FormData();
            form.append('upload', file, file.name || 'pasted_' + id + '.jpg');

            var xhr = new XMLHttpRequest();
            xhr.open("POST", url);

            xhr.onload = function(d){
                var imageUrl = xhr.responseText.match(/2,\s*'(.*?)',/)[1];
                var theImage = editor.document.$.querySelector("[alt=" + id + "]");
                theImage.setAttribute( 'data-cke-saved-src', imageUrl);
                theImage.setAttribute( 'src', imageUrl);
                theImage.removeAttribute( 'alt' );
            };
            xhr.send(form);
        };

        var processText = function(editor, text){
            var img = text.match( /<img src="(data:image\/([a-z]+);base64,.*?)" alt="">/);

            if (!img) return text;

            var data = img[1];
            var blob = dataURItoBlob(data);
            return processFile(editor, blob);
        };

        var processFile = function(editor, file){
            var id = CKEDITOR.tools.getNextId();

            doSend(editor, file, id);
            return '<img src="#" alt="' + id +'" />';
        };

        var onPaste = function(e, editor, sourceName){
            var event = e.data.$;
            var source = event[sourceName];

            if (!source) return;

            var items = (source.files && source.files.length) ? source.files : source.items;
            var txt = '';
            if (items && items.length) {
                for (var j = 0, c = items.length; j < c; j++ ){
                    var item = items[j];
                    if (item.type.match(/image\/([a-z]+)/)) {
                        e.data.$.preventDefault();
                        txt += processFile(editor, item.getAsFile ? item.getAsFile() : item);
                    }
                }

                if (txt) {
                    editor.fire('paste', {
                        type: 'html',
                        dataValue: txt
                    });
                }
            }
        };

        editor.on( 'instanceReady', function( e ) {
            // Chrome, insert from editor
            editor.editable().on('paste', function(e){
                onPaste(e, editor, 'clipboardData');
            });

            // Chrome, FF, drag by mouse
            editor.editable().on('drop', function(e){
                onPaste(e, editor, 'dataTransfer');
            });

            editor.on( 'mode', function( evt ) {
                if (editor.mode == 'wysiwyg' ) {
                    editor.editable().on('paste', function(e){
                        onPaste(e, editor, 'clipboardData');
                    });

                    editor.editable().on('drop', function(e){
                        onPaste(e, editor, 'dataTransfer');
                    });
                }
            });
        });

        // FF, insert from editor
        editor.on( 'paste', function(e) {
            if (e.data.type !== 'html') return;
            e.data.dataValue = processText(editor,e.data.dataValue);
        });

    } //Init
} );
