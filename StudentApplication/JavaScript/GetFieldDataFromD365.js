function GetFieldDataFromD365(executionContext) {

    debugger;

    var formContext = executionContext.getFormContext();

    //string

    var title = formContext.getAttribute("ss_title").getValue()
    var firstName = formContext.getAttribute("ss_name").getValue()

    //OptionSet

    var gender = formContext.getAttribute("ss_gender").getValue()

    //Lookup
    var course = formContext.getAttribute("ss_course").getValue();
    if (course != null)
        var couseGuid = course[0].id;
    var courseName = course[0].name;

    //Two option 
    var flag = formContext.getAttribute("ss_flag").getValue();

    //DateTime
    var dob = formContext.getAttribute("ss_dateofbirth").getValue();

    //Long Text
    var description = formContext.getAttribute("ss_description").getValue();

    var lookopValue = new Array();
    lookopValue[0] = new Object();
    lookopValue[0].id = couseGuid;
    lookopValue[0].name = courseName;
    lookopValue[0].entityType = "ss_course"

    formContext.getAttribute("ss_course").setValue(lookopValue);
    CreateRecord(formContext, couseGuid);
}



function CreateRecord(formContext, couseGuid) {
    // define the data to create new account
    var data =
    {
        "ss_title": title,
        "ss_name": firstName,
        "ss_gender": gender,
        "ss_description": "This is the description of the sample account",
        "ss_lastname": "Test 1",
        "ss_coursefees": 5000000,
        "ss_dateofbirth": new Date(dob),
        "ss_email": firstName + "test 1" + "@gmail.com",
        "ss_Course@odata.bind": "/ss_courses(" + couseGuid + ")"

        

    }

    // create account record
    Xrm.WebApi.createRecord("ss_student", data).then(
        function success(result) {
            console.log("Account created with ID: " + result.id);
            // perform operations on record creation
        },
        function (error) {
            console.log(error.message);
            // handle error conditions
        }
    );


}
   


