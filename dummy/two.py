import sys
from PySide6.QtWidgets import QVBoxLayout, QHBoxLayout, QLabel, QPushButton, QApplication, QWidget, QTableView
from PySide6.QtCore import QTimer, Qt, QAbstractTableModel

app = QApplication(sys.argv)

window = QWidget()

label = QLabel()
label.setText("Hello World")
label.setAlignment(Qt.AlignCenter)

fa = QPushButton("This button!")

encompassor = QVBoxLayout()
encompassor.addWidget(label)
encompassor.addWidget(fa)
window.setLayout(encompassor)




def thisCode():
    label.setText("Hello Unix!")
    QTimer.singleShot(1775, lambda: label.setText("Hello World"))

fa.clicked.connect(thisCode)





#-------------------------------------------------------revved--------------------------------------------------

class ProcessTable(QAbstractTableModel):
    def __init__(self, data):
        super().__init__()
        self._data = data
        self._header = ["PID", "Process Name", "Memory", "CPU"]

    def rowCount(self, parent = None):
        return len(self._data)

    def columnCount(self, parent = None):
        return len(self._data[0])

    def data(self, index, role = Qt.DisplayRole):
        if role == Qt.DisplayRole:
            return str(self._data[index.row()][index.column()])

        return None

    def headerData(self, section, orientation, role = Qt.DisplayRole):
        if role == Qt.DisplayRole and orientation == Qt.Horizontal:
            return self._header[section]
        return None

# app = QApplication(sys.argv)

data = [
    [1234, "chrome.exe", 20.5, 15.2],
    [5678, "python.exe", 12.0, 10.8],
    [9101, "explorer.exe", 5.3, 7.1],
    [1213, "spotify.exe", 8.9, 6.4],
    [1415, "discord.exe", 3.2, 4.7],
]

model = ProcessTable(data)
table = QTableView()
table.setModel(model)

table.show()

window.show()

app.exec_()
















def ProcessTable(QDataTableModel):
    return QDataTableModel()
data = []




