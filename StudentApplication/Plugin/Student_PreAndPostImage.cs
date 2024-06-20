using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApplication
{
    public class Student_PreAndPostImage : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                string firstName = string.Empty, lastName = string.Empty, email = string.Empty;
                int gender = 0, title = 0;
                Guid courseID = Guid.Empty;
                decimal coursefees = 0;
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                    IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                    if (context.MessageName.ToLower() == "update" && context.Stage == 10)
                    {
                        Entity entity = (Entity)context.InputParameters["Target"];
                        Entity preImage = (Entity)context.PreEntityImages["PreImage"];
                        Entity postImage = (Entity)context.PostEntityImages["PostImage"];


                        //Get
                        if (preImage.Attributes.Contains("ss_title") && preImage.Attributes["ss_title"] != null)
                        {
                            title = ((OptionSetValue)(preImage.Attributes["ss_title"])).Value;
                            trace.Trace("title:" + title);
                        }
                        if (preImage.Attributes.Contains("ss_name") && preImage.Attributes["ss_name"] != null)
                        {
                            firstName = preImage.Attributes["ss_name"].ToString();
                            trace.Trace("firstName:" + firstName);
                        }
                        if (preImage.Attributes.Contains("ss_lastname") && preImage.Attributes["ss_lastname"] != null)
                        {
                            lastName = preImage.Attributes["ss_lastname"].ToString();
                            trace.Trace("lastName:" + lastName);
                        }
                        if (preImage.Attributes.Contains("ss_gender") && preImage.Attributes["ss_gender"] != null)
                        {
                            gender = ((OptionSetValue)preImage.Attributes["ss_gender"]).Value;
                            trace.Trace("gender:" + gender);
                        }
                        if (preImage.Attributes.Contains("ss_email") && preImage.Attributes["ss_email"] != null)
                        {
                            email = preImage.Attributes["ss_email"].ToString(); ;
                            trace.Trace("email:" + email);
                        }
                        if (preImage.Attributes.Contains("ss_course") && preImage.Attributes["ss_course"] != null)
                        {
                            courseID = ((EntityReference)(preImage.Attributes["ss_course"])).Id;
                            trace.Trace("courseID:" + courseID);
                        }
                        if (preImage.Attributes.Contains("ss_coursefees") && preImage.Attributes["ss_coursefees"] != null)
                        {
                            coursefees = (decimal)(preImage.Attributes["ss_coursefees"]);
                            trace.Trace("coursefees:" + coursefees);
                        }



                        Entity studentEntity = new Entity("ss_student");
                        studentEntity.Attributes["ss_title"] = new OptionSetValue(title);
                        studentEntity.Attributes["ss_name"] = firstName;
                        studentEntity.Attributes["ss_lastname"] = lastName;
                        studentEntity.Attributes["ss_gender"] = new OptionSetValue(gender);
                        studentEntity.Attributes["ss_email"] = email;
                        studentEntity.Attributes["ss_course"] = new EntityReference("ss_course", courseID);
                        studentEntity.Attributes["ss_coursefees"] = new Money(coursefees);
                        service.Create(studentEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }


    }

}

