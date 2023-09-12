using MySql.Data.MySqlClient;
using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocketGameServer.DAO
{
    class UserData
    {
        private MySqlConnection mysqlCon;

        private string connstr = "database=sys;data source=localhost;user=root;password=root;pooling=false;charset=utf8;port=3306";

        public UserData()
        {
            ConnectMysql();
        }

        public MySqlConnection GetMysqlcon
        {
            get
            {
                return mysqlCon;
            }
        }
        private void ConnectMysql()
        {
            try
            {
                mysqlCon = new MySqlConnection(connstr);
                mysqlCon.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine("连接数据库失败!"+e.Message);
            }
        }

        public bool Register(MainPack pack)
        {
            string username = pack.Loginpack.Username;

            string password = pack.Loginpack.Password;

            //string sql = "SELECT * FROM sys.userdata where username='@username'";
            //MySqlCommand comd = new MySqlCommand(sql, mysqlCon);
            //MySqlDataReader read = comd.ExecuteReader();
            //if(read.Read())
            //{
            //    //用户名已被注册
            //    return false;
            //}
            //read.Close();
            string sql = "INSERT INTO `sys`.`userdata` (`username`,`password`) VALUES ('" + username + "','"+password+"')";
            MySqlCommand comd = new MySqlCommand(sql, mysqlCon);
            try
            {
                comd.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            
        }


        public bool Logon(MainPack pack)
        {
            string username = pack.Loginpack.Username;
            string password = pack.Loginpack.Password;

            string sql = "SELECT * FROM userdata WHERE username='"+username+"' AND password='"+password+"'";

            MySqlCommand comd = new MySqlCommand(sql, mysqlCon);

            MySqlDataReader reader = comd.ExecuteReader();
            bool res = reader.Read();
            reader.Close();
            return res;
        }
    }
}
