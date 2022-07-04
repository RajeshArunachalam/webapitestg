using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR_API.App_Code
{
    public static class ClsProcedures
    {
        public static readonly string uspRuleAutoAllocation = "uspRuleAutoAllocation";
        
        public static readonly string uspSubmitPreAuditAcknowledgement = "uspSubmitPreAuditAcknowledgement";
        public static readonly string uspGetBotResponseView = "uspGetBotResponseView";
        public static readonly string uspGetBotAccounts = "uspGetBotAccounts";
        public static readonly string UspBotProcess = "uspbotprocess";
        public static readonly string UspInsertAvailityAccountData = "uspinsertbotdata";
        
        public static readonly string UspSubmitRebuttalData = "UspSubmitRebuttalData";
        public static readonly string Uspupdaterebuttalaccount = "Uspupdaterebuttalaccount";
        public static readonly string UspGetAgentAnalysis = "UspGetAgentAnalysis";
        public static readonly string UspGetProductionPerformanceChart = "UspGetProductionPerformanceChart";
        public static readonly string uspProductionHourChartCommon = "uspProductionHourChartCommon";
        public static readonly string UspErrorAnalysisCommon = "UspErrorAnalysisCommon";
        public static readonly string uspInventoryAnalysisCommon = "uspInventoryAnalysisCommon";
        public static readonly string UspSubErrorAnalysisCommon = "UspSubErrorAnalysisCommon";
        public static readonly string UspGetQualityAnalysisCommon = "UspGetQualityAnalysisCommon";
        public static readonly string uspTimeAnalysisCommon = "uspTimeAnalysisCommon";
        public static readonly string UspAHTAnalysisCommon = "UspAHTAnalysisCommon";
        public static readonly string UspPerformanceAnalysisCommon = "UspPerformanceAnalysisCommon";
        public static readonly string uspGetAllusers = "uspGetAllusers";

        public static readonly string uspQCAnalysis = "uspQCAnalysis";
        public static readonly string UspGetQualityAnalysis = "UspGetQualityAnalysis";

        public static readonly string UspGetUserDetailsbyCondition = "UspGetUserDetailsbyCondition";
        public static readonly string UspGetProductionHourChart = "UspGetProductionHourChart";
        public static readonly string UspDeleteClientdocument = "UspDeleteClientdocument";

        public static readonly string GetClientDocuments = "GetClientDocuments";
        public static readonly string GetClarification = "GetClarification";
        public static readonly string UspInsertdocuments = "UspInsertdocuments";
        public static readonly string UspResetPassword = "UspResetPassword";
        public static readonly string UspSaveTokenDetails = "uspSaveTokenDetails";
        public static readonly string UspCheckTokenDetails = "uspCheckTokenDetails";
        public static readonly string UspGetConfigurationsDetails = "uspGetConfigurationsDetails";
        public static readonly string UspGetUserValidationDetails = "uspGetUserValidationDetails";
        public static readonly string UspGetUserDetails = "uspGetUserDetails";
        public static readonly string UspGetClientProjectMapping = "uspGetClientProjectMapping";
        public static readonly string UspUpdateLogoutTime = "uspUpdateLogoutTime";
        public static readonly string UspGetMenuDetails = "uspGetMenuDetails";
        public static readonly string UspClientProjectMapping = "uspClientProjectMapping";
        public static readonly string UspGetMasterPreLoadData = "uspGetMasterPreLoadData";
        public static readonly string UspChangeStatusOfPHM = "uspChangeStatusOfPHM";
        public static readonly string uspChangeStatusOfCHM = "uspChangeStatusOfCHM";
        public static readonly string UspGetFieldMappingDetails = "uspGetFieldMappingDetails";
        public static readonly string UspGetSupplyTypes = "uspGetSupplyTypes";
        public static readonly string UspSaveMappedFieldDetails = "uspSaveMappedFieldDetails";
        public static readonly string UspGetAdditionalCaptures = "uspGetAdditionalCaptures";
        public static readonly string UspGetRoles = "uspGetRoles";
        public static readonly string UspGetUserMasterPreLoadData = "uspGetUserMasterPreLoadData";
        public static readonly string UspAddAdditionalCapture = "uspAddAdditionalCapture";
        public static readonly string UspEnableDisableAddCapture = "uspEnableDisableAddCapture";
        public static readonly string UspReOrderAdditionalCapture = "uspReOrderAdditionalCapture";
        public static readonly string UspGetMasterProcessflowPreLoadData = "uspGetMasterProcessflowPreLoadData";
        public static readonly string UspGetProcessFlowDeails = "uspGetProcessFlowDeails";
        public static readonly string UspEnableDisableAddprocess = "uspEnableDisableAddprocess";
        public static readonly string UspAddProcessFlow = "uspAddProcessFlow";
        public static readonly string UspUsersMappingDetails = "uspUsersMappingDetails";
        public static readonly string UspAddEmployeeDetails = "uspAddEmployeeDetails";
        public static readonly string UspClientAccessMappingByEmpID = "uspClientAccessMappingByEmpID";
        public static readonly string UspSubmitMappUserDetails = "uspSubmitMappUserDetails";
        public static readonly string UspGetMasterRule = "uspGetMasterRule";
        public static readonly string UspGetRuleBuilderProject = "uspGetRuleBuilderProject";
        public static readonly string UspGetImportFilesByPHMID = "UspGetImportFilesByPHMID";
        public static readonly string UspGetAccountDetailsByImportFileID = "UspGetAccountDetailsByImportFileID";
        public static readonly string UspAddNewRule = "uspAddNewRule";
        public static readonly string UspGetRuletypeDetails = "uspGetRuletypeDetails";
        public static readonly string UspChangeRuleStatus = "uspChangeRuleStatus";

        public static readonly string UspGetWorkAllotmentPreLoadData = "uspGetWorkAllotmentPreLoadData";
        public static readonly string UspGetWorkAllotmentAccounts = "uspGetWorkAllotmentAccounts";
        public static readonly string UspGetFieldMappings = "uspGetFieldMappings";
        public static readonly string UspSSISPackageExecution = "uspSSISPackageExecution";
        public static readonly string UspGetSupplyTemplate = "uspGetSupplyTemplate";
        public static readonly string UspGetImportFileDetails = "uspGetImportFileDetails";
        public static readonly string UspGetUserDetailsByPHMID = "uspGetUserDetailsByPHMID";
        public static readonly string UspGetMappedUsersForRule = "uspGetMappedUsersForRule";
        public static readonly string UspAssignUserToRule = "UspAssignUserToRule";
        public static readonly string UspAllocationProduction = "uspAllocationProduction";
        public static readonly string UspGetImportFileList = "uspGetImportFileList";
        public static readonly string UspAddClientDetails = "uspAddClientDetails";
        public static readonly string UspGetProductionInboxPreLoadData = "uspGetProductionInboxPreLoadData";
        public static readonly string UspProductionAllocation = "uspProductionAllocation";
        public static readonly string UspGetAccountRuleDetails = "uspGetAccountRuleDetails";
        public static readonly string UspUpdateAccountStartedTime = "uspUpdateAccountStartedTime";
        public static readonly string UspSubmitProductionTransaction = "uspSubmitProductionTransaction";
        public static readonly string UspGetAuditAccounts = "uspGetAuditAccounts";
        public static readonly string UspDashboardrolewise = "uspDashboardrolewise";
        public static readonly string UspGetAuditInboxPreLoadData = "uspGetAuditInboxPreLoadData";
        public static readonly string UspSubmitAuditTransaction = "uspSubmitAuditTransaction";
        public static readonly string uspEditAuditTransaction = "uspEditAuditTransaction";
        public static readonly string UspGetErrorCategoryMappedDetails = "uspGetErrorCategoryMappedDetails";
        public static readonly string UspGetAccountHistoryDetails = "uspGetAccountHistoryDetails";
        public static readonly string UspGetAccountBotReponseDetails = "uspGetBotResponseData";
        public static readonly string UspGetSMEInboxPreLoadData = "uspGetSMEInboxPreLoadData";
        public static readonly string UspSMEAllocation = "uspSMEAllocation";
        public static readonly string UspSubmitSMETransaction = "uspSubmitSMETransaction";
        public static readonly string UspGetReportRDLFileDetails = "uspGetReportRDLFileDetails";
        public static readonly string UspGetFileDetails = "uspGetFileDetails";
        public static readonly string UspGetErrorRecords = "uspGetErrorRecords";
        public static readonly string uspGetAccountStatusSummary = "uspGetAccountStatusSummary";
        public static readonly string UspGetWorkAllocationFilterData = "uspGetWorkAllocationFilterData";
        public static readonly string UspSubmitClarification = "uspSubmitClarification";
        public static readonly string UspSubmitClarificationResponse = "uspSubmitClarificationResponce";
        public static readonly string UspSubmitSMEClarification = "uspSubmitSMEClarification";
        public static readonly string UspSubmitTLClarificationResponse = "uspSubmitTLClarificationResponse";
        public static readonly string UspSubmitNonWorkable = "uspSubmitNonWorkable";
        public static readonly string UspGetManualCaptures = "uspGetManualCaptures";
        public static readonly string UspSumbitManualCaptures = "uspSumbitManualCaptures";
        public static readonly string UspGetManualEntryPreLoadData = "uspGetManualEntryPreLoadData";
        public static readonly string UspEnableDisableManualCapture = "uspEnableDisableManualCapture";
        public static readonly string UspGetManualAccountLoadData = "uspGetManualAccountLoadData";
        public static readonly string UspSubmitManualEntryTransaction = "uspSubmitManualEntryTransaction";
        public static readonly string UspDeleteManualCapture = "uspDeleteManualCapture";
        public static readonly string UspManualCapture = "uspManualCapture";
        public static readonly string UspGetSupplyEntryAccountLoadData = "uspGetSupplyEntryAccountLoadData";
        public static readonly string UspSubmitSupplyEntryTransaction = "uspSubmitSupplyEntryTransaction";

        public static readonly string UspGetBotInputDetails = "uspGetBotInputDetails";
        public static readonly string UspSumbitBotInputs = "uspSumbitBotInputs";
        public static readonly string UspBotInputEnableOrDisable = "uspBotInputEnableOrDisable";
        public static readonly string UspGetBotInputPreLoadData = "uspGetBotInputPreLoadData";
        public static readonly string UspResetAccounts = "uspResetAccounts";
        public static readonly string UspFindAccuntInfo = "uspFindAccuntInfo";
        public static readonly string UspupdateManualAvailityAccountData = "UspUpdateManualAvailityAccountData";
        public static readonly string UspGetAvailityConfig = "uspGetAvailityConfig";
        public static readonly string UspGetManualAccountRuleOutcomeInfo = "uspGetManualAccountRuleOutcomeInfo";



    }
}