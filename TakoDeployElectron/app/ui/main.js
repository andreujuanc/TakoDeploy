const path = require('path');
const dm = require('../wrapper/documentManager');
let mainRactive = null;


let init = function () {

    console.log('main init!');
    var ipc = require('electron').ipcRenderer;
    window.onerror = function (error, url, line) {
        ipc.send('errorInWindow', error);
    };
    
    function showOpenDocument() {
        const { dialog } = require('electron').remote;
        dialog.showOpenDialog(
            { properties: ['openFile', 'multiSelections'] }, openDocument);
    }
    function openDocument(files) {
        if (!files || files.length === 0) return;
        debugger;
    }

    function newDocument(){
        dm.newDocument()
        .then((x)=>mainRactive.set('document', x));
    }

    function addSource(){
        dm.addSource()
        .then((x)=>mainRactive.set('document', x));
    }

    function validate (){
         dm.validate({
             event_handler: function(){
                 debugger;
             }
         })
        .then((x)=>mainRactive.set('document', x));
    }

    var CommandBarElements = document.querySelectorAll(".ms-CommandBar");
    for (var i = 0; i < CommandBarElements.length; i++) {
        new fabric['CommandBar'](CommandBarElements[i]);
    }



    //***************************MONACO *******************88 */
    function uriFromPath(_path) {
        var pathName = path.resolve(_path).replace(/\\/g, '/');
        if (pathName.length > 0 && pathName.charAt(0) !== '/') {
            pathName = '/' + pathName;
        }
        return encodeURI('file://' + pathName);
    }
    amdRequire.config({
        baseUrl: uriFromPath(path.join(__dirname, '../../node_modules/monaco-editor/min'))
    });
    // workaround monaco-css not understanding the environment
    self.module = undefined;
    // workaround monaco-typescript not understanding the environment
    self.process.browser = true;
    amdRequire(['vs/editor/editor.main'], function () {
        window.editor = monaco.editor.create(document.getElementById('editorcontainer'), {
            value: [
                'SELECT *',
                'FROM dbo.Potatoes',
                'WHERE id = 3'
            ].join('\n'),
            language: 'sql',
            scrollBeyondLastLine: false
        });
        window.addEventListener('change', () => editor.layout())
        window.addEventListener('resize', () => editor.layout())
    });
    /*****END MONACO LOAD *******/



    var maintemplate = require('./main.html');
    mainRactive = new Ractive({
        el: '#app',
        template: maintemplate,
        showOpenDocument: showOpenDocument,
        newDocument: newDocument,
        addSource: addSource,
        validate: validate
    });

};

module.exports = {
    init: init
}