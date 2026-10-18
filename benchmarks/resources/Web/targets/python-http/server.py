# A minimal Python HTTP/1.1 server for the Web area's out-of-process benchmarks (targets/catalog.json, "python").
# Usage: python3 server.py <port>
import sys
from http.server import BaseHTTPRequestHandler, HTTPServer

BODY = b"Hello, World!"


class Handler(BaseHTTPRequestHandler):
    protocol_version = "HTTP/1.1"

    def do_GET(self):
        self.send_response(200)
        self.send_header("Content-Type", "text/plain")
        self.send_header("Content-Length", str(len(BODY)))
        self.end_headers()
        self.wfile.write(BODY)

    def log_message(self, format, *args):
        pass


port = int(sys.argv[1]) if len(sys.argv) > 1 else 8080
print(f"listening on http://127.0.0.1:{port}/", flush=True)
HTTPServer(("127.0.0.1", port), Handler).serve_forever()
