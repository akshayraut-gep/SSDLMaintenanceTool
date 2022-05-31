using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using CC = SSDLMaintenanceTool.Constants.ConsolidationConstants;

namespace SSDLMaintenanceTool.Models
{
    [DataContract]
    [Serializable]
    public class DataBricksJobResponse
    {
        [DataMember]
        public string partnercode { get; set; }

        [DataMember]
        public int activity_id { get; set; }

        [DataMember]
        public int event_id { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public int job_id { get; set; }

        [DataMember]
        public string submissionid { get; set; }

        [DataMember]
        public string start_date_time { get; set; }

        [DataMember]
        public string end_date_time { get; set; }

        [DataMember]
        public string errormessage { get; set; }

        [DataMember]
        public string warningmessage { get; set; }

        [DataMember]
        public ReturnObject return_object { get; set; }

        [DataMember]
        public List<FilesList> loaddataoutput { get; set; }

        [DataMember]
        public string DCCOutput { get; set; }

        [DataMember]
        public List<string> aiOutput { get; set; }

        [DataMember]
        public Output output { get; set; }

        [DataMember]
        public string eventName { get; set; }

        [DataMember]
        public List<RollbackEvents> rollbackEvents { get; set; }

        [DataMember]
        public List<ErrorDataFiles> errorDataFiles { get; set; }

        [DataMember]
        public string information { get; set; }

    }

    [DataContract]
    [Serializable]
    public class FilesList
    {
        [DataMember]
        public string file_name { get; set; }

        [DataMember]
        public int fileLogId { get; set; }
        [DataMember]
        public string file_path { get; set; }
        [DataMember]
        public string sheetName { get; set; }

    }

    [DataContract]
    [Serializable]
    public class Output
    {
        [DataMember]
        public List<load_data> load_data { get; set; }
        [DataMember]
        public List<ConsolidationADBOutput> consolidation { get; set; }
        [DataMember]
        public Profile profile { get; set; }
        [DataMember]
        public object ai { get; set; }
        [DataMember]
        public List<ResponseObject> vne { get; set; }
        [DataMember]
        public List<ResponseObject> classification { get; set; }
        [JsonProperty("Import")]
        [JsonPropertyName("Import")]
        public List<Import> Import { get; set; }
        [DataMember]
        public List<Export> Export { get; set; }
        [DataMember]
        public Bucket bucket { get; set; }
        [DataMember]
        public List<ResponseObject> publish { get; set; }
        [DataMember]
        public List<MBNGenerateMasterADBOutput> generateMaster { get; set; }
        [DataMember]
        [JsonProperty("activeColumns")]
        public List<ActiveColumn> ActiveColumns { get; set; }
        [DataMember]
        [JsonProperty("sourceSystems")]
        public List<SourceSystemValue> SourceSystems { get; set; }

        [DataMember]
        public List<RWB> rwb { get; set; }
    }

    [DataContract]
    [Serializable]
    public class Profile
    {
        [DataMember]
        public object TrendChecks { get; set; }
        [DataMember]
        public object DataIntegrityChecks { get; set; }
        [DataMember]
        public object SummaryReport { get; set; }
        [DataMember]
        public object OutlierReport { get; set; }
        [DataMember]
        public object FieldWiseReport { get; set; }
        [DataMember]
        public ResponseObject StagingCubePublish { get; set; }
        [DataMember]
        public object ExclusionSummary { get; set; }
        [DataMember]
        public object GenerateExcel { get; set; }
        [DataMember]
        public object Approval { get; set; }
    }

    public class Import
    {
        public string PreviewFlag { get; set; }

        public string AffectedSpend { get; set; }

        public string AffectedRows { get; set; }

        public string PreviewFilePath { get; set; }

        public string ErrorFilepath { get; set; }

        public string CompleteDownloadFilePath { get; set; }
    }

    public class Export
    {
        [DataMember]
        public string PreviewFlag { get; set; }
        [DataMember]
        public string DownloadFilePath { get; set; }
        [DataMember]
        public string AffectedRows { get; set; }
        [DataMember]
        public string FilteredRowCount { get; set; }
        [DataMember]
        public string ExportUIDColumn { get; set; }
    }

    [DataContract]
    [Serializable]
    public class Bucket
    {
        [DataMember]
        public string EventName { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string WarningMessage { get; set; }
        [DataMember]
        public string ViewSummaryFilePath { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ActiveColumn
    {
        [DataMember]
        [JsonProperty("tableName")]
        public string TableName { get; set; }

        [DataMember]
        [JsonProperty("filePath")]
        public string FilePath { get; set; }
    }

    [DataContract]
    [Serializable]
    public class SourceSystemValue
    {
        [DataMember]
        [JsonProperty("tableName")]
        public string TableName { get; set; }

        [DataMember]
        [JsonProperty("columnName")]
        public string ColumnName { get; set; }

        [DataMember]
        [JsonProperty("filePath")]
        public string FilePath { get; set; }
    }

    [DataContract]
    [Serializable]
    public class RWB
    {
        [DataMember]
        public string eventName { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string errorMessage { get; set; }
        [DataMember]
        public string warningMessage { get; set; }
        [DataMember]
        public string downloadFilePath { get; set; }
    }


    [DataContract]
    [Serializable]
    public class RollbackEvents
    {
        [DataMember]
        public int event_id { get; set; }

        [DataMember]
        public string eventName { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ErrorDataFiles
    {
        [DataMember]
        public string fileName { get; set; }

        [DataMember]
        public string filePath { get; set; }

        [DataMember]
        public int count { get; set; }
    }

    [DataContract]
    [Serializable]
    public class load_data
    {
        [DataMember]
        public string ResponseType { get; set; }
        [DataMember]
        public string ResponseTypeId { get; set; }
        [DataMember]
        public LoadResponseType ResponseTypeIdEnum { get; set; }
        [DataMember]
        public string file_path { get; set; }
        [DataMember]
        public string file_name { get; set; }
        [DataMember]
        public string table_name { get; set; }
        [DataMember]
        public int file_log_id { get; set; }
        [DataMember]
        public string ValidationStatus { get; set; }
        [DataMember]
        public string LoadDataStatus { get; set; }
        [DataMember]
        public string ImpactedCount { get; set; }
        [DataMember]
        [JsonProperty(PropertyName = "Validation")]
        public List<Validation> Validation { get; set; }
    }

    [DataContract]
    [Serializable]
    public class Validation
    {
        [DataMember]
        public string dataQualityCheckCode { get; set; }
        [DataMember]
        public string dataQualityCheckStatus { get; set; }
        [DataMember]
        public string errormessage { get; set; }
        [DataMember]
        public string warningmessage { get; set; }
    }

    public enum LoadResponseType
    {
        Import = 1,
        ValidationAndLoad = 2,
        Delete = 3
    }
    [DataContract]
    [Serializable]
    public class ReturnObject
    {

        [DataMember]
        public UserExecutionContext UserContext { get; set; }
        [DataMember]
        public string callback_url { get; set; }

        [DataMember]
        public string guid { get; set; }

        [DataMember]
        public string jwtkey { get; set; }

        [DataMember]
        public int activity_id { get; set; }

        [DataMember]
        public int transactionId { get; set; }

        [DataMember]
        public int workflowJobId { get; set; }

        [DataMember]
        public string submissionid { get; set; }
    }

    public class ConsolidationADBOutput
    {
        [DataMember]
        [JsonProperty(CC.Query_Id)]
        [JsonPropertyName(CC.Query_Id)]
        public string Query_Id { get; set; }

        [DataMember]
        [JsonProperty(CC.Query_Name)]
        [JsonPropertyName(CC.Query_Name)]
        public string Query_Name { get; set; }

        [DataMember]
        [JsonProperty(CC.Status)]
        [JsonPropertyName(CC.Status)]
        public string Status { get; set; }

        [DataMember]
        [JsonProperty(CC.ErrorMessage)]
        [JsonPropertyName(CC.ErrorMessage)]
        public string ErrorMessage { get; set; }

        [DataMember]
        [JsonProperty(CC.FilePathFulldata)]
        [JsonPropertyName(CC.FilePathFulldata)]
        public string FilePathFulldata { get; set; }

        [DataMember]
        [JsonProperty(CC.FilePathMissingTransaction)]
        [JsonPropertyName(CC.FilePathMissingTransaction)]
        public string FilePathMissingTransaction { get; set; }

        [DataMember]
        [JsonProperty(CC.UploadFilePath)]
        [JsonPropertyName(CC.UploadFilePath)]
        public string UploadFilePath { get; set; }

        [DataMember]
        [JsonProperty(CC.HistoricalCount)]
        [JsonPropertyName(CC.HistoricalCount)]
        public int HistoricalCount { get; set; }

        [DataMember]
        [JsonProperty(CC.CurrentCount)]
        [JsonPropertyName(CC.CurrentCount)]
        public int CurrentCount { get; set; }

        [DataMember]
        [JsonProperty(CC.EventName)]
        [JsonPropertyName(CC.EventName)]
        public string EventName { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ResponseObject : DownloadPathData
    {
        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        [JsonProperty("sequenceId")]
        public int SequenceId { get; set; }

        [DataMember]
        [JsonProperty("status")]
        public string Status { get; set; }

        [DataMember]
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember]
        [JsonProperty("warningMessage")]
        public string WarningMessage { get; set; }

        [DataMember]
        [JsonProperty("runs")]
        public List<Runs> Runs { get; set; }

        [DataMember]
        [JsonProperty("modelGeneration")]
        public List<ModelGeneration> ModelGeneration { get; set; }

        [DataMember]
        [JsonProperty("predefinedQueriesStatus")]
        public List<PreDefinedResponseObject> predefinedQueriesStatus { get; set; }

    }

    public class DownloadPathData
    {
        [DataMember]
        [JsonProperty("eventName")]
        public string EventName { get; set; }
        [DataMember]
        [JsonProperty("rowsAffected")]
        public string RowsAffected { get; set; }
        [DataMember]
        [JsonProperty("numberOfNewClusters")]
        public string NumberOfNewClusters { get; set; }
        [DataMember]
        [JsonProperty("currentDateRange")]
        public CurrentDateRange CurrentDateRange { get; set; }
        [DataMember]
        [JsonProperty("numberOfDaysInCurrentData")]
        public string NumberOfDaysInCurrentData { get; set; }
        [DataMember]
        [JsonProperty("startDateDerivedFromData")]
        public string StartDateDerivedFromData { get; set; }
        [DataMember]
        [JsonProperty("endDateDerivedFromData")]
        public string EndDateDerivedFromData { get; set; }
        [DataMember]
        [JsonProperty("historicalDateRange")]
        public HistoricalDateRange HistoricalDateRange { get; set; }
        [DataMember]
        [JsonProperty("HeaderPosition")]
        public string HeaderPosition { get; set; }
        [DataMember]
        [JsonProperty("DownloadFilePath")]
        public string DownloadFilePath { get; set; }

        [DataMember]
        [JsonProperty("qualityCheckResult")]
        public string QualityCheckResult { get; set; }

    }
    public class HistoricalDateRange
    {
        [DataMember]
        [JsonProperty("numberOfDaysInHistoricalData")]
        public string NumberOfDaysInHistoricalData { get; set; }
        [DataMember]
        [JsonProperty("startDateDerivedFromData")]
        public string StartDateDerivedFromData { get; set; }
        [DataMember]
        [JsonProperty("endDateDerivedFromData")]
        public string EndDateDerivedFromData { get; set; }
    }

    public class CurrentDateRange
    {
        [DataMember]
        [JsonProperty("numberOfDaysInCurrentData")]
        public string NumberOfDaysInCurrentData { get; set; }
        [DataMember]
        [JsonProperty("startDateDerivedFromData")]
        public string StartDateDerivedFromData { get; set; }
        [DataMember]
        [JsonProperty("endDateDerivedFromData")]
        public string EndDateDerivedFromData { get; set; }
    }

    public class Runs
    {
        [DataMember]
        [JsonProperty("runName")]
        public string RunName { get; set; }
        [DataMember]
        [JsonProperty("runStatus")]
        public string RunStatus { get; set; }
    }

    public class ModelGeneration
    {
        [DataMember]
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [DataMember]
        [JsonProperty("modelStatus")]
        public string ModelStatus { get; set; }
    }
 
    public class MBNGenerateMasterADBOutput
    {
        [DataMember]
        [JsonProperty(CC.MasterName)]
        [JsonPropertyName(CC.MasterName)]
        public string MasterName { get; set; }

        [DataMember]
        [JsonProperty(CC.Status_SM)]
        [JsonPropertyName(CC.Status_SM)]
        public string Status { get; set; }

        [DataMember]
        [JsonProperty(CC.ErrorMessage_SM)]
        [JsonPropertyName(CC.ErrorMessage_SM)]
        public string ErrorMessage { get; set; }

        [DataMember]
        [JsonProperty(CC.Schema)]
        [JsonPropertyName(CC.Schema)]
        public List<Schema> Schema { get; set; }

        [DataMember]
        [JsonProperty(CC.DownloadFilePath)]
        [JsonPropertyName(CC.DownloadFilePath)]
        public string DownloadFilePath { get; set; }
    }

    public class Schema
    {
        [DataMember]
        [JsonProperty(CC.ColumnName)]
        [JsonPropertyName(CC.ColumnName)]
        public string ColumnName { get; set; }

        [DataMember]
        [JsonProperty(CC.DataType)]
        [JsonPropertyName(CC.DataType)]
        public string DataType { get; set; }
    }
    public class PreDefinedResponseObject
    {
        [DataMember]
        public string rowsAffected { get; set; }
        [DataMember]
        public string queryName { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string queryId { get; set; }
        [DataMember]
        public string errorMessage { get; set; }

    }

    public class UserExecutionContext
    {
        public const string ERROR_DOCUMENT_ACCESS_DENIED = "501";
        public const string ERROR_DOCUMENT_DELETED = "502";
        public const string ERROR_DOCUMENT_NOT_FOUND = "503";

        public UserExecutionContext()
        {

        }

        public long ServingEntityDetailCode { get; set; }
        public int ServingEntityId { get; set; }
        public long BelongingEntityDetailCode { get; set; }
        public int BelongingEntityId { get; set; }
        public string DefaultCurrencyCode { get; set; }
        public bool IsSuperUser { get; set; }
        public bool IsSupplier { get; set; }
        public bool IsAdmin { get; set; }
        public long BuyerPartnerCode { get; set; }
        public long ClientID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public int ShipToLocationId { get; set; }
        public string Culture { get; set; }
        public GEPSuite Product { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string LoggerCode { get; set; }
        public long ContactCode { get; set; }
        public int UserId { get; set; }
        public string ClientName { get; set; }
        public Collection<string> Contexts { get; }
        public string BuyerPartnerName { get; set; }
    }

    [CLSCompliant(true)]
    public enum GEPSuite
    {
        None = 0,
        eAuction = 1,
        eRFx = 2,
        eContract = 3,
        eSpend = 4,
        ePurchase = 5,
        eProjects = 6,
        eSupplier = 7,
        eCatalog = 8,
        eCompany = 9,
        Report = 10,
        eCategory = 11,
        eReports = 12,
        eCompliance = 13,
        eDemand = 14,
        eMarketPrice = 15,
        eInterface = 16,
        ApprovalSys = 17,
        ItemMaster = 18
    }
}
