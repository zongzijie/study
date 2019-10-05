function use(ids, callback) {
  if (!Array.isArray(ids)) ids = [ids];
  Promise.all(ids.map(function(id) {
    return load(id);
  })).then(function(list) {
    callback.apply(global, list); // 加载完成， 调用回调函数
  }, function(error) {
    throw error;
  });
}

function load(id) {
  return new Promise(function(resolve, reject) {
    var module = myLoader.modules[id] || Module.create(id); // 取得模块或者新建模块 此时模块正在加载或者已经加载完成
    module.on("complete", function() {
      var exports = getModuleExports(module);
      resolve(exports); // 加载完成-> 通知调用者
    })
    module.on("error", reject);
  })
}

function require(id) {
  var module = myLoader.modules[myLoader.config.root + id];
  if (!module) throw "can not load find module by id:" + id;
  else {
    return getModuleExports(module); // 返回模块的对外接口。
  }
}

function getModuleExports(module) {
  if (!module.exports) {
    module.exports = {};
    module.factory(require, module.exports, module);
  }
  return module.exports;
}
