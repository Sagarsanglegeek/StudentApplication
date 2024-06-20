using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace StudentApplication.Plugin
{
    public class Student_UpdateChildRecord : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            
            IPluginExecutionContext executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                IOrganizationServiceFactory organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = (IOrganizationService)organizationServiceFactory.CreateOrganizationService(executionContext.UserId);

                

                    Guid courseId = ((EntityReference)executionContext.InputParameters["Course"]).Id;
                Guid studentRecordID = ((EntityReference)executionContext.InputParameters["CurrentRecordGUID"]).Id;

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
                       
                        updateChild["ss_course"] = new EntityReference("ss_course", courseId );
                        service.Update(updateChild);
                    }

                
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
            

        }
    }
}
