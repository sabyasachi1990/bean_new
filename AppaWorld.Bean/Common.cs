using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppaWorld.Bean
{
	public class Common
	{


		#region Document_History_Fill_Method
		public static void SaveDocumentHistory(List<DocumentHistoryModel> documentHistoryModel, string connectionString)
		{

			SqlConnection con = null;
			SqlDataReader dr = null;
			SqlCommand cmd = null;
			string query = string.Empty;
			ListtoDataTable lsttodt = new ListtoDataTable();

			using (con = new SqlConnection(connectionString))
			{
				try
				{
					DataSet ds = new DataSet();
					if (con.State != ConnectionState.Open)
						con.Open();
					cmd = new SqlCommand("Bean_DocumentHistory", con);
					cmd.CommandType = CommandType.StoredProcedure;

					SqlParameter parm = new SqlParameter();
					parm.ParameterName = "@BCDocumentHistoryType";
					parm.TypeName = "dbo.DocumentHistoryTableType";
					parm.Value = ToDataTable(documentHistoryModel);
					cmd.Parameters.Add(parm);
					SqlDataAdapter sqlDA = new SqlDataAdapter();
					sqlDA.SelectCommand = cmd;
					sqlDA.Fill(ds);
					con.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}
		public static DataTable ToDataTable<T>(IList<T> data)
		{
			PropertyDescriptorCollection props =
				TypeDescriptor.GetProperties(typeof(T));
			DataTable table = new DataTable();
			for (int i = 0; i < props.Count; i++)
			{
				PropertyDescriptor prop = props[i];

				//table.Columns.Add(prop.Name, prop.PropertyType);

				table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
			prop.PropertyType) ?? prop.PropertyType);


			}
			object[] values = new object[props.Count];
			foreach (T item in data)
			{
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = props[i].GetValue(item);
				}
				table.Rows.Add(values);
			}
			return table;
		}
		#endregion Document_History_Fill_Method
		public class ListtoDataTable
		{
			public DataTable ToDataTable<T>(List<T> items)
			{
				DataTable dataTable = new DataTable(typeof(T).Name);
				//Get all the properties by using reflection   
				PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
				foreach (PropertyInfo prop in Props)
				{
					//Setting column names as Property names  
					dataTable.Columns.Add(prop.Name);
				}
				foreach (T item in items)
				{
					var values = new object[Props.Length];
					for (int i = 0; i < Props.Length; i++)
					{

						values[i] = Props[i].GetValue(item, null);
					}
					dataTable.Rows.Add(values);
				}

				return dataTable;
			}
		}
		public static List<DocumentHistoryModel> FillDocumentHistory(Guid transationId, long companyId, Guid documentId, string docType, string docSubType, string documentState, string docCurrency, decimal grandTotal, decimal balanceAmount, decimal exchangeRate, string modifiedBy, string remarks, DateTime? postingDate, decimal? appliedAmount, decimal? roundingAmount)
		{
			DocumentHistoryModel documentHistoryModel = new DocumentHistoryModel();
			documentHistoryModel.TransactionId = transationId != null ? transationId : documentId;
			documentHistoryModel.CompanyId = companyId;
			documentHistoryModel.DocumentId = documentId;
			documentHistoryModel.DocType = docType;
			documentHistoryModel.DocSubType = docSubType;
			documentHistoryModel.DocState = documentState;
			documentHistoryModel.DocCurrency = docCurrency;
			documentHistoryModel.DocAmount = grandTotal;
			documentHistoryModel.DocBalanaceAmount = balanceAmount;
			documentHistoryModel.ExchangeRate = exchangeRate;
			documentHistoryModel.BaseAmount = Math.Round(Convert.ToDecimal(((exchangeRate != null ? exchangeRate : 1) * grandTotal)), 2);
			documentHistoryModel.BaseBalanaceAmount = Math.Round(Convert.ToDecimal(((exchangeRate != null ? exchangeRate : 1) * balanceAmount)), 2);
			documentHistoryModel.StateChangedBy = modifiedBy;
			documentHistoryModel.Remarks = remarks;
			documentHistoryModel.PostingDate = postingDate;
			documentHistoryModel.DocAppliedAmount = appliedAmount;
			documentHistoryModel.BaseAppliedAmount = appliedAmount != null ? ((Math.Round(Convert.ToDecimal(((exchangeRate != null ? exchangeRate : 1) * documentHistoryModel.DocAppliedAmount)), 2, MidpointRounding.AwayFromZero)) + (roundingAmount)) : (decimal?)null;
			List<DocumentHistoryModel> lstdocumet = new List<DocumentHistoryModel>();
			lstdocumet.Add(documentHistoryModel);
			return lstdocumet;
		}


		#region Common_Posting_Method
		public static void SavePosting(long companyId, Guid documentId, string docType, string connectionString)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					if (con.State != ConnectionState.Open)
						con.Open();
					SqlCommand cmd = new SqlCommand("Bean_Posting", con);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandTimeout = 0;
					cmd.Parameters.AddWithValue("@SourceId", documentId);
					cmd.Parameters.AddWithValue("@Type", docType);
					cmd.Parameters.AddWithValue("@CompanyId", companyId);
					cmd.ExecuteNonQuery();
					if (con.State != ConnectionState.Closed)
						con.Close();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public static void SaveMultiplePosting(long companyId, Guid masterId, Guid docId, string docType, string docSubType, bool isReverseExcess, bool isOffset, string connectionString, string roundingAmount)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					if (con.State != ConnectionState.Open)
						con.Open();
					SqlCommand cmd = new SqlCommand("Bean_Multiple_Posting", con);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@MasterId", masterId);
					cmd.Parameters.AddWithValue("@DocumentId", docId);
					cmd.Parameters.AddWithValue("@DocType", docType);
					cmd.Parameters.AddWithValue("@DocSubType", docSubType);
					cmd.Parameters.AddWithValue("@IsOffset", isOffset);
					cmd.Parameters.AddWithValue("@IsreverseExcess", isReverseExcess);
					cmd.Parameters.AddWithValue("@CompanyId", companyId);
					cmd.Parameters.AddWithValue("@RoundingAmount", roundingAmount != null ? roundingAmount : string.Empty);
					cmd.ExecuteNonQuery();
					if (con.State != ConnectionState.Closed)
						con.Close();
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		//multiple posting for BT
		public static void SaveMultiplePosting(long companyId, Guid documentId, string docType, string connectionString)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					if (con.State != ConnectionState.Open)
						con.Open();
					SqlCommand cmd = new SqlCommand("Bean_IC_Posting_ALL", con);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@SourceId", documentId);
					cmd.Parameters.AddWithValue("@Type", docType);
					cmd.Parameters.AddWithValue("@CompanyId", companyId);
					cmd.ExecuteNonQuery();
					if (con.State != ConnectionState.Closed)
						con.Close();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion



		public static void SaveDocNoSequence(long companyId, string docType, string connectionString)
		{
			try
			{
				string query = string.Empty;
				query = $"Update Bean.AutoNumber set GeneratedNumber=(Select GeneratedNumber from Bean.AutoNumber where CompanyId={companyId} and ModuleMasterId=4 and EntityType='{docType}')-1  where CompanyId={companyId} and ModuleMasterId=(SELECT  m.Id FROM Common.ModuleMaster as m WHERE Name = 'Bean Cursor' and CompanyId = 0) and EntityType='{docType}'";
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					if (con.State != ConnectionState.Open)
						con.Open();
					SqlCommand cmd = new SqlCommand(query, con);
					cmd.ExecuteNonQuery();
					if (con.State == ConnectionState.Open)
						con.Close();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
