CKEDITOR.plugins.add('charcount', {
    	init: function (editor) {
    		var defaultLimit = 'unlimited';
    		var defaultFormat = '<span class="cke_charcount_count">%count%</span> of <span class="cke_charcount_limit">%limit%</span> characters';
    		var limit = defaultLimit;
    		var format = defaultFormat;

    		var intervalId;
    		var lastCount = 0;
    		var limitReachedNotified = false;
    		var limitRestoredNotified = false;

            CKEDITOR.document.appendStyleSheet(this.path + 'css/charcount.css');

            function counterId(editor) {
                return 'cke_charcount_' + editor.name;
            }

            function counterElement(editor) {
                return document.getElementById(counterId(editor));
            }

            function updateCounter(editor) {
                //IE
                if (!editor) return;

                var count = editor.getData().length;
                if (count == lastCount) {
                    return true;
                } else {
                    lastCount = count;
                }
                if (!limitReachedNotified && count > limit) {
                    limitReached(editor);
                } else if (!limitRestoredNotified && count <= limit) {
                    limitRestored(editor);
                }

                var html = format.replace('%count%', count).replace('%limit%', limit);
                counterElement(editor).innerHTML = html;
            }

            function limitReached(editor) {
                limitReachedNotified = true;
                limitRestoredNotified = false;

                new CKEDITOR.dom.element(counterElement(editor)).addClass('cke_charcount_error');
                editor.fire('charcount_limit_reached');
            }

            function limitRestored(editor) {
                limitRestoredNotified = true;
                limitReachedNotified = false;

                new CKEDITOR.dom.element(counterElement(editor)).removeClass('cke_charcount_error');
                editor.fire('charcount_limit_restored');
            }

            editor.on('uiSpace', function(event) {
                if (event.data.space == 'bottom') {
                    event.data.html += '<div id="' + counterId(event.editor) + '" class="cke_charcount"' +
                  ' title="' + CKEDITOR.tools.htmlEncode('Character Counter') + '"' +
                  '>&nbsp;</div>';
                }
            }, editor, null, 100);

            editor.on('instanceReady', function (event) {
                if (event.editor.config.charcount_limit != undefined) {
                    limit = event.editor.config.charcount_limit;
                }

                if (event.editor.config.charcount_format != undefined) {
                    format = event.editor.config.charcount_format;
                }
                updateCounter(event.editor);
            }, editor, null, 100);

            editor.on('dataReady', function (event) {
                if (event.editor.config.charcount_limit != undefined) {
                    limit = event.editor.config.charcount_limit;
                }

                if (event.editor.config.charcount_format != undefined) {
                    format = event.editor.config.charcount_format;
                }


                var count = event.editor.getData().length;
                if (count > limit) {
                    limitReached(event.editor);
                }
                updateCounter(event.editor);
            }, editor, null, 100);

            editor.on('key', function (event) {
                updateCounter(event.editor);
            }, editor, null, 100);
            editor.on('afterPaste', function (event) {
                updateCounter(event.editor);
            }, editor, null, 100);

            editor.on('focus', function (event) {
                intervalId = window.setInterval(function (editor) {
                    updateCounter(event.editor)
                }, 1000, event.editor);
            }, editor, null, 100);

            editor.on('blur', function (event) {
                if (intervalId)
                    clearInterval(intervalId);
            }, editor, null, 100);


            editor.on('destroy', function (event) {
                if (intervalId)
                    clearInterval(intervalId);
            }, editor, null, 100);
    	}
});
