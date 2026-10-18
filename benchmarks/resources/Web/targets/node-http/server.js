// A minimal Node.js HTTP/1.1 server for the Web area's out-of-process benchmarks (targets/catalog.json, "node").
// Usage: node server.js <port>
'use strict';

const http = require('http');

const port = Number(process.argv[2] || 8080);
const body = Buffer.from('Hello, World!');

http
  .createServer((request, response) => {
    response.writeHead(200, { 'Content-Type': 'text/plain', 'Content-Length': body.length });
    response.end(body);
  })
  .listen(port, '127.0.0.1', () => console.log(`listening on http://127.0.0.1:${port}/`));
