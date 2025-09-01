import sys

from time import sleep
from PySide6.QtWidgets import QApplication, QWidget, QLabel, QPushButton, QVBoxLayout
from PySide6.QtCore import QTimer

app = QApplication(sys.argv)

window = QWidget()
window.setWindowTitle("Dummy Front")

label = QLabel("This is the label")
button = QPushButton("Click Me!")


def buttonClick():
    label.setText("Click was registered!")
    QTimer.singleShot(1000, lambda: label.setText("This is the label"))
    # label.setText("This is the label")


button.clicked.connect(buttonClick)

encompass = QVBoxLayout()
encompass.addWidget(label)
encompass.addWidget(button)
window.setLayout(encompass)

window.show()

sys.exit(app.exec())
