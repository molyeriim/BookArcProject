using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
namespace BookArcProject
{
    internal class Database
    {
        static SqlConnection con;
        static SqlDataAdapter da;
        static SqlCommand cmd;
        static SqlDataReader dr;
        static System.Data.DataSet ds;
        public static string SqlCon = @"Data Source = XAIVER\SQLEXPRESS;Initial Catalog = BookArchive; Integrated Security = True";
        public static bool BağlantiDurum()
        {
            //database baglantı kontrolu
            using (con = new SqlConnection(SqlCon))
            {
                try
                {
                    con.Open();

                    return true;
                }

                catch (SqlException exp)
                {
                    System.Windows.Forms.MessageBox.Show(exp.Message);
                    return false;
                }

            }

        }

        public static bool LoginControl(string kullaniciadi, string sifre)
        {

            string sorgu = "select * from User where uNickname= @user and uPass= @password";
            con = new SqlConnection(SqlCon);
            cmd = new SqlCommand(sorgu, con);
            cmd.Parameters.AddWithValue("@user", kullaniciadi);
            cmd.Parameters.AddWithValue("@password", Database.MD5Sifrele(sifre));
            con.Open();
            dr = cmd.ExecuteReader();          //yürütme okuyucusu,sorguyu çalıştırır

            if (dr.Read())      //veri geldiyse
            {

                con.Close();
                return true;
            }

            else
            {
                //MessageBox.Show("kullanıcı adı veya şifre hatalı..");
                con.Close();
                return false;
            }
        }

        public static string MD5Sifrele(string sifrelenecekMetin)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] dizi = Encoding.UTF8.GetBytes(sifrelenecekMetin);
            dizi = md5.ComputeHash(dizi);   //dizinin hash değeri çıkarılıyor

            StringBuilder sb = new StringBuilder();
            foreach (byte item in dizi)
                sb.Append(item.ToString("x2").ToLower());
            return sb.ToString();

        }





        //public static DataGridView GridTümünüDoldur(DataGridView gridim, string sqlSelectSorgu) //bir tablonun hepsini seçeceksek yazılacak sorgudur bu 
        //{
        //    con = new SqlConnection(SqlCon);
        //    da = new SqlDataAdapter("select * from " + sqlSelectSorgu, con);
        //    ds = new System.Data.DataSet();
        //    con.Open();
        //    da.Fill(ds, sqlSelectSorgu);

        //    gridim.DataSource = ds.Tables[sqlSelectSorgu];//burada tablo adı string olarak gelir 
        //    //gridim.DataSource = ds.Tables[0];           //burada tablo indexine göre gelir
        //    con.Close();
        //    return gridim;

        //}

        public static void KomutYolla(string sql)
        {
            con = new SqlConnection(SqlCon);
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            con.Close();

        }

    }
}
