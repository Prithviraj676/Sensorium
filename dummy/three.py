import sys
import psutil
from typing import ReadOnly

from PySide6.QtCore import QTimer, Qt, QAbstractTableModel, QFile, QFileInfo
from PySide6.QtUiTools import QUiLoader
from PySide6.QtWidgets import QFileIconProvider, QWidget, QApplication, QMainWindow, QMessageBox, QTableView, QHeaderView


class Table(QAbstractTableModel):
    def __init__(self, data):
        super().__init__()
        self._data = data
        self._header = ["PID", "Process", "Memory", "CPU", "Status", "Threads Occupied"]

    def rowCount(self, parent=None):
        return len(self._data)

    def columnCount(self, parent=None):
        return len(self._data[0])

    def data(self, index, role=Qt.DisplayRole):
        if not index.isValid():
            return None

        value = self._data[index.row()][index.column()]
        if role == Qt.DisplayRole:
            if isinstance(value, tuple):
                return value[1]
            return str(value)

        elif role == Qt.DecorationRole and isinstance(value, tuple):
            return value[0]


    def headerData(self, section, orientation, role=Qt.DisplayRole):
        if orientation == Qt.Horizontal and role == Qt.DisplayRole:
            return str(self._header[section])

        return None

data = [
    [1234, "chrome.exe", 20.5, 15.2],
    [5678, "python.exe", 12.0, 10.8],
    [9101, "explorer.exe", 5.3, 7.1],
    [1213, "spotify.exe", 8.9, 6.4],
    [1415, "discord.exe", 3.2, 4.7],
]

def getProcesses(limit=30):
    process_info = []
    for proc in psutil.process_iter(["pid", "name", "memory_percent", "cpu_percent",
        "status", "num_threads", "exe"]):
        try:
            exe_path = proc.info["exe"]
            if exe_path:
                fileInfo = QFileInfo(exe_path)

                icon = provider.icon(fileInfo)
            else:
                icon = None

            process_info.append([
                proc.info["pid"],
                (icon, proc.info["name"]),
                round(proc.info["memory_percent"], 2),
                round(proc.info["cpu_percent"], 2),
                proc.info["status"],
                proc.info["num_threads"]])


        except (psutil.NoSuchProcess, psutil.AccessDenied):
            pass
    process_info = sorted(process_info, key=lambda x: x[2], reverse=True)
    return process_info

if __name__ == '__main__':
    app = QApplication(sys.argv)
    loader = QUiLoader()
    file = QFile(r"C:\Users\divrg\Documents\proj\fan\Sensorium\dummy\main.ui")
    file.open(QFile.ReadOnly)
    window = loader.load(file)
    file.close()

    provider = QFileIconProvider()

    table = window.findChild(QTableView, "tableView")
    data = getProcesses(100)
    model = Table(data)
    table.setModel(model)
    table.setAlternatingRowColors(True)
    header = table.horizontalHeader()
    header.setSectionResizeMode(QHeaderView.Stretch)

    # for child in window.findChildren(QWidget):
    #     print(child.objectName(), "----", type(child), "\n\n")
    #     pass
    #
    window.show()
    app.exec()
