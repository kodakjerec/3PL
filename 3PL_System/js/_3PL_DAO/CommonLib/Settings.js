//宣告全域變數

//連結伺服器
//網址
var Global_Server = 'localhost:1683/3PL';
//var Global_Server = '172.20.236.10/3PL';
//var Global_Server = '192.168.120.162/3PL';
var Set_timeout = 1500;
var packageVersion = '17.09.19';
var DefaultServer = "3PL";

//程式作用的變數
var ProgParameters = {
    params: [
       
    ]
    ,
    set(name, value) {
        this.params.push({ Name: name, Value: value });
    }
    ,
    get(name) {
        var object = undefined;

        this.params.forEach(value => {
            if (value.Name == name)
                object = value.Value;
        });
        return object;
    }
}