using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace DB_16_September
{
    internal class Material
    {
        public string id;
        public string title;
        public string count_in_pack;
        public string unit;
        public string count_in_stock;
        public string min_count;
        public string description;
        public string cost;
        public string image;
        public MaterialType material_type;

        //Material types from table
        public static Dictionary<string, MaterialType> types;
        public static void MaterialTypeLoad() { 

            types = new Dictionary<string, MaterialType>();

            DataTable dt = DB.query("SELECT * FROM MaterialType");

            string id, title;

            foreach (DataRow dr in dt.Rows)
            {
                id = dr["ID"].ToString();
                title = dr["Title"].ToString();

                types.Add(id, new MaterialType(id, title));
            }  
        }

        //Loading from database
        public Material(DataRow row)
        {
 
            id =            row["ID"].ToString();
            title =         row["Title"].ToString();
            count_in_pack = row["CountInPack"].ToString();
            unit =          row["Unit"].ToString();
            count_in_stock = row["CountInStock"].ToString();
            min_count =     row["MinCount"].ToString();
            description =   row["Description"].ToString();
            cost =          row["Cost"].ToString().Replace(",00","");
            image =         row["Image"].ToString();

            string typename = row["m_name"].ToString();
            string id_type = row["id_mat"].ToString();

            material_type = types[id_type];                  
        }

        public Material()
        {
            id = "-1";
        }

        //Look in ListViews
        public override string ToString()
        {
            return $"{title}: {count_in_stock} {unit}. {description}";
        }


        public void delete_from_db()
        {
            DB.query("DELETE FROM Material WHERE ID = " + id);
        }
        public void insert_as_new()
        {
            if (Convert.ToInt32(id) > 0)
            {
                MessageBox.Show("This material already exists!");
            }
            else
            {
                DB.addParam("ttl", title);
                DB.addParam("cip", count_in_pack);
                DB.addParam("u", unit);
                DB.addParam("cis", count_in_stock);
                DB.addParam("mc", min_count);
                DB.addParam("d", description);
                DB.addParam("c", cost);
                DB.addParam("img", image);

                if (material_type == null)
                {
                    MessageBox.Show("Сначала укажите тип материала");
                    return;
                }


                DB.addParam("typee", material_type.id);

                DB.query("INSERT INTO Material ([Title],[CountInPack],[Unit],[CountInStock],[MinCount],[Description],[Cost],[Image],[MaterialTypeID]) " +
                    "VALUES (@ttl, @cip, @u, @cis, @mc, @d, @c, @img, @typee)");

                id = DB.query("SELECT IDENT_CURRENT('Material') as m").Rows[0][0].ToString();
            }
        }
        public void update_in_db()
        {
            if (Convert.ToInt32(id) < 0)
            {
                MessageBox.Show("This material already doesn't exist!");
            }
            else
            {
                DB.addParam("id", id);
                DB.addParam("ttl", title);
                DB.addParam("cip", count_in_pack);
                DB.addParam("u", unit);
                DB.addParam("cis", count_in_stock);
                DB.addParam("mc", min_count);
                DB.addParam("d", description);
                DB.addParam("c", cost);
                DB.addParam("img", image);
                DB.addParam("typee", material_type.id);

                DB.query("UPDATE Material SET [Title] = @ttl, [CountInPack] = @cip, [Unit] = @u, [CountInStock] = @cis, [MinCount] = @mc," +
                    " [Description] = @d, [Cost] = @c, [Image] = @img, [MaterialTypeID] = @typee WHERE [ID] = @id");
            }
        }
    }
}
