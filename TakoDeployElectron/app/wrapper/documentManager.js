let path = require('path');
const edge = require('electron-edge');

var NewDocument = edge.func({
    source: path.join(__dirname, 'documentManager.cs'),
    references: [
        'System.Data.dll',
        path.join(__dirname, '../../lib/Dapper.dll'),
        path.join(__dirname, '../../lib/Newtonsoft.Json.dll'),
        path.join(__dirname, '../../lib/Microsoft.SqlServer.TransactSql.ScriptDom.dll'),
        path.join(__dirname, '../../lib/TakoDeployLib.dll')
    ]
});

var addSource = edge.func({
    source: path.join(__dirname, 'documentManager.cs'),
    methodName: 'AddSource',
    references: [
        'System.Data.dll',
        path.join(__dirname, '../../lib/Dapper.dll'),
        path.join(__dirname, '../../lib/Newtonsoft.Json.dll'),
        path.join(__dirname, '../../lib/Microsoft.SqlServer.TransactSql.ScriptDom.dll'),
        path.join(__dirname, '../../lib/TakoDeployLib.dll')
    ]
});

var validate = edge.func({
    source: path.join(__dirname, 'documentManager.cs'),
    methodName: 'Validate',
    references: [
        'System.Data.dll',
        path.join(__dirname, '../../lib/Dapper.dll'),
        path.join(__dirname, '../../lib/Newtonsoft.Json.dll'),
        path.join(__dirname, '../../lib/Microsoft.SqlServer.TransactSql.ScriptDom.dll'),
        path.join(__dirname, '../../lib/TakoDeployLib.dll')
    ]
});

const IPromiseYou = function (fun) {
    return function () {
        let params = Array.prototype.slice.call(arguments);
        if (params.length === 0) params.push(0);
        return new Promise(function (resolve, reject) {
            params.push(function (error, result) {
                if (error) return reject(error);
                return resolve(result);
            });
            try {
                fun.apply(null, params);
            } catch (ex) {
                reject(ex)
            }
            //fun(params[0], params[1]);
        })
        .catch(function(error){
            debugger;
            alert(error);
        });
    }
};


module.exports = {
    newDocument: IPromiseYou(NewDocument),
    addSource: IPromiseYou(addSource),
    validate: IPromiseYou(validate)
}