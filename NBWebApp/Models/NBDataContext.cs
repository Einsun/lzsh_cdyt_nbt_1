using Microsoft.EntityFrameworkCore;
using NBWebApp.Models;

namespace NBWebApp.Models
{
    /// <summary>
    /// 数据对象
    /// </summary>
    public class NBDataContext : DbContext
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<UserInfo> UserInfos { get; set; }
        /// <summary>
        /// 产线
        /// </summary>
        public DbSet<ProductLine> ProductLines { get; set; }

        /// <summary>
        /// 采集规则
        /// </summary>
        public DbSet<GatherRule> GatherRules { get; set; }
        /// <summary>
        /// 报警规则
        /// </summary>
        public DbSet<AlarmRule> AlarmRules { get; set; }
        /// <summary>
        /// 传输设备
        /// </summary>
        public DbSet<CommDevice> CommDevices { get; set; }
        /// <summary>
        /// 采集设备
        /// </summary>
        public DbSet<Device> Devices { get; set; }
        /// <summary>
        /// 声发射数据
        /// </summary>
        public DbSet<AeData> AeDatas { get; set; }
        /// <summary>
        /// 声发射波形数据
        /// </summary>
        public DbSet<AeWaveData> AeWaveDatas { get; set; }
        /// <summary>
        /// 传感器数据
        /// </summary>
        public DbSet<SensorData> SensorDatas { get; set; }
        /// <summary>
        /// 报警数据
        /// </summary>
        public DbSet<AlarmData> AlarmDatas { get; set; }
        /// <summary>
        /// 声发射规则
        /// </summary>
        public DbSet<AeRule> AeRules { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public NBDataContext(DbContextOptions<NBDataContext> options) : base(options) { }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entity.Name, builder =>
                {
                    builder.ToTable(entity.ClrType.Name.ToLower()+"s");
                    foreach (var property in entity.GetProperties())
                    {
                        builder.Property(property.Name).HasColumnName(property.Name.ToLower());
                    }
                });
            }
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserInfo>().HasData(new UserInfo() { Name = "admin",Id=1});
        }
        /// <summary>
        /// 信号类型
        /// </summary>
        public DbSet<NBWebApp.Models.SignalType> SignalType { get; set; }
    }
}