import sys
import time
import psutil

from PySide6.QtCore import (
    Qt, QAbstractTableModel, QModelIndex, QTimer, QSortFilterProxyModel, QRegularExpression
)
from PySide6.QtGui import QAction, QIcon, QRegularExpressionValidator
from PySide6.QtWidgets import (
    QApplication, QMainWindow, QWidget, QVBoxLayout, QHBoxLayout, QLineEdit,
    QLabel, QProgressBar, QTableView, QHeaderView, QStatusBar
)

# ---------------------------
# Qt Model for process table
# ---------------------------
class ProcessTableModel(QAbstractTableModel):
    """
    Columns: PID | Name | CPU% | MEM% | User | Status | Nice
    Backed by psutil; updated externally via set_rows(data).
    """
    HEADERS = ["PID", "Process", "CPU %", "MEM %", "User", "Status", "Nice"]

    def __init__(self, rows=None, parent=None):
        super().__init__(parent)
        self._rows = rows or []
        self._sort_col = 0
        self._sort_order = Qt.AscendingOrder

    def rowCount(self, parent=QModelIndex()):
        return len(self._rows)

    def columnCount(self, parent=QModelIndex()):
        return len(self.HEADERS)

    def data(self, index, role=Qt.DisplayRole):
        if not index.isValid():
            return None
        row = self._rows[index.row()]
        col = index.column()

        if role in (Qt.DisplayRole, Qt.ToolTipRole):
            if col == 0:
                return row["pid"]
            elif col == 1:
                return row["name"]
            elif col == 2:
                return f"{row['cpu']:.1f}"
            elif col == 3:
                return f"{row['mem']:.1f}"
            elif col == 4:
                return row["user"]
            elif col == 5:
                return row["status"]
            elif col == 6:
                return row["nice"]

        # Align numeric columns right
        if role == Qt.TextAlignmentRole and col in (0, 2, 3, 6):
            return Qt.AlignRight | Qt.AlignVCenter

        return None

    def headerData(self, section, orientation, role=Qt.DisplayRole):
        if role != Qt.DisplayRole:
            return None
        if orientation == Qt.Horizontal:
            return self.HEADERS[section]
        return section + 1

    def set_rows(self, rows):
        self.beginResetModel()
        self._rows = rows
        self.endResetModel()

    def sort(self, column, order=Qt.AscendingOrder):
        self.layoutAboutToBeChanged.emit()
        reverse = (order == Qt.DescendingOrder)

        def key(row):
            if column == 0:
                return row["pid"]
            if column == 1:
                return row["name"].lower()
            if column == 2:
                return row["cpu"]
            if column == 3:
                return row["mem"]
            if column == 4:
                return row["user"].lower()
            if column == 5:
                return row["status"].lower()
            if column == 6:
                return row["nice"]
            return 0

        self._rows.sort(key=key, reverse=reverse)
        self._sort_col, self._sort_order = column, order
        self.layoutChanged.emit()


# ---------------------------
# Main Window
# ---------------------------
class NeoHtopMinimal(QMainWindow):
    REFRESH_MS = 1000  # 1 second

    def __init__(self):
        super().__init__()
        self.setWindowTitle("NeoHtop (Minimal, PySide6 + psutil)")
        self.resize(1000, 640)

        # Central widget & layout
        central = QWidget()
        self.setCentralWidget(central)
        outer = QVBoxLayout(central)
        outer.setContentsMargins(12, 12, 12, 12)
        outer.setSpacing(10)

        # --- Top metrics bar (CPU, Mem) ---
        top = QHBoxLayout()
        top.setSpacing(12)
        outer.addLayout(top)

        self.cpu_label = QLabel("CPU: 0%")
        self.mem_label = QLabel("Memory: 0%")

        self.cpu_bar = QProgressBar()
        self.cpu_bar.setRange(0, 100)
        self.cpu_bar.setFormat("%p%")
        self.cpu_bar.setValue(0)

        self.mem_bar = QProgressBar()
        self.mem_bar.setRange(0, 100)
        self.mem_bar.setFormat("%p%")
        self.mem_bar.setValue(0)

        cpu_box = self._labeled_box("CPU Usage", self.cpu_bar, self.cpu_label)
        mem_box = self._labeled_box("Memory Usage", self.mem_bar, self.mem_label)

        top.addWidget(cpu_box, 1)
        top.addWidget(mem_box, 1)

        # --- Search & table ---
        search_row = QHBoxLayout()
        search_row.setSpacing(8)
        outer.addLayout(search_row)

        search_label = QLabel("Search (regex):")
        self.search_edit = QLineEdit()
        self.search_edit.setPlaceholderText(r"e.g., python$ | ^chrome | (exe|service)")
        search_row.addWidget(search_label)
        search_row.addWidget(self.search_edit, 1)

        self.table = QTableView()
        self.table.setSortingEnabled(True)
        self.table.verticalHeader().setVisible(False)
        self.table.setSelectionBehavior(QTableView.SelectRows)
        self.table.setAlternatingRowColors(True)
        self.table.setEditTriggers(QTableView.NoEditTriggers)
        self.table.horizontalHeader().setStretchLastSection(True)
        self.table.horizontalHeader().setSectionResizeMode(QHeaderView.Interactive)

        self.model = ProcessTableModel([])
        self.proxy = QSortFilterProxyModel(self)
        self.proxy.setSourceModel(self.model)
        self.proxy.setFilterKeyColumn(1)  # Process name column
        self.proxy.setFilterCaseSensitivity(Qt.CaseInsensitive)
        self.table.setModel(self.proxy)

        outer.addWidget(self.table, 1)

        # Status bar
        self.status = QStatusBar()
        self.setStatusBar(self.status)

        # Menu: Refresh now
        refresh_action = QAction("Refresh Now", self)
        refresh_action.triggered.connect(self.refresh_now)
        self.menuBar().addAction(refresh_action)

        # Signals
        self.search_edit.textChanged.connect(self._apply_filter)
        self.table.sortByColumn(2, Qt.DescendingOrder)  # default sort by CPU%

        # Prime psutil CPU counters for accurate deltas on first tick
        psutil.cpu_percent(interval=None)
        for p in psutil.process_iter(["pid"]):
            try:
                p.cpu_percent(None)
            except Exception:
                pass

        # Timer
        self.timer = QTimer(self)
        self.timer.timeout.connect(self.refresh_now)
        self.timer.start(self.REFRESH_MS)

        # Initial fill
        self.refresh_now()

    def _labeled_box(self, title_text, widget, value_label):
        box = QWidget()
        lay = QVBoxLayout(box)
        lay.setContentsMargins(12, 12, 12, 12)
        lay.setSpacing(6)
        title = QLabel(title_text)
        title.setProperty("role", "title")
        lay.addWidget(title)
        lay.addWidget(widget)
        lay.addWidget(value_label)
        return box

    def _apply_filter(self, text: str):
        try:
            if text.strip():
                self.proxy.setFilterRegularExpression(QRegularExpression(text))
            else:
                self.proxy.setFilterRegularExpression(QRegularExpression())
        except Exception:
            # If invalid regex, fall back to literal contains
            safe = QRegularExpression.escape(text)
            self.proxy.setFilterRegularExpression(QRegularExpression(safe))

    def refresh_now(self):
        # Top bars
        cpu = psutil.cpu_percent(interval=None)
        mem = psutil.virtual_memory().percent

        self.cpu_bar.setValue(int(cpu))
        self.mem_bar.setValue(int(mem))
        self.cpu_label.setText(f"CPU: {cpu:.1f}%")
        self.mem_label.setText(f"Memory: {mem:.1f}%")

        # Processes
        rows = []
        for proc in psutil.process_iter(
            ["pid", "name", "username", "status", "nice", "memory_percent"]
        ):
            try:
                rows.append(
                    {
                        "pid": proc.info.get("pid", 0),
                        "name": proc.info.get("name") or f"[{proc.pid}]",
                        "cpu": proc.cpu_percent(None),  # delta since last call
                        "mem": float(proc.info.get("memory_percent") or 0.0),
                        "user": proc.info.get("username") or "",
                        "status": proc.info.get("status") or "",
                        "nice": proc.info.get("nice") if proc.info.get("nice") is not None else 0,
                    }
                )
            except (psutil.NoSuchProcess, psutil.AccessDenied, psutil.ZombieProcess):
                continue

        # Optional: small sleep to let cpu_percent settle on first run (not strictly required)
        # time.sleep(0.0)

        # Update model
        self.model.set_rows(rows)
        self.status.showMessage(
            f"Processes: {len(rows)} | Refresh: {self.REFRESH_MS} ms | CPU cores: {psutil.cpu_count(logical=True)}"
        )


# ---------------------------
# Minimal dark QSS (NeoHtop-ish)
# ---------------------------
DARK_QSS = """
QMainWindow {
    background-color: #15171a;
    color: #e6e6e6;
    font-family: "Segoe UI", "Inter", "Roboto", "Noto Sans", Arial;
    font-size: 12.5pt;
}

QMenuBar, QMenu {
    background-color: #15171a;
    color: #e6e6e6;
    border: none;
}
QMenu::item:selected { background: #2a2f36; }

QStatusBar {
    background-color: #131519;
    color: #bfc7d5;
}

QLabel[role="title"] {
    color: #bfc7d5;
    font-weight: 600;
    letter-spacing: 0.3px;
}

QProgressBar {
    background-color: #232730;
    border: 1px solid #2f3540;
    border-radius: 8px;
    text-align: center;
    padding: 2px;
    color: #e6e6e6;
}
QProgressBar::chunk {
    border-radius: 8px;
    background-color: #34c759; /* green accent */
}

QLineEdit {
    background-color: #1b1f26;
    color: #e6e6e6;
    border: 1px solid #2f3540;
    border-radius: 10px;
    padding: 6px 10px;
    selection-background-color: #2a2f36;
}

QTableView {
    background-color: #15171a;
    color: #e6e6e6;
    gridline-color: #2a2f36;
    border: 1px solid #2a2f36;
    border-radius: 10px;
    selection-background-color: #2a2f36;
    selection-color: #ffffff;
    alternate-background-color: #181c22;
}
QHeaderView::section {
    background-color: #1b1f26;
    color: #cdd5e0;
    border: none;
    border-right: 1px solid #2a2f36;
    padding: 6px 8px;
}
QTableCornerButton::section {
    background-color: #1b1f26;
    border: none;
}
"""

def main():
    app = QApplication(sys.argv)
    app.setStyleSheet(DARK_QSS)
    win = NeoHtopMinimal()
    win.show()
    sys.exit(app.exec())

if __name__ == "__main__":
    main()

