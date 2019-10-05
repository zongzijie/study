function use(path,callback){
  const me=this;
  $.ajax({
    url: path,
    success(str) {
      function defined(fn) {

        const module = {
          exports: {}
        }
        fn(function(path) {
          me.use(path);
        }, module.exports, module);
        callback(module.exports)
      }


      let fn_body = str.substring(str.indexOf('{') + 1, str.lastIndexOf('}'));
      let arr = fn_body.match(/require\([^\(\)]+\)/g)?fn_body.match(/require\([^\(\)]+\)/g).map(item => {
        return item.substring(item.search(/\"|\'/), item.search(/\)/));
      }):[];


      //解析代码
      let i = 0;
      let json = {};
      next();

      function next() {
        if (arr.length) {
          $.ajax({
            url: arr[i].match(/[^\'\"]+/),
            success(str2) {
              parseStr(str2, function(mod) {
                json[arr[i]] = mod;
                i++;
                if (i==arr.length) {
                  $.each(json,function(k,v){
                      str2=str2.replace(k,v);
                  });
                 //运行代码
                 eval(str2);
                }else{
                  next();
                }
              })
            },
            error(){
              alert('失败');
            }
          });
        }

      }
      eval(str1);

      //运行代码
      //eval(str);

      // Promise.all(arr).then(function(require_contents){
      //   let require_strs=fn_body.match(/require\([^\(\)]+\)/g);
      //   for (var i = 0; i < require_strs.length; i++) {
      //     fn_str=fn_str.replace(require_strs[i],require_contents[i]);
      //   }
      //   alert(fn_str)
      // });

    },
    error() {
      alert('失败了')
    }
  })
}
