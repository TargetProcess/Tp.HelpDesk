/*global CKEDITOR*/
CKEDITOR.plugins.add('sourceautogrow', {
    init: function(editor) {
        editor.on('instanceReady', function(e) {
            var contentSpan = document.getElementById('cke_' + editor.name);
            var contentDiv, contentParent, computedHeight;

            var findContent = function(contentSpan) {
                contentDiv = contentDiv || contentSpan.getElementsByTagName('div')[0];
                contentParent = contentParent || contentDiv.parentNode;
            };

            var isNotMaximized = function(evt) {
                return evt.editor.getCommand('maximize').state == CKEDITOR.TRISTATE_OFF;
            };

            findContent(contentSpan);

            editor.on('beforeModeUnload', function(evt) {
                findContent(contentSpan);
                if (evt.editor.mode == 'wysiwyg' && isNotMaximized(evt)) {
                    computedHeight = window.getComputedStyle(contentDiv, null).getPropertyValue("height");
                }
                computedHeight = (computedHeight == '100%') ? window.getComputedStyle(contentParent, null).getPropertyValue("height") : computedHeight;
            });

            editor.on('mode', function(evt) {
                if (evt.editor.mode == 'source' && isNotMaximized(evt)) {
                    findContent(contentSpan);
                    var textarea = contentParent.getElementsByTagName('textarea')[0];
                    textarea.style.height = computedHeight;
                }
            });

            editor.on('beforeCommandExec', function(evt) {
                if (evt.data.name != 'maximize') {
                    return;
                }

                findContent(contentSpan);

                if (evt.editor.mode == 'source') {
                    var textarea = contentParent.getElementsByTagName('textarea')[0];
                    textarea.style.height = isNotMaximized(evt) ? '100%' : computedHeight;
                } else if (evt.editor.mode == 'wysiwyg') {
                    computedHeight = window.getComputedStyle(contentDiv, null).getPropertyValue("height");
                    computedHeight = (computedHeight == '100%') ? window.getComputedStyle(contentParent, null).getPropertyValue("height") : computedHeight;
                }
            });
        });
    }
});
