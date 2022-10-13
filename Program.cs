using System;
using System.Data;
using System.Diagnostics;//used for Stopwatch class

using MySql.Data;
using MySql.Data.MySqlClient;

using MySqlAccess;
using BusinessLogic;
using System.Collections;
//Yuval Ben Simhon , 318916335
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Welcome To My Ice Cream Shop!");
Console.WriteLine("The current time is " + DateTime.Now);

Stopwatch stopwatch = new Stopwatch();

int userInput = 0;
do
{
    
    Console.WriteLine("Please choose a task:");
    Console.WriteLine("0 - create a new DB");
    Console.WriteLine("1-To make an order of ice-cream");
    Console.WriteLine("2-To answer the queries");
    Console.WriteLine("");
    Console.WriteLine("(-1) - for exit");

    userInput = Int32.Parse(Console.ReadLine());

    switch (userInput)
    {
        case 0:
            MySqlAccess.MySqlAccess.createTables();
            BusinessLogic.Logic.fillTables(10);

            break;

        case 1:
            BusinessLogic.Logic.InsertSale();

            break;

        case 2:
            BusinessLogic.Logic.executeQueries();
            break;
    }

} while (userInput != -1);

Console.WriteLine("Thank you for your time");


