using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApplication
{
    public class Student_CreateAutoNumberPre : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            
            IPluginExecutionContext executionContext=(IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracingService=(ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                IOrganizationServiceFactory organizationServiceFactory=(IOrganizationServiceFactory)serviceProvider.GetService (typeof(IOrganizationServiceFactory));
                IOrganizationService service = (IOrganizationService)organizationServiceFactory.CreateOrganizationService(executionContext.UserId);

                Entity studentEntity = (Entity)executionContext.InputParameters["Target"];
                if(executionContext.MessageName.ToLower()!="create")
                {
                   return;
                }

                string autoNumber = string.Empty, prefix = string.Empty, suffix = string.Empty, seprator = string.Empty, currentnumber = string.Empty,
                year = string.Empty, month = string.Empty, day = string.Empty;

                DateTime today= DateTime.Now;
                year = today.Year.ToString();
                month = today.Month.ToString("00");  //06
                day = today.Day.ToString("00");

                QueryExpression queryExpression = new QueryExpression()
                {
                    EntityName = "ss_studentnumberconfig",
                    ColumnSet = new ColumnSet("ss_name", "ss_prefix", "ss_separator", "ss_suffix", "ss_currentnumber")
                };

                queryExpression.Criteria.AddCondition("ss_name", ConditionOperator.Equal, "studentautonumber");

               EntityCollection entityCollection= service.RetrieveMultiple(queryExpression);

               if(entityCollection.Entities.Count>0 )
                {
                    tracingService.Trace("Student config record count" + entityCollection.Entities.Count);

                    Entity studentAutonumber= entityCollection.Entities[0];
                    prefix = studentAutonumber["ss_prefix"].ToString();
                    suffix = studentAutonumber["ss_suffix"].ToString() ;
                    seprator = studentAutonumber["ss_separator"].ToString();
                    currentnumber = studentAutonumber["ss_currentnumber"].ToString();
                    tracingService.Trace("Current record number on Config Entity:" + currentnumber);
                    int tempcurrentNumber = int.Parse(currentnumber);
                    tempcurrentNumber++;  //0000000 //000001
                    currentnumber=tempcurrentNumber.ToString("000000");

                    autoNumber = prefix + seprator + year + month + day + seprator + suffix + seprator + currentnumber;
                    tracingService.Trace("AutoNumber:" + autoNumber);

                   


                    QueryExpression qeStudent = new QueryExpression()
                    {
                        EntityName = "ss_student",
                        ColumnSet = new ColumnSet("ss_studentnumber")

                    };

                    qeStudent.Criteria.AddCondition("ss_studentnumber", ConditionOperator.Equal, autoNumber);
                    EntityCollection studentRecord=service.RetrieveMultiple(qeStudent);
                    if(studentRecord.Entities.Count>0 )
                    {
                        throw new Exception("Duplicate Student found with Student ID:" + autoNumber);
                    }

                    // Student number
                    //Entity entity1 = new Entity("ss_student");
                    //entity1.Id = studentEntity.Id;
                    //entity1["ss_studentnumber"] = autoNumber.ToString();
                    //service.Update(entity1);

                    //The below code will update student config number
                    studentEntity.Attributes["ss_studentnumber"] = autoNumber.ToString();
                    Entity entity = new Entity("ss_studentnumberconfig");
                    entity.Id = studentAutonumber.Id;
                    entity["ss_currentnumber"] = currentnumber;
                    service.Update(entity);

                    //Entity entity = new Entity("ss_student");
                    //entity.Id = executionContext.PrimaryEntityId;
                    //entity["ss_studentnumber"] = autoNumber;
                    //service.Update(entity);






                }

            }
            catch(Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }


            
        }
    }
}
