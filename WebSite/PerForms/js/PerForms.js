/*!
* PerForms Framework v0.8.0
* https://github.com/tagcosta/PerForms
*
* Copyright 2010, Tiago Costa
* Licensed under the GPL Version 2 license.
* http://www.gnu.org/licenses/gpl-2.0.html
* 
* The following projects made PerForms possible:
* jQuery: http://jquery.com/
* jQueryUI: http://jqueryui.com/
* jQuery blockUI: http://malsup.com/jquery/block/
* jqGrid: http://www.trirand.com/blog/
* jQuery tmpl: https://github.com/jquery/jquery-tmpl
* JSON: http://www.json.org/js.html
* Valums AJAX Upload: http://valums.com/ajax-upload/
* jQuery UI Multiselect: http://quasipartikel.at/multiselect_next/
* NewtonSoftJSON: http://james.newtonking.com/pages/json-net.aspx
* NPOI: http://npoi.codeplex.com/
* NHibernate: http://nhforge.org/
*/

/* AJAX */
function performs_AJAX(method_name) {
    this.WebServiceURL = "PerForms/PerFormsWS.asmx";
    this.MethodName = method_name;
    this.Args = [];
    this.add = function (key, value) {
        this.Args[this.Args.length] = new performs_KeyValue(key, JSON.stringify(value));
    };
    this.addKeyValues = function (keyValues) {
        var keys = [], values = [], i;
        for (i = 0; i < keyValues.length; i = i + 1) {
            keys[keys.length] = keyValues[i].Key;
            values[values.length] = keyValues[i].Value;
        }
        this.add("keys", keys);
        this.add("values", values);
    };
    this.request = function (success_funtion) {
        var s, i;
        s = "{";
        for (i = 0; i < this.Args.length; i = i + 1) {
            s += this.Args[i].Key + ":" + this.Args[i].Value;
            if (i < this.Args.length - 1) {
                s += ",";
            }
        }
        s += "}";
        try {
            $.ajax({
                type: "POST",
                url: this.WebServiceURL + "/" + this.MethodName,
                data: s,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d === undefined) {
                        success_funtion(msg);
                    }
                    else {
                        success_funtion(msg.d);
                    }
                },
                error: function (msg) {
                    if (msg.responseText != null) {
                        performs_showMessageDialog("ERROR", performs_Translate("[ERROR]"), "", msg.responseText);
                    }
                    else {
                        performs_showMessageDialog("ERROR", performs_Translate("[ERROR]"), "", msg.get_responseText);
                    }
                }
            });
        }
        catch (exp) {
            performs_showMessageDialog("ERROR", performs_Translate("[ERROR]"), "", exp);
        }
    };
}

/* BlockUI */
function performs_blockUI(title, message) {
    $.blockUI({
        theme: true,
        title: title,
        message: message
    });
}

function performs_unblockUI() {
    $.unblockUI();
}

function performs_block(form, title, message) {
    $(form).block({
        theme: true,
        title: title,
        message: message
    });
}

function performs_unblock(form) {
    $(form).unblock();
}

/* KeyValue */
var performs_KeyValue = function (key, value) {
    this.Key = key;
    this.Value = value;
};

/* TokenString */
function performs_SplitTokenString(tokenString) {
    var charArray = tokenString.split("");
    if (charArray[0] != "[" || charArray[charArray.length - 1] != "]") return [tokenString];

    var tokenArray = tokenString.split("][");
    if (tokenArray.length > 1) {
        return $.map(tokenArray, function (element, index) {
            if (index == 0) return element + "]";
            else return element.slice(0, element.length - 1);
        });
    }
    else return tokenArray;
}

/* Message Dialog */
function performs_showMessageDialog(type, title, subtitle, message) {
    performs_showMessageDialog(type, title, subtitle, message, null);
}

function performs_showMessageDialog(type, title, subtitle, message, callback) {
    if (title != null && title != "") title = performs_Translate(title);
    if (subtitle != null && subtitle != "") subtitle = performs_Translate(subtitle);
    if (message != null && message != "") message = performs_Translate(message);

    var msgClass = "ui-state-highlight";
    if (type == "ERROR" || type == "0") {
        msgClass = "ui-state-error";
    }
    var html = "";
    html += "<div class='ui-widget ui-helper-hidden'>";
    html += "   <div class='" + msgClass + " ui-corner-all' style='padding: 0 .7em;'>";
    html += "       <p>";
    html += "           <span class='ui-icon ui-icon-alert' style='float: left; margin-right: .3em;'></span>";
    if (subtitle != null && subtitle != "") {
        html += "       <span class='message'><strong>" + subtitle + ": </strong>" + message + "</span>";
    }
    else {
        html += "       <span class='message'>" + message + "</span>";
    }
    html += "       </p>";
    html += "   </div>";
    html += "</div>";
    $(html).dialog({
        modal: true,
        title: title,
        draggable: false,
        resizable: false,
        buttons: { "Ok": function () { $(this).dialog("close"); } },
        close: function () { if (callback != null) callback(); }
    });
}

/* Validation */
var performs_Validation = function (regex, message) {
    this.RegEx = regex;
    this.Message = message;

    this.isValid = function (value) {
        return value == "" || new RegExp(this.RegEx).test(value);
    };
};

/* GetAJAXActions */
var performs_GetAJAXActions = function (form, actionKey, fieldGroupKey) {
    if (form.validateGroup(fieldGroupKey)) {
        var keyValues = form.getGroupKeyValues(fieldGroupKey);
        var ajax = new performs_AJAX("GetAJAXActions");
        ajax.addKeyValues(keyValues);
        ajax.add("actionKey", actionKey);
        performs_block("#" + form.FormKey, performs_Translate("[INFO]"), performs_Translate("[PleaseWait]"));
        ajax.request(function (x) {
            performs_unblock("#" + form.FormKey);

            $(x.Actions).each(function (index, element) {
                switch (element.AJAXActionType) {
                    //ShowMessage   
                    case 0:
                        performs_showMessageDialog(element.MessageType, element.MessageTitle, element.MessageSubtitle, element.MessageContent);
                        break;
                    //ShowForm   
                    case 1:
                        if (form.NextForm != null) {
                            if (form.NextForm.FormKey != form.FormKey) {
                                form.NextForm.remove();
                            }
                            form.NextForm = null;
                        }
                        var parent = $('#' + form.FormKey).parent();
                        parent.append(element.Form);
                        var nextForm = parent.find('div.performs-form-div:last').data('Perform');
                        if (nextForm != null) {
                            form.NextForm = nextForm;
                            nextForm.PreviousForm = form;
                        }
                        switch (element.PreviousFormActionType) {
                            //Append     
                            case 0:
                                break;
                            //Merge     
                            case 1:
                                $('#' + form.FormKey).data('Perform').hideFooter();
                                break;
                            //Hide     
                            case 2:
                                $('#' + form.FormKey).hide();
                                break;
                            //Destroy     
                            case 3:
                                $('#' + form.FormKey).remove();
                                if (nextForm != null) nextForm.PreviousForm = null;
                                break;
                        }
                        break;
                    //UpdateItems               
                    case 2:
                        var targetField = form.getFieldByKey(element.FieldKey);
                        targetField.updateItems(element.KeyValues);
                        break;
                    //RedirectToUrl      
                    case 3:
                        window.location = x.RedirectToURL;
                        break;
                    //SetValues     
                    case 4:
                        var targetField = form.getFieldByKey(element.FieldKey);
                        targetField.setValues(element.Values);
                        break;
                }
            });
        });
    }
};

/* Forms */
var performs_Form = function (formKey, o) {
    this.FormKey = formKey;
    this.Header = o.header || "";
    this.Footer = o.footer || "";
    this.MinWidth = o.minWidth || "100%";
    this.LabelWidth = o.labelWidth || "0";
    this.Fields = [];
    this.FormReady = "";

    this.Groups = [];
    this.Groups[0] = new performs_KeyValue("ALL", []);

    this.TabAreas = [];
    this.tabCounter = 0;

    this.NextForm = null;
    this.PreviousForm = null;

    this.onFormReady = function () {
        var formReady = new Function("form", this.FormReady);
        formReady(this);
        $("#performs-form-" + this.FormKey + " div.performs-tabarea-div").tabs();
        $("#performs-form-" + this.FormKey + " input:visible:first").focus();
    };
    this.removeMergeWithNextOnLastField = function () {
        this.Fields[this.Fields.length - 1].MergeWithNext = false;
    };
    this.removeInvalidTabAreasAndTabs = function () {
        for (var i = 0; i < this.TabAreas.length; i++) {
            if (this.TabAreas[i].open) {
                this.TabAreas.splice(i, 1);
                i--;
            }
            else {
                for (var j = 0; j < this.TabAreas[i].tabs.length; j++) {
                    if (this.TabAreas[i].tabs[j].open || (this.TabAreas[i].tabs[j].startIndex > this.TabAreas[i].tabs[j].endIndex)) {
                        this.TabAreas[i].tabs.splice(j, 1);
                        j--;
                    }
                }
                if (this.TabAreas[i].tabs.length == 0) {
                    this.TabAreas.splice(i, 1);
                    i--;
                }
            }
        }
    };
    this.calcMerges = function () {
        var merging = false;
        $(this.Fields).each(function (index, value) {
            if (value.MergeWithNext) {
                if (merging) {
                    value.ContinueMerge = true;
                }
                else {
                    value.BeginMerge = true;
                }
                merging = true;
            }
            else {
                if (merging) {
                    value.EndMerge = true;
                }
                else {
                    value.NoMerge = true;
                }
                merging = false;
            }
        });
    };
    this.calcTabAreasAndTabs = function () {
        var thisForm = this;
        $(this.TabAreas).each(function (tabAreaIndex, tabAreaValue) {
            thisForm.Fields[tabAreaValue.startIndex].BeginTabAreas[thisForm.Fields[tabAreaValue.startIndex].BeginTabAreas.length] = tabAreaValue;
            thisForm.Fields[tabAreaValue.endIndex].EndTabAreas[thisForm.Fields[tabAreaValue.endIndex].EndTabAreas.length] = tabAreaValue;

            $(tabAreaValue.tabs).each(function (tabIndex, tabValue) {
                thisForm.Fields[tabValue.startIndex].BeginTabs[thisForm.Fields[tabValue.startIndex].BeginTabs.length] = tabValue;
                thisForm.Fields[tabValue.endIndex].EndTabs[thisForm.Fields[tabValue.endIndex].EndTabs.length] = tabValue;
            });
        });
    };
    this.renderForm = function () {
        var thisForm = this;

        this.removeMergeWithNextOnLastField();
        this.removeInvalidTabAreasAndTabs();
        this.calcMerges();
        this.calcTabAreasAndTabs();

        var html = "";
        html += "<table cellspacing='0' id='performs-form-" + this.FormKey + "' class='ui-widget performs-form' width='" + this.MinWidth + "'>";
        if (this.Header.length > 0) html += this.renderHeader();
        html += "<tr class='ui-widget-content'><td class='performs-form-content'><table cellspacing='0' width='100%'>";

        $(this.Fields).each(function (index, value) {
            value.LabelWidth = thisForm.LabelWidth;

            //Tabs
            if (value.BeginTabs.length > value.BeginTabAreas.length) {
                html += "<div id='" + thisForm.FormKey + "-tab-" + value.BeginTabs[0].number + "'><table cellspacing='0'>";
            }
            if (value.BeginTabAreas.length > 0) {
                $(value.BeginTabAreas).each(function (tabAreaIndex, tabAreaValue) {
                    html += "<tr><td colspan='2'><div class='performs-tabarea-div'><ul>";

                    $(tabAreaValue.tabs).each(function (tabIndex, tabValue) {
                        html += "<li><a href='#" + thisForm.FormKey + "-tab-" + tabValue.number + "'>" + tabValue.label + "</a></li>";
                    });

                    html += "</ul><div id='" + thisForm.FormKey + "-tab-" + tabAreaValue.tabs[0].number + "'><table cellspacing='0'>";
                });
            }

            //Merging
            if (value.BeginMerge) {
                var placeHolder = $("<div />");
                $("#performs_template_label").tmpl(value).appendTo(placeHolder);
                html += "<tr>" + placeHolder.html();
                value.FieldLabel = "";
                value.LabelWidth = "0";
                html += "<td><table cellspacing='0' cellpadding='0'><tr><td><table cellspacing='0' cellpadding='0'>" + value.renderField() + "</table></td>";
            }

            if (value.ContinueMerge) {
                value.LabelWidth = "0";
                html += "<td><table cellspacing='0' cellpadding='0'>" + value.renderField() + "</table></td>";
            }

            if (value.EndMerge) {
                value.LabelWidth = "0";
                html += "<td><table cellspacing='0'>" + value.renderField() + "</table></td></tr></table></td></tr>";
            }

            if (value.NoMerge) {
                html += value.renderField();
            }

            //Tabs
            if (value.EndTabAreas.length > 0) {
                $(value.EndTabAreas).each(function (tabAreaIndex, tabAreaValue) {
                    html += "</table></div></div></td></tr>";
                });
            }
            if (value.EndTabs.length > value.EndTabAreas.length) {
                html += "</table></div>";
            }
        });
        html += "</table></td></tr>";
        if (this.Footer.length > 0) html += this.renderFooter();
        html += "</table>";
        return html;
    };
    this.renderHeader = function () {
        return "<tr class='ui-widget-header'><td class='performs-form-header'>" + this.Header + "</td></tr>";
    };
    this.renderFooter = function () {
        return "<tr class='ui-widget-header'><td class='performs-form-footer'>" + this.Footer + "</td></tr>";
    };
    this.hideFooter = function () {
        $("#" + this.FormKey + " td.performs-form-footer").parent().hide();
    };
    this.showFooter = function () {
        $("#" + this.FormKey + " td.performs-form-footer").parent().show();
    };
    this.getFieldByKey = function (fieldKey) {
        var field = null;
        $(this.Fields).each(function (index, value) {
            if (value.FieldKey == fieldKey) {
                field = value;
                return;
            }
        });
        return field;
    };
    this.getFieldsByGroup = function (groupKey) {
        var fields = [];
        var thisForm = this;
        $(this.Groups).each(function (i, group) {
            if (group.Key == groupKey) {
                $(group.Value).each(function (i, fieldKey) {
                    fields[fields.length] = thisForm.getFieldByKey(fieldKey);
                });
            }
            return;
        });
        return fields;
    };
    this.getGroupKeyValues = function (groupKey) {
        var keyValues = [];
        var fields = this.getFieldsByGroup(groupKey);
        $(fields).each(function (i, field) {
            //Ignore FieldButtons and Fields that are hidden (except FieldLabel)
            if (field.FieldType != "FieldButton" && (field.FieldType == "FieldLabel" || !field.Hidden)) {
                var values = field.getValues();
                $(values).each(function (i, value) {
                    keyValues[keyValues.length] = new performs_KeyValue(field.FieldKey, value);
                });
            }
        });
        return keyValues;
    };
    this.addGroup = function (groupKey, fieldKeys) {
        this.Groups[this.Groups.length] = new performs_KeyValue(groupKey, fieldKeys);
    };
    this.validateGroup = function (groupKey) {
        var valid = true;
        var fields = this.getFieldsByGroup(groupKey);
        $(fields).each(function (i, field) {
            if (valid) {
                valid = field.validate();
            }
            else {
                field.validate();
            }
        });
        return valid;
    };
    this.remove = function () {
        $("#" + this.FormKey).remove();
    };
    this.addTabArea = function () {
        var tabArea = {};
        tabArea.tabs = [];
        tabArea.open = true;
        tabArea.startIndex = this.Fields.length;

        this.TabAreas[this.TabAreas.length] = tabArea;
        return this;
    };
    this.endTabArea = function () {
        for (var i = this.TabAreas.length - 1; i >= 0; i--) {
            if (this.TabAreas[i].open) {
                this.TabAreas[i].open = false;
                this.TabAreas[i].endIndex = this.Fields.length - 1;
                break;
            }
        }
        return this;
    };
    this.addTab = function (tabLabel) {
        for (var i = this.TabAreas.length - 1; i >= 0; i--) {
            if (this.TabAreas[i].open) {
                var tab = {};
                tab.label = tabLabel;
                tab.open = true;
                tab.startIndex = this.Fields.length;
                tab.number = this.tabCounter++;
                this.TabAreas[i].tabs[this.TabAreas[i].tabs.length] = tab;
                break;
            }
        }
        return this;
    };
    this.endTab = function () {
        var breaker = false;
        for (var i = this.TabAreas.length - 1; i >= 0; i--) {
            if (this.TabAreas[i].open) {
                for (var j = this.TabAreas[i].tabs.length - 1; j >= 0; j--) {
                    if (this.TabAreas[i].tabs[j].open) {
                        this.TabAreas[i].tabs[j].open = false;
                        this.TabAreas[i].tabs[j].endIndex = this.Fields.length - 1;
                        breaker = true;
                        break;
                    }
                }
            }
            if (breaker) break;
        }
        return this;
    };
    this.addFieldText = function (fieldKey, fieldLabel, o) {
        var field = new performs_FieldText(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        return field;
    };
    this.addFieldTextArea = function (fieldKey, fieldLabel, o) {
        var field = new performs_FieldTextArea(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        return field;
    };
    this.addFieldRadios = function (fieldKey, fieldLabel, keyValues, o) {
        var field = new performs_FieldRadios(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, keyValues, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        this.FormReady += "form.getFieldByKey('" + fieldKey + "').applyRadios();";
        return field;
    };
    this.addFieldCheckBoxes = function (fieldKey, fieldLabel, keyValues, o) {
        var field = new performs_FieldCheckBoxes(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, keyValues, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        this.FormReady += "form.getFieldByKey('" + fieldKey + "').applyCheckBoxes();";
        return field;
    };
    this.addFieldComboBox = function (fieldKey, fieldLabel, keyValues, o) {
        var field = new performs_FieldComboBox(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, keyValues, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        this.FormReady += "form.getFieldByKey('" + fieldKey + "').applyComboBox();";
        return field;
    };
    this.addFieldButton = function (fieldKey, buttonLabel, fieldLabel, o) {
        var field = new performs_FieldButton(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, buttonLabel, fieldLabel, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        this.FormReady += "form.getFieldByKey('" + fieldKey + "').applyButton();";
        return field;
    };
    this.addFieldMultiSelectBox = function (fieldKey, fieldLabel, keyValues, o) {
        var field = new performs_FieldMultiSelectBox(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, keyValues, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        this.FormReady += "form.getFieldByKey('" + fieldKey + "').applyMultiSelectBox();";
        return field;
    };
    this.addFieldGridView = function (fieldKey, fieldLabel, columns, rows, actionKey, clientSide, tableLabel, multipleSelection, o) {
        var field = new performs_FieldGridView(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, columns, rows, actionKey, clientSide, tableLabel, multipleSelection, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        this.FormReady += "form.getFieldByKey('" + fieldKey + "').applyGridView();";
        return field;
    };
    this.addFieldLabel = function (fieldKey, fieldLabel, o) {
        var field = new performs_FieldLabel(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        return field;
    };
    this.addFieldDate = function (fieldKey, fieldLabel, o) {
        var field = new performs_FieldDate(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        this.FormReady += "form.getFieldByKey('" + fieldKey + "').applyDatePicker();";
        return field;
    };
    this.addFieldUpload = function (fieldKey, fieldLabel, root, o) {
        var field = new performs_FieldUpload(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, root, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        this.FormReady += "form.getFieldByKey('" + fieldKey + "').applyUpload();";
        return field;
    };
    this.addFieldFree = function (fieldKey, fieldLabel, source, getValues, setValues, o) {
        var field = new performs_FieldFree(this, "performs-form-" + this.FormKey + "-field-" + fieldKey, fieldKey, fieldLabel, source, getValues, setValues, o);
        this.Fields[this.Fields.length] = field;
        this.Groups[0].Value[this.Groups[0].Value.length] = fieldKey;
        this.addGroup(fieldKey, [fieldKey]);
        return field;
    };
};

/* Fields */
var performs_Field = function () {
    this.base = function (form, fieldID, fieldKey, fieldLabel, fieldType, fieldClass, fieldTemplate, o) {
        this.Form = form;
        this.FieldID = fieldID;
        this.FieldKey = fieldKey;
        this.FieldLabel = fieldLabel;
        this.FieldType = fieldType;
        this.FieldClass = fieldClass;
        this.FieldTemplate = fieldTemplate;
        this.Validations = [];
        this.EagerValidation = false;
        this.Hidden = false;

        this.Required = o == null || o.required == null ? false : o.required;
        this.MergeWithNext = o == null || o.mergeWithNext == null ? false : o.mergeWithNext;

        //These helper variables will be assigned prior to rendering the form.
        this.BeginMerge = false;
        this.EndMerge = false;
        this.ContinueMerge = false;
        this.NoMerge = false;
        this.BeginTabAreas = [];
        this.EndTabAreas = [];
        this.BeginTabs = [];
        this.EndTabs = [];
    };
    this.hide = function () {
        $("#" + this.FieldID).hide();
        this.Hidden = true;
    };
    this.show = function () {
        $("#" + this.FieldID).show();
        this.Hidden = false;
    };
    this.addValidation = function (regex, message) {
        var validation = new performs_Validation(regex, message);
        this.Validations[this.Validations.length] = validation;
    };
    this.validate = this.base_validate = function (fieldValues) {
        var errorMessages = [];
        if ($("#" + this.FieldID).is(":visible")) {
            $(this.Validations).each(function (validation_index, validation_value) {
                $(fieldValues).each(function (fieldValue_index, fieldValue_value) {
                    if (!validation_value.isValid(fieldValue_value)) errorMessages[errorMessages.length] = validation_value.Message;
                });
            });
            if (this.Required && fieldValues.length == 1 && (fieldValues[0] == null || fieldValues[0] == "")) {
                errorMessages[errorMessages.length] = performs_Translate("[RequiredField]");
            }
        }
        return errorMessages;
    };
    this.clearErrorMessages = function () {
        $("#" + this.FieldID + " .performs-field-message").html("");
        $("#" + this.FieldID + " .performs-field-content").removeClass("ui-state-error");
    };
    this.showErrorMessages = function (errorMessages) {
        this.clearErrorMessages();
        var that = this;
        $(errorMessages).each(function (index, value) {
            var message = $("#" + that.FieldID + " .performs-field-message");
            message.html(message.html() + "<div><span class='ui-icon ui-icon-alert' style='float: left;'></span><span>" + value + "</span></div>");
            if (index != $(errorMessages).length - 1) {
                message.html(message.html() + "<br />");
            }
            $("#" + that.FieldID + " .performs-field-content").addClass("ui-state-error");
        });
    };
};

var performs_FieldText = function (form, fieldID, fieldKey, fieldLabel, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldText", "performs-field-fieldtext", "performs_fieldtemplate_text", o);

    this.validate = function () {
        if (!this.EagerValidation) {
            this.EagerValidation = true;
            var that = this;
            $("#" + this.FieldID + " input:text").bind("keyup", function () {
                that.validate();
            });
        }
        var errorMessages = this.base_validate(this.getValues());
        this.showErrorMessages(errorMessages);
        return errorMessages.length == 0;
    };
    this.getValues = function () {
        return [$("#" + this.FieldID + " input:text")[0].value];
    };
    this.setValues = function (values) {
        $("#" + this.FieldID + " input:text")[0].value = values[0];
    };
    this.renderField = function () {
        var that = this;
        $("#" + this.FieldID + " input:text").live("hover", function () {
            $(this).toggleClass("ui-state-hover");
        })
        .live("focus", function () {
            $(this).addClass("ui-state-focus");
        })
        .live("blur", function () {
            $(this).removeClass("ui-state-focus");
            that.validate();
        });
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
};
performs_FieldText.prototype = new performs_Field();


var performs_FieldTextArea = function (form, fieldID, fieldKey, fieldLabel, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldTextArea", "performs-field-fieldtextarea", "performs_fieldtemplate_textarea", o);

    this.validate = function () {
        if (!this.EagerValidation) {
            this.EagerValidation = true;
            var that = this;
            $("#" + this.FieldID + " textarea").bind("keyup", function () {
                that.validate();
            });
        }
        var errorMessages = this.base_validate(this.getValues());
        this.showErrorMessages(errorMessages);
        return errorMessages.length == 0;
    };
    this.getValues = function () {
        return [$("#" + this.FieldID + " textarea")[0].value];
    };
    this.setValues = function (values) {
        $("#" + this.FieldID + " textarea")[0].value = values[0];
    };
    this.renderField = function () {
        var that = this;
        $("#" + this.FieldID + " textarea").live("hover", function () {
            $(this).toggleClass("ui-state-hover");
        })
        .live("focus", function () {
            $(this).addClass("ui-state-focus");
        })
        .live("blur", function () {
            $(this).removeClass("ui-state-focus");
            that.validate();
        });
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
};
performs_FieldTextArea.prototype = new performs_Field();

var performs_FieldRadios = function (form, fieldID, fieldKey, fieldLabel, keyValues, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldRadios", "performs-field performs-field-fieldradios", "performs_fieldtemplate_radios", o);
    this.KeyValues = keyValues;

    this.validate = function () {
        var errorMessages = this.base_validate(this.getValues());
        this.showErrorMessages(errorMessages);
        return errorMessages.length == 0;
    };
    this.getValues = function () {
        var selectedRadio = $("#" + this.FieldID + " input:radio:checked")[0];
        if (selectedRadio != null) {
            return [selectedRadio.value];
        }
        return [""];
    };
    this.setValues = function (values) {
        var radios = $("#" + this.FieldID + " input:radio");
        $(radios).each(function (index, element) {
            element.checked = false;
            $(element).change().button("option", "icons", { primary: 'ui-icon-radio-on', secondary: null });
            if (element.value == values[0]) {
                element.checked = true;
                $(element).change().button("option", "icons", { primary: 'ui-icon-check', secondary: null });
                return;
            }
        });
    };
    this.renderField = function () {
        var that = this;
        $("#" + this.FieldID + " input:radio").live('focus', function () {
            $(this).next('label').addClass("ui-state-focus");
        })
        .live('blur', function () {
            $(this).next('label').removeClass("ui-state-focus");
        })
        .live('click', function () {
            $(this).parents(".performs-field-fieldradios:first input:radio[name = '" + $(this)[0].name + "']").button("option", "icons", { primary: 'ui-icon-radio-on', secondary: null });
            $(this).button("option", "icons", { primary: 'ui-icon-check', secondary: null });
        });
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
    this.applyRadios = function () {
        $("#" + this.FieldID + " input:radio").button({ icons: { primary: 'ui-icon-radio-on', secondary: null} });
    };
    this.addConditionalVisibility = function (greenValues, targetGroupKey) {
        var thisField = this;
        $("#" + this.FieldID + " input:radio").click(function () {
            var green = false;
            var values = thisField.getValues();
            var fields = thisField.Form.getFieldsByGroup(targetGroupKey);
            $(values).each(function (i, value) {
                $(greenValues).each(function (i, greenValue) {
                    if (value == greenValue) {
                        green = true;
                        return;
                    }
                });
            });
            if (green) {
                $(fields).each(function (i, field) {
                    field.show();
                });
            }
            else {
                $(fields).each(function (i, field) {
                    field.hide();
                });
            }
        });
    };
};
performs_FieldRadios.prototype = new performs_Field();

var performs_FieldCheckBoxes = function (form, fieldID, fieldKey, fieldLabel, keyValues, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldCheckBoxes", "performs-field-fieldcheckboxes", "performs_fieldtemplate_checkboxes", o);
    this.KeyValues = keyValues;

    this.validate = function () {
        if (!this.EagerValidation) {
            this.EagerValidation = true;
            var that = this;
            $("#" + this.FieldID + " input:checkbox").bind("click", function () {
                that.validate();
            });
        }
        var errorMessages = this.base_validate(this.getValues());
        this.showErrorMessages(errorMessages);
        return errorMessages.length == 0;
    };
    this.getValues = function () {
        var selectedCheckBoxes = $("#" + this.FieldID + " input:checkbox:checked");
        if (selectedCheckBoxes.length > 0) {
            var values = [];
            $(selectedCheckBoxes).each(function (index, value) {
                values[values.length] = value.value;
            });
            return values;
        }
        else {
            return [""];
        }
    };
    this.setValues = function (values) {
        var checkBoxes = $("#" + this.FieldID + " input:checkbox");
        $(checkBoxes).each(function (index, element) {
            element.checked = false;
            $(element).change().button("option", "icons", { primary: 'ui-icon-radio-on', secondary: null });
            $(values).each(function (value_index, value_element) {
                if (element.value == value_element) {
                    element.checked = true;
                    $(element).change().button("option", "icons", { primary: 'ui-icon-check', secondary: null });
                    return;
                }
            });
        });
    };
    this.renderField = function () {
        var that = this;
        $("#" + this.FieldID + " input:checkbox").live('focus', function () {
            $(this).next('label').addClass("ui-state-focus");
        })
        .live('blur', function () {
            $(this).next('label').removeClass("ui-state-focus");
        })
        .live('click', function () {
            if ($(this).is(':checked')) {
                $(this).button("option", "icons", { primary: 'ui-icon-check', secondary: null });
            }
            else {
                $(this).button("option", "icons", { primary: 'ui-icon-radio-on', secondary: null });
            }
            that.validate();
        });
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
    this.applyCheckBoxes = function () {
        $("#" + this.FieldID + " input:checkbox").button({ icons: { primary: 'ui-icon-radio-on', secondary: null} });
    };
};
performs_FieldCheckBoxes.prototype = new performs_Field();

var performs_FieldComboBox = function (form, fieldID, fieldKey, fieldLabel, keyValues, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldComboBox", "performs-field-fieldcombobox", "performs_fieldtemplate_combobox", o);

    this.KeyValues = keyValues;
    this.OnChange = function () { };

    this.validate = function () {
        var errorMessages = this.base_validate(this.getValues());
        this.showErrorMessages(errorMessages);
        return errorMessages.length == 0;
    };
    this.getValues = function () {
        var values = [""];
        var value = $("#" + this.FieldID + " input:text")[0].value;
        var items = $("#" + this.FieldID + " input:text").autocomplete("option", "source");
        $(items).each(function (i, item) {
            if (item.label.toLowerCase() == value.toLowerCase()) {
                values = [item.id];
                return;
            }
        });
        return values;
    };
    this.setValues = function (values) {
        var thisField = this;
        var items = $("#" + this.FieldID + " input:text").autocomplete("option", "source");
        $(items).each(function (index, element) {
            if (element.id == values[0]) {
                $("#" + thisField.FieldID + " input:text:first").val(element.label);
                return;
            }
        });
    };
    this.updateItems = function (keyValues) {
        var items = [];
        $(keyValues).each(function (index, keyValue) {
            items[items.length] = { id: keyValue.Key, label: keyValue.Value };
        });
        $("#" + this.FieldID + " input:text").autocomplete("option", "source", items);
        $("#" + this.FieldID + " input:text").val("");
    };
    this.renderField = function () {
        $("#" + this.FieldID + " input:text").live("hover", function () {
            $(this).toggleClass("ui-state-hover");
        })
        .live("focus", function () {
            $(this).addClass("ui-state-focus");
        })
        .live("blur", function () {
            $(this).removeClass("ui-state-focus");
        });
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
    this.applyComboBox = function () {
        var thisField = this;
        var element = $("#" + this.FieldID + " input:text");
        var items = [];
        $(this.KeyValues).each(function (index, keyValue) {
            items[items.length] = { id: keyValue.Key, label: keyValue.Value };
        });

        element.autocomplete({
            source: items,
            delay: 0,
            minLength: 0
        }).unbind("blur.autocomplete")
        .blur(function () {
            var element = this;
            var valid = false;
            var currentItems = $(this).autocomplete("option", "source");
            $(currentItems).each(function (index, currentItem) {
                if (currentItem.label.toLowerCase() == element.value.toLowerCase()) {
                    valid = true;
                    return;
                }
            });
            if (valid) {
                if ($(this).prevAll('input:hidden')[0].value != this.value) {
                    $(this).prevAll('input:hidden')[0].value = this.value;
                    $(this).autocomplete("close");
                    thisField.clearErrorMessages();
                    thisField.OnChange();
                }
            }
            else {
                this.value = "";
            }
        });
        $("<span style='position: absolute;' class='ui-state-default'><span style='margin-top: 5px; margin-left: 3px;' class='ui-icon ui-icon-triangle-1-s'></span></span>")
	    .insertAfter(element)
        .width(25)
        .height(element.height() + 10)
        .hover(function () {
            $(this).toggleClass("ui-state-hover");
        })
	    .click(function () {
	        setTimeout(function () {
	            element.focus();
	            if (element.autocomplete("widget").is(":visible")) {
	                element.autocomplete("close");
	            }
	            else {
	                element.autocomplete("search", "");
	            }
	            return false;
	        }, 100);
	    });
        $("<input type='hidden' />").insertBefore(element);
    };
    this.addGetAJAXActions = function (actionKey, fieldGroupKey) {
        var thisField = this;
        this.OnChange = function () {
            performs_GetAJAXActions(thisField.Form, actionKey, fieldGroupKey);
        }
    };
};
performs_FieldComboBox.prototype = new performs_Field();

var performs_FieldButton = function (form, fieldID, fieldKey, buttonLabel, fieldLabel, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldButton", "performs-field-fieldbutton", "performs_fieldtemplate_button", o);

    this.ButtonLabel = buttonLabel;

    this.validate = function () {
        return true;
    };
    this.getValues = function () {
        return [""];
    };
    this.setValues = function (values) {

    };
    this.renderField = function () {
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
    this.applyButton = function () {
        $("#" + this.FieldID + " input:button").button();
    };
    this.addGetAJAXActions = function (actionKey, fieldGroupKey) {
        var thisField = this;
        $("#" + this.FieldID + " input:button").click(function () {
            performs_GetAJAXActions(thisField.Form, actionKey, fieldGroupKey);
        });
    };
    this.addCancel = function () {
        var thisField = this;
        $("#" + this.FieldID + " input:button").click(function () {
            thisField.Form.PreviousForm.NextForm = null;
            thisField.Form.PreviousForm.showFooter();
            thisField.Form.PreviousForm = null;
            var parent = $("#" + thisField.Form.FormKey).parent();
            $("#" + thisField.Form.FormKey).remove();
            parent.find("div.performs-form-div:last").show();
        });
    };
};
performs_FieldButton.prototype = new performs_Field();

var performs_FieldMultiSelectBox = function (form, fieldID, fieldKey, fieldLabel, keyValues, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldMultiSelectBox", "performs-field-fieldmultiselectbox", "performs_fieldtemplate_multiselectbox", o);

    this.KeyValues = keyValues;
    this.OnChange = function () { };

    this.validate = function () {
        var errorMessages = this.base_validate(this.getValues());
        this.showErrorMessages(errorMessages);
        return errorMessages.length == 0;
    };
    this.getValues = function () {
        var values = $("#" + this.FieldID + " select").multiselect('selectedValues');
        if (values.length == 0) {
            return [""];
        }
        else {
            return values;
        }
    };
    this.setValues = function (values) {
        var thisField = this;
        $(values).each(function (i, value) {
            $("#" + thisField.FieldID + " select").multiselect('select', $("#" + thisField.FieldID + " select").find("option[value='" + value + "']").text());
        });
    };
    this.updateItems = function (keyValues) {
        var options = "";
        $(keyValues).each(function (index, keyValue) {
            options += "<option value='" + keyValue.Key + "'>" + keyValue.Value + "</option>";
        });
        $("#" + this.FieldID + " select").multiselect('destroy');
        $("#" + this.FieldID + " select").html(options);
        this.applyMultiSelectBox();
    };
    this.renderField = function () {
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
    this.applyMultiSelectBox = function () {
        var thisField = this;
        $("#" + this.FieldID + " select").multiselect({
            animated: 'fast',
            droppable: 'none',
            sortable: 'none',
            nodeComparator: null,
            searchDelay: 0,
            dividerLocation: 0.5,
            onChange: thisField.OnChange
        });
    };
    this.addGetAJAXActions = function (actionKey, fieldGroupKey) {
        var thisField = this;
        this.OnChange = function () {
            performs_GetAJAXActions(thisField.Form, actionKey, fieldGroupKey);
        }
    };
};
performs_FieldMultiSelectBox.prototype = new performs_Field();

var performs_FieldGridView = function (form, fieldID, fieldKey, fieldLabel, columns, rows, actionKey, clientSide, tableLabel, multipleSelection, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldGridView", "performs-field-fieldgridview", "performs_fieldtemplate_gridview", o);

    this.Columns = columns;
    this.Rows = rows;
    this.ActionKey = actionKey;
    this.ClientSide = clientSide;
    this.TableLabel = tableLabel;
    this.MultipleSelection = multipleSelection;

    this.validate = function () {
        var errorMessages = this.base_validate(this.getValues());
        this.showErrorMessages(errorMessages);
        return errorMessages.length == 0;
    };
    this.getValues = function () {
        if (this.MultipleSelection) {
            var value = $("#" + this.FieldID + "-gridview").getGridParam("selarrrow");
            if (value.length == 0) value = [""];
            return value;
        }
        else {
            return [$("#" + this.FieldID + "-gridview").getGridParam("selrow")];
        }
    };
    this.setValues = function (values) {

    };
    this.renderField = function () {
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
    this.applyGridView = function () {
        var thisField = this;
        var columnNames = [];
        $(this.Columns).each(function (i, column) {
            columnNames[columnNames.length] = column.column;
        });
        if (this.ClientSide) {
            $("#" + this.FieldID + "-gridview").jqGrid({
                datatype: "local",
                data: this.Rows,
                colNames: columnNames,
                colModel: this.Columns,
                rowNum: 10,
                rowList: [10, 20, 30],
                caption: this.TableLabel,
                pager: "#" + this.FieldID + "-pager",
                sortname: columnNames[0],
                sortorder: 'asc',
                height: "100%",
                autowidth: false,
                viewrecords: true,
                gridview: true,
                hidegrid: false,
                hoverrows: false,
                ignoreCase: true,
                multiselect: thisField.MultipleSelection,
                onSelectRow: function (id) {
                    thisField.clearErrorMessages();
                }
            });
        }
        else {
            $("#" + this.FieldID + "-gridview").jqGrid({
                url: 'PerForms/GetDataTable.aspx?actionKey=' + this.ActionKey,
                datatype: 'json',
                colNames: columnNames,
                colModel: this.Columns,
                rowNum: 10,
                rowList: [10, 20, 30],
                caption: this.TableLabel,
                pager: "#" + this.FieldID + "-pager",
                sortname: columnNames[0],
                sortorder: 'asc',
                height: "100%",
                autowidth: false,
                viewrecords: true,
                gridview: true,
                hidegrid: false,
                hoverrows: false,
                multiselect: thisField.MultipleSelection,
                onSelectRow: function (id) {
                    thisField.clearErrorMessages();
                }
            });
        }
        $("#" + this.FieldID + "-gridview").jqGrid('navGrid', "#" + this.FieldID + "-pager",
            {
                edit: false,
                add: false,
                del: false,
                refresh: false
            },
            {}, // edit options
            {}, // add options
            {}, // del options
            {multipleSearch: true} // search options
        );

        //Export To Excel
        if (this.ActionKey != null && this.ActionKey != "") {
            $("#" + this.FieldID + "-gridview").jqGrid('navButtonAdd', "#" + this.FieldID + "-pager", {
                buttonicon: "ui-icon-document",
                title: performs_Translate("[ExportToExcel]"),
                caption: performs_Translate("[ExportToExcel]"),
                onClickButton: function () {
                    window.open("PerForms/FileDownload.aspx?actionKey=" + thisField.ActionKey);
                }
            });
        }

        //Nav toolbar doesn't work properly when column names have spaces
        //$("#" + this.FieldID + "-gridview").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

        //Resizing the grid breaks collapse functionality - update: removed the collapse button (does anyone ever use it?)
        $("#" + this.FieldID + "-gridview").jqGrid('gridResize', {});
    };
};
performs_FieldGridView.prototype = new performs_Field();

var performs_FieldLabel = function (form, fieldID, fieldKey, fieldLabel, o) {    
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldLabel", "performs-field-fieldlabel", "performs_fieldtemplate_label", o);

    this.validate = function () {
        return true;
    };
    this.getValues = function () {
        return [$("#" + this.FieldID + " span")[0].innerHTML];
    };
    this.setValues = function (values) {
        $("#" + this.FieldID + " span")[0].innerHTML = values[0];
    };
    this.renderField = function () {
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
};
performs_FieldLabel.prototype = new performs_Field();

var performs_FieldDate = function (form, fieldID, fieldKey, fieldLabel, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldDate", "performs-field-fielddate", "performs_fieldtemplate_date", o);

    this.validate = function () {
        var errorMessages = this.base_validate(this.getValues());
        this.showErrorMessages(errorMessages);
        return errorMessages.length == 0;
    };
    this.getValues = function () {
        return [$("#" + this.FieldID + " input:text")[0].value];
    };
    this.setValues = function (values) {
        $("#" + this.FieldID + " input:text")[0].value = values[0];
    };
    this.renderField = function () {
        $("#" + this.FieldID + " input:text").live("hover", function () {
            $(this).toggleClass("ui-state-hover");
        })
        .live("focus", function () {
            $(this).addClass("ui-state-focus");
        })
        .live("blur", function () {
            $(this).removeClass("ui-state-focus");
        });
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
    this.applyDatePicker = function () {
        var thisField = this;
        var element = $("#" + this.FieldID + " input.performs-field-fielddate-input");
        $("<span style='position: absolute;' class='ui-state-default'><span style='margin-top: 5px; margin-left: 3px;' class='ui-icon ui-icon-calendar'></span></span>")
	    .insertAfter(element)
        .width(25)
        .height(element.height() + 10)
        .hover(function () {
            $(this).toggleClass("ui-state-hover");
        })
	    .click(function () {
	        element.datepicker({
	            showAnim: '',
	            dayNamesMin: [performs_Translate("[Sunday_Short]"),
                            performs_Translate("[Monday_Short]"),
                            performs_Translate("[Tuesday_Short]"),
                            performs_Translate("[Wednesday_Short]"),
                            performs_Translate("[Thursday_Short]"),
                            performs_Translate("[Friday_Short]"),
                            performs_Translate("[Saturday_Short]")],
	            monthNames: [performs_Translate("[January]"),
                            performs_Translate("[February]"),
                            performs_Translate("[March]"),
                            performs_Translate("[April]"),
                            performs_Translate("[May]"),
                            performs_Translate("[June]"),
                            performs_Translate("[July]"),
                            performs_Translate("[August]"),
                            performs_Translate("[September]"),
                            performs_Translate("[October]"),
                            performs_Translate("[November]"),
                            performs_Translate("[December]")],
	            dateFormat: performs_Translate("[Date_Format_Long]")
	        });
	        element.datepicker('show');
	    });
    };
};
performs_FieldDate.prototype = new performs_Field();

var performs_FieldUpload = function (form, fieldID, fieldKey, fieldLabel, root, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldUpload", "performs-field-fieldupload", "performs_fieldtemplate_upload", o);
    
    this.Root = root;
    this.Files = [];

    this.validate = function () {
        return true;
    };
    this.getValues = function () {
        // Guid, FileName, Guid, FileName, etc.
        if (this.Files.length == 0) return [''];
        else {
            var output = [];
            $(this.Files).each(function (index, value) {
                output[output.length] = value.guid;
                output[output.length] = value.filename;
            });
            return output;
        }
    };
    this.setValues = function (values) {

    };
    this.renderField = function () {
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
    this.applyUpload = function () {
        var thisField = this;
        var element = $("#" + thisField.FieldID + " div.performs-field-fieldupload-upload")[0];
        var interval;
        var uploader = new qq.FileUploader({
            element: element,
            action: thisField.Root + 'PerForms/FileUploader.ashx',
            onComplete: function (id, filename, responseJSON) {
                if (responseJSON.success) {
                    thisField.Files[thisField.Files.length] = { guid: responseJSON.guid, filename: responseJSON.filename };
                }
            },
            template: '<div class="qq-uploader">' +
                        '' +
                        '<table cellspacing="0" cellpadding="0" border="0"><tr><td><div class="qq-upload-button ui-state-default ui-corner-all" style="padding: 4px 11px 4px 11px;">' + performs_Translate("[ChooseFile]") + '</div></td>' +
                        '<td><div class="qq-upload-drop-area"><span>' + performs_Translate("[DropFileHere]") + '</span></div></td><td><ul class="qq-upload-list"></ul></td></tr></table>' +
                     '</div>',
            fileTemplate: '<li>' +
                            '<span class="qq-upload-file"></span>' +
                            '<span class="qq-upload-spinner" style="display: none"></span>' +
                            '<span class="qq-upload-size"></span>' +
                            '<a class="qq-upload-cancel" href="#">' + performs_Translate("[Cancel]") + '</a>' +
                            '<span class="qq-upload-failed-text">Failed</span>' +
                        '</li>'
        });
        $("#" + thisField.FieldID + " div.qq-upload-button").mousedown(function () {
            $(this).addClass("ui-state-active");
        }).mouseup(function () {
            $(this).removeClass("ui-state-active");
        });
    };
};
performs_FieldUpload.prototype = new performs_Field();

var performs_FieldFree = function (form, fieldID, fieldKey, fieldLabel, source, getValues, setValues, o) {
    this.base(form, fieldID, fieldKey, fieldLabel, "FieldFree", "performs-field-fieldfree", "performs_fieldtemplate_free", o);
    
    this.Source = source;
    this.GetValues = getValues;
    this.SetValues = setValues;

    this.validate = function () {
        return true;
    };
    this.getValues = function () {
        this.GetValues();
    };
    this.setValues = function (values) {
        this.SetValues(values);
    };
    this.renderField = function () {
        var placeHolder = $("<div />");
        $("#performs_fieldtemplate").tmpl(this).appendTo(placeHolder);
        return placeHolder.html();
    };
};
performs_FieldFree.prototype = new performs_Field();