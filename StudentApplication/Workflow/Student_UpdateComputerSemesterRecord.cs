using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace StudentApplication.Workflow
{
    public class Student_UpdateComputerSemesterRecord : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory=context.GetExtension<IOrganizationServiceFactory>();

            //Service Object
            IOrganizationService service = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);

            try
            {

                [RequiredArgument]
                [Input("CouseID")]
                
                PublicInArgument<EntityReference> couseID  { get; set; }


                Guid courseId = ((EntityReference)workflowContext.InputParameters["Course"]).Id;
                Guid studentRecordID = ((EntityReference)workflowContext.InputParameters["CurrentRecordGUID"]).Id;

                QueryExpression queryExpression = new QueryExpression()
                {
                    EntityName = "ss_semester",
                    ColumnSet = new ColumnSet("ss_student")
                };

                queryExpression.Criteria.AddCondition("ss_student", ConditionOperator.Equal, studentRecordID);


                EntityCollection childRecords = service.RetrieveMultiple(queryExpression);
                foreach (Entity childRecord in childRecords.Entities)
                {
                    Entity updateChild = new Entity("ss_semester", childRecord.Id);

                    updateChild["ss_course"] = new EntityReference("ss_course", courseId);
                    service.Update(updateChild);
                }

            }
            catch (Exception ex)
            {


            }


        }
    }
}
