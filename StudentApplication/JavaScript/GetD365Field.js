function getAttribute(executionContext) {

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
    var courseName = course[0].Name;

    /
}
