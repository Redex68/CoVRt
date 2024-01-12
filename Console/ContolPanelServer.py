import http.server
import socketserver
import serial
import threading

data = ""

class MyHandler(http.server.SimpleHTTPRequestHandler):
    def do_GET(self):
        if self.path == '/data':
            self.send_response(200)
            self.send_header('Content-type', 'text/plain')
            self.end_headers()
            response = data
            self.wfile.write(response.encode())
        else:
            super().do_GET()
        
ser = serial.Serial('COM3', 9600)

PORT = 42069

def read():
    while True:
        global data
        data = ser.readline().decode('utf-8')  # Read a line from the serial input
        #print(data.decode('utf-8'))

my_thread = threading.Thread(target=read, daemon=True)
my_thread.start()

with socketserver.TCPServer(("", PORT), MyHandler) as httpd:
    print(f"Server is running at http://localhost:{PORT}")
    httpd.serve_forever()