package org.backup;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.io.FileWriter;
import org.json.JSONArray;
import org.json.JSONObject;
import javax.swing.*;

public class OracleBackup {
    public static void main(String[] args) {
        /*
        CDB$ROOT -- "jdbc:oracle:thin:@URL:PORT:SID"
        PDB -- "jdbc:oracle:thin:@URL:PORT/Service Name"
        */
        String jdbcUrl = "jdbc:oracle:thin:@localhost:1521/xepdb1";
        String username = "C##UDB";
        String password = "1234567";
        String query = "SELECT id_producto, descripcion, costo, precio FROM producto";

        try (Connection connection = DriverManager.getConnection(jdbcUrl, username, password);
             Statement statement = connection.createStatement();
             ResultSet resultSet = statement.executeQuery(query)) {

            JSONArray jsonArray = new JSONArray();
            StringBuilder xmlBuilder = new StringBuilder();
            xmlBuilder.append("<productos>\n");

            while (resultSet.next()) {
                // Create JSON object
                JSONObject record = new JSONObject();
                record.put("id_producto", resultSet.getString("id_producto"));
                record.put("descripcion", resultSet.getString("descripcion"));
                record.put("costo", resultSet.getInt("costo"));
                record.put("precio", resultSet.getInt("precio"));
                jsonArray.put(record);

                // Create XML element
                xmlBuilder.append("  <producto>\n");
                xmlBuilder.append("    <id_producto>").append(resultSet.getString("id_producto")).append("</id_producto>\n");
                xmlBuilder.append("    <descripcion>").append(resultSet.getString("descripcion")).append("</descripcion>\n");
                xmlBuilder.append("    <costo>").append(resultSet.getInt("costo")).append("</costo>\n");
                xmlBuilder.append("    <precio>").append(resultSet.getInt("precio")).append("</precio>\n");
                xmlBuilder.append("  </producto>\n");
            }

            xmlBuilder.append("</productos>");

            // Write JSON to file
            try (FileWriter file = new FileWriter("oracledb.json")) {
                file.write(jsonArray.toString(4));
            }

            // Write XML to file
            try (FileWriter file = new FileWriter("oracle.xml")) {
                file.write(xmlBuilder.toString());
            }

        } catch (Exception e) {
            // Show error message in a JPanel
            JOptionPane.showMessageDialog(null, "Error: " + e.getMessage(), "Error", JOptionPane.ERROR_MESSAGE);
            e.printStackTrace();
        }
    }
}
