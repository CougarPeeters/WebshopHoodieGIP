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
            bool mand = true;
            if(dtr.HasRows)
            {
               mand = true;
            }
            else
            {
                mand = false;

            }
            conn.Close();

            conn.Open();
            if (mand==true)
            {
                string qry2 = "update tblwinkelmand SET Aantal = Aantal +'" + winkelmanditem.Aantal + "' where(KlantID = '"+ winkelmanditem.KlantNr+"') and(ArtikelID = '"+winkelmanditem.ArtikelNr+"')";
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

        

        public List<WinkelmandItem> MandOphalen()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select tblartikel.ArtikelID,Naam,aantal,foto,prijs,round((prijs*aantal),2) as totaal " +
                "from tblartikel inner join tblwinkelmand on tblartikel.artikelID = tblwinkelmand.artikelID";
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

        public bool HeeftItems()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select tblartikel.ArtikelID,Naam,aantal,foto,prijs,round((prijs*aantal),2) as totaal " +
                "from tblartikel inner join tblwinkelmand on tblartikel.artikelID = tblwinkelmand.artikelID";
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();
            if (dtr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }




        public Klant KlantOphalen(int klantid)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select * from tblklanten where KlantID=" + klantid;
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();
            Klant klant = new Klant();
            while (dtr.Read())
            {
                klant.Naam = Convert.ToString(dtr["Naam"]);
                klant.Adres = Convert.ToString(dtr["Adres"]);
                klant.Gemeente = Convert.ToString(dtr["Gemeente"]);
                klant.PostCode = Convert.ToInt32(dtr["Postcode"]);
            }
            conn.Close();
            return klant;
        }

        public Totaal BerekenTotaal()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select round(sum(tblartikel.Prijs* tblwinkelmand.aantal),2) as totaalexcl, round(sum((tblartikel.prijs*tblwinkelmand.Aantal)*0.21),2) as btw, round(sum((tblartikel.Prijs * tblwinkelmand.Aantal)*1.21),2) as totaalincl from tblartikel " +
                "inner join tblwinkelmand on tblartikel.artikelID = tblwinkelmand.artikelID";
            MySqlCommand cmd = new MySqlCommand(qry, conn);
            MySqlDataReader dtr = cmd.ExecuteReader();
            Totaal totaal = new Totaal();
            while (dtr.Read())
            {
                totaal.TotaalExcl = Convert.ToDouble(dtr["totaalexcl"]);
                totaal.BTW = Convert.ToDouble(dtr["btw"]);
                totaal.TotaalIncl = Convert.ToDouble(dtr["totaalincl"]);
            }
            conn.Close();
            return totaal;
        }
      
        public void Verwijder(WinkelmandItem winkelmandItem)
        {
           
                //voorraad + '" + winkelmandItem.Aantal + "' where ArtikelID = '" + winkelmandItem.ArtikelNr +"'";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry1= "update tblartikel SET voorraad =  voorraad + " + winkelmandItem.Aantal + " where(ArtikelID = " + winkelmandItem.ArtikelNr + ")";
            MySqlCommand cmd1 = new MySqlCommand(qry1, conn);
            cmd1.ExecuteNonQuery();
            conn.Close();

   
            conn.Open();
            string qry2 = "delete from tblwinkelmand where ArtikelID=" + winkelmandItem.ArtikelNr;
            MySqlCommand cmd2 = new MySqlCommand(qry2, conn);
            cmd2.ExecuteNonQuery();
            conn.Close();


        }

        public int AantalOphalen(WinkelmandItem winkelmandItem)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string qry = "select aantal from tblwinkelmand where ArtikelID = '" + winkelmandItem.ArtikelNr + "'";
            MySqlCommand cmd = new MySqlCommand(qry, conn);

            MySqlDataReader dtr = cmd.ExecuteReader();
            int Aantal=0;
            while (dtr.Read())
            {
                Aantal = Convert.ToInt32(dtr["aantal"]);
            }
            conn.Close();
            return Aantal;
        }

    }
}
