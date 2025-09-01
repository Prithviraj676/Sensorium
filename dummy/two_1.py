import sys

from PySide6.QtCore import QAbstractTableModel, QTimer, QFile, Qt
from PySide6.QtUiTools import QUiLoader
from PySide6.QtWidgets import QWidget, QApplication, QVBoxLayout, QTableView, QHeaderView


class ProcessTableModel(QAbstractTableModel):
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


data = [
    [1234, "chrome.exe", 20.5, 15.2],
    [5678, "python.exe", 12.0, 10.8],
    [9101, "explorer.exe", 5.3, 7.1],
    [1213, "spotify.exe", 8.9, 6.4],
    [1415, "discord.exe", 3.2, 4.7],
]


if __name__ == "__main__":

    app = QApplication(sys.argv)
    loader = QUiLoader()
    file = QFile(r"C:\Users\divrg\Documents\proj\fan\Sensorium\dummy\main.ui")
    file.open(QFile.ReadOnly)
    window = loader.load(file)
    file.close()

    table = window.findChild(QTableView, "tableView")

    model = ProcessTableModel(data)
    table.setModel(model)
    header = table.horizontalHeader()
    header.setSectionResizeMode(QHeaderView.Stretch)

    # for col in range(model.columnCount()):
    #     header.setSectionResizeMode(col, QHeaderView.ResizeToContents)

    header.setSectionResizeMode(QHeaderView.Interactive)

    header.setSectionResizeMode(1, QHeaderView.Stretch)
    # header.setStretchLastSection(True)
    # table.show()
    window.show()
    sys.exit(app.exec())

#
# model = ProcessTableModel(data)
#
# table.setModel(model)
#
#
#
# table.show()