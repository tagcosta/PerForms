﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Default</title>
    <%-- You can download more themes or build your own at http://jqueryui.com/themeroller/ --%>
    <link href="PerForms/css/themes/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />
    <!-- <link rel="stylesheet" type="text/css" href="PerForms/css/themes/ui-darkness/jquery-ui-1.8.6.custom.css" /> -->
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal ID="litForm" runat="server" EnableViewState="false"></asp:Literal>
    </form>
</body>
</html>
