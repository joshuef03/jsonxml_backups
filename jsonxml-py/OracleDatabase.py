import cx_Oracle
import pandas as pd
import json


class OracleDatabase:
    def __init__(self, dsn, user, password):
        self.connection = cx_Oracle.connect(
            user=user,
            password=password,
            dsn=dsn
        )
        self.cursor = self.connection.cursor()

    def execute_query(self, query):
        self.cursor.execute(query)
        return self.cursor

    def close(self):
        self.cursor.close()
        self.connection.close()


def export_to_json(cursor, file_path):
    rows = cursor.fetchall()
    columns = [desc[0] for desc in cursor.description]
    data = [dict(zip(columns, row)) for row in rows]

    with open(file_path, 'w') as json_file:
        json.dump(data, json_file, indent=4)


def export_to_xml(cursor, file_path):
    rows = cursor.fetchall()
    columns = [desc[0] for desc in cursor.description]
    df = pd.DataFrame(rows, columns=columns)
    df.to_xml(file_path, index=False, root_name='productos', row_name='producto')


def main():
    db = OracleDatabase(
        dsn="localhost:1521/xepdb1",
        user="C##UDB",
        password="1234567"
    )

    cursor = db.execute_query("SELECT id_producto, descripcion, costo, precio FROM producto")
    export_to_json(cursor, 'oracle.json')

    cursor = db.execute_query("SELECT id_producto, descripcion, costo, precio FROM producto")
    export_to_xml(cursor, 'oracle.xml')

    db.close()


if __name__ == "__main__":
    main()
