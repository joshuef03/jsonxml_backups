import psycopg2
from decimal import Decimal
import pandas as pd
import json


class PostgreSQLDatabase:
    def __init__(self, dbname, user, password, host, port):
        self.connection = psycopg2.connect(
            dbname=dbname,
            user=user,
            password=password,
            host=host,
            port=port
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

    for record in data:
        for key, value in record.items():
            if isinstance(value, Decimal):
                record[key] = float(value)

    with open(file_path, 'w') as json_file:
        json.dump(data, json_file, indent=4)


def export_to_xml(cursor, file_path):
    rows = cursor.fetchall()
    columns = [desc[0] for desc in cursor.description]
    df = pd.DataFrame(rows, columns=columns)
    df.to_xml(file_path, index=False, root_name='productos', row_name='producto')


def main():
    db = PostgreSQLDatabase(
        dbname="BDPRODUCTO",
        user="postgres",
        password="PSG123",
        host="localhost",
        port="5432"
    )

    cursor = db.execute_query("SELECT id_producto, descripcion, costo, precio FROM producto")
    export_to_json(cursor, 'postgres.json')

    cursor = db.execute_query("SELECT id_producto, descripcion, costo, precio FROM producto")
    export_to_xml(cursor, 'postgres.xml')

    db.close()


if __name__ == "__main__":
    main()
