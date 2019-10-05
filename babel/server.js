const http = require("http")
const fs = require("fs")

var server = http.createServer(function(req, res) {
  console.log(`www${req.url}`)
  fs.readFile(`www${req.url}`,function(err,data){
      if (err) {
        res.writeHead(404)
      }else{
        res.write(data)
      }
      res.end();
  })
})

server.listen(8080)
