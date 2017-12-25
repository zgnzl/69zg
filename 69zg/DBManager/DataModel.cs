namespace _69zg
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using _69zg.Models;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=sqlconnectionstr")
        {
        }

        public virtual DbSet<statisticsLog> statisticsLog { get; set; }
        public virtual DbSet<statisticsLog> statisticsUser { get; set; }
        public virtual DbSet<C_directories> C_directories { get; set; }
        public virtual DbSet<C_freedirectories> C_freedirectories { get; set; }
        public virtual DbSet<C_functioninfo> C_functioninfo { get; set; }
        public virtual DbSet<C_roleinfo> C_roleinfo { get; set; }
        public virtual DbSet<C_userinfo> C_userinfo { get; set; }
        public virtual DbSet<仓库记录> 仓库记录 { get; set; }
        public virtual DbSet<仓库转移类别> 仓库转移类别 { get; set; }
        public virtual DbSet<合同记录> 合同记录 { get; set; }
        public virtual DbSet<合同类别> 合同类别 { get; set; }
        public virtual DbSet<区域代码> 区域代码 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<C_directories>()
                .Property(e => e.type)
                .IsFixedLength();

            modelBuilder.Entity<C_directories>()
                .Property(e => e.source)
                .IsFixedLength();

            modelBuilder.Entity<C_directories>()
                .Property(e => e.code)
                .IsFixedLength();

            modelBuilder.Entity<C_directories>()
                .Property(e => e.character)
                .IsFixedLength();

            modelBuilder.Entity<C_directories>()
                .Property(e => e.other)
                .IsFixedLength();

            modelBuilder.Entity<C_freedirectories>()
                .Property(e => e.typeorcode)
                .IsFixedLength();

            modelBuilder.Entity<C_freedirectories>()
                .Property(e => e.freelink)
                .IsFixedLength();

            modelBuilder.Entity<C_freedirectories>()
                .Property(e => e.other)
                .IsFixedLength();

            modelBuilder.Entity<C_functioninfo>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<C_functioninfo>()
                .Property(e => e.displayname)
                .IsFixedLength();

            modelBuilder.Entity<C_roleinfo>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<C_roleinfo>()
                .Property(e => e.functionids)
                .IsUnicode(false);

            modelBuilder.Entity<C_roleinfo>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<C_userinfo>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<C_userinfo>()
                .Property(e => e.pwd)
                .IsFixedLength();

            modelBuilder.Entity<C_userinfo>()
                .Property(e => e.phone)
                .IsFixedLength();

            modelBuilder.Entity<C_userinfo>()
                .Property(e => e.tel)
                .IsFixedLength();

            modelBuilder.Entity<C_userinfo>()
                .Property(e => e.email)
                .IsFixedLength();

            modelBuilder.Entity<C_userinfo>()
                .Property(e => e.address)
                .IsFixedLength();

            modelBuilder.Entity<C_userinfo>()
                .Property(e => e.uid)
                .IsFixedLength();

            modelBuilder.Entity<仓库记录>()
                .Property(e => e.type)
                .IsFixedLength();

            modelBuilder.Entity<仓库记录>()
                .Property(e => e.source)
                .IsFixedLength();

            modelBuilder.Entity<仓库记录>()
                .Property(e => e.转移连单编号)
                .IsFixedLength();

            modelBuilder.Entity<仓库记录>()
                .Property(e => e.登记人)
                .IsFixedLength();

            modelBuilder.Entity<仓库记录>()
                .Property(e => e.登记日志)
                .IsUnicode(false);

            modelBuilder.Entity<仓库记录>()
                .Property(e => e.车辆情况)
                .IsUnicode(false);

            modelBuilder.Entity<仓库记录>()
                .Property(e => e.数量单位)
                .IsFixedLength();

            modelBuilder.Entity<仓库记录>()
                .Property(e => e.ohter)
                .IsFixedLength();

            modelBuilder.Entity<仓库转移类别>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<仓库转移类别>()
                .Property(e => e.other)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.合同编号)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.合同内容)
                .IsUnicode(false);

            modelBuilder.Entity<合同记录>()
                .Property(e => e.甲方联系人)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.乙方联系人)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.甲方联系电话)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.乙方联系电话)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.甲方传真)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.乙方传真)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.甲方邮箱)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.乙方邮箱)
                .IsFixedLength();

            modelBuilder.Entity<合同记录>()
                .Property(e => e.other)
                .IsFixedLength();

            modelBuilder.Entity<合同类别>()
                .Property(e => e.code)
                .IsFixedLength();

            modelBuilder.Entity<合同类别>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<合同类别>()
                .Property(e => e.other)
                .IsFixedLength();

            modelBuilder.Entity<区域代码>()
                .Property(e => e.code)
                .IsFixedLength();

            modelBuilder.Entity<区域代码>()
                .Property(e => e.area)
                .IsFixedLength();

            modelBuilder.Entity<区域代码>()
                .Property(e => e.province)
                .IsFixedLength();

            modelBuilder.Entity<区域代码>()
                .Property(e => e.city)
                .IsFixedLength();

            modelBuilder.Entity<区域代码>()
                .Property(e => e.other)
                .IsFixedLength();
        }
    }
}
