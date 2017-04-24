using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace maturityXML
{

    class CreateXML
    {
        public static double CreateMFee(string PolNo)
        {
            //Takes the first letter of the string and determines management fee % to return
            var firstPolNo = PolNo.Substring(0, 1);
            switch (firstPolNo)
            {
                case "A":
                    return 0.03;
                    break;
                case "B":
                    return 0.05;
                    break;
                default:
                    return 0.07;
                    break;
            }
        }

        public static double CalculateMaturity(double prem, double mFee, double disBonus, double upLift)
        {
            //Calculates maturity value
            return (prem - (prem * mFee) + disBonus) * upLift;
        }

        static void Main(string[] args)
        {
            // Reads each line from CSV and creates an array
            if (File.Exists(@"C:\Users\Public\MaturityData.csv"))
            {
                var lines = File.ReadAllLines(@"C:\Users\Public\MaturityData.csv");

                //Creates Root of XML
                XElement XmlExp = new XElement("Root",
                    new XAttribute("Maturity_values", "Numbers"));

                //Loops through each line of CSV
                for (int i = 1; i < lines.Length; i++)
                {
                    //Splits each line into an array
                    string[] row = lines[i].Split(',').ToArray();
                    //Adds policy number into the XML
                    XElement XmlExpa = new XElement("policy_number", row[0]);
                    //Sends to CreateMFee function to check percentage of management fee
                    double mFee = CreateMFee(row[0]);
                    //Runs the maturity value formula, converts to a string, adds a £ sign and rounds up to no decimal places
                    double mature = Math.Round(CalculateMaturity(Convert.ToDouble(row[2]), mFee, Convert.ToDouble(row[4]), Convert.ToDouble(row[5])), 0);
                    string matureStr = "£" + mature.ToString();
                    //Adds maturity value into XML
                    XElement XmlExpb = new XElement("maturity_value", matureStr);
                    //Adds the elements into the main XML variable
                    XmlExp.Add(XmlExpa);
                    XmlExp.Add(XmlExpb);
                }

                //Saves XML to system
                XmlExp.Save(@"C:\Users\Public\MaturityXML.xml");
            }

        }
    }
}
