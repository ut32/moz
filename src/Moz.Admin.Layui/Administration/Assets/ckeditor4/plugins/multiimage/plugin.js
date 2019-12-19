window.tempEditor = null;
window.insertPathToCkeditor = function(paths){
    if (window.tempEditor){
        if (paths){
            var html = paths.split(',').map(function(v){
                return '<img src="' + v + '" />';
            }).join(' ');
            window.tempEditor.insertHtml( html );
        }
    } 
};
CKEDITOR.plugins.add( 'multiimage', {
    icons: 'multiimage',
    init: function( editor ) {
        window.tempEditor = editor;
        editor.ui.addButton( 'Multiimage', {
            label: '多图插入',
            command: 'insertMultiImage',
            toolbar: 'insert'
        });
        editor.addCommand( 'insertMultiImage', {
            exec: function( editor ) {
                window.open('filebrowser?type=image&CKEditor=content&CKEditorFuncNum=1&multiimage=yes&langCode=zhcn', '图片浏览器', 'height=500, width=900')
            }
        });
    }
});
