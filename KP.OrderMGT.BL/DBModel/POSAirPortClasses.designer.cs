﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KP.OrderMGT.BL.DBModel
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="NDCCSVB_Train")]
	public partial class POSAirPortClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public POSAirPortClassesDataContext() : 
				base(global::KP.OrderMGT.BL.Properties.Settings.Default.NDCCSVB_TrainConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public POSAirPortClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public POSAirPortClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public POSAirPortClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public POSAirPortClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.get_sale_passport4")]
		public ISingleResult<get_sale_passport4Result> get_sale_passport4([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> ihour, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Char(20)")] string cpass, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="DateTime")] System.Nullable<System.DateTime> cdate, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Char(1)")] System.Nullable<char> cstatus)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), ihour, cpass, cdate, cstatus);
			return ((ISingleResult<get_sale_passport4Result>)(result.ReturnValue));
		}
	}
	
	public partial class get_sale_passport4Result
	{
		
		private System.Nullable<decimal> _net;
		
		private System.Nullable<int> _lq;
		
		private System.Nullable<int> _tb;
		
		public get_sale_passport4Result()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_net", DbType="Decimal(38,2)")]
		public System.Nullable<decimal> net
		{
			get
			{
				return this._net;
			}
			set
			{
				if ((this._net != value))
				{
					this._net = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_lq", DbType="Int")]
		public System.Nullable<int> lq
		{
			get
			{
				return this._lq;
			}
			set
			{
				if ((this._lq != value))
				{
					this._lq = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_tb", DbType="Int")]
		public System.Nullable<int> tb
		{
			get
			{
				return this._tb;
			}
			set
			{
				if ((this._tb != value))
				{
					this._tb = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
