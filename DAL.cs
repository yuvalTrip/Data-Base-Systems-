using MySql.Data;
using MySql.Data.MySqlClient;

using BusinessLogic;
using System.Collections;
using System;

namespace MySqlAccess
{
    class MySqlAccess
    {

        static string connStr = "server=localhost;user=root;port=3306;password=1q2w3e4r";

        /*
        this call will represent CRUD operation
        CRUD stands for Create,Read,Update,Delete
        */
        public static void createTables()
        {

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();


                string sql = "DROP DATABASE IF EXISTS Ice_Cream_Shop;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "CREATE DATABASE Ice_Cream_Shop;";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                // create Ingredient
                sql = "CREATE TABLE `Ice_Cream_Shop`.`Ingredients` (" +
             "`idIngredient` INT NOT NULL AUTO_INCREMENT, " +
             "`IngredientName` VARCHAR(45) NULL," +
             "`TypeID` INT NULL," +
             "PRIMARY KEY (`idIngredient`));";


                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                // create types
                sql = "CREATE TABLE `Ice_Cream_Shop`.`Types` (" +
             "`idType` INT NOT NULL AUTO_INCREMENT, " +
             "`TypeName` VARCHAR(45) NOT NULL," +
             "PRIMARY KEY (`idType`));";

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                // create  Sales
                sql = "CREATE TABLE `Ice_Cream_Shop`.`Sales` (" +
           "`idSale` INT NOT NULL AUTO_INCREMENT, " +
           "`Date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP," +
           "`Price` INT NULL," +
           "`Phone_Number` VARCHAR(45) NOT NULL," +
           "PRIMARY KEY (`idSale`));";

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                // create Orders
                sql = "CREATE TABLE `Ice_Cream_Shop`.`Orders` (" +
           "`idOrder` INT NOT NULL AUTO_INCREMENT, " +
           "`idSale` INT NULL," +
           "`idIngredient` INT NULL," +
           "`Ingredient_Amount` INT NULL," +
           "PRIMARY KEY (`idOrder`));";
               

                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    
     
        public static void insertObject(Object obj)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = null;

                if (obj is BusinessLogic.Type)
                {
                    BusinessLogic.Type type = (BusinessLogic.Type)obj;
                    sql = "INSERT INTO `Ice_Cream_Shop`.`Types` (`TypeName`) " +
                    "VALUES ('" + type.getName() + "');";
                }


                if (obj is BusinessLogic.Order)
                {
                    BusinessLogic.Order order = (BusinessLogic.Order)obj;
                    sql = "INSERT INTO `Ice_Cream_Shop`.`Orders` (`idSale`,`idIngredient`,`Ingredient_Amount`)  " +
                    "VALUES ('" + order.getidIngredient() + "', '" + order.getidSale() + "', '" + order.getIngredient_Amount() + "');";
                }

                if (obj is BusinessLogic.Ingredient)
                {
                    BusinessLogic.Ingredient task = (BusinessLogic.Ingredient)obj;
                    sql = "INSERT INTO `Ice_Cream_Shop`.`Ingredients` (`IngredientName`, `TypeID`) " +
                    "VALUES ('" + task.getIName() + "', '" + task.getTypeID() + "');";
                }

                if (obj is BusinessLogic.Sale)
                {
                    BusinessLogic.Sale sale = (BusinessLogic.Sale)obj;
                    sql = "INSERT INTO `Ice_Cream_Shop`.`sales` (Price,Phone_Number)  " +
                    "VALUES (" + sale.getPrice() + ", \""+sale.getPhoneNum()+ "\");";
                }

                //Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static void updatePrice (int price, int id_Sale)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            //Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            string sql="UPDATE `Ice_Cream_Shop`.`sales` SET Price ="+ price+ " WHERE idSale ="+ id_Sale;
            //Console.WriteLine(sql);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public static int  CountFlavours(String flavour,int idSale)
        {
            ArrayList all = new ArrayList();

            //Function get flavour string and return how many flavours there are in the DB
            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT COUNT(*) FROM `Ice_Cream_Shop`.`orders`,`Ice_Cream_Shop`.`ingredients`, `Ice_Cream_Shop`.`types`" +
                                        " WHERE `Ice_Cream_Shop`.`orders`.`idSale`=" + idSale +
                                        " AND `Ice_Cream_Shop`.`ingredients`.`idIngredient`= `Ice_Cream_Shop`.`orders`.`idIngredient`" +
                                        " AND `Ice_Cream_Shop`.`ingredients`.`IngredientName`= \"" + flavour + "\" " +
                                        " AND `Ice_Cream_Shop`.`ingredients`.`TypeID`=`Ice_Cream_Shop`.`types`.`idType`" +
                                        " AND `Ice_Cream_Shop`.`types`.`TypeName`= \"Flavour\"";
                //Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            Object[] row = (object[])all[0];
            Console.WriteLine(row[0]);
            return (int)(long)row[0];//https://www.oreilly.com/library/view/mysql-cookbook/0596001452/ch01s15.html
    }
        public static ArrayList getAllTops(String forbiddenTops)
        {
            ArrayList all = new ArrayList();
            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql= "SELECT `Ice_Cream_Shop`.`ingredients`.* from `Ice_Cream_Shop`.`ingredients`,`Ice_Cream_Shop`.`types` WHERE `IngredientName` NOT IN(" + forbiddenTops + ") AND `Ice_Cream_Shop`.`ingredients`.`TypeID` = `Ice_Cream_Shop`.`types`.`idType` AND `Ice_Cream_Shop`.`types`.`TypeName` =\"Top\"";
                //Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return all;
    
        }


        public static ArrayList readAll(string tableName)
        {
            ArrayList all = new ArrayList();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT * FROM `Ice_Cream_Shop`." + tableName;
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return all;
        }
        public static int getTypeID(string typeName)
        {
            ArrayList all = new ArrayList();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT * FROM `Ice_Cream_Shop`.Types WHERE TypeName=\"" + typeName + "\"";
                //Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            Object[] row= (object[])all[0];
            return (int)row[0];
        }

        public static int getIDSale(string colName) 
        {//function get the last id value in column of idSale
            ArrayList all_ = new ArrayList();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT MAX(idSale) FROM `Ice_Cream_Shop`.Sales";////OR should it be idSale=colName? 
                //Console.WriteLine(sql);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all_.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Object[] row = (object[])all_[0];
            int tid = (int)row[0];
            return (int)row[0];
        }

        public static int getIngredID(int Typeid ,string ingridientName)
        {
            ArrayList all_ = new ArrayList();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT * FROM `Ice_Cream_Shop`.`ingredients` WHERE TypeID=" + Typeid+" AND IngredientName=\""+ingridientName+"\""; //+ typeName + "\""///OR should it be idSale=colName? 
                //Console.WriteLine(sql);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all_.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Object[] row = (object[])all_[0];
            return (int)row[0];
        }
        public static void MostPopularFlavour ()
        {
            ArrayList all_ = new ArrayList();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql="SELECT IngredientName, COUNT(*) FROM `Ice_Cream_Shop`.`orders`,`Ice_Cream_Shop`.`ingredients`, `Ice_Cream_Shop`.`types` WHERE `Ice_Cream_Shop`.`ingredients`.`TypeID`=`Ice_Cream_Shop`.`types`.`idType` AND `Ice_Cream_Shop`.`orders`.`idIngredient`=`Ice_Cream_Shop`.`ingredients`.`idIngredient` AND TypeName = \"Flavour\" GROUP BY IngredientName HAVING COUNT(*) = (SELECT COUNT(IngredientName) AS Quantity FROM  `Ice_Cream_Shop`.`orders`,`Ice_Cream_Shop`.`ingredients`, `Ice_Cream_Shop`.`types` WHERE `Ice_Cream_Shop`.`ingredients`.`TypeID`=`Ice_Cream_Shop`.`types`.`idType` AND `Ice_Cream_Shop`.`orders`.`idIngredient`=`Ice_Cream_Shop`.`ingredients`.`idIngredient` AND TypeName = \"Flavour\" GROUP BY IngredientName ORDER BY COUNT(IngredientName) desc limit 1)";

                //Console.WriteLine(sql);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all_.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            ArrayList allPopularFlavours= new ArrayList();
            int i = 0;
            foreach (object obj in all_)
            {
                Object[] row = (object[])all_[i];
                i++;
                string MostpopulerFlavour = (string)row[0];
                allPopularFlavours.Add(MostpopulerFlavour);
            }
           
            Console.WriteLine("The most popular flavours:");
            //print the list
            foreach (Object[] row in all_)
            {
                Console.WriteLine((string)row[0]);
            }

        }
        public static void MostPopularIngredient()
        {
            ArrayList all_ = new ArrayList();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql= "SELECT IngredientName,`Ice_Cream_Shop`.`orders`.`idIngredient`,COUNT(*) FROM  `Ice_Cream_Shop`.`orders`,`Ice_Cream_Shop`.`ingredients`, `Ice_Cream_Shop`.`types`WHERE `Ice_Cream_Shop`.`ingredients`.`TypeID`=`Ice_Cream_Shop`.`types`.`idType` AND `Ice_Cream_Shop`.`orders`.`idIngredient`=`Ice_Cream_Shop`.`ingredients`.`idIngredient` GROUP BY `Ice_Cream_Shop`.`orders`.`idIngredient` HAVING COUNT(*)= (SELECT COUNT(`Ice_Cream_Shop`.`orders`.`idIngredient`) AS Quantity FROM  `Ice_Cream_Shop`.`orders`,`Ice_Cream_Shop`.`ingredients`, `Ice_Cream_Shop`.`types` WHERE `Ice_Cream_Shop`.`ingredients`.`TypeID`=`Ice_Cream_Shop`.`types`.`idType` AND `Ice_Cream_Shop`.`orders`.`idIngredient`=`Ice_Cream_Shop`.`ingredients`.`idIngredient` GROUP BY `Ice_Cream_Shop`.`orders`.`idIngredient` ORDER BY COUNT(`Ice_Cream_Shop`.`orders`.`idIngredient`) desc limit 1)";
                //Console.WriteLine(sql);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all_.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            ArrayList allPopularIngredient = new ArrayList();
            int i = 0;
            foreach (object obj in all_)
            {
                Object[] row = (object[])all_[i];
                i++;
                string MostpopulerFlavour = (string)row[0];
                allPopularIngredient.Add(MostpopulerFlavour);
            }
            Console.WriteLine("The most popular ingredient:");
            foreach (Object[] row in all_)
            {
                Console.WriteLine((string)row[0]);
            }
        }
        public static void UncomplitedSales()
        {
            ArrayList all_ = new ArrayList();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * from `Ice_Cream_Shop`.`sales` WHERE Price=0";
                //Console.WriteLine(sql);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all_.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("There are " + all_.Count + " uncompleted orders");
            
            foreach (Object[] curr_row in all_)
            {
                Console.WriteLine("SaleID: "+(string)curr_row[0]+ ", Date: " + (string)curr_row[1]+", Phone Number: " + (string)curr_row[3]);
            }

        }
        public static void endOfDayReport (string date)
        {
            //first we will count the number of sales in the specific date
            ArrayList all_ = new ArrayList();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql= "SELECT COUNT(Date) AS Number_of_sales, Date, SUM(Price) AS Total_Price FROM `Ice_Cream_Shop`.`sales` WHERE date_format(`Date`,'%Y-%m-%d')= \"" + date + "\" GROUP BY date_format(`Date`,'%Y-%m-%d')";
                //Console.WriteLine(sql);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all_.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Object[] row = (object[])all_[0];
            int numOfSales_PerDay= (int)(long)row[0];
            Console.WriteLine("The date and the time of the orders is:" + (Convert.ToString(row[1])));
            Console.WriteLine("Total sales number for this date:" + (int)(long)row[0]);
            Console.WriteLine("Total price for all sales in this date:" + row[2]);

            int sum = Convert.ToInt32(row[2]);
            int quantity = Convert.ToInt32((int)(long)row[0]);
            Console.WriteLine("Averege price per sale:" + sum / quantity);
        }
        public static void getInvoice(int id_sale)
        //Customer invoice (including date, price)

        {
            ArrayList all_ = new ArrayList();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT Date,SUM(Price) AS Total_Price FROM `Ice_Cream_Shop`.`sales` WHERE `idSale`="+ id_sale;
                //Console.WriteLine(sql);

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Object[] numb = new Object[rdr.FieldCount];
                    rdr.GetValues(numb);
                    all_.Add(numb);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Object[] row = (object[])all_[0];
            Console.WriteLine("The date of the order and the time when the sale has been started is:"+ (Convert.ToString(row[0])));
            Console.WriteLine("The price of the order is:"+ row[1]);

        }
    }


}

