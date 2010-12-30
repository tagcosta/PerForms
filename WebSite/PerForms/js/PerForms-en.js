function performs_Translate(key) {
    key = performs_SplitTokenString(key);
    switch (key[0]) {
        case "[RequiredField]": return "This field is required.";
        case "[ERROR]": return "Error";
        case "[INFO]": return "Information";
        case "[PleaseWait]": return "Please wait...";
        case "[ExceptionErrorMessage]": return "A situation has occured.<br />Our team has been notified and will check as soon as possible.<br />If you wish to contact our team, please provide the following key: <strong>" + key[1] + "</strong>";
        case "[GenericErrorMessage]": return "A situation has occured..<br />Our team has been notified and will check as soon as possible.<br />";
        case "[SessionExpired]": return "Your session has expired. Please, login again.";
        case "[ChooseFile]": return "Choose file...";
        case "[DropFileHere]": return "- drop file here -";
        case "[Cancel]": return "Cancel";
        case "[Date_Format_Long]": return "MM dd, yy";
        case "[Date_Format_Short]": return "mm-dd-yy";
        case "[January]": return "January";
        case "[February]": return "February";
        case "[March]": return "March";
        case "[April]": return "April";
        case "[May]": return "May";
        case "[June]": return "June";
        case "[July]": return "July";
        case "[August]": return "August";
        case "[September]": return "September";
        case "[October]": return "October";
        case "[November]": return "November";
        case "[December]": return "December";
        case "[Sunday_Long]": return "Sunday";
        case "[Monday_Long]": return "Monday";
        case "[Tuesday_Long]": return "Tuesday";
        case "[Wednesday_Long]": return "Wednesday";
        case "[Thursday_Long]": return "Thursday";
        case "[Friday_Long]": return "Friday";
        case "[Saturday_Long]": return "Saturday";
        case "[Sunday_Short]": return "S";
        case "[Monday_Short]": return "M";
        case "[Tuesday_Short]": return "T";
        case "[Wednesday_Short]": return "W";
        case "[Thursday_Short]": return "T";
        case "[Friday_Short]": return "F";
        case "[Saturday_Short]": return "S";
        case "[ExportToExcel]": return "Export to Excel";
        default: return key[0];
    }
}