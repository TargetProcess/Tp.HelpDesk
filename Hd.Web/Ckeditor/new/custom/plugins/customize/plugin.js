CKEDITOR.plugins.add('customize', {

    init: function (editor) {



        CKEDITOR.dom.element.prototype.disableContextMenu = function() {

           this.on( 'contextmenu', function( event ) {
               var $el = $(event.data.getTarget().$);
                  // native problems with div and right menu on CTRL
               if (!$el.hasClass('cke_enable_context_menu') && (!$el.parents('.cke_wysiwyg_div.cke_enable_context_menu').length))
                   event.data.preventDefault();
              }
           );
        };

        editor.on( 'mode', function() {
            if (editor.editable()) {
                $(editor.editable().$).addClass('cke_enable_context_menu').removeAttr('title');
            }
        });

        if (editor.dataProcessor && editor.dataProcessor.htmlFilter){
            editor.dataProcessor.htmlFilter.addRules({
                elements: {
                    'a': function( element ) {
                        if (!('target' in element.attributes)){
                            element.attributes['target'] = '_blank';
                        }
                    }
                }
            });
        }
    }
});
