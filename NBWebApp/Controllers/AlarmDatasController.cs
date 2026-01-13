using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBWebApp.Models;
using Npoi.Mapper;
using Npoi.Mapper.Attributes;
using NPOI.SS.Formula.Functions;
using System.Data;
using System.Dynamic;

namespace NBWebApp.Controllers
{
    /// <summary>
    /// 告警数据接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmDatasController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<AlarmDatasController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public AlarmDatasController(NBDataContext context, ILogger<AlarmDatasController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// 按设备获取分页告警数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagecount">个数</param>
        /// <param name="deviceid">设备id</param>
        /// <returns></returns>
        [HttpGet("PageAlarmData")]
        public ActionResult PageAlarmData(int page, int pagecount, int deviceid)
        {
            try
            {
                var data = _context.AlarmDatas.Where(t => 1 == 1);
                if (deviceid > 0)
                {
                    data = data.Where(t => t.DeviceId == deviceid);
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Count = data.Count();
                rst.Data = data.OrderByDescending(t => t.Id).Skip((page - 1) * pagecount).Take(pagecount).ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        /// <summary>
        /// 按类型获取分页告警数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagecount">个数</param>
        /// <param name="productlineid">产线id</param>
        /// <param name="commdeviceid">网关id</param>
        /// <param name="type">故障类型 0 正常;1 报警;2 故障;3 全部</param>
        /// <param name="gathertype">设备类型 0 全部;1:声发射 ; 2:温度传感器 ;3 压力传感器;4 流量传感器;5 振动设备</param>
        /// <param name="name">设备名称</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">截止时间</param>
        /// <returns></returns>
        [HttpGet("PageTypeAlarmData")]
        public ActionResult PageTypeAlarmData(int page, int pagecount, int productlineid, int commdeviceid, int type, int gathertype, string name, string starttime, string endtime)
        {
            try
            {
                var devs= _context.Devices.Where(t => 1 == 1);
                if (productlineid > 0)
                {
                    devs = devs.Where(t => t.ProductLineId == productlineid);
                }
                if (commdeviceid > 0)
                {
                    devs = devs.Where(t => t.CommDeviceId == commdeviceid);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    devs = devs.Where(t => t.Name.Contains(name));
                }
                if (gathertype > 0)
                {
                    devs = devs.Where(t => t.Type == (GatherType)(gathertype - 1));
                }
                var ids = devs.Select(t => t.Id).ToList();
                var qu = _context.AlarmDatas.Where(t => ids.Contains(t.DeviceId));
                if (type < 3)
                {
                    qu = qu.Where(t => t.AlarmType == type);
                }
                if (!string.IsNullOrEmpty(starttime))
                {
                    if (DateTime.TryParse(starttime, out var time))
                    {
                        qu = qu.Where(t => t.Time > time);
                    }
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    if (DateTime.TryParse(endtime, out var time))
                    {
                        qu = qu.Where(t => t.Time < time);
                    }
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Count = qu.Count();


                var data=qu.OrderByDescending(t => t.Id).Skip((page - 1) * pagecount).Take(pagecount).ToList();
                
                var _data = from u in data
                            join d in _context.Devices on u.DeviceId equals d.Id
                            join c in _context.CommDevices on d.CommDeviceId equals c.Id
                            join p in _context.ProductLines on d.ProductLineId equals p.Id
                            select new
                            {
                                u.Id,
                                productlineid = p.Id,
                                commdeviceid = c.Id,
                                deviceid = d.Id,
                                productLineName = p.Name,
                                commDeviceName = c.Name,
                                devicename = d.Name,
                                alarmType = u.AlarmType,
                                deviceType = d.Type,
                                devno = c.SN,
                                d.State,
                                p.Level,
                                u.Reason,
                                u.Value,
                                u.Settle,
                                u.Time
                            };

                rst.Data = _data.ToList();

                //var queru = from u in _context.AlarmDatas
                //            join d in devs on u.DeviceId equals d.Id
                //            join c in _context.CommDevices on d.CommDeviceId equals c.Id
                //            join p in _context.ProductLines on d.ProductLineId equals p.Id
                //            select new
                //            {
                //                u.Id,
                //                productlineid = p.Id,
                //                commdeviceid = c.Id,
                //                deviceid = d.Id,
                //                productLineName = p.Name,
                //                commDeviceName = c.Name,
                //                devicename = d.Name,
                //                alarmType = u.AlarmType,
                //                deviceType = d.Type,
                //                devno = c.SN,
                //                d.State,
                //                p.Level,
                //                u.Reason,
                //                u.Value,
                //                u.Settle,
                //                u.Time
                //            };
                ////if (productlineid > 0)
                ////{
                ////    queru = queru.Where(t => t.productlineid == productlineid);
                ////}
                ////if (commdeviceid > 0)
                ////{
                ////    queru = queru.Where(t => t.commdeviceid == commdeviceid);
                ////}
                ////if (!string.IsNullOrEmpty(name))
                ////{
                ////    queru = queru.Where(t => t.devicename.Contains(name));
                ////}
                //if (type < 3)
                //{
                //    queru = queru.Where(t => t.alarmType == type);
                //}
                //if (!string.IsNullOrEmpty(starttime))
                //{
                //    if (DateTime.TryParse(starttime, out var time))
                //    {
                //        queru = queru.Where(t => t.Time > time);
                //    }
                //}
                //if (!string.IsNullOrEmpty(endtime))
                //{
                //    if (DateTime.TryParse(endtime, out var time))
                //    {
                //        queru = queru.Where(t => t.Time < time);
                //    }
                //}
                ////if (gathertype > 0)
                ////{
                ////    queru = queru.Where(t => t.deviceType == (GatherType)(gathertype - 1));
                ////}
                //ResultInfo rst = new ResultInfo();
                //rst.Code = 0;
                //rst.Count = queru.Count();
                //rst.Data = queru.OrderByDescending(t => t.Id).Skip((page - 1) * pagecount).Take(pagecount).ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        class DataInfo
        {
            [Column("设备ID")]
            public int DeviceId { get; set; }
            [Column("设备名称")]
            public string DeviceName { get; set; }
            [Ignore]
            public int PLId { get; set; }
            [Column("产线名称")]
            public string PLName { get; set; }
            [Ignore]
            public int CMId { get; set; }
            [Column("传输设备")]
            public string CMName { get; set; }
            [Column("报警类型")]
            public int DeviceType { get; set; }
            [Ignore]
            public int AlarmType { get; set; }
            [Column("说明")]
            public string Reason { get; set; }
            [Column("时间", CustomFormat = "yyyy-MM-dd HH:mm:ss")]
            public DateTime Time { get; set; }
            [Column("值")]
            public decimal Value { get; set; }
        }
        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="productlineid">产线id</param>
        /// <param name="commdeviceid">网关id</param>
        /// <param name="type">故障类型 0 正常;1 报警;2 故障;3 全部</param>
        /// <param name="gathertype">设备类型 0 全部;1:声发射 ; 2:温度传感器 ;3 压力传感器;4 流量传感器;5 振动设备</param>
        /// <param name="name">设备名称</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">截止时间</param>
        /// <returns></returns>
        [HttpGet("DownLoad")]
        public ActionResult DownLoad(int productlineid, int commdeviceid, int type, int gathertype, string name, string starttime, string endtime)
        {
            try
            {
                var queru = from u in _context.AlarmDatas
                            join d in _context.Devices on u.DeviceId equals d.Id
                            join c in _context.CommDevices on d.CommDeviceId equals c.Id
                            join p in _context.ProductLines on d.ProductLineId equals p.Id
                            select new DataInfo()
                            {
                                PLId = p.Id,
                                CMId = c.Id,
                                DeviceId = d.Id,
                                PLName = p.Name,
                                CMName = c.Name,
                                DeviceName = d.Name,
                                AlarmType = u.AlarmType,
                                DeviceType = (int)d.Type,
                                Reason = u.Reason,
                                Value = u.Value,
                                Time = u.Time
                            };
                if (productlineid > 0)
                {
                    queru = queru.Where(t => t.PLId == productlineid);
                }
                if (commdeviceid > 0)
                {
                    queru = queru.Where(t => t.CMId == commdeviceid);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    queru = queru.Where(t => t.DeviceName.Contains(name));
                }
                if (type < 3)
                {
                    queru = queru.Where(t => t.AlarmType == type);
                }
                if (!string.IsNullOrEmpty(starttime))
                {
                    if (DateTime.TryParse(starttime, out var time))
                    {
                        queru = queru.Where(t => t.Time > time);
                    }
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    if (DateTime.TryParse(endtime, out var time))
                    {
                        queru = queru.Where(t => t.Time < time);
                    }
                }
                if (gathertype > 0)
                {
                    queru = queru.Where(t => t.DeviceType ==(gathertype - 1));
                }
                var mapper = new Mapper();
                mapper.Map<DataInfo>("报警类型", o => o.DeviceType, null, (c, t) =>
                {
                    switch (c.CurrentValue.ToString())
                    {
                        case "1": c.CurrentValue = "温度传感器"; break;
                        case "2": c.CurrentValue = "压力传感器"; break;
                        case "3": c.CurrentValue = "流量传感器"; break;
                        case "4": c.CurrentValue = "振动设备"; break;
                        default: c.CurrentValue = "其他"; break;
                    }
                    return true;
                });
                //mapper.Map("",queru.ElementType)
                MemoryStream stream = new MemoryStream();
                //将students集合生成的Excel直接放置到Stream中
                mapper.Save(stream, queru, "sensordata", false, overwrite: true, xlsx: true);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","历史记录.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        /// <summary>
        /// 获取指定产线传感器数据生成波形
        /// </summary>
        /// <param name="productLineId">产线id</param>
        /// <returns></returns>
        [HttpGet("AlarmWavedata")]
        public ActionResult AlarmWavedata(int productLineId)
        {
            try
            {
                var devices = _context.Devices.Where(t => t.ProductLineId == productLineId).ToList();//   from d in _context.Devices where d.ProductLineId == productLineId && d.Type!=GatherType.AE select d;
                List<object> data = new List<object>();
                var sql = """
                    SELECT Time,DeviceId, COUNT( * ) AS Num 
                    FROM
                    	(
                    	SELECT deviceid as DeviceId,
                    		DATE_FORMAT(
                    			concat( date( time ), ' ', HOUR ( time ), ':', floor( MINUTE ( time ) / 30 ) * 30 ),
                    			'%Y-%m-%d %H:%i' 
                    		) AS Time 
                    	FROM alarmdatas
                    	WHERE  time>curdate() 
                    	) a 
                    GROUP BY DeviceId,Time 
                    ORDER BY DeviceId,Time;
                    """;

                sql = """
                     select deviceid
                    	,value as num
                    	,DATE_FORMAT(concat (
                    			date (time)
                    			,' '
                    			,HOUR(time)
                    			,':'
                    			,floor(MINUTE(time) / 30) * 30
                    			), '%Y-%m-%d %H:%i') AS time
                    from alarmdatas
                    where id in (
                    		select min(id) as id
                    		from (
                    			SELECT deviceid
                    				,id
                    				,value
                    				,time
                    				,DATE_FORMAT(concat (
                    						date (time)
                    						,' '
                    						,HOUR(time)
                    						,':'
                    						,floor(MINUTE(time) / 30) * 30
                    						), '%Y-%m-%d %H:%i') AS formatTime
                    			FROM alarmdatas
                    			WHERE time > curdate()
                    			) t
                    		group by deviceid
                    			,formatTime
                    		)
                    """;

                var con = _context.Database.GetDbConnection();
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                con.Close();

                foreach (var d in devices)
                {
                    if (d.Type == GatherType.AE)
                        continue;
                    // var rows=dt.Select($"deviceid={d.Id}");
                    List<object> cnts = new List<object>();
                    // foreach(var row in rows){
                    //     cnts.Add(row["num"]);
                    // }
                    var starttime = DateTime.Now.Date;
                    //var endtime = DateTime.Now.Date.AddMinutes(30);
                    while (starttime < DateTime.Now)
                    {
                        var rows = dt.Select($"deviceid={d.Id} and time='{starttime:yyyy-MM-dd HH:mm}'");
                        if (rows.Length == 0)
                        {
                            cnts.Add(0);
                        }
                        else
                        {
                            cnts.Add(rows[0]["num"]);
                        }
                        // var cnt=_context.AlarmDatas.Where(t=>t.DeviceId==d.Id&&t.Time>=starttime&&t.Time<endtime).Count();
                        // cnts.Add(cnt);
                        starttime = starttime.AddMinutes(30);
                        //endtime = starttime.AddMinutes(30);
                    }
                    dynamic devdata = new ExpandoObject();
                    devdata.id = d.Id;
                    devdata.name = d.Name;
                    devdata.comid = d.CommDeviceId;
                    devdata.type = d.Type;
                    devdata.data = cnts;
                    data.Add(devdata);
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = data;
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 设置指定id告警数据为已处理
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("SettleAlarm")]
        public ActionResult SettleAlarm(int[] ids)
        {
            try
            {
                var alarms = _context.AlarmDatas.Where(t => ids.Contains(t.Id)).ToList();
                foreach (var alarm in alarms)
                {
                    alarm.Settle = true;
                }
                _context.SaveChanges();
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定产线指定日期传感器数据生成波形
        /// </summary>
        /// <param name="productLineId">产线id</param>
        /// <param name="date">数据日期</param>
        /// <returns></returns>
        [HttpGet("AlarmWavedataDate")]
        public ActionResult AlarmWavedataDate(int productLineId, string date)
        {
            try
            {
                var devices = _context.Devices.Where(t => t.ProductLineId == productLineId).ToList();//   from d in _context.Devices where d.ProductLineId == productLineId && d.Type!=GatherType.AE select d;
                List<object> data = new List<object>();
                var sql = $"""
                     select deviceid
                    	,value as num
                    	,DATE_FORMAT(concat (
                    			date (time)
                    			,' '
                    			,HOUR(time)
                    			,':'
                    			,floor(MINUTE(time) / 30) * 30
                    			), '%Y-%m-%d %H:%i') AS time
                    from alarmdatas
                    where id in (
                    		select min(id) as id
                    		from (
                    			SELECT deviceid
                    				,id
                    				,value
                    				,time
                    				,DATE_FORMAT(concat (
                    						date (time)
                    						,' '
                    						,HOUR(time)
                    						,':'
                    						,floor(MINUTE(time) / 30) * 30
                    						), '%Y-%m-%d %H:%i') AS formatTime
                    			FROM alarmdatas
                    			WHERE to_days(time)=to_days('{date}')
                    			) t
                    		group by deviceid
                    			,formatTime
                    		)
                    """;

                var con = _context.Database.GetDbConnection();
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                con.Close();
                var _date = DateTime.Parse(date);
                foreach (var d in devices)
                {
                    if (d.Type == GatherType.AE)
                        continue;
                    // var rows=dt.Select($"deviceid={d.Id}");
                    List<object> cnts = new List<object>();
                    // foreach(var row in rows){
                    //     cnts.Add(row["num"]);
                    // }
                    var starttime = _date;
                    //var endtime = DateTime.Now.Date.AddMinutes(30);
                    while (starttime.Date == _date)
                    {
                        var rows = dt.Select($"deviceid={d.Id} and time='{starttime:yyyy-MM-dd HH:mm}'");
                        if (rows.Length == 0)
                        {
                            cnts.Add(0);
                        }
                        else
                        {
                            cnts.Add(rows[0]["num"]);
                        }
                        // var cnt=_context.AlarmDatas.Where(t=>t.DeviceId==d.Id&&t.Time>=starttime&&t.Time<endtime).Count();
                        // cnts.Add(cnt);
                        starttime = starttime.AddMinutes(30);
                        //endtime = starttime.AddMinutes(30);
                    }
                    dynamic devdata = new ExpandoObject();
                    devdata.id = d.Id;
                    devdata.name = d.Name;
                    devdata.comid = d.CommDeviceId;
                    devdata.type = d.Type;
                    devdata.data = cnts;
                    data.Add(devdata);
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = data;
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定产线当月传感器数据生成波形
        /// </summary>
        /// <param name="productLineId"></param>
        /// <returns></returns>
        [HttpGet("AlarmWavedataMonth")]
        public ActionResult AlarmWavedataMonth(int productLineId)
        {
            try
            {
                var devices = _context.Devices.Where(t => t.ProductLineId == productLineId).ToList();//   from d in _context.Devices where d.ProductLineId == productLineId && d.Type!=GatherType.AE select d;
                List<object> data = new List<object>();
                var sql = """
                    select deviceid
                       	,value as num
                       	,DATE_FORMAT(time, '%Y-%m-%d') AS time
                    from alarmdatas
                    where id in (
                          		select min(id) as id
                          		from (
                             			SELECT deviceid
                                				,id
                                				,to_days(time) AS formatTime
                             			FROM alarmdatas
                             			WHERE time > date_add(curdate(), interval - day(curdate()) + 1 day)
                             			) t
                          		group by deviceid
                             			,formatTime
                          		)
                    """;

                var con = _context.Database.GetDbConnection();
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                con.Close();

                foreach (var d in devices)
                {
                    if (d.Type == GatherType.AE)
                        continue;
                    // var rows=dt.Select($"deviceid={d.Id}");
                    List<object> cnts = new List<object>();
                    // foreach(var row in rows){
                    //     cnts.Add(row["num"]);
                    // }
                    var starttime = DateTime.Now.Date.AddDays(1-DateTime.Now.Day);
                    //var endtime = DateTime.Now.Date.AddMinutes(30);
                    while (starttime < DateTime.Now)
                    {
                        var rows = dt.Select($"deviceid={d.Id} and time='{starttime:yyyy-MM-dd}'");
                        if (rows.Length == 0)
                        {
                            cnts.Add(0);
                        }
                        else
                        {
                            cnts.Add(rows[0]["num"]);
                        }
                        // var cnt=_context.AlarmDatas.Where(t=>t.DeviceId==d.Id&&t.Time>=starttime&&t.Time<endtime).Count();
                        // cnts.Add(cnt);
                        starttime = starttime.AddDays(1);
                        //endtime = starttime.AddMinutes(30);
                    }
                    dynamic devdata = new ExpandoObject();
                    devdata.id = d.Id;
                    devdata.name = d.Name;
                    devdata.comid = d.CommDeviceId;
                    devdata.type = d.Type;
                    devdata.data = cnts;
                    data.Add(devdata);
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = data;
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定产线指定时间点30分钟前传感器数据生成波形
        /// </summary>
        /// <param name="productLineId">产线id</param>
        /// <param name="time">时间点</param>
        /// <returns></returns>
        [HttpGet("AlarmWavedataTime")]
        public ActionResult AlarmWavedataTime(int productLineId,DateTime time)
        {
            try
            {
                _logger.LogInformation("{}",time);
                var devices = _context.Devices.Where(t => t.ProductLineId == productLineId).ToList();//   from d in _context.Devices where d.ProductLineId == productLineId && d.Type!=GatherType.AE select d;
                List<object> data = new List<object>();

                var sql = $"""
                    select deviceid
                       	,value as num
                       	,DATE_FORMAT(time, '%Y-%m-%d %H:%i') AS time
                    from alarmdatas
                    where id in (
                          		select min(id) as id
                          		from (
                             			SELECT deviceid
                                				,id
                                				,DATE_FORMAT(time,'%Y-%m-%d %H:%i') AS formatTime
                             			FROM alarmdatas
                             			WHERE time > '{time.AddMinutes(-30):yyyy-MM-dd HH:mm:ss}'
                                        and time <= '{time:yyyy-MM-dd HH:mm:ss}'
                             			) t
                          		group by deviceid
                             			,formatTime
                          		)
                    """;

                var con = _context.Database.GetDbConnection();
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                con.Close();

                foreach (var d in devices)
                {
                    if (d.Type == GatherType.AE)
                        continue;
                    // var rows=dt.Select($"deviceid={d.Id}");
                    List<object> cnts = new List<object>();
                    // foreach(var row in rows){
                    //     cnts.Add(row["num"]);
                    // }
                    var starttime = time.AddMinutes(-30);
                    //var endtime = DateTime.Now.Date.AddMinutes(30);
                    while (starttime < time)
                    {
                        var rows = dt.Select($"deviceid={d.Id} and time='{starttime:yyyy-MM-dd HH:mm}'");
                        if (rows.Length == 0)
                        {
                            cnts.Add(0);
                        }
                        else
                        {
                            cnts.Add(rows[0]["num"]);
                        }
                        // var cnt=_context.AlarmDatas.Where(t=>t.DeviceId==d.Id&&t.Time>=starttime&&t.Time<endtime).Count();
                        // cnts.Add(cnt);
                        starttime = starttime.AddMinutes(1);
                        //endtime = starttime.AddMinutes(30);
                    }
                    dynamic devdata = new ExpandoObject();
                    devdata.id = d.Id;
                    devdata.name = d.Name;
                    devdata.comid = d.CommDeviceId;
                    devdata.type = d.Type;
                    devdata.data = cnts;
                    data.Add(devdata);
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = data;
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定产线当年传感器数据生成波形
        /// </summary>
        /// <param name="productLineId">产线id</param>
        /// <returns></returns>
        [HttpGet("AlarmWavedataYear")]
        public ActionResult AlarmWavedataYear(int productLineId)
        {
            try
            {
                var devices = _context.Devices.Where(t => t.ProductLineId == productLineId).ToList();//   from d in _context.Devices where d.ProductLineId == productLineId && d.Type!=GatherType.AE select d;
                List<object> data = new List<object>();
                var sql = """
                    select deviceid
                       	,value as num
                       	,DATE_FORMAT(time, '%Y-%m') AS time
                    from alarmdatas
                    where id in (
                          		select min(id) as id
                          		from (
                             			SELECT deviceid
                                				,id
                                				,DATE_FORMAT(time, '%Y-%m') AS formatTime
                             			FROM alarmdatas
                             			WHERE time > concat(year(now()),'-01-01')
                             			) t
                          		group by deviceid
                             			,formatTime
                          		)
                    """;

                var con = _context.Database.GetDbConnection();
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                con.Close();

                foreach (var d in devices)
                {
                    if (d.Type == GatherType.AE)
                        continue;
                    // var rows=dt.Select($"deviceid={d.Id}");
                    List<object> cnts = new List<object>();
                    // foreach(var row in rows){
                    //     cnts.Add(row["num"]);
                    // }
                    var starttime = DateTime.Parse($"{DateTime.Now.Year}-01-01");
                    //var endtime = DateTime.Now.Date.AddMinutes(30);
                    while (starttime < DateTime.Now)
                    {
                        var rows = dt.Select($"deviceid={d.Id} and time='{starttime:yyyy-MM}'");
                        if (rows.Length == 0)
                        {
                            cnts.Add(0);
                        }
                        else
                        {
                            cnts.Add(rows[0]["num"]);
                        }
                        // var cnt=_context.AlarmDatas.Where(t=>t.DeviceId==d.Id&&t.Time>=starttime&&t.Time<endtime).Count();
                        // cnts.Add(cnt);
                        starttime = starttime.AddMonths(1);
                        //endtime = starttime.AddMinutes(30);
                    }
                    dynamic devdata = new ExpandoObject();
                    devdata.id = d.Id;
                    devdata.name = d.Name;
                    devdata.comid = d.CommDeviceId;
                    devdata.type = d.Type;
                    devdata.data = cnts;
                    data.Add(devdata);
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = data;
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

    }
}
