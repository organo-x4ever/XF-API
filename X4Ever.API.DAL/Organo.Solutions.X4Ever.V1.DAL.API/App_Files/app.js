const http = require('http');

//requiring path and fs modules
const path = require('path');
const fs = require('fs');

const hostname = '127.0.0.1';
const port = 3000;

const server = http.createServer((req, res) => {
    res.statusCode = 200;
    res.setHeader('Content-Type', 'text/plain');

    //joining path of directory
    var directoryPath = path.join(__dirname, '../Uploads');
    //passsing directoryPath and callback function
    fs.readdir(directoryPath, function (err, files) {
        //handling error
        if (err) {
            return console.log('Unable to scan directory: ' + err);
        }
        //listing all files using forEach
        files.forEach((file) => {
            // Do whatever you want to do with the file
            console.log(file);
            res.end(file);
        });
    });
    res.end('Done\n');
});

server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});