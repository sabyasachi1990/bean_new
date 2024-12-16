using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReportsModule.Infra
{
    public class BeanLogConstant
    {
        public static string DashBoardService = "Bean DashBoard Service";

        //ForConnection
        public static string Log_GetObjective_GetCall_Connection_Message = "Enter to open the connection";
        public static string Log_GetObjective_GetCall_ConnectionStatusOpen_Message = "Now Connection open Successfully";
        public static string Log_GetObjective_GetCall_ConnectionStatusClose_Message = "Now Connection Close Successfully";

        //For default SP
        public static string Log_GetObjective_GetCall_DefaultSpCalling_Message = "Enter to call Default SP";
        public static string Log_GetObjective_GetCall_ForDefaultSPResult_Message = "Successfully call Default SP";
        //For Left/Right SP
        public static string Log_GetObjective_GetCall_LeftOrRightSPCalling_Message = "Enters to Call Left/Right Side StoreProcedure";
        public static string Log_GetObjective_GetCall_LeftOrRightSPResult_Message = "Successfully call Left/Right Side StoeProcedure";
        //For From&ToDate
        public static string Log_GetObjective_GetCall_DateMethodCalling_Message = "Enter to Call GetFromandToDate Method";
        public static string Log_GetObjective_GetCall_DateMethod_Message = "Enter to Start GetFromandToDate Method";
        public static string Log_GetObjective_GetCall_DateMethodResult_Message = "Successfully Compleated GetFromandToDate Method";
        //ForCommandExecution
        public static string Log_GetObjective_GetCall_Set_ParametersToSP_Message = "Ready to set Parameters for SP Throw Command";
        public static string Log_GetObjective_GetCall_CommandStatus_Message = "Command Execution Will Be Started";
        public static string Log_GetObjective_GetCall_CommandStatusResult_Message = "Command Execution process succesfully Compleated";

        //For LeftChart
        public static string Log_GetObjective_GetCall_LeftChart_Message = "Enter to call The leftChart Method ";
        public static string Log_GetObjective_GetCall_LeftChartMethod_Message = "Enter to Start The leftChart Method ";
        public static string Log_GetObjective_GetCall_LeftChartMethodResult_Message = "Succesfully Compleated LeftChart Method Execution";


        //For Right Chart
        public static string Log_GetObjective_GetCall_RightChart_Message = "Enter to call The RightChart Method ";
        public static string Log_GetObjective_GetCall_RightChartClling_Message = "Enter Into The RightChart Method";
        public static string Log_GetObjective_GetCall_RightChartResult_Message = "RightChart Method Execution successfully Compleated";
        //For Catch Block
        public static string Log_GetObjective_GetCall_Exception_Message = "Enter Into The Catch Block Because This Method Throw Exception";


        //For KPI
        public static string Log_GetObjective_GetCall_DataReaderStatusCheck_Message1 = "Enter to Check DataReader Have Column Or Not";
        public static string Log_GetObjective_GetCall_DataReaderStatus_Message1 = "DataReader Having Columns so KPI Values Ready to Bind With Values";
        public static string Log_GetObjective_GetCall_DataReaderStatusResult_Message1 = "KPI Values Bind With Some Values Successfully";
        public static string Log_GetObjective_GetCall_DataReaderStatus_Message2 = "DataReader Having No Columns so KPI Values Ready to Bind With Null";
        public static string Log_GetObjective_GetCall_DataReaderStatusResult_Message2 = "KPI Values Bind With Some Default Values";


        //for Legend Values getting
        public static string Log_GetObjective_GetCall_Legends_Message = "Enter to Get Legend List Of Values";
        public static string Log_GetObjective_GetCall_LegendsResult_Message = "Successfully Get Legends List";
        public static string Log_GetObjective_GetCall_DataTable_Status_Message = "Enter to Check DataTable Have Values or Not";
        public static string Log_GetObjective_GetCall_DataTable_StatusResult_Message = "DataTable Having Rows More Than Zero So Enter to If Block";
        public static string Log_GetObjective_GetCall_DataTable_StatusResultForZeroColumns_Message = "DataTable Having Rows Less Than Zero So Enter to Else Block";
        public static string Log_GetObjective_GetCall_ChartSeriesDataResult_Message = "SeriesData List Successfully Get";
        public static string Log_GetObjective_GetCall_ChartSeriesDataCalling_Message = "Enter To Get SeriesData List";



        //for Charts
        public static string Log_GetObjective_GetCall_ChartDataTable_Message = "Enter To start the data table loading";
        public static string Log_GetObjective_GetCall_ChartDataTableSuccess_Message = "Successfully Load The DataTable And Ready to Bind Values For Chart";

        public static string Log_GetObjective_GetCall_ChartDataTableConditon_Message = "Enter to Check Condition DataTable Having Columns Or Not";
        public static string Log_GetObjective_GetCall_ChartDataTableResult1_Message = "Data Table Having Columns So Successfully enter Into if() Block to bind Datatable values to Chart values";
        public static string Log_GetObjective_GetCall_ChartDataTableResult2_Message = "Data Table not having values so enter to if block for retuning NO_Data Message";
        public static string Log_GetObjective_GetCall_ChartDataList_Message = "Ready to Start List Of Datatable values bind to model values";
        public static string Log_GetObjective_GetCall_ChartDataListResult_Message = "Succesfully compleateted.";


        public const string Bean_CustVen_LUs = "[dbo].[Bean_CustVen_LUs]";


        public const string log_GetGeneralLedgerLu_Entering_ApplicationService = "log GetGeneralLedgerLu Entering ApplicationService";
        public const string log_GetGeneralLedgerLu_Completed_ApplicationService = "log GetGeneralLedgerLu Completed ApplicationService";
        public const string log_GetGeneralLedgerLu_Failed_ApplicationService = "log GetGeneralLedgerLu Failed ApplicationService";
        public const string log_GetFinancialLu_Entering_ApplicationService = "log GetFinancialLu Entering ApplicationService";
        public const string log_CustVenAgingLU_Entering_ApplicationService = "log CustVenAgingLU Entering ApplicationService";
        public const string log_CustVenAgingLU_Completed_ApplicationService = "log CustVenAgingLU Completed ApplicationService";
        public const string log_GetFinancialLu_Failed_ApplicationService = "log GetFinancialLu Failed ApplicationService";
        public const string ReportsReadOnlyApplicationService = "ReportsReadOnly ApplicationService";
    }

}
