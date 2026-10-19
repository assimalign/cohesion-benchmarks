# A minimal Python HTTP/1.1 server for the Web area's out-of-process benchmarks (targets/catalog.json, "python").
# Usage: python3 server.py <port>
import sys
from http.server import BaseHTTPRequestHandler, HTTPServer

BODY = b"Hello, World!"


class Handler(BaseHTTPRequestHandler):
    protocol_version = "HTTP/1.1"
    # The head and the body leave in two writes. With Nagle on, the body waits for the client's delayed ACK of the
    # head (about 40 ms on Linux), and the benchmark would measure that stall instead of the server.
    disable_nagle_algorithm = True

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
