const http=require('http')
const io=require('socket.io')

const httpServer=http.createServer();
httpServer.listen(8080)
const wsServer=io.listen(httpServer)
wsServer.on('connect',sock=>{
  sock.on('a',function(n1,n2,n3,n4,n5){
    console.log(n1,n2,n3,n4,n5)
  })
})
