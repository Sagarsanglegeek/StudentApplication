
function CallingCustomAction(primaryControl) {
	debugger;
//	var courseGuid;
//	var formContext = primaryControl;
//	//var serverURL = Xrm.Utility.getGlobalContext().getClientUrl();

//	var actionName ="ss_UpdateCourseDetails"

//	var course = formContext.getAttribute("ss_course");
//	if (course != null) {
//		courseGuid = (course.getValue()[0].id).replace(/{|}/g,"");


//		// Parameters
//		var data = {
//			Course: courseGuid
//		}

//		//Create the HttpRequestObject to send WEB API Request
//		var req = new XMLHttpRequest();
//		req.open("POST", Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/ss_UpdateCourseDetails", true);
//		req.setRequestHeader("OData-MaxVersion", "4.0");
//		req.setRequestHeader("OData-Version", "4.0");
//		req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
//		req.setRequestHeader("Accept", "application/json");
//		req.onreadystatechange = function () {
//			if (this.readyState === 4) {
//				req.onreadystatechange = null;
//				if (this.status === 200 || this.status === 204) {
//					alert("Action called Sucessfully.....")
//					var result = JSON.parse(this.response);
//					console.log(result);
//					// Return Type: mscrm.ss_UpdateCourseDetailsResponse
//					// Output Parameters
//					var outputResult = result["Output"]; // Edm.String
//					alert(outputResult.Output);
//				} else {
//					console.log(this.responseText);
//				}
//			}
//		};
//		req.send(JSON.stringify(data));
//	}

	// Parameters
	var parameters = {};
	parameters.CurrentRecordGUID = { "@odata.type": "Microsoft.Dynamics.CRM.ss_student", ss_studentid: "69541f6f-7828-ef11-840a-000d3a0a828a" }; // mscrm.ss_student
	parameters.Course = { "@odata.type": "Microsoft.Dynamics.CRM.ss_course", ss_courseid: "d6484e63-2f24-ef11-840a-000d3a0a77fc" }; // mscrm.ss_course

	var req = new XMLHttpRequest();
	req.open("POST", Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/ss_UpdateCourseDetails", true);
	req.setRequestHeader("OData-MaxVersion", "4.0");
	req.setRequestHeader("OData-Version", "4.0");
	req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
	req.setRequestHeader("Accept", "application/json");
	req.onreadystatechange = function () {
		if (this.readyState === 4) {
			req.onreadystatechange = null;
			if (this.status === 200 || this.status === 204) {
				var result = JSON.parse(this.response);
				console.log(result);
				// Return Type: mscrm.ss_UpdateCourseDetailsResponse
				// Output Parameters
				var output = result["Output"]; // Edm.String
			} else {
				console.log(this.responseText);
			}
		}
	};
	req.send(JSON.stringify(parameters));
}