using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AeDataService
{
    internal class DataHelper
    {
        public string ConStr { get; set; } = "";
        public AeWave GetData()
        {
            AeWave wave = new AeWave();
            using (DataTable dt = new DataTable())
            {
                string sql = "select id,deviceid,sn,data from aewavedatas where settle=0 order by id desc limit 1";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, ConStr))
                {
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count > 0)
                {
                    wave.DBId = (int)dt.Rows[0]["id"];
                    wave.DeviceId = (int)dt.Rows[0]["deviceid"];
                    wave.SN = dt.Rows[0]["sn"].ToString() ?? "";
                    wave.Path = dt.Rows[0]["data"].ToString() ?? "";
                }
            }
            return wave;
        }

        public void Update(int id,JsonNode json)
        {
            string sql = $"update aewavedatas set rootmeansquare={json["RootMeanSquare"]}," +
                $"faultsta={json["FaultSta"]},faultenum='{json["FaultEnum"] ?? ""}'," +
                $"mesdataimage1='{json["MesDataimage1"]?.ToString().Replace("\\","\\\\")??""}',settle=1 where id={id}";
            MySqlConnection con=new MySqlConnection(ConStr);
            con.Open();
            MySqlCommand cmd=new MySqlCommand(sql, con);
            cmd.ExecuteScalar();
        }
    }
}
