using System.Collections.Generic;
using WaiterBackend.Database;
using WaiterBackend.Models;

namespace WaiterBackend.Services.Endpoints
{
    public class ItemService
    {
        private readonly WaiterBackend.Database.Database _db;

        public ItemService(WaiterBackend.Database.Database db) => _db = db;

        public List<Item> Get()
        {
            using var conn = _db.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Items";

            var items = new List<Item>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new Item
                {
                    Id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    harga = reader.GetDecimal(2),
                    IsAvaible = reader.GetBoolean(3),
                    deskripsi = reader.GetString(4)
                });
            }

            return items;
        }

        public Item Post(Item items){
          using var conn = _db.GetConnection();
          var cmd = conn.CreateCommand();
          cmd.CommandText = "INSERT INTO Items(nama,harga,tersedia,deskripsi) VALUES (@nama,@harga,@tersedia,@des)";
          cmd.Parameters.AddWithValue("@nama",items.name);
          cmd.Parameters.AddWithValue("@harga",items.harga);
          cmd.Parameters.AddWithValue("@tersedia",items.IsAvaible);
          cmd.Parameters.AddWithValue("@des",items.deskripsi);
          cmd.ExecuteScalar();
          return items;
        }

        public bool Delete(Item item){
          using var conn = _db.GetConnection();
          var cmd = conn.CreateCommand();
          cmd.CommandText = "DELETE FROM Items WHERE id=@id";
          cmd.Parameters.AddWithValue("@id",item.Id);
          
          return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(Item items){
          using var conn = _db.GetConnection();
          var cmd = conn.CreateCommand();
          cmd.CommandText = "UPDATE Items SET nama=@nama,harga=@harga,tersedia=@tersedia,deskripsi=@des WHERE id=@id";
          cmd.Parameters.AddWithValue("@nama",items.name);
          cmd.Parameters.AddWithValue("@harga",items.harga);
          cmd.Parameters.AddWithValue("@tersedia",items.IsAvaible);
          cmd.Parameters.AddWithValue("@des",items.deskripsi);
          cmd.Parameters.AddWithValue("@id",items.Id);
         
          return cmd.ExecuteNonQuery() > 0;
        } 
    }
}

