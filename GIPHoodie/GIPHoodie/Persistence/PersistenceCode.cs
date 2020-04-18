using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIPHoodie.Models;
using MySql.Data.MySqlClient;

namespace GIPHoodie.Persistence
{
    public class PersistenceCode
    {
        string connStr = "server=localhost;user id=root;password=Maxart2494;database=dbwebshop";

        public List<Artikel> loadArtikels() //laden van de verschillende artikels in de webshop
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select * from tblartikel";
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();
            List<Artikel> lijst = new List<Artikel>();
            while (dtr.Read())
            {
                Artikel artikel = new Artikel();
                artikel.ArtikelID = Convert.ToInt32(dtr["artikelid"]);
                artikel.Naam = Convert.ToString(dtr["naam"]);
                artikel.Voorraad = Convert.ToInt32(dtr["voorraad"]);
                artikel.Prijs = Convert.ToDouble(dtr["prijs"]);
                artikel.Foto = Convert.ToString(dtr["foto"]);
                lijst.Add(artikel);
            }
            conn.Close();
            return lijst;
        }


        public Artikel loadArtikel(int ArtID) //laden van het geselecteerde artikel om deze in de winkelmand toe te voegen
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select * from tblartikel where artikelid=" + ArtID;
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();
            Artikel artikel = new Artikel();
            while (dtr.Read())
            {
                artikel.ArtikelID = Convert.ToInt32(dtr["artikelid"]);
                artikel.Naam = Convert.ToString(dtr["naam"]);
                artikel.Voorraad = Convert.ToInt32(dtr["voorraad"]);
                artikel.Prijs = Convert.ToDouble(dtr["prijs"]);
                artikel.Foto = Convert.ToString(dtr["foto"]);
            }
            conn.Close();
            return artikel;
        }

        public void PasMandAan(WinkelmandItem winkelmanditem) // een geselecteerd artikel in de database in een winkelmand opslaan of aanpassen
        {
            MySqlConnection conn = new MySqlConnection(connStr);  //KlantID=" + winkelmanditem.KlantNr + " and 
            conn.Open();
            string qry1 = "select * from tblwinkelmand where KlantID=" + winkelmanditem.KlantNr + " and  ArtikelID="
                + winkelmanditem.ArtikelNr;
            MySqlCommand cmd = new MySqlCommand(qry1, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();


            bool mand;
            if (dtr.HasRows)
            {

                mand = true;
            }
            else
            {
                mand = false;

            }
            conn.Close();

            conn.Open();
            if (mand == true)
            {
                string qry2 = "update tblwinkelmand SET Aantal = Aantal +'" + winkelmanditem.Aantal + "' where(KlantID = '" + winkelmanditem.KlantNr + "') and(ArtikelID = '" + winkelmanditem.ArtikelNr + "')";
                MySqlCommand cmd2 = new MySqlCommand(qry2, conn);
                cmd2.ExecuteNonQuery();

            }
            else
            {
                string qry3 = "insert into tblwinkelmand(KlantID, ArtikelID, Aantal) values(" + winkelmanditem.KlantNr +
                 "," + winkelmanditem.ArtikelNr + "," + winkelmanditem.Aantal + ")";
                MySqlCommand cmd3 = new MySqlCommand(qry3, conn);
                cmd3.ExecuteNonQuery();

            }
            conn.Close();
            conn.Open();
            string qry4 = "update tblartikel set Voorraad = Voorraad - '" + winkelmanditem.Aantal + "' where ArtikelID=" + winkelmanditem.ArtikelNr;
            MySqlCommand cmd4 = new MySqlCommand(qry4, conn);
            cmd4.ExecuteNonQuery();
            conn.Close();
        }



        public List<WinkelmandItem> MandOphalen(int KlantID) //het ophalen van alle artikelen die de klant in zijn mand heeft gezet.
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select tblartikel.ArtikelID,Naam,aantal,foto,prijs,round((prijs*aantal),2) as totaal " +
                "from tblartikel inner join tblwinkelmand on tblartikel.artikelID = tblwinkelmand.artikelID where klantID=" + KlantID;
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();
            List<WinkelmandItem> lijst = new List<WinkelmandItem>();

            while (dtr.Read())
            {
                WinkelmandItem winkelmanditem = new WinkelmandItem();
                winkelmanditem.ArtikelNr = Convert.ToInt32(dtr["ArtikelID"]);
                winkelmanditem.naam = Convert.ToString(dtr["Naam"]);
                winkelmanditem.Aantal = Convert.ToInt32(dtr["Aantal"]);
                winkelmanditem.Prijs = Convert.ToDouble(dtr["Prijs"]);
                winkelmanditem.Foto = Convert.ToString(dtr["Foto"]);
                winkelmanditem.totaal = Convert.ToDouble(dtr["totaal"]);
                lijst.Add(winkelmanditem);
            }
            conn.Close();
            return lijst;
        }



        public Klant KlantOphalen(int klantid) //gegeven van de klant ophalen en vanboven bij de winkelmand in een tabel zetten
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select * from tblklanten where KlantID=" + klantid;
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();
            Klant klant = new Klant();
            while (dtr.Read())
            {
                klant.KlantID = Convert.ToInt32(dtr["KlantID"]);
                klant.Naam = Convert.ToString(dtr["Naam"]);
                klant.Adres = Convert.ToString(dtr["Adres"]);
                klant.Gemeente = Convert.ToString(dtr["Gemeente"]);
                klant.PostCode = Convert.ToInt32(dtr["Postcode"]);
                klant.Mail = Convert.ToString(dtr["Mail"]);
            }
            conn.Close();
            return klant;
        }

        public bool MandChecken(int klantid) //kijken of er iets in de mand zit
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select * from tblwinkelmand where KlantID=" + klantid;

            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();
            if (dtr.HasRows == true)
            {
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }



        public void Verwijder(WinkelmandItem winkelmandItem) // het verwijderen van een artikel uit de winkelmand, het aantal verwijderde artikels bij aantal terug toevoegen.
        {


            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry1 = "update tblartikel SET voorraad =  voorraad + " + winkelmandItem.Aantal + " where(ArtikelID = " + winkelmandItem.ArtikelNr + ")";
            MySqlCommand cmd1 = new MySqlCommand(qry1, conn);
            cmd1.ExecuteNonQuery();
            conn.Close();


            conn.Open();
            string qry2 = "delete from tblwinkelmand where KlantID='" + winkelmandItem.KlantNr + "'and ArtikelID =" + winkelmandItem.ArtikelNr;
            MySqlCommand cmd2 = new MySqlCommand(qry2, conn);
            cmd2.ExecuteNonQuery();
            conn.Close();


        }



        public Totaal BerekenTotaal(int klantid) // kijken of er artikels in winkelmand zitten, zo wel de: btw, totaalincl, totaalexcl berekenen.
        {

            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry1 = "select * from tblwinkelmand where klantid=" + klantid;
            MySqlCommand cmd = new MySqlCommand(qry1, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();


            bool check;
            if (dtr.HasRows)
            {

                check = true;
            }
            else
            {
                check = false;

            }
            conn.Close();

            conn.Open();
            Totaal totaal = new Totaal();
            if (check == true)
            {
                string qry2 = "select sum((prijs * aantal)) as totaalexcl, sum(((prijs * aantal)*0.21)) as btw, sum( ((prijs * aantal)*1.21)) as totaalincl from tblartikel inner join tblwinkelmand on tblwinkelmand.ArtikelID = tblArtikel.ArtikelID where klantid =" + klantid;
                ;
                MySqlCommand cmd2 = new MySqlCommand(qry2, conn);
                MySqlDataReader dtr2 = cmd2.ExecuteReader();


                while (dtr2.Read())
                {
                    totaal.BTW = Math.Round(Convert.ToDouble(dtr2["btw"]), 2);
                    totaal.TotaalExcl = Math.Round(Convert.ToDouble(dtr2["totaalexcl"]), 2);
                    totaal.TotaalIncl = Math.Round(Convert.ToDouble(dtr2["totaalincl"]), 2);


                }

            }
            else
            {
                string qry3 = "select sum((prijs *0)) as totaalexcl, sum(((prijs * 0)*0.21)) as btw, sum( ((prijs * 0)*1.21)) as totaalincl from tblartikel inner join tblwinkelmand on tblwinkelmand.ArtikelID = tblArtikel.ArtikelID where klantid=" + klantid;
                ;
                MySqlCommand cmd3 = new MySqlCommand(qry3, conn);
                cmd.ExecuteNonQuery();



                totaal.BTW = 0;
                totaal.TotaalExcl = 0;
                totaal.TotaalIncl = 0;




            }
            conn.Close();
            return totaal;

        }



        public int AantalOphalen(WinkelmandItem winkelmandItem) //aantal dat naar de winkelmand wordt gestuurd vanuit de catalogus.
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select aantal from tblwinkelmand where ArtikelID = '" + winkelmandItem.ArtikelNr + "'";
            MySqlCommand cmd = new MySqlCommand(qry, conn);

            MySqlDataReader dtr = cmd.ExecuteReader();
            int Aantal = 0;
            while (dtr.Read())
            {
                Aantal = Convert.ToInt32(dtr["aantal"]);
            }
            conn.Close();
            return Aantal;
        }

        //1. Een nieuwe bestelling aanmaken in tblBestelling
        //2. Het ordernr dat zonet gegenereerd werd ophalen want dit is ook nodig in tblBestellijnen
        //3. Een list maken van alle artikels in de winkelmand
        //4. aanmaken van een bestellijn waar de artikels ingestoken worden en je winkelmand terug leeg maken.


        public Bestelling Bevestigen(int KlantID)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            Bestelling bestelling = new Bestelling();
            string datum = DateTime.Now.ToString("yyyy-MM-dd");
            string qry = "insert into tblbestelling (KlantID,Orderdatum) values ('" + KlantID + "', '" + datum + "')";
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            conn.Open();
            string qry1 = "select OrderID from tblbestelling where KlantID=" + KlantID
            + " Order by OrderID desc limit 1";
            MySqlCommand cmd1 = new MySqlCommand(qry1, conn);
            MySqlDataReader dtr = cmd1.ExecuteReader();
            while (dtr.Read())
            {
                bestelling.OrderID = Convert.ToInt32(dtr["OrderID"]);

            }
            conn.Close();

            conn.Open();
            string qry2 = "select tblwinkelmand.ArtikelID, tblwinkelmand.Aantal,prijs from tblwinkelmand " +
            "inner join tblartikel on tblartikel.ArtikelID = tblwinkelmand.ArtikelID " +
            "where tblwinkelmand.KlantID =" + KlantID;
            MySqlCommand cmd2 = new MySqlCommand(qry2, conn);
            MySqlDataReader dtr2 = cmd2.ExecuteReader();
            List<WinkelmandItem> lijst = new List<WinkelmandItem>();
            while (dtr2.Read())
            {
                WinkelmandItem wmItem = new WinkelmandItem();
                wmItem.ArtikelNr = Convert.ToInt32(dtr2["ArtikelID"]);
                wmItem.Aantal = Convert.ToInt32(dtr2["Aantal"]);
                wmItem.Prijs = Convert.ToDouble(dtr2["Prijs"]);

                lijst.Add(wmItem);
            }
            conn.Close();

            conn.Open();
            foreach (var artikel in lijst)
            {


                string corrPrijs = Convert.ToString(artikel.Prijs).Replace(",", ".");
                string qry3 = "insert into tblbestellijn (ArtikelID, OrderID,Aantal,Prijs) values " +
                "(" + artikel.ArtikelNr + "," + bestelling.OrderID + "," + artikel.Aantal + "," + corrPrijs + ")";
                MySqlCommand cmd3 = new MySqlCommand(qry3, conn);
                cmd3.ExecuteNonQuery();

            }
            conn.Close();


            conn.Open();
            string qry4 = "delete from tblwinkelmand where klantID = " + KlantID;
            MySqlCommand cmd4 = new MySqlCommand(qry4, conn);
            cmd4.ExecuteNonQuery();
            conn.Close();

            return bestelling;
        }

        public int CheckCredentials(Klant klant)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select KlantID,GebrNaam,Wachtwoord from tblklanten where GebrNaam='" + klant.Naam + "'and binary wachtwoord ='" + klant.Wachtwoord + "'";
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();
            int userID = -1;
            while (dtr.Read())
            {
                userID = Convert.ToInt32(dtr["klantID"]);
            }
            conn.Close();
            return userID;
        }



    }
}
