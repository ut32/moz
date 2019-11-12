/**
 * @license Copyright (c) 2003-2018, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	config.language = 'zhcn';
	config.skinName = 'moonolisa';
    config.baseFloatZIndex = 1000000;
    config.filebrowserBrowseUrl  = 'filebrowser?type=file';
    config.filebrowserImageBrowseUrl = 'filebrowser?type=image';
    config.removeDialogTabs='link:upload;image:upload';
    config.extraPlugins = 'html5video,multiimage,widget,widgetselection,clipboard,lineutils';
    config.toolbar = [
        { name: 'do', items: [ 'Undo', 'Redo' ] },
        { name: 'styles', items: [ 'Format', 'Font', 'FontSize' ] },
        { name: 'colors', items: [ 'TextColor', 'BGColor' ] },
        { name: 'basicstyles', items: [ 'Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript' ] },
        { name: 'paragraph', items: [ 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock' ,'-',    'NumberedList', 'BulletedList', ] },
        { name: 'clipboard', items: [ 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'CopyFormatting', 'RemoveFormat' ] },
        { name: 'links', items: [ 'Link', 'Unlink' ] },
        { name: 'insert', items: [ 'Image','Multiimage','Html5video', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe' ] },
        { name: 'tools', items: [ 'Maximize', 'Source' ] }
    ]; 
};
