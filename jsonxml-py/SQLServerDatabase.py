import pyodbc
import pandas as pd
import json
from decimal import Decimal

class SQLServerDatabase:
    def __init__(self, server, database, user, password):
        self.connection = pyodbc.connect(
            'DRIVER={ODBC Driver 17 for SQL Server};'
            f'SERVER={server};'
            f'DATABASE={database};'
            f'UID={user};'
            f'PWD={password}'
        )
        self.cursor = self.connection.cursor()

    def execute_query(self, query):
        self.cursor.execute(query)
        return self.cursor

    def close(self):
        self.cursor.close()
        self.connection.close()

def decimal_default(obj):
    if isinstance(obj, Decimal):
        return float(obj)
    raise TypeError(f"Object of type {obj.__class__.__name__} is not JSON serializable")

def export_to_json(cursor, file_path):
    rows = cursor.fetchall()
    columns = [desc[0] for desc in cursor.description]
    data = [dict(zip(columns, row)) for row in rows]

    with open(file_path, 'w') as json_file:
        json.dump(data, json_file, indent=4, default=decimal_default)

def export_to_xml(cursor, file_path):
    # Re-ejecutar la consulta para obtener las filas nuevamente
    rows = cursor.fetchall()
    columns = [desc[0] for desc in cursor.description]

    # Asegúrate de que cada fila es una tupla
    df = pd.DataFrame([tuple(row) for row in rows], columns=columns)
    df.to_xml(file_path, index=False, root_name='productos', row_name='producto')

def main():
    db = SQLServerDatabase(
        server="localhost",
        database="BDPRODUCTO",
        user="sa",
        password="MS3123"
    )

    cursor = db.execute_query("SELECT id_producto, descripcion, costo, precio FROM producto")
    export_to_json(cursor, 'sqlserver.json')

    # Re-ejecutar la consulta antes de exportar a XML
    cursor = db.execute_query("SELECT id_producto, descripcion, costo, precio FROM producto")
    export_to_xml(cursor, 'sqlserver.xml')

    db.close()

if __name__ == "__main__":
    main()
