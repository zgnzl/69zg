﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//     Website: http://ITdos.com/Dos/ORM/Index.html
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dos.ORM;

namespace TAHB.Model
{
    /// <summary>
    /// 实体类statisticsUser。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Table("statisticsUser")]
    [Serializable]
    public partial class statisticsUser : Entity
    {
        #region Model
		private int _id;
		private string _username;
		private string _userpwd;
		private string _useruid;
		private DateTime? _regtime;
		private DateTime? _endtime;
		private string _email;
		private string _tel;
		private string _description;

		/// <summary>
		/// 
		/// </summary>
		[Field("id")]
		public int id
		{
			get{ return _id; }
			set
			{
				this.OnPropertyValueChange("id");
				this._id = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("username")]
		public string username
		{
			get{ return _username; }
			set
			{
				this.OnPropertyValueChange("username");
				this._username = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("userpwd")]
		public string userpwd
		{
			get{ return _userpwd; }
			set
			{
				this.OnPropertyValueChange("userpwd");
				this._userpwd = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("useruid")]
		public string useruid
		{
			get{ return _useruid; }
			set
			{
				this.OnPropertyValueChange("useruid");
				this._useruid = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("regtime")]
		public DateTime? regtime
		{
			get{ return _regtime; }
			set
			{
				this.OnPropertyValueChange("regtime");
				this._regtime = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("endtime")]
		public DateTime? endtime
		{
			get{ return _endtime; }
			set
			{
				this.OnPropertyValueChange("endtime");
				this._endtime = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("email")]
		public string email
		{
			get{ return _email; }
			set
			{
				this.OnPropertyValueChange("email");
				this._email = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("tel")]
		public string tel
		{
			get{ return _tel; }
			set
			{
				this.OnPropertyValueChange("tel");
				this._tel = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("description")]
		public string description
		{
			get{ return _description; }
			set
			{
				this.OnPropertyValueChange("description");
				this._description = value;
			}
		}
		#endregion

		#region Method
        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
				_.id,
			};
        }
		/// <summary>
        /// 获取实体中的标识列
        /// </summary>
        public override Field GetIdentityField()
        {
            return _.id;
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
				_.id,
				_.username,
				_.userpwd,
				_.useruid,
				_.regtime,
				_.endtime,
				_.email,
				_.tel,
				_.description,
			};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._id,
				this._username,
				this._userpwd,
				this._useruid,
				this._regtime,
				this._endtime,
				this._email,
				this._tel,
				this._description,
			};
        }
        /// <summary>
        /// 是否是v1.10.5.6及以上版本实体。
        /// </summary>
        /// <returns></returns>
        public override bool V1_10_5_6_Plus()
        {
            return true;
        }
        #endregion

		#region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
			/// <summary>
			/// * 
			/// </summary>
			public readonly static Field All = new Field("*", "statisticsUser");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field id = new Field("id", "statisticsUser", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field username = new Field("username", "statisticsUser", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field userpwd = new Field("userpwd", "statisticsUser", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field useruid = new Field("useruid", "statisticsUser", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field regtime = new Field("regtime", "statisticsUser", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field endtime = new Field("endtime", "statisticsUser", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field email = new Field("email", "statisticsUser", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field tel = new Field("tel", "statisticsUser", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field description = new Field("description", "statisticsUser", "");
        }
        #endregion
	}
}