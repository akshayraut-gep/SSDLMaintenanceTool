using Newtonsoft.Json;
using SSDLMaintenanceTool.Helpers;
using SSDLMaintenanceTool.Implementations;
using SSDLMaintenanceTool.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SSDLMaintenanceTool.Forms
{
    public partial class PredefinedQueriesEditor : Form
    {
        DataTable _queriesTable;

        string sqlQueryTemplateForJobVariable = "DECLARE @JobId INT = {JobId}, @EventId INT = {EventId};\n";
        #region Consolidation queries
        string sqlQueryTemplateWorkflowEventSettingStep = "DECLARE @ActivityId INT = {ActivityId}, @StageId INT = {StageId};\n";
        string sqlQueryTemplateWorkflowEventSettingStepInsert = "\nINSERT INTO SSDL.WorkflowEventSetting(JobId, ActivityId, StageId, EventId, SettingName, SettingValue, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)\n"
                                        + "VALUES(@JobId, @ActivityId, @StageId, null, '{StepName}', '{StepSettingJson}', 1, GETDATE(), 1, GETDATE());\n";
        string sqlQueryTemplateWorkflowEventSettingUpdateExistingStep = "\nUPDATE SSDL.WorkflowEventSetting"
                                        + "\nSET\n"
                                        + "\tSettingValue='{StepSettingJson}'\n"
                                        + "WHERE JobId = {JobId} AND StageId = {StageId} AND SettingName = '{StepName}' AND EventId IS NULL AND ParentId IS NULL;\n";
        string sqlQueryTemplateWorkflowEventSettingParentId = "\nDECLARE @ParentId BIGINT;\n"
                                        + "SELECT @ParentId = Id FROM SSDL.WorkflowEventSetting WHERE JobId = @JobId AND StageId = @StageId AND SettingName = '{StepName}';\n";
        string sqlQueryTemplateWorkflowEventSettingTaskInsert = "INSERT INTO SSDL.WorkflowEventSetting(JobId, ActivityId, StageId, EventId, SettingName, SettingValue, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, ParentId)\n"
                                        + "VALUES (@JobId, @ActivityId, @StageId, @EventID, '{TaskName}', '{TaskSettingJson}', 1, GETDATE(), 1, GETDATE(), @ParentId);\n";
        #endregion

        #region Publish queries
        string sqlQueryTemplateJobDetailsTaskInsert = "\nINSERT INTO SSDL.QueryMaster(QueryName, QueryDescription, Query, IsActive,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,ActivityId,EventId,BaseTableId)\n"
                                        + "VALUES('{QueryName}', '{QueryDescription}', '{Query}', 1, GETDATE(), 1, GETDATE(), 1, @EventID, 1);\n";
        #endregion

        List<QueryType> _queryTypes;


        public PredefinedQueriesEditor()
        {
            InitializeComponent();
        }

        private void PredefinedQueriesEditor_Load(object sender, EventArgs e)
        {
            GetEventDetails();
            this.cbQueryTypes.DataSource = _queryTypes;

            this.cbQueryTypes.ValueMember = "EventId";
            this.cbQueryTypes.DisplayMember = "DisplayName";
            this.cbQueryTypes.SelectedIndex = -1;
            this.cbQueryTypes.Text = "Select query type";

            this._queriesTable = new DataTable();
            this._queriesTable.Columns.Add("SequenceNo");
            this._queriesTable.Columns["SequenceNo"].DataType = typeof(int);
            this._queriesTable.Columns.Add("QueryName");
            this._queriesTable.Columns.Add("Query");
            this._queriesTable.Columns.Add("QueryDescription");
            this.dgvQueries.DataSource = _queriesTable;

            this.cbQueryTypes.SelectedIndexChanged += this.cbQueryTypes_SelectedIndexChanged;
            this.nudSequence.Maximum = Int32.MaxValue;
        }

        private void GetEventDetails()
        {
            this._queryTypes = new List<QueryType>();
            this._queryTypes.Add(new QueryType(2200, 2210, 2211, "Cleanse Data in Source Table", typeof(CleansingAtSourceJsonValidator)));
            this._queryTypes.Add(new QueryType(2200, 2300, 2301, "System defined Steps", typeof(SystemDefinedQueryJsonValidator), true));
            this._queryTypes.Add(new QueryType(6600, 6610, 6611, "Pre-defined Steps", typeof(PublishPredefinedQueryJsonValidator)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (cbQueryTypes.SelectedIndex < 0)
            //{
            //    MessageBox.Show("Select a query type first");
            //    return;
            //}
            _queriesTable.Rows.Add(_queriesTable.Rows.Count + 1, "", "");
            //NewQuery newQuery = new NewQuery(this);
            //newQuery.ShowDialog();
        }

        public bool OnQuerySave(string name, string query)
        {
            var matchingDataRow = _queriesTable.Rows.Find(name);
            if (matchingDataRow != null)
            {
                MessageBox.Show("A query with same name already exists");
                return false;
            }
            _queriesTable.Rows.Add(_queriesTable.Rows.Count + 1, cbQueryTypes.SelectedValue, name, query);
            return true;
        }

        private IWorkflowEventJsonValidator GetJsonEventValidatorByEventId(Type queryTypeValidator)
        {
            if (queryTypeValidator == null)
                return null;

            return Activator.CreateInstance(queryTypeValidator) as IWorkflowEventJsonValidator;
        }

        private void cbQueryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbQueryTypes.SelectedItem != null)
            {
                var matchingQueryType = _queryTypes.FirstOrDefault(a => a.EventId == (cbQueryTypes.SelectedItem as QueryType).EventId);
                if (matchingQueryType.AllowMultiple)
                {
                    tbStepName.Text = matchingQueryType.Name + "-1";
                    tbStepName.ReadOnly = false || !cbExistingStep.Checked;
                }
                else
                {
                    tbStepName.ReadOnly = true;
                    tbStepName.Text = matchingQueryType.Name;
                }
            }
        }

        private void cbExistingStep_CheckedChanged(object sender, EventArgs e)
        {
            if (cbQueryTypes.SelectedItem != null)
            {
                var matchingQueryType = _queryTypes.FirstOrDefault(a => a.EventId == (cbQueryTypes.SelectedItem as QueryType).EventId);
                if (matchingQueryType.AllowMultiple)
                {
                    tbStepName.ReadOnly = !cbExistingStep.Checked;
                }
            }
        }

        private string ValidateForm()
        {
            if (cbQueryTypes.SelectedValue == null)
            {
                return "Query Type is not selected";
            }
            else if (_queriesTable == null || _queriesTable.Rows.Count == 0)
            {
                return "Queries are missing";
            }
            else if (!tbJobID.Text.HasContent())
            {
                return "Job ID is missing";
            }
            else if (!tbStepName.Text.HasContent())
            {
                return "Name of step is missing";
            }

            foreach (DataRow dataRow in _queriesTable.Rows)
            {
                if (dataRow["QueryName"] == null || dataRow["QueryName"] == DBNull.Value || dataRow["QueryName"].ToString().Trim() == "")
                {
                    return "Query name is missing for one of the queries";
                }

                if (dataRow["SequenceNo"] == null || dataRow["SequenceNo"] == DBNull.Value || dataRow["SequenceNo"].ToString().Trim() == "" || (int)dataRow["SequenceNo"] < 1)
                {
                    return "Sequence number is either missing or invalid for one of the queries";
                }
            }
            return "";
        }

        private void btnGenerateSQL_Click(object sender, EventArgs e)
        {
            var validationResult = ValidateForm();
            if (validationResult.HasContent())
            {
                MessageBox.Show(validationResult);
                return;
            }

            var diaglogResult = MessageBox.Show("This SQL query will use " + nudSequence.Value.ToString() + " as the position of the Step. If this is correct then click OK otherwise Cancel.", "Confirm", MessageBoxButtons.OKCancel);
            if (diaglogResult == DialogResult.Cancel)
                return;

            var matchingQueryType = _queryTypes.FirstOrDefault(a => a.EventId == (int)cbQueryTypes.SelectedValue);

            StringBuilder sbQueries = new StringBuilder(sqlQueryTemplateForJobVariable);
            sbQueries.Replace("{JobId}", tbJobID.Text);
            sbQueries.Replace("{EventId}", matchingQueryType.EventId.ToString());

            if (matchingQueryType.ActivityId == 2200)
            {
                sbQueries.Append(sqlQueryTemplateWorkflowEventSettingStep);
                if (cbExistingStep.Checked)
                    sbQueries.Append(sqlQueryTemplateWorkflowEventSettingUpdateExistingStep);
                else
                    sbQueries.Append(sqlQueryTemplateWorkflowEventSettingStepInsert);

                sbQueries.Append(sqlQueryTemplateWorkflowEventSettingParentId);

                sbQueries.Replace("{ActivityId}", matchingQueryType.ActivityId.ToString());
                sbQueries.Replace("{StageId}", matchingQueryType.StageId.ToString());
                sbQueries.Replace("{StepName}", tbStepName.Text);

                var stepSetting = new StepTaskJson();
                stepSetting.ChildEventUIDetail = new List<ChildEventUIDetail>();

                foreach (DataRow dataRow in _queriesTable.Rows)
                {
                    var childEventUIDetail = new ChildEventUIDetail();
                    childEventUIDetail.Name = dataRow["QueryName"].ToString();
                    childEventUIDetail.EventId = matchingQueryType.EventId;
                    childEventUIDetail.SequenceId = (int)dataRow["SequenceNo"];
                    stepSetting.ChildEventUIDetail.Add(childEventUIDetail);

                    var jsonValidator = GetJsonEventValidatorByEventId(matchingQueryType.QueryTypeValidator);
                    var jsonEvent = jsonValidator.GetJsonEvent(childEventUIDetail.Name, dataRow["Query"].ToString(), childEventUIDetail.SequenceId);
                    if (jsonEvent != null)
                    {
                        var taskSetting = new StepTaskJson();
                        taskSetting.EventUIDetail = new List<object>();
                        taskSetting.SequenceId = childEventUIDetail.SequenceId;
                        taskSetting.EventUIDetail.Add(jsonEvent);

                        sbQueries.Append(sqlQueryTemplateWorkflowEventSettingTaskInsert);
                        sbQueries.Replace("{TaskName}", childEventUIDetail.Name);
                        var taskSettingJson = JsonConvert.SerializeObject(taskSetting);
                        sbQueries.Replace("{TaskSettingJson}", taskSettingJson);
                    }
                }
                stepSetting.SequenceId = (int)nudSequence.Value;
                var stepSettingJson = JsonConvert.SerializeObject(stepSetting);
                sbQueries.Replace("{StepSettingJson}", stepSettingJson);
            }
            else if (matchingQueryType.ActivityId == 6600)
            {
                foreach (DataRow dataRow in _queriesTable.Rows)
                {
                    sbQueries.Append(sqlQueryTemplateJobDetailsTaskInsert);

                    var queryName = dataRow["QueryName"].ToString();
                    var query = dataRow["Query"].ToString();
                    var queryDescription = dataRow["QueryDescription"].ToString();

                    sbQueries.Replace("{QueryName}", queryName);
                    sbQueries.Replace("{QueryDescription}", queryDescription);
                    sbQueries.Replace("{Query}", query);
                }
            }

            tbQueries.Text = sbQueries.ToString();
        }
    }
}
