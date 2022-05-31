﻿namespace SSDLMaintenanceTool.Constants
{
    public class ConsolidationConstants
    {
        #region JSON property names
        public const string QueryName = "queryName";
        public const string QueryId = "queryId";
        public const string Query = "query";
        public const string SortOrder = "sortOrder";
        public const string SourceTableId = "sourceTableId";
        public const string SourceTable = "sourceTable";
        public const string Action = "action";
        public const string ColumnList = "columnList";
        public const string SourceTableColumn = "sourceTableColumn";
        public const string MainTableColumn = "mainTableColumn";
        public const string SourceTableSchemaId = "sourceTableSchemaId";
        public const string KeyValue = "keyValue";
        public const string ActivityValue = "activityValue";
        public const string KeyKeySequence = "keyKeySequence";
        public const string Type = "type";
        public const string TypeId = "typeId";
        public const string JsonFormat = "jsonFormat";
        public const string OutOfDateRangeBasedOnColumn = "outOfDateRangeBasedOnColumn";
        public const string DateRange = "dateRange";
        public const string DateRangeId = "dateRangeId";
        public const string FromDate = "fromDate";
        public const string ToDate = "toDate";
        public const string DuplicateBasedOn = "duplicateBasedOn";
        public const string DuplicateBasedOnId = "duplicateBasedOnId";
        public const string DuplicateCheckActualColumns = "duplicateCheckActualColumns";
        public const string DuplicateCheckDisplayColumns = "duplicateCheckDisplayColumns";
        public const string QuerySequence = "querySequence";
        public const string Conditions = "conditions";
        public const string Comments = "comments";
        public const string ConditionOperator = "conditionOperator";
        public const string ReferenceColumn = "referenceColumn";
        public const string Criteria = "criteria";
        public const string QueryTag = "queryTag";
        public const string ReferenceValue = "referenceValue";
        public const string WorkflowName = "workflowName";
        public const string ActivityId = "activityId";
        public const string ActivityJson = "activityJson";
        public const string MainTable = "mainTable";
        public const string EventJson = "eventJson";
        public const string StageId = "stageId";
        public const string EventId = "eventId";
        public const string StageName = "stageName";
        public const string EventName = "eventName";
        public const string NextStage = "nextStage";
        public const string NextEvent = "nextEvent";
        public const string IsSelected = "isSelected";
        public const string UpdateList = "updateList";
        public const string UpdateCondition = "updateCondition";
        public const string UpdateConditionSourceColumnTableColumn = "updateConditionSourceColumnTableColumn";
        public const string UpdateConditionSourceColumnTableSchemaId = "updateConditionSourceColumnTableSchemaId";
        public const string UpdateConditionMainTableColumn = "updateConditionMainTableColumn";
        public const string UpdateConditionMainTableSchemaId = "updateConditionMainTableSchemaId";
        public const string FilterOnTableType = "filterOnTableType";
        public const string Operator = "operator";
        public const string CustomFilter = "customFilter";
        public const string Value = "value";
        public const string DateNormType = "dateNormType";
        public const string FiscalYear = "fiscalYear";
        public const string Code = "code";
        public const string Name = "name";
        public const string SourceSystem = "sourceSystem";
        public const string Column = "column";
        public const string Parameters = "parameters";
        public const string StartDate = "startDate";
        public const string EndDate = "endDate";
        public const string Nomenclature = "nomenclature";
        public const string CurrencyColumnMainTable = "currencyColumnMainTable";
        public const string CurrencyColumnSourceTable = "currencyColumnSourceTable";
        public const string SourceCurrencyColumn = "sourceCurrencyColumn";
        public const string TargetCurrencyColumn = "targetCurrencyColumn";
        public const string ConversionFactorColumn = "conversionFactorColumn";
        public const string PeriodDetailsPeriodBasedConversionAllowed = "periodBasedConversionAllowed";
        public const string Period = "period";
        public const string FromDateCol = "fromDateColumn";
        public const string ToDateCol = "toDateColumn";
        public const string SelectedDate = "selectedDate";
        public const string SelectedMonth = "selectedMonth";
        public const string SelectedYear = "selectedYear";
        public const string SpendNormType = "spendNormType";
        public const string TargettedSpendNormCol = "targettedSpendNormCol";
        public const string TargettedCurrency = "targettedCurrency";
        public const string SelectedQuarter = "selectedQuarter";
        public const string SourceSystemColumn = "sourceSystemColumn";
        public const string SourceSystemColumnDisplayName = "sourceSystemColumnDisplayName";
        public const string RefDateNormType = "refDateNormType";
        public const string IsSystemGenerated = "isSystemGenerated";
        public const string BasedOnConditionalCriteria = "basedOnConditionalCriteria";
        public const string ExclusionType = "exclusionType";
        public const string ExclusionTypeId = "exclusionTypeId";
        public const string JobPeriodId = "jobPeriodId";
        public const string QueryType = "queryType";
        public const string CurrentStatus = "currentStatus";
        public const string IsDelete = "isDelete";
        public const string DisplayName = "displayName";
        public const string InputFields = "inputFields";
        public const string NormalizedFields = "normalizedFields";
        public const string WithFilter = "withFilter";
        public const string QueryOperator = "queryOperator";
        public const string FilterConditions = "filterConditions";
        public const string InputFile = "inputFile";
        public const string DataType = "dataType";
        public const string DataLength = "dataLength";
        public const string SequenceId = "sequenceId";
        public const string SystemDefinedQuery = "systemDefinedQuery";
        public const string Source = "source";
        public const string Target = "target";
        public const string WithBodmas = "withBodmas";
        public const string BodmasColumn = "bodmasColumn";
        public const string Query_Id = "Query_Id";
        public const string Query_Name = "Query_Name";
        public const string Status = "Status";
        public const string ErrorMessage = "ErrorMessage";
        public const string FilePathFulldata = "FilePathFulldata";
        public const string FilePathMissingTransaction = "FilePathMissingTransaction";
        public const string UploadFilePath = "UploadFilePath";
        public const string HistoricalCount = "HistoricalCount";
        public const string CurrentCount = "CurrentCount";
        public const string MasterName = "masterName";
        public const string Status_SM = "status";
        public const string ErrorMessage_SM = "errorMessage";
        public const string Schema = "schema";
        public const string ColumnName = "columnName";
        public const string DownloadFilePath = "downloadFilePath";
        public const string DataLakeConfigPath = "{partnerCode}/Spend/Config/{jobId}/{activityId}/{submissionId}";
        public const string DataLakeActivityConfigFileName = "activityConfiguration.json";
        public const string DataLakeProjectConfigFileName = "ProjectConfiguration.json";

        #endregion

        #region event IDs
        public const int LastEventMain = 2291;
        public const int GenerateMaster = 2272;
        #endregion
    }
}
