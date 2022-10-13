using System;
using System.Collections;
using MySqlAccess;
namespace BusinessLogic
{

    public class Logic
    {

        static string[] cars = { "Toyota Prius+", "Hundai i30", "Volvo sx40" };
        static string[] colors = { "Yellow", "White", "Black", "Green", "Transparent" };
        static string[] tasks = { "Service 10K", "Wheels", "BodyWork" };
        static string[] desc = { "Replace filets of oil,gasoline,air contidioner", "Change 4 tires", "fix scratchs" };
        static string[] names = { "Arkady", "Aharon", "Zeev", "Yehonatan" };
        static string[] cities = { "Jerusalem", "Tel Aviv", "Beeh Sheva", "Ariel" };

        static string[] types = { "Top", "Flavour", "Cup" };
        static string[] flavours = { "Chocolate", "Vanilla", "Mekupelet", "Strawbery", "Coconut", "Gummy", "Oreo", "Kinder", "Fistuk", "Caramel" };
        static string[] tops = { "Chocolate", "Pinuts", "Meipel" };
        static string[] cups = { "Regular", "Special", "Box" };
        static string[] IngridientsNames = { "Regular", "Special", "Box", "HotChoco", "Pinuts", "Meipel", "Top", "Flavour", "Cup", "Chocolate", "Vanilla", "Mekupelet", "Strawbery", "Coconut", "Gummy", "Oreo", "Kinder", "Fistuk", "Caramel" };


        public static void fillTables(int num)
        {

            //generate  values for types
            for (int i = 0; i < types.Length; i++)
            {
                String typeN = types[i];
                Type o = new Type(typeN);
                MySqlAccess.MySqlAccess.insertObject(o);
            }

            //generate  values for Ingridients

            int typeID = MySqlAccess.MySqlAccess.getTypeID("Top");
            for (int j = 0; j < tops.Length; j++)
            {
                Ingredient o = new Ingredient(tops[j], typeID);
                MySqlAccess.MySqlAccess.insertObject(o);
            }
            typeID = MySqlAccess.MySqlAccess.getTypeID("Flavour");
            for (int j = 0; j < flavours.Length; j++)
            {
                Ingredient o = new Ingredient(flavours[j], typeID);
                MySqlAccess.MySqlAccess.insertObject(o);
            }
            typeID = MySqlAccess.MySqlAccess.getTypeID("Cup");
            for (int j = 0; j < cups.Length; j++)
            {
                Ingredient o = new Ingredient(cups[j], typeID);
                MySqlAccess.MySqlAccess.insertObject(o);
            }


            
        }
        public static void executeQueries()
        {
            Console.WriteLine("Which query would you like to display?");
            Console.WriteLine("1- Create invoice for sale ID");
            Console.WriteLine("2- End of the day report");
            Console.WriteLine("3- Uncompleted sales.");
            Console.WriteLine("4- Most popular ingredient and flavour.");
            int userInput = Int32.Parse(Console.ReadLine());
            switch (userInput)
            {
                case 1:
                    Console.WriteLine("What is the sale ID you would like to get invoice for?");
                    int id_Sale = Int32.Parse(Console.ReadLine());
                    MySqlAccess.MySqlAccess.getInvoice(id_Sale);
                    break;
                case 2:
                    Console.WriteLine("Please enter the date of the day you would like to get it end of the day report. (The format need to be YYYY-MM-DD)");
                    string date = Console.ReadLine();
                    MySqlAccess.MySqlAccess.endOfDayReport(date);
                    break;
                case 3:
                    MySqlAccess.MySqlAccess.UncomplitedSales();
                    break;
                case 4:
                    MySqlAccess.MySqlAccess.MostPopularFlavour();
                    MySqlAccess.MySqlAccess.MostPopularIngredient();
                    break;
            }
        }

        public static void InsertSale()
        {//generate values for Sales number
            Console.WriteLine("Please enter your phone number:");
            string phoneNumber = Console.ReadLine();
            int id_Sale = 0;
            Sale o = new Sale(0, phoneNumber);//insert a row with price 0
            MySqlAccess.MySqlAccess.insertObject(o);
            id_Sale = MySqlAccess.MySqlAccess.getIDSale("idSale");//get the last id sale that appear in table
            Console.WriteLine("Please remember your sale number is " + id_Sale);
            int price = InsertOrder(id_Sale);
            Console.WriteLine("Do you finish your order and want to pay? (yes/no).");
            string costumerAnswer = Console.ReadLine();
            if (costumerAnswer == "yes")
            {
                MySqlAccess.MySqlAccess.updatePrice(price, id_Sale);
                Console.WriteLine("Your bill is: " + price);

            }

        }

        public static int InsertOrder(int id_Sale)
        {//function insert ingredients to order and return the sum of the order
            
            //Now I will choose random cup from 'cups' array
            Console.WriteLine("Please choose a cup:");
            for (int i = 0; i < cups.Length; i++)
            {
                Console.WriteLine(i+"- "+ cups[i]);
            }
            int cupKind = Convert.ToInt32(Console.ReadLine());
            int TID = MySqlAccess.MySqlAccess.getTypeID("Cup");
            int ingredID = MySqlAccess.MySqlAccess.getIngredID(TID, cups[cupKind]);
            Order o = new Order(id_Sale, ingredID, 1);//insert a row to orders table
            MySqlAccess.MySqlAccess.insertObject(o);
            int max_balls_num = 0;
            if (cups[cupKind] == "Box")
            {
                max_balls_num = 999;  //In box there is no limit
            }
            else//If rugular or special cup
            {
                max_balls_num = 3;//It is possible to put 0,1,2,3 balls  
            }
            Console.WriteLine("Please choose how many balls you would like to put. (Max number of "+ max_balls_num+")");
            int ballsNum = Convert.ToInt32(Console.ReadLine());


            String flavour_kind = "";
            int chosen_flavour = 0;
            ingredID = 0;
            TID = 0;
            Console.WriteLine("Please choose flavours from below:");
            for (int i = 0; i < flavours.Length; i++)
            {
                Console.WriteLine(i + "- " + flavours[i]);
            }
            for (int y = 0; y < ballsNum; y++)//Now we will choose the flavours of all balls we have 
            {
                Console.WriteLine("Please choose flavour number "+y);
                chosen_flavour = Convert.ToInt32(Console.ReadLine());
                flavour_kind = flavours[chosen_flavour];
                TID = MySqlAccess.MySqlAccess.getTypeID("Flavour");
                ingredID = MySqlAccess.MySqlAccess.getIngredID(TID, flavour_kind);
                Order ob = new Order(id_Sale, ingredID, 1);//insert a row to orders table
                MySqlAccess.MySqlAccess.insertObject(ob);
                //Console.WriteLine(flavour_kind);
            }
            int MaxtopNum = 0;
            if (ballsNum >= 2 && cups[cupKind] == "Regular")
            {
                MaxtopNum = 1;
            }
            else if (ballsNum < 2 && cups[cupKind] == "Regular")
            {
                MaxtopNum = 0;
            }
            else { MaxtopNum = 999; }
            int topNum = 0;
            if (MaxtopNum > 0)
            {
                Console.WriteLine("Please choose how many tops you would like to put. (Max number of " + MaxtopNum + ")");
                topNum = Convert.ToInt32(Console.ReadLine());

                int howManyChocolate = MySqlAccess.MySqlAccess.CountFlavours("Chocolate", id_Sale);
                int howManyMekupelet = MySqlAccess.MySqlAccess.CountFlavours("Mekupelet", id_Sale);
                int howManyVanilla = MySqlAccess.MySqlAccess.CountFlavours("Vanilla", id_Sale);
                String forbiddenTops = "\" \"";
                if (howManyChocolate != 0 || howManyMekupelet != 0)
                {
                    forbiddenTops = forbiddenTops + ",\"Chocolate\"";
                }
                if (howManyVanilla != 0)
                {
                    forbiddenTops = forbiddenTops + ",\"Meipel\"";
                }
                ArrayList allowedTops = MySqlAccess.MySqlAccess.getAllTops(forbiddenTops);
                Console.WriteLine("All allowed tops you can add:");
                int k = 0;
                foreach (Object[] row in allowedTops)
                {
                    Console.WriteLine(k + "-" + (string)row[1]);
                    k++;
                }
                int chosen_top = 0;
                for (int j = 0; j < topNum; j++)
                {
                    Console.WriteLine("Please choose top number " + j);
                    chosen_top = Convert.ToInt32(Console.ReadLine());
                    object[] row = (object[])allowedTops[chosen_top];
                    ingredID = (int)row[0];
                    Order ob = new Order(id_Sale, ingredID, 1);//insert a row to orders table
                    MySqlAccess.MySqlAccess.insertObject(ob);
                }
            }
            
            //compute the price for the order
            int price = calculatePrice(ballsNum, cups[cupKind], topNum);

            return price;
        }
        public static int calculatePrice(int Numofballs,string cupName, int numOfTops)
            //function calculate the price for each sale event 
        {
            int price = 0;
            //price for balls
            if (Numofballs ==1)
            {
                price =  7;
            }
            else if (Numofballs ==2)
            {
                price =  12;
            }
            else if (Numofballs == 3)
            {
                price =  18;
            }
            else if (Numofballs>3)
            {
                price =  18 + (6 * (Numofballs - 3));
            }
            //price for cup
            if (cupName=="Box")
            {
                price = price + 5;
            }
            else if (cupName=="Special")
            {
                price = price + 2;
            }
            //price for tops
            price = price + (2 * numOfTops);
            //Console.WriteLine(price);   
            return price;

        }


        public static ArrayList getTableData(string tableName)
        {
            ArrayList all = MySqlAccess.MySqlAccess.readAll(tableName);
            ArrayList results = new ArrayList();

            if (tableName == "Types")
            {
                foreach (Object[] row in all)
                {
                    Type o = new Type((string)row[1]);
                    results.Add(o);
                }
            }

            if (tableName == "Tasks")
            {
                foreach (Object[] row in all)
                {
                    Ingredient o = new Ingredient((string)row[1],(int)row[3]);
                    results.Add(o);
                }
            }

            if (tableName == "Vehicles")
            {
                foreach (Object[] row in all)
                {
                    Order o = new Order((int)row[1], (int)row[2], (int)row[3]);
                    results.Add(o);
                }
            }

            return results;
        }

    }







    // data holder classes (theoreticaly may be a struct ?)
    class Type
    {
        string Tname;
  

        public Type(string name)
        {
            this.Tname = name;
           
        }

        public string getName() { return Tname; }
        

        public override string ToString()
        {
            return base.ToString() + ": " + Tname; 
        }
    }

    class Sale
    {
        
        int price;
        string phoneNum;
        public Sale(int price, string phoneNum)
        {
            this.price = price;
            this.phoneNum = phoneNum;

        }
 
        public int getPrice() { return price; }
        public string getPhoneNum() { return phoneNum; }


    }
    class Order
    {
        int idIngredient;
        int idSale;
        int Ingredient_Amount;

        public Order(int idIngredient, int idSale, int Ingredient_Amount)
        {
            this.idIngredient = idIngredient;
            this.idSale = idSale;
            this.Ingredient_Amount = Ingredient_Amount;
        }
        public int getidIngredient() { return idIngredient; }
        public int getidSale() { return idSale; }
        public int getIngredient_Amount() { return Ingredient_Amount; }

        public override string ToString()
        {
            return base.ToString() + ": " + idIngredient + " , " + idSale + " , " + Ingredient_Amount;
        }
    }


    class Ingredient
    {
        string Iname;
        int TypeID;

        public Ingredient(string Iname, int TypeID)
        {
            this.Iname = Iname;
            this.TypeID = TypeID;
        }

        public string getIName() { return Iname; }
        public int getTypeID() { return TypeID; }

        public override string ToString()
        {
            return base.ToString() + ": " + Iname +  " , " + TypeID;
        }
    }
}