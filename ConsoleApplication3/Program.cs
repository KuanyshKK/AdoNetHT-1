using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace ConsoleApplication3
{
    class Program
    {
        SqlConnection conn = null;

        public Program()
        {
            conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
        }
        static void Main(string[] args)
        {
            Program pr = new Program();
            //pr.InsertQuery();
            //pr.InsertQuery2();
            //pr.Reader();
            pr.Reader2();
            pr.ExecStoredProcedure();

        }

        public void InsertQuery()
        {
            try
            {
                conn.Open();

                string[] insertString = new string[3];
                insertString[0] = @"insert into
                        Authors (FirstName, LastName)
                        values
                            ('Abai', 'Qunanbai')";
                insertString[1] = @"insert into
                        Authors (FirstName, LastName)
                        values
                            ('Teodor', 'Draizer')";
                insertString[2] = @"insert into
                        Authors (FirstName, LastName)
                        values
                            ('Stephane', 'King')";

                foreach(string insert in insertString)
                {
                    SqlCommand cmd = new SqlCommand(insert, conn);
                    cmd.ExecuteNonQuery();
                }
                                                  
            }
            finally
            {
                if(conn!=null)
                {
                    conn.Close();
                }
            }
        }

        public void InsertQuery2()
        {
            try
            {
                conn.Open();

                string[] insertString = new string[3];
                insertString[0] = @"insert into Books
                                    (Authorid, Title, PRICE, PAGES)
                                     values ('1','Kara sozder','10','100')";
                insertString[1] = @"insert into Books
                                    (Authorid, Title, PRICE, PAGES)
                                     values ('2','Finansist','10','300')";
                insertString[2] = @"insert into Books
                                    (Authorid, Title, PRICE, PAGES)
                                     values ('3','SomeTitle','10','200')";

                foreach (string insert in insertString)
                {
                    SqlCommand cmd = new SqlCommand(insert, conn);
                    cmd.ExecuteNonQuery();
                }

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

        }

        public void Reader()
        {
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("select * from Authors", conn);

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[1]+" "+rdr[2]);
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null){
                    conn.Close();
                }
            }
        }

        public void Reader2()
        {
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Authors; select * from Books", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;
                do
                {
                    while (rdr.Read())
                    {
                        if (line == 0)
                        {
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                Console.Write(rdr.GetName(i).ToString() + "\t");
                            }
                            Console.WriteLine();
                        }
                        line++;
                        Console.WriteLine(rdr[0] + "\t" + rdr[1] + "\t" + rdr[2]);
                    }
                    Console.WriteLine("Total records processed: " + line.ToString());
                } while (rdr.NextResult());
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public void ExecStoredProcedure()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("getBooksNumber", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AuthorId",
                System.Data.SqlDbType.Int).Value = 1;

            SqlParameter outputParam = new SqlParameter("@BookCount", System.Data.SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();
            Console.WriteLine(cmd.Parameters["@BookCount"].Value.ToString());
        }
    }
}
