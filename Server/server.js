const express = require('express');
const app = express();
const port = 3000;
const path = require('path');

// 设置静态资源目录为你的Addressables构建输出路径
const addressablesBuildPath = path.join('D:', 'ujob', '2dgame', 'ServerData', 'StandaloneWindows64');

console.log(`静态资源路径: ${addressablesBuildPath}`);

app.use(express.static(addressablesBuildPath));

// 根路径返回简单信息，便于验证服务器运行状态
app.get('/', (req, res) => {
  res.send('Addressables热更新服务器正在运行');
});

app.listen(port, () => {
  console.log(`服务器运行在 http://localhost:${port}`);
});