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
    /// 实体类区域代码。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Table("区域代码")]
    [Serializable]
    public partial class 区域代码 : Entity
    {
        #region Model
		private int _id;
		private string _code;
		private string _area;
		private string _province;
		private string _city;
		private string _other;

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
		[Field("code")]
		public string code
		{
			get{ return _code; }
			set
			{
				this.OnPropertyValueChange("code");
				this._code = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("area")]
		public string area
		{
			get{ return _area; }
			set
			{
				this.OnPropertyValueChange("area");
				this._area = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("province")]
		public string province
		{
			get{ return _province; }
			set
			{
				this.OnPropertyValueChange("province");
				this._province = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("city")]
		public string city
		{
			get{ return _city; }
			set
			{
				this.OnPropertyValueChange("city");
				this._city = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("other")]
		public string other
		{
			get{ return _other; }
			set
			{
				this.OnPropertyValueChange("other");
				this._other = value;
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
				_.code,
				_.area,
				_.province,
				_.city,
				_.other,
			};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._id,
				this._code,
				this._area,
				this._province,
				this._city,
				this._other,
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
			public readonly static Field All = new Field("*", "区域代码");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field id = new Field("id", "区域代码", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field code = new Field("code", "区域代码", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field area = new Field("area", "区域代码", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field province = new Field("province", "区域代码", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field city = new Field("city", "区域代码", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field other = new Field("other", "区域代码", "");
        }
        #endregion
	}
}