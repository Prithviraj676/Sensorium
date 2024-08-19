import sys
import socket
from PyQt5.QtCore import Qt, QTimer
from PyQt5.QtGui import QPainter, QPen
from PyQt5.QtWidgets import QMainWindow, QApplication, QVBoxLayout, QTextEdit
from PyQt5.QtChart import QChart, QChartView, QPieSeries, QPieSlice


class SensorDataWindow(QMainWindow):

    def __init__(self):
        super().__init__()
        self.initUI()

    def initUI(self):
        self.setWindowTitle('Sensor Data')
        self.setGeometry(100, 100, 600, 400)
        layout = QVBoxLayout()
        self.dataDisplay = QTextEdit(self)
        self.dataDisplay.setReadOnly(True)
        layout.addWidget(self.dataDisplay)

    def append_data(self, data):
        self.dataDisplay.append(data)

def retrieve_data(obj):
    host = '127.0.0.1'
    port = 1919

    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((host, port))
        while True:
            data = s.recv(1024).decode('utf-8')
            if not data:
                break
            sensor_window.append_data(data)

if __name__ == '__main__':

    app = QApplication(sys.argv)
    obj = SensorDataWindow()
    obj.show()
    import threading as th
    thread = th.Thread(group = None, target = retrieve_data, name = None,  args = [obj])
    thread.start()
    sys.exit(app.exec_())
