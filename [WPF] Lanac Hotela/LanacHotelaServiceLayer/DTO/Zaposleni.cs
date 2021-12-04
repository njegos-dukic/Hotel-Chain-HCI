using System.Collections.Generic;

namespace LanacHotelaServiceLayer
{
    public class Zaposleni
    {
        public int ZaposleniID { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string LozinkaCiph { get; set; }
        public bool JeMenadzer { get; set; }
        public int HotelID { get; set; }

        public Zaposleni(int zaposleniID, string korisnickoIme, string lozinka, bool jeMenadzer, int hotelID)
        {
            ZaposleniID = zaposleniID;
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            for (int i = 0; i < Lozinka.Length; i++)
            {
                LozinkaCiph += "*";
            }

            JeMenadzer = jeMenadzer;
            HotelID = hotelID;
        }

        public void UpdatePassword()
        {
            if (LozinkaCiph.Contains("*"))
            {
                return;
            }

            Lozinka = LozinkaCiph;
            for (int i = 0; i < Lozinka.Length; i++)
            {
                LozinkaCiph += "*";
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Zaposleni zaposleni &&
                   KorisnickoIme == zaposleni.KorisnickoIme &&
                   Lozinka == zaposleni.Lozinka &&
                   JeMenadzer == zaposleni.JeMenadzer &&
                   HotelID == zaposleni.HotelID;
        }

        public override int GetHashCode()
        {
            int hashCode = 1123107417;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(KorisnickoIme);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Lozinka);
            hashCode = hashCode * -1521134295 + JeMenadzer.GetHashCode();
            hashCode = hashCode * -1521134295 + HotelID.GetHashCode();
            return hashCode;
        }
    }
}
