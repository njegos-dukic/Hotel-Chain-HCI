using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    public class ZaposleniService : IReadable<Zaposleni>, IUniquelyUpdateable<Zaposleni>, IUniquelyDeleteable, IInsertable<Zaposleni>
    {
        public async Task<List<Zaposleni>> GetAll()
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Zaposleni> zaposleni = new List<Zaposleni>();

            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM ZAPOSLENI;", cnn);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    zaposleni.Add(new Zaposleni(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetBoolean(3), rdr.GetInt32(4)));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return zaposleni;
        }

        public async Task<int> Delete(int id)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                string query = "CALL ObrisiZaposlenog(@ID);";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@ID", id);

                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        public async Task<int> Insert(Zaposleni t)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                foreach (Zaposleni z in await GetAll())
                {
                    if (z.ZaposleniID == t.ZaposleniID)
                    {
                        return -1;
                    }
                }

                string query = @"INSERT INTO ZAPOSLENI VALUES (0, @KorisnickoIme, @Lozinka, @JeMenadzer, 1);";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@KorisnickoIme", t.KorisnickoIme);
                command.Parameters.AddWithValue("@Lozinka", t.Lozinka);
                command.Parameters.AddWithValue("@JeMenadzer", (t.JeMenadzer == true ? 1 : 0));

                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        public async Task<int> Update(Zaposleni t)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                string query = "UPDATE ZAPOSLENI SET korisnickoIme = @KorisnickoIme, lozinka = @Lozinka, jeMenadzer = @JeMenadzer WHERE zaposleniID = @ID;";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@KorisnickoIme", t.KorisnickoIme);
                command.Parameters.AddWithValue("@Lozinka", t.Lozinka);
                command.Parameters.AddWithValue("@JeMenadzer", (t.JeMenadzer == true ? 1 : 0));
                command.Parameters.AddWithValue("@ID", t.ZaposleniID);

                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }
    }
}
