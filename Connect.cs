using System.Data.SqlClient;

public class Connect {

  public static string connectionString() {
    string connectionString = "Data Source=localhost;Initial Catalog=pp;User Id=sa;Password=root;";
    return connectionString;
  }

  public static SqlConnection getConnection() {
    SqlConnection connection = new SqlConnection(Connect.connectionString());
    connection.Open();
    return connection;
  }

  public static void Insert(string query) {
    SqlConnection connection = Connect.getConnection();
    SqlCommand command = new SqlCommand(query, connection);
    command.ExecuteNonQuery();
    connection.Close();
  }

}