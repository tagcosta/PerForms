#region License
// Copyright (c) 2010 Tiago Costa
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PerForms.Fields;
using PerForms.Util;
using System.Web.UI;

namespace PerForms
{
    /// <summary>
    /// <para>This class represents a PerForm.</para>
    /// <para>If you are using ASP.NET WebForms you should set 'EnableViewState' to false on the LiteralControl containing this form.</para>
    /// <para>It is the only entry point to creating a new form.</para>
    /// <para>Contains all properties of the form, its fields, interactions, etc.</para>
    /// <para>All forms should be identified by an unique key.</para>
    /// <para>This allows you to have multiple forms in the same page without they interfering with each other.</para>
    /// <para>You cannot have nested forms (forms inside other forms).</para>
    /// </summary>
    public class PrForm
    {
        private string FormHeader { get; set; }
        private string FormFooter { get; set; }
        private string MinWidth { get; set; }

        /// <summary>
        /// <para>This variable 'getter' is internal because most fields will need to read its value to render the interactions.</para>
        /// </summary>
        internal string FormKey { get; private set; }

        private Dictionary<string, PrField> Fields { get; set; }

        /// <summary>
        /// <para>This variable is internal because it can be used by each field to render interactions.</para>
        /// <para>Each field should only add to the existing string (OnFormReady += '';). Never replace it.</para>
        /// </summary>
        internal string OnFormReady { get; set; }
        internal string OnPreFormReady { get; set; }

        private Dictionary<string, List<string>> FieldGroups;
        private string LabelWidth { get; set; }

        private bool Dialog { get; set; }
        private bool Modal { get; set; }

        /// <summary>
        /// <para>Initializes the form with the default values.</para>
        /// <para>Minimum form width is 100%, label width is 0 (least ammount possible).</para>
        /// <para>Header and footer are empty and will not be rendered.</para>
        /// </summary>
        /// <param name="formKey">Unique identifier of the form.</param>
        public PrForm(string formKey)
        {
            Fields = new Dictionary<string, PrField>();
            FormKey = formKey;
            OnFormReady = string.Empty;
            FieldGroups = new Dictionary<string, List<string>>();
            MinWidth = "100%";
            LabelWidth = "250";
            Dialog = false;
            Modal = false;
        }

        /// <summary>
        /// <para>Specify the header of the form.</para>
        /// <para>If empty, the header won't be rendered.</para>
        /// <para>You can use html.</para>
        /// <para>If you want a header with no text, you can use the html space character '&nbsp;'</para>
        /// </summary>
        public PrForm SetHeader(string header)
        {
            FormHeader = header;
            return this;
        }

        /// <summary>
        /// <para>Specify the footer of the form.</para>
        /// <para>If empty, the footer won't be rendered.</para>
        /// <para>You can use html.</para>
        /// <para>If you want a footer with no text, you can use the html space character: &nbsp;</para>
        /// </summary>
        public PrForm SetFooter(string footer)
        {
            FormFooter = footer;
            return this;
        }

        /// <summary>
        /// <para>This method sets the minimum width of the form.</para>
        /// <para>The value can be either in pixels or percentage, for example, "100px" or "100%".</para>
        /// <para>If you don't specify the unit, for example, "100", it defaults to pixels.</para>
        /// <para>Default is "100%".</para>
        /// <para>You can pass "0" if you want the form to ocupy the least ammount of space possible.</para>
        /// </summary>
        /// <param name="minWidth">Width in pixels or percentage.</param>
        public PrForm SetMinWidth(string minWidth)
        {
            MinWidth = minWidth;
            return this;
        }

        /// <summary>
        /// <para>This method sets the minimum width of each label in the form.</para>
        /// <para>The value can be either in pixels or percentage, for example, "100px" or "100%".</para>
        /// <para>If you don't specify the unit, for example, "100", it defaults to pixels.</para>
        /// <para>You can pass "0" if you want the label to ocupy the least ammount of space possible.</para>
        /// <para>Default is "0".</para>
        /// <para>If you use validations, setting the label width to a percentage might not be a good idea because the content will stretch/shrink when displaying/hiding the validation message.</para>
        /// </summary>
        /// <param name="minWidth">Width in pixels or percentage.</param>
        public PrForm SetLabelWidth(string labelWidth)
        {
            LabelWidth = labelWidth;
            return this;
        }

        /// <summary>
        /// <para>Groups of fields are logical.</para>
        /// <para>They are used if you want to do partial submits or other types of interactions where you don't want the whole form to be affected.</para>
        /// <para>By default, each form has a group called 'ALL' with all fields and a group for each field with the same key as that field key.</para>
        /// <para>Groups will not affect the way fields are rendered.</para>
        /// </summary>
        /// <param name="groupKey">Unique identifier of the group.</param>
        /// <param name="fieldKeys">Comma separated list of field keys that should belong to this group.</param>
        /// <returns></returns>
        public PrForm AddGroup(string groupKey, params string[] fieldKeys)
        {
            if (!FieldGroups.ContainsKey(groupKey)) FieldGroups.Add(groupKey, new List<string>());
            foreach (string fieldKey in fieldKeys)
            {
                FieldGroups[groupKey].Add(fieldKey);
            }
            return this;
        }

        public PrForm SetDialog()
        {
            Dialog = true;
            Modal = false;
            return this;
        }

        public PrForm SetModalDialog()
        {
            Dialog = true;
            Modal = true;
            return this;
        }

        #region Fields

        private T GetField<T>(string fieldKey) where T : PrField
        {
            return (T)Fields[fieldKey];
        }

        private PrField GetField(string fieldKey)
        {
            return Fields[fieldKey];
        }

        /// <summary>
        /// <para>Base method for adding a field to the form.</para>
        /// <para>Specific methods for adding fields call this method.</para>
        /// <para>You should call the specific methods unless you want to do some secret magic with the fields.</para>
        /// <para>Each field is identified by an unique key. You cannot have two fields with the same key in this form.</para>
        /// <para>Each field has a label of text rendered in the left and the content itself rendered in the right.</para>
        /// <para>The alignment of the form is based on that virtual line that separates label from content.</para>
        /// <para>You can pass '', as the label, if you want the field itself to occupy the whole line or ' ' if you want the content aligned with the right but with no label.</para>
        /// </summary>
        public PrField AddField(PrField field)
        {
            Fields.Add(field.FieldKey, field);
            return field;
        }

        /// <summary>
        /// <para>Add a new FieldText to the form.</para>
        /// <para>FieldText resembles ASP.NET WebForms TextBoxes.</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldText AddFieldText(string fieldKey, string label)
        {
            return (PrFieldText)AddField(new PrFieldText(this, fieldKey, label));
        }

        /// <summary>
        /// <para>Add a new FieldTextArea to the form.</para>
        /// <para>FieldTextArea resembles ASP.NET WebForms TextBoxes with the TextMode set to MultiLine.</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldTextArea AddFieldTextArea(string fieldKey, string label)
        {
            return (PrFieldTextArea)AddField(new PrFieldTextArea(this, fieldKey, label));
        }

        public PrFieldComboBox AddFieldComboBox(string fieldKey, string label)
        {
            return AddFieldComboBox(fieldKey, label, new List<KeyValue>());
        }

        /// <summary>
        /// <para>Add a new FieldComboBox to the form.</para>
        /// <para>FieldComboBox renders a 'select' html element with autocomplete.</para>
        /// <para>The user can only choose one option among the available ones.</para>
        /// <para>You can use the 'AJAXUpdateItems' interaction if you have dependent ComboBoxes (ex: Make and Model).</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldComboBox AddFieldComboBox(string fieldKey, string label, List<KeyValue> data)
        {
            return (PrFieldComboBox)AddField(new PrFieldComboBox(this, fieldKey, label, data));
        }

        /// <summary>
        /// <para>This overload allows you to create a FieldComboBox based on a DataTable instead of a List of KeyValues.</para>
        /// <para>You must specify which column of the DataTable should be considered the Key and which should be considered the Value.</para>
        /// </summary>
        /// <seealso cref="AddFieldComboBox"/>
        public PrFieldComboBox AddFieldComboBox(string fieldKey, string label, DataTable dt, string keyColumnName, string valueColumnName)
        {
            return AddFieldComboBox(fieldKey, label, new Converter().ToKeyValueList(dt, keyColumnName, valueColumnName));
        }

        /// <summary>
        /// <para>This overload allows you to create a FieldComboBox based on a DataTable instead of a List of KeyValues.</para>
        /// <para>If the DataTable only has one column, it will be used as both the Key and the Value.</para>
        /// <para>Otherwise, the first column is considered to be the Key and the second column the Value.</para>
        /// </summary>
        /// <seealso cref="AddFieldComboBox"/>
        public PrFieldComboBox AddFieldComboBox(string fieldKey, string label, DataTable dt)
        {
            if (dt.Columns.Count > 1)
            {
                return AddFieldComboBox(fieldKey, label, dt, dt.Columns[0].ColumnName, dt.Columns[1].ColumnName);
            }
            else
            {
                return AddFieldComboBox(fieldKey, label, dt, dt.Columns[0].ColumnName, dt.Columns[0].ColumnName);
            }
        }

        /// <summary>
        /// <para>Add a new FieldButton to the form.</para>
        /// <para>Usually used for submitting the entire form or part of it, or getting a new form (when you have a form that depends on user input).</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldButton AddFieldButton(string fieldKey, string buttonText)
        {
            return (PrFieldButton)AddField(new PrFieldButton(this, fieldKey, buttonText));
        }

        public PrFieldRadios AddFieldRadios(string fieldKey, string label)
        {
            return AddFieldRadios(fieldKey, label, new List<KeyValue>());
        }

        /// <summary>
        /// <para>Add a new FieldRadios to the form.</para>
        /// <para>You can use this field to show/hide parts of the form on the client-side.</para>
        /// <para>If there are a considerable ammount of items, you might consider using a FieldComboBox instead.</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldRadios AddFieldRadios(string fieldKey, string label, List<KeyValue> data)
        {
            return (PrFieldRadios)AddField(new PrFieldRadios(this, fieldKey, label, data));
        }

        /// <summary>
        /// <para>This overload allows you to create a FieldRadios based on a DataTable instead of a List of KeyValues.</para>
        /// <para>You must specify which column of the DataTable should be considered the Key and which should be considered the Value.</para>
        /// </summary>
        /// <seealso cref="AddFieldRadios"/>
        public PrFieldRadios AddFieldRadios(string fieldKey, string label, DataTable dt, string keyColumnName, string valueColumnName)
        {
            return AddFieldRadios(fieldKey, label, new Converter().ToKeyValueList(dt, keyColumnName, valueColumnName));
        }

        /// <summary>
        /// <para>This overload allows you to create a FieldRadios based on a DataTable instead of a List of KeyValues.</para>
        /// <para>If the DataTable only has one column, it will be used as both the Key and the Value.</para>
        /// <para>Otherwise, the first column is considered to be the Key and the second column the Value.</para>
        /// </summary>
        /// <seealso cref="AddFieldRadios"/>
        public PrFieldRadios AddFieldRadios(string fieldKey, string label, DataTable dt)
        {
            if (dt.Columns.Count > 1)
            {
                return AddFieldRadios(fieldKey, label, dt, dt.Columns[0].ColumnName, dt.Columns[1].ColumnName);
            }
            else
            {
                return AddFieldRadios(fieldKey, label, dt, dt.Columns[0].ColumnName, dt.Columns[0].ColumnName);
            }
        }

        public PrFieldCheckBoxes AddFieldCheckBoxes(string fieldKey, string label)
        {
            return AddFieldCheckBoxes(fieldKey, label, new List<KeyValue>());
        }

        /// <summary>
        /// <para>Add a new FieldCheckBoxes to the form.</para>
        /// <para>If there are a considerable ammount of items, you might consider using a FieldMultiSelectBox instead.</para>
        /// <para>If there is only one item, you might consider using a FieldRadios with two items instead.</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldCheckBoxes AddFieldCheckBoxes(string fieldKey, string label, List<KeyValue> data)
        {
            return (PrFieldCheckBoxes)AddField(new PrFieldCheckBoxes(this, fieldKey, label, data));
        }

        /// <summary>
        /// <para>This overload allows you to create a FieldCheckBoxes based on a DataTable instead of a List of KeyValues.</para>
        /// <para>You must specify which column of the DataTable should be considered the Key and which should be considered the Value.</para>
        /// </summary>
        /// <seealso cref="AddFieldCheckBoxes"/>
        public PrFieldCheckBoxes AddFieldCheckBoxes(string fieldKey, string label, DataTable dt, string keyColumnName, string valueColumnName)
        {
            return AddFieldCheckBoxes(fieldKey, label, new Converter().ToKeyValueList(dt, keyColumnName, valueColumnName));
        }

        /// <summary>
        /// <para>This overload allows you to create a FieldCheckBoxes based on a DataTable instead of a List of KeyValues.</para>
        /// <para>If the DataTable only has one column, it will be used as both the Key and the Value.</para>
        /// <para>Otherwise, the first column is considered to be the Key and the second column the Value.</para>
        /// </summary>
        /// <seealso cref="AddFieldCheckBoxes"/>
        public PrFieldCheckBoxes AddFieldCheckBoxes(string fieldKey, string label, DataTable dt)
        {
            if (dt.Columns.Count > 1)
            {
                return AddFieldCheckBoxes(fieldKey, label, dt, dt.Columns[0].ColumnName, dt.Columns[1].ColumnName);
            }
            else
            {
                return AddFieldCheckBoxes(fieldKey, label, dt, dt.Columns[0].ColumnName, dt.Columns[0].ColumnName);
            }
        }

        public PrFieldMultiSelectBox AddFieldMultiSelectBox(string fieldKey, string label)
        {
            return AddFieldMultiSelectBox(fieldKey, label, new List<KeyValue>());
        }

        /// <summary>
        /// <para>Add a new FieldMultiSelectBox to the form.</para>
        /// <para>Renders a control that allows the user to select multiple items from a predefined list with autocomplete.</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldMultiSelectBox AddFieldMultiSelectBox(string fieldKey, string label, List<KeyValue> data)
        {
            return (PrFieldMultiSelectBox)AddField(new PrFieldMultiSelectBox(this, fieldKey, label, data));
        }

        /// <summary>
        /// <para>This overload allows you to create a FieldMultiSelectBox based on a DataTable instead of a List of KeyValues.</para>
        /// <para>You must specify which column of the DataTable should be considered the Key and which should be considered the Value.</para>
        /// </summary>
        /// <seealso cref="AddFieldMultiSelectBox"/>
        public PrFieldMultiSelectBox AddFieldMultiSelectBox(string fieldKey, string label, DataTable dt, string keyColumnName, string valueColumnName)
        {
            return AddFieldMultiSelectBox(fieldKey, label, new Converter().ToKeyValueList(dt, keyColumnName, valueColumnName));
        }

        /// <summary>
        /// <para>This overload allows you to create a FieldMultiSelectBox based on a DataTable instead of a List of KeyValues.</para>
        /// <para>If the DataTable only has one column, it will be used as both the Key and the Value.</para>
        /// <para>Otherwise, the first column is considered to be the Key and the second column the Value.</para>
        /// </summary>
        /// <seealso cref="AddFieldMultiSelectBox"/>
        public PrFieldMultiSelectBox AddFieldMultiSelectBox(string fieldKey, string label, DataTable dt)
        {
            if (dt.Columns.Count > 1)
            {
                return AddFieldMultiSelectBox(fieldKey, label, dt, dt.Columns[0].ColumnName, dt.Columns[1].ColumnName);
            }
            else
            {
                return AddFieldMultiSelectBox(fieldKey, label, dt, dt.Columns[0].ColumnName, dt.Columns[0].ColumnName);
            }
        }

        /// <summary>
        /// <para>Add a new FieldGridView to the form.</para>
        /// <para>This field is used to display tabular data to the user. Selecting or multi-selecting, paging, sorting, filtering and exporting to excel is builtin client and server side.</para>
        /// <para>You can use this field to select one or multiple items.</para>
        /// <para>By default, this field renders with the label set as ''. You can change that by calling 'SetFieldLabel'.</para>
        /// </summary>
        /// <param name="formService">You should pass 'this' if you are calling it from Custom_PerFormsService or an instance of Custom_PerFormsService if somewhere else.</param>
        /// <seealso cref="AddField"/>
        public PrFieldGridView AddFieldGridView(string fieldKey, string actionKey, FormService formService, bool clientSide, string tableLabel)
        {
            return (PrFieldGridView)AddField(new PrFieldGridView(this, fieldKey, actionKey, formService, clientSide, tableLabel));
        }

        /// <summary>
        /// <para>This overload allows you to create a FieldGridView based on a DataTable.</para>
        /// <para>The gridview will be client-side and you won't have the export to excel feature.</para>
        /// </summary>
        /// <seealso cref="AddFieldGridView"/>
        public PrFieldGridView AddFieldGridView(string fieldKey, DataTable dt, string tableLabel)
        {
            return (PrFieldGridView)AddField(new PrFieldGridView(this, fieldKey, dt, tableLabel));
        }

        /// <summary>
        /// <para>Add a new FieldLabel to the form.</para>
        /// <para>Used to display read-only text to the user. You can use html.</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldLabel AddFieldLabel(string fieldKey, string label, string defaultValue)
        {
            return (PrFieldLabel)AddField(new PrFieldLabel(this, fieldKey, label, defaultValue));
        }

        /// <summary>
        /// <para>Add a new FieldDate to the form.</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldDate AddFieldDate(string fieldKey, string label)
        {
            return (PrFieldDate)AddField(new PrFieldDate(this, fieldKey, label));
        }

        /// <summary>
        /// <para>Add a new FieldUpload to the form.</para>
        /// <para>Allows multi-uploads.</para>
        /// <para>All uploads will be placed in /PerForms/Uploads and the file name will be changed to a guid.</para>
        /// <para>On submits, the list of values for this field will be the guid and the original filename (2 strings for each upload).</para>
        /// <para>There are helpers methods you can call from Custom_PerFormsService to get and delete the uploads.</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldUpload AddFieldUpload(string fieldKey, string label)
        {
            return (PrFieldUpload)AddField(new PrFieldUpload(this, fieldKey, label));
        }

        /// <summary>
        /// <para>Add a new FieldFree to the form.</para>
        /// <para>This field is used to render custom html to the client.</para>
        /// </summary>
        /// <seealso cref="AddField"/>
        public PrFieldFree AddFieldFree(string fieldKey, string label, string source)
        {
            return (PrFieldFree)AddField(new PrFieldFree(this, fieldKey, label, source));
        }

        /// <summary>
        /// <para>This overload allows sending any type of Control to the form.</para>
        /// <para>Note that PerForms doesn't use viewstate, controls that rely on viewstate won't work.</para>
        /// </summary>
        /// <seealso cref="AddFieldFree"/>
        public PrFieldFree AddFieldFree(string fieldKey, string label, Control source)
        {
            return (PrFieldFree)AddField(new PrFieldFree(this, fieldKey, label, source));
        }

        /// <summary>
        /// <para>This field allows executing javascript code between the creation of the fields.</para>
        /// <para>The framework uses it to create tabs.</para>
        /// <para>You should use it only if you reaaaaaaaally know what you are doing.</para>
        /// </summary>
        public PrFieldPlaceHolder AddFieldPlaceHolder(string render)
        {
            return (PrFieldPlaceHolder)AddField(new PrFieldPlaceHolder(this, render));
        }

        #endregion

        /// <summary>
        /// <para>Add a new tab.</para>
        /// <para>Should be called right after calling AddTabArea.</para>
        /// <para>Any field you add after calling this method, will be inside this tab.</para>
        /// </summary>
        public PrForm AddTab(string label)
        {
            AddFieldPlaceHolder("form_" + FormKey + ".addTab('" + label + "');");
            return this;
        }

        /// <summary>
        /// <para>Add a new tab area.</para>
        /// <para>You should call AddTab after this.</para>
        /// <para>You can have nested TabAreas, but make sure each TabArea except the first is contained in a tab.</para>
        /// </summary>
        public PrForm AddTabArea()
        {
            AddFieldPlaceHolder("form_" + FormKey + ".addTabArea();");
            return this;
        }

        /// <summary>
        /// <para>Mark the end of a tab.</para>
        /// <para>If this is the last tab of the tabarea, you should call EndTabArea next.</para>
        /// </summary>
        public PrForm EndTab()
        {
            AddFieldPlaceHolder("form_" + FormKey + ".endTab();");
            return this;
        }

        /// <summary>
        /// <para>Mark the end of a tab area.</para>
        /// <para>Before calling this method, make sure you called EndTab first.</para>
        /// </summary>
        public PrForm EndTabArea()
        {
            AddFieldPlaceHolder("form_" + FormKey + ".endTabArea();");
            return this;
        }

        /// <summary>
        /// <para>This is where all the magic happens.</para>
        /// </summary>
        public string RenderForm()
        {
            string dialogHeader = ", title: '" + FormHeader + "'";
            if (Dialog) { FormHeader = ""; }

            string o = "{";
            o += "header: '" + FormHeader + "',";
            o += "footer: '" + FormFooter + "',";
            o += "labelWidth: '" + LabelWidth + "',";
            o += "minWidth: '" + MinWidth + "'";
            o += "}";
 

            string js = "<div id='" + FormKey + "' class='performs-form-div'></div><script type='text/javascript'>$(function () { ";
            js += "var form_" + FormKey + " = new performs_Form('" + FormKey + "'," + o + ");$('#" + FormKey + "').data('Perform', form_" + FormKey + ");";
            foreach (KeyValuePair<string, PrField> kvp in Fields)
            {
                js += kvp.Value.Render();
                OnFormReady += kvp.Value.RenderValidations();
            }

            foreach (KeyValuePair<string, List<string>> kvp in FieldGroups)
            {
                string fieldKeys = "[";
                foreach (string fieldKey in kvp.Value)
                {
                    fieldKeys += "'" + fieldKey + "',";
                }
                fieldKeys = fieldKeys.TrimEnd(',') + "]";
                js += "form_" + FormKey + ".addGroup('" + kvp.Key + "', " + fieldKeys + ");";
            }

            js += "$('#" + FormKey + "').append(form_" + FormKey + ".renderForm());" + OnPreFormReady + "form_" + FormKey + ".onFormReady();" + OnFormReady;
            if (Dialog)
            {
                string modal = Modal ? ", modal: true" : "";
                js += "$('#" + FormKey + "').dialog({ width: 'auto', resizable: false" + modal + dialogHeader + " });";
            }
            js += "});</script>";

            return js;
        }
    }
}
